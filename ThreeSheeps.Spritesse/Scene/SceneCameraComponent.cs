using Microsoft.Xna.Framework;
using ThreeSheeps.Spritesse.Graphics;

namespace ThreeSheeps.Spritesse.Scene
{
    /// <summary>
    /// Implements the ISceneCameraService service.
    /// </summary>
    public class SceneCameraComponent : GameComponent, ISceneCameraService
    {
        public SceneCameraComponent(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(ISceneCameraService), this);
        }

        #region ISceneCameraService implementation

        public Point Position { get; set; }

        public Rectangle ViewRectangle { get { return this.viewRectangle; } }

        public void AttachToObject(ISceneObject obj)
        {
            this.attachedTo = obj;
        }

        #endregion

        public override void Initialize()
        {
            base.Initialize();
            this.renderer = this.Game.Services.GetService(typeof(ISceneRendererService)) as ISceneRendererService;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (this.attachedTo != null)
            {
                this.Position = this.attachedTo.Position;
            }
            this.viewRectangle.Width = this.renderer.Viewport.Width;
            this.viewRectangle.Height = this.renderer.Viewport.Height;
            this.viewRectangle.X = this.Position.X - this.viewRectangle.Width / 2;
            this.viewRectangle.Y = this.Position.Y - this.viewRectangle.Height / 2;
        }

        private ISceneRendererService renderer;
        private Rectangle viewRectangle;
        private ISceneObject attachedTo;
    }
}
