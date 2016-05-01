using Microsoft.Xna.Framework;

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
            this.Scale = 1.0f;
        }

        #region ISceneCameraService implementation

        public Point Position { get; set; }

        public float Scale { get; set; }

        public Rectangle ViewRectangle
        {
            get
            {
                Rectangle clientBounds = this.Game.Window.ClientBounds;
                int width = (int)(0.5f + clientBounds.Width / this.Scale);
                int height = (int)(0.5f + clientBounds.Height / this.Scale);
                return new Rectangle(this.Position.X - width / 2, this.Position.Y - height / 2, width, height);
            }
        }

        public void AttachToObject(ISceneObject obj)
        {
            this.attachedTo = obj;
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (this.attachedTo != null)
            {
                this.Position = this.attachedTo.Position;
            }
        }

        private ISceneObject attachedTo;
    }
}
