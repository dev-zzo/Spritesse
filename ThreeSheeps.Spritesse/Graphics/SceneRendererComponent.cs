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
            this.spriteBatch = this.context.SpriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ISceneCameraService camera = this.Game.Services.GetService(typeof(ISceneCameraService)) as ISceneCameraService;
            if (camera != null)
            {
                this.context.CameraRectangle = camera.ViewRectangle;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.spriteBatch.Begin();
            this.DrawLayers(this.backLayers);
            this.spriteBatch.End();

            this.spriteBatch.Begin();
            this.DrawObjects(this.objects);
            this.spriteBatch.End();

            this.spriteBatch.Begin();
            this.DrawLayers(this.foreLayers);
            this.spriteBatch.End();

            this.PostProcess();
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

        private void PostProcess()
        {
        }

        private readonly IList<IRenderable> backLayers = new List<IRenderable>();
        private readonly IList<IRenderable> foreLayers = new List<IRenderable>();
        private readonly ISet<IRenderable> objects = new HashSet<IRenderable>();
        private SpriteBatch spriteBatch;
        private SceneRenderContext context = new SceneRenderContext();
    }
}
