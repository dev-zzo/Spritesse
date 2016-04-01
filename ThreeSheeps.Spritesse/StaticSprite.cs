using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThreeSheeps.Spritesse.Content;

namespace ThreeSheeps.Spritesse
{
    /// <summary>
    /// A static sprite uses a pre-defined sheet and sprite index.
    /// </summary>
    public class StaticSprite : Sprite
    {
        public StaticSprite(SpriteSheet sheet, int index)
        {
            this.texture = sheet.Texture;
            this.definition = sheet.Definitions[index];
        }

        public override void Draw(SpriteBatch batch, float depth)
        {
            // Calculate a corrected screen position
            Vector2 position = new Vector2(
                this.Position.X - this.definition.PivotOffset.X,
                this.Position.Y - this.definition.PivotOffset.Y);
            // Issue a draw call
            batch.Draw(
                this.texture,
                position,
                this.definition.SourceRectangle,
                this.TintColor,
                this.Rotation,
                Vector2.Zero,
                this.Scale,
                this.Effects,
                depth);
        }

        private Texture2D texture;
        private SpriteDefinition definition;
    }
}
