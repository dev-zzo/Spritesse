using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThreeSheeps.Spritesse.Content;

namespace ThreeSheeps.Spritesse.Scene
{
    /// <summary>
    /// Manages the typical scene (backgrounds, objects, overlays)
    /// </summary>
    public class SceneComponent : DrawableGameComponent, ISceneService
    {
        public SceneComponent(Game game)
            : base(game)
        {
            this.sceneState = this.CreateSceneState();

            game.Services.AddService(typeof(ISceneService), this);
        }

        #region ISceneService implementation

        public void Clear()
        {
            this.location = null;
            this.locationName = null;
            this.objects.Clear();
            this.drawables.Clear();
        }

        public void LoadLocation(string name)
        {
            this.locationName = name;
            this.location = this.Game.Content.Load<Location>(name);
            float depth = START_DEPTH_BACKGROUND;
            foreach (TileMap layer in this.location.BackgroundLayers)
            {
                TileMapObject obj = new TileMapObject(layer);
                obj.Depth = depth;
                this.AddObject(obj);
                depth -= 0.001f;
            }
            depth = START_DEPTH_FOREGROUND;
            foreach (TileMap layer in this.location.ForegroundLayers)
            {
                TileMapObject obj = new TileMapObject(layer);
                obj.Depth = depth;
                this.AddObject(obj);
                depth -= 0.001f;
            }
        }

        public void AddObject(ISceneObject obj)
        {
            this.objects.Add(obj);
            IDrawableSceneObject drawable = obj as IDrawableSceneObject;
            if (drawable != null)
            {
                this.drawables.Add(drawable);
            }
        }

        public void RemoveObject(ISceneObject obj)
        {
            this.objects.Remove(obj);
            IDrawableSceneObject drawable = obj as IDrawableSceneObject;
            if (drawable != null)
            {
                this.drawables.Remove(drawable);
            }
        }

        #endregion

        #region Game component overrides

        public override void Initialize()
        {
            base.Initialize();
            this.spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.UpdateSceneState();
            
            foreach (ISceneObject obj in this.objects)
            {
                obj.Update(gameTime, this.sceneState);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Rectangle cameraBox = this.sceneState.CameraRectangle;
            this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            foreach (IDrawableSceneObject obj in this.drawables)
            {
                if (!obj.Visible)
                    continue;
                Rectangle boundingBox = obj.BoundingBox;
                if (!(cameraBox.Intersects(boundingBox) || cameraBox.Contains(boundingBox)))
                    continue;

                float depth = obj.Depth;
                if (depth < 0.0f)
                {
                    float depthRange = END_DEPTH_DYNAMIC - START_DEPTH_DYNAMIC;
                    float posDiff = boundingBox.Bottom - cameraBox.Y;
                    depth = END_DEPTH_DYNAMIC - depthRange * posDiff / cameraBox.Height;
                }

                obj.Draw(this.spriteBatch, this.sceneState, depth);
            }
            this.spriteBatch.End();
        }

        #endregion

        protected SceneState CreateSceneState()
        {
            return new SceneState();
        }

        protected void UpdateSceneState()
        {
            ISceneCameraService camera = this.Game.Services.GetService(typeof(ISceneCameraService)) as ISceneCameraService;
            Rectangle cameraRect;
            if (camera != null)
            {
                cameraRect = camera.ViewRectangle;
            }
            else
            {
                cameraRect = this.Game.Window.ClientBounds;
                cameraRect.X = cameraRect.Y = 0;
            }
            if (this.sceneState.CameraRectangle != cameraRect)
            {
                this.sceneState.CameraRectangle = cameraRect;
                this.sceneState.Revision++;
            }
        }

        private const float START_DEPTH_BACKGROUND = 0.9f;
        private const float START_DEPTH_DYNAMIC = 0.3f;
        private const float END_DEPTH_DYNAMIC = 0.8f;
        private const float START_DEPTH_FOREGROUND = 0.3f;

        private readonly HashSet<ISceneObject> objects = new HashSet<ISceneObject>();
        private readonly HashSet<IDrawableSceneObject> drawables = new HashSet<IDrawableSceneObject>();
        private readonly SceneState sceneState;
        private SpriteBatch spriteBatch;
        private string locationName;
        private Location location;

    }
}
