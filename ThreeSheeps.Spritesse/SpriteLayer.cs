using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThreeSheeps.Spritesse
{
    /// <summary>
    /// A sprite layer is a collection of sprites rendered together.
    /// </summary>
    public abstract class SpriteLayer
    {
        /// <summary>
        /// Constructs a base layer object
        /// </summary>
        /// <param name="manager">SpriteManager instance to bind to</param>
        /// <param name="name">Layer name</param>
        /// <param name="depth">Layer depth</param>
        public SpriteLayer(SpriteManager manager, string name, float depth = 0.0f)
        {
            if (depth > 1.0f || depth < 0.0f)
            {
                throw new ArgumentOutOfRangeException("depth");
            }

            this.manager = manager;
            this.Name = name;
            this.Depth = depth;
        }

        /// <summary>
        /// Sprite manager this layer is attached to.
        /// </summary>
        public SpriteManager SpriteManager
        {
            get { return this.SpriteManager; }
        }

        /// <summary>
        /// Layer name for reference
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Depth to be used when rendering.
        /// Layers with larger depth will be drawn before layers with smaller depth.
        /// </summary>
        public float Depth { get; private set; }

        /// <summary>
        /// Allows the layer to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Allows the layer to perform actions before drawing.
        /// </summary>
        /// <param name="batch">Provides a sprite batch object to use.</param>
        public virtual void BeginDraw(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);
        }

        /// <summary>
        /// Allows the layer to draw itself.
        /// </summary>
        /// <remarks>No need to call Begin() or End().</remarks>
        /// <param name="batch">Provides a sprite batch object to use.</param>
        public abstract void Draw(SpriteBatch batch);

        /// <summary>
        /// Allows the layer to perform actions after drawing.
        /// </summary>
        /// <param name="batch">Provides a sprite batch object to use.</param>
        public virtual void EndDraw(SpriteBatch batch)
        {
            batch.End();
        }

        private SpriteManager manager;
    }
}
