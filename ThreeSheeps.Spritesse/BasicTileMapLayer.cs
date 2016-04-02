using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThreeSheeps.Spritesse.Content;

namespace ThreeSheeps.Spritesse
{
    /// <summary>
    /// Implements a basic tile map layer (simple sprites, no animations)
    /// </summary>
    public class BasicTileMapLayer : SpriteLayer
    {
        public BasicTileMapLayer(SpriteManager manager, string name, float depth, TileMap tileMap)
            : base(manager, name, depth)
        {
            this.tileMap = tileMap;
            this.Scale = 1.0f;
        }

        /// <summary>
        /// The associated tile map content
        /// </summary>
        public TileMap TileMap
        {
            get { return this.tileMap; }
        }

        public float Scale { get; set; }

        protected Point TileSize
        {
            get { return new Point((int)(this.TileMap.TileSize.X * this.Scale), (int)(this.TileMap.TileSize.Y * this.Scale)); }
        }

        public override void Update(GameTime gameTime)
        {
            // Check if cached data is invalid
            if (this.cacheValid && this.cachedCameraOffset == this.SpriteManager.CameraOffset)
            {
                return;
            }
            // Update cached parameters
            this.cacheValid = true;
            this.cachedCameraOffset = this.SpriteManager.CameraOffset;
            // Do the calculations
            this.UpdateVisibility();
        }

        public override void Draw(SpriteBatch batch)
        {
            // Draw visible tiles
            int endCol = this.startPosition.X + this.tileCount.X;
            int endRow = this.startPosition.Y + this.tileCount.Y;
            SpriteSheet sheet = null;
            Point tileSize = this.TileSize;
            Vector2 position = this.offset;
            for (int rowIndex = this.startPosition.Y; rowIndex < endRow; ++rowIndex)
            {
                position.X = this.offset.X;
                for (int colIndex = this.startPosition.X; colIndex < endCol; ++colIndex)
                {
                    Tile tile = this.tileMap.GetTile(colIndex, rowIndex);
                    // Skip empty tiles
                    if (tile.SheetIndex != Tile.EMPTY_TILE)
                    {
                        sheet = this.tileMap.SpriteSheets[tile.SheetIndex];
                        SpriteDefinition definition = sheet.Definitions[tile.SpriteIndex];
                        batch.Draw(
                            sheet.Texture,
                            position,
                            definition.SourceRectangle,
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            this.Scale,
                            SpriteEffects.None,
                            this.Depth);
                    }
                    position.X += tileSize.X;
                }
                position.Y += tileSize.Y;
            }
        }

        private void UpdateVisibility()
        {
            // To draw the visible area, the following is required:
            // + Starting indices of the top left visible tile
            // + Tile counts in both dimensions
            // + Offset to be added to its visible position

            int cachedCameraX = (int)this.cachedCameraOffset.X;
            int cachedCameraY = (int)this.cachedCameraOffset.Y;
            Point tileSize = this.TileSize;
            // Calculate the starting indices
            // NOTE: Clip the camera offset to positive values only
            if (cachedCameraX >= 0)
            {
                this.startPosition.X = cachedCameraX / tileSize.X;
                this.offset.X = -(cachedCameraX % tileSize.X);
            }
            else
            {
                this.startPosition.X = 0;
                this.offset.X = cachedCameraX;
            }
            if (cachedCameraY >= 0)
            {
                this.startPosition.Y = cachedCameraY / tileSize.Y;
                this.offset.Y = -(cachedCameraY % tileSize.Y);
            }
            else
            {
                this.startPosition.Y = 0;
                this.offset.Y = cachedCameraY;
            }
            // Calculate tile counts
            // NOTE: Clip tile counts to keep them within bounds
            // TODO: implement zooming here.
            Rectangle windowBounds = this.SpriteManager.Game.Window.ClientBounds;
            Point tileMapDims = this.TileMap.Dimensions;
            this.tileCount.X = Math.Min(1 + (windowBounds.Width / tileSize.X), tileMapDims.X - this.startPosition.X);
            this.tileCount.Y = Math.Min(1 + (windowBounds.Height / tileSize.Y), tileMapDims.Y - this.startPosition.Y);
        }

        private TileMap tileMap;
        private bool cacheValid = false;
        private Vector2 cachedCameraOffset;
        private Point startPosition;
        private Point tileCount;
        private Vector2 offset;
    }
}
