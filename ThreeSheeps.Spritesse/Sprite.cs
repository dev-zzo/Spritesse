using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThreeSheeps.Spritesse
{
    /// <summary>
    /// A drawable sprite (with no implementation)
    /// </summary>
    public abstract class Sprite
    {
        public Sprite()
        {
            this.Scale = 1.0f;
            this.TintColor = Color.White;
            this.Effects = SpriteEffects.None;
        }

        /// <summary>
        /// Position of the sprite (its pivot point) on the screen
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// Rotation angle, in radians
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Scale used to draw the sprite
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// Color to be used for tinting
        /// </summary>
        public Color TintColor { get; set; }

        /// <summary>
        /// Flipping effects to apply
        /// </summary>
        public SpriteEffects Effects { get; set; }

        /// <summary>
        /// Implements internal sprite state update, as required
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public virtual void Update(GameTime gameTime)
        {
            // Default implementation does nothing.
        }

        /// <summary>
        /// Implements the draw operation.
        /// </summary>
        /// <param name="batch">Sprite batch to be used</param>
        /// <param name="depth">Depth value to be passed to Draw()</param>
        public abstract void Draw(SpriteBatch batch, float depth);
    }
}
