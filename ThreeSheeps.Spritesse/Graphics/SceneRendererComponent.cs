using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThreeSheeps.Spritesse.Scene;

namespace ThreeSheeps.Spritesse.Graphics
{
    /// <summary>
    /// Implements scene rendering.
    /// </summary>
    public class SceneRendererComponent : DrawableGameComponent, ISceneRendererService
    {
        public SceneRendererComponent(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(ISceneRendererService), this);
        }

        public Viewport Viewport { get { return this.sceneRenderViewport; } }

        public void RegisterObject(IRenderable obj)
        {
            this.objects.Add(obj);
        }

        public void UnregisterObject(IRenderable obj)
        {
            this.objects.Remove(obj);
        }

        public void AddBackgroundObject(IRenderable obj)
        {
            this.backLayers.Add(obj);
        }

        public void AddForegroundObject(IRenderable obj)
        {
            this.foreLayers.Add(obj);
        }

        public void Clear()
        {
            this.backLayers.Clear();
            this.foreLayers.Clear();
            this.objects.Clear();
        }

        public override void Initialize()
        {
            base.Initialize();
            this.camera = this.Game.Services.GetService(typeof(ISceneCameraService)) as ISceneCameraService;

            GraphicsDevice gd = this.Game.GraphicsDevice;

            this.spriteBatch = this.context.SpriteBatch = new SpriteBatch(gd);

            int viewWidth = gd.PresentationParameters.BackBufferWidth / 2;
            int viewHeight = gd.PresentationParameters.BackBufferHeight / 2;
            this.sceneRenderViewport = new Viewport(0, 0, viewWidth, viewHeight);

            this.sceneRenderTarget = new RenderTarget2D(gd,
                (int)MathHelper.RountToPowerOfTwo((uint)viewWidth),
                (int)MathHelper.RountToPowerOfTwo((uint)viewHeight));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (this)
                {
                    this.sceneRenderTarget.Dispose();
                    this.spriteBatch.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (this.camera != null)
            {
                this.context.CameraRectangle = this.camera.ViewRectangle;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.DrawScene();
            this.PostProcess();
        }

        protected virtual void DrawScene()
        {
            // NOTE: Positive values translate the view up/left.
            Vector3 cameraTranslation = Vector3.Zero;
            if (camera != null)
            {
                Rectangle cameraView = camera.ViewRectangle;
                Point cameraPosition = camera.Position;
                cameraTranslation.X = cameraView.Width / 2 - cameraPosition.X;
                cameraTranslation.Y = cameraView.Height / 2 - cameraPosition.Y;
            }
            Matrix cameraTransform = Matrix.CreateTranslation(cameraTranslation);

            GraphicsDevice gd = this.Game.GraphicsDevice;
            Viewport defaultViewport = gd.Viewport;
            gd.SetRenderTarget(this.sceneRenderTarget);
            gd.Viewport = this.sceneRenderViewport;

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cameraTransform);
            this.DrawLayers(this.backLayers);
            this.spriteBatch.End();

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cameraTransform);
            this.DrawObjects(this.objects);
            this.spriteBatch.End();

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cameraTransform);
            this.DrawLayers(this.foreLayers);
            this.spriteBatch.End();

            gd.SetRenderTarget(null);
            gd.Viewport = defaultViewport;
        }

        private void DrawLayers(IList<IRenderable> layers)
        {
            foreach (IRenderable obj in layers)
            {
                obj.Draw(this.context);
            }
        }

        private void DrawObjects(ISet<IRenderable> objects)
        {
            foreach (IRenderable obj in objects)
            {
                obj.Draw(this.context);
            }
        }

        protected virtual void PostProcess()
        {
            this.spriteBatch.Begin();
            this.spriteBatch.Draw(this.sceneRenderTarget,
                this.Game.GraphicsDevice.Viewport.Bounds,
                this.sceneRenderViewport.Bounds,
                Color.White);
            this.spriteBatch.End();
        }

        private readonly IList<IRenderable> backLayers = new List<IRenderable>();
        private readonly IList<IRenderable> foreLayers = new List<IRenderable>();
        private readonly ISet<IRenderable> objects = new HashSet<IRenderable>();
        private SpriteBatch spriteBatch;
        private Viewport sceneRenderViewport;
        private RenderTarget2D sceneRenderTarget;
        private ISceneCameraService camera;
        private SceneRenderContext context = new SceneRenderContext();
    }
}
