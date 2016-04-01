using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThreeSheeps.Spritesse.Content;

namespace ThreeSheeps.Spritesse
{
    public class BasicTileMapLayer : SpriteLayer
    {
        public BasicTileMapLayer(SpriteManager manager, string name, float depth = 0.0f)
            : base(manager, name, depth)
        {
        }

        public override void Update(GameTime gameTime)
        {
            // Check if cached data is invalid
            // Determine visible tiles
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch batch)
        {
            // Draw visible tiles
            throw new NotImplementedException();
        }

        private TileMap tileMap;
    }
}
