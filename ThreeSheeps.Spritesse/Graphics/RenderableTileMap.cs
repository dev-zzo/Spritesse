using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThreeSheeps.Spritesse.Content;

namespace ThreeSheeps.Spritesse.Graphics
{
    /// <summary>
    /// Represents a tile map instance on the screen.
    /// </summary>
    public class RenderableTileMap : IRenderable
    {
        public RenderableTileMap(TileMap asset)
        {
            this.tileMap = asset;
            this.Visible = true;
        }

        /// <summary>
        /// Object position in screen space, in pixels
        /// </summary>
        public Point Position { get; set; }

        public bool Visible { get; set; }

        public Rectangle BoundingBox
        {
            get
            {
                Point tileSize = this.tileMap.TileSize;
                Point tileMapDims = this.tileMap.Dimensions;
                return new Rectangle(
                    this.Position.X, this.Position.Y,
                    tileMapDims.X * tileSize.X, tileMapDims.Y * tileSize.Y);
            }
        }

        public void Update(GameTime gameTime)
        {
            // Nothing so far.
        }

        public void Draw(SceneRenderContext context)
        {
            Point tileSize = this.tileMap.TileSize;
            Rectangle visibleTiles = this.CalculateVisibleTiles(context.CameraRectangle);
            // Draw visible tiles
            int endCol = visibleTiles.Right;
            int endRow = visibleTiles.Bottom;
            Rectangle destRect = new Rectangle(
                0, this.Position.Y + visibleTiles.Y * tileSize.Y, 
                tileSize.X, tileSize.Y);
            for (int rowIndex = visibleTiles.Y; rowIndex < endRow; ++rowIndex)
            {
                destRect.X = this.Position.X + visibleTiles.X * tileSize.X;
                for (int colIndex = visibleTiles.X; colIndex < endCol; ++colIndex)
                {
                    Tile tile = this.tileMap.GetTile(colIndex, rowIndex);
                    // Skip empty tiles
                    if (tile.SheetIndex != Tile.EMPTY_TILE)
                    {
                        SpriteSheet sheet = this.tileMap.SpriteSheets[tile.SheetIndex];
                        SpriteDefinition definition = sheet.Definitions[tile.SpriteIndex];
                        context.SpriteBatch.Draw(
                            sheet.Texture,
                            destRect,
                            definition.SourceRectangle,
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            SpriteEffects.None,
                            0.0f);
                    }
                    destRect.X += tileSize.X;
                }
                destRect.Y += tileSize.Y;
            }
        }

        private Rectangle CalculateVisibleTiles(Rectangle cameraRect)
        {
            Rectangle visible = Rectangle.Intersect(cameraRect, this.BoundingBox);
            // No overlap?
            if (visible.IsEmpty)
                return visible;
            visible.Offset(-this.Position.X, -this.Position.Y);
            // To draw the visible area, the following is required:
            // + Starting indices of the top left visible tile
            // + Tile counts in both dimensions
            Point tileSize = this.tileMap.TileSize;
            Point tileMapDims = this.tileMap.Dimensions;
            // Calculate the starting indices
            // NOTE: Clip the camera offset to positive values only
            int startTileX, startTileY;
            startTileX = visible.X > 0 ? visible.X / tileSize.X : 0;
            startTileY = visible.Y > 0 ? visible.Y / tileSize.Y : 0;
            // Calculate tile counts
            // NOTE: Clip tile counts to keep them within bounds
            int tileCountX, tileCountY;
            tileCountX = Math.Min(1 + (visible.Width / tileSize.X), tileMapDims.X - startTileX);
            tileCountY = Math.Min(1 + (visible.Height / tileSize.Y), tileMapDims.Y - startTileY);
            return new Rectangle(startTileX, startTileY, tileCountX, tileCountY);
        }

        private TileMap tileMap;
    }
}
