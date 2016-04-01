using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThreeSheeps.Spritesse
{
    /// <summary>
    /// Implements a basic sprite layer, where sprites are drawn as is.
    /// </summary>
    public class BasicSpriteLayer : SpriteLayer
    {
        public BasicSpriteLayer(SpriteManager manager, string name, float depth = 0.0f)
            : base(manager, name, depth)
        {
        }

        public ICollection<Sprite> Sprites
        {
            get { return this.sprites; }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Sprite sprite in this.sprites)
            {
                sprite.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            foreach (Sprite sprite in this.sprites)
            {
                sprite.Draw(batch, this.Depth);
            }
        }

        private HashSet<Sprite> sprites = new HashSet<Sprite>();
    }
}
