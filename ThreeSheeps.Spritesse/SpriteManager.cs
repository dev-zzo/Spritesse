using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThreeSheeps.Spritesse
{
    /// <summary>
    /// Subclass this component to create your own layer structure.
    /// </summary>
    public abstract class SpriteManager : DrawableGameComponent
    {
        public SpriteManager(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// A camera offset for layers that make use of it
        /// </summary>
        public Vector2 CameraOffset { get; set; }

        public override void Initialize()
        {
            base.Initialize();
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.CreateLayers();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (SpriteLayer layer in this.layers)
            {
                layer.Update(gameTime);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the game component to draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            // Assume layers are sorted back to front
            foreach (SpriteLayer layer in this.layers)
            {
                layer.BeginDraw(this.spriteBatch);
                layer.Draw(this.spriteBatch);
                layer.EndDraw(this.spriteBatch);
            }
        }

        /// <summary>
        /// Implements layer set creation
        /// </summary>
        protected abstract void CreateLayers();

        /// <summary>
        /// Adds a new layer to the scene
        /// </summary>
        /// <param name="layer">Layer to be added</param>
        protected void AddLayer(SpriteLayer layer)
        {
            this.layers.Add(layer);
            this.layers.Sort(this.layerComparer);
        }

        #region Private members

        private SpriteBatch spriteBatch;
        private readonly List<SpriteLayer> layers = new List<SpriteLayer>();
        private readonly IComparer<SpriteLayer> layerComparer = new SpriteLayerDepthComparer();

        #endregion
    }

    /// <summary>
    /// Sorts the layers with larger depth first.
    /// </summary>
    sealed class SpriteLayerDepthComparer : IComparer<SpriteLayer>
    {
        public int Compare(SpriteLayer x, SpriteLayer y)
        {
            float depthX = x.Depth;
            float depthY = y.Depth;
            if (depthX < depthY)
            {
                return 1;
            }
            if (depthY < depthX)
            {
                return -1;
            }
            return 0;
        }
    }
}
