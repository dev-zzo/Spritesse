using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThreeSheeps.Spritesse.Content;

namespace ThreeSheeps.Spritesse.Scene
{
    public class TileMapObject : IDrawableSceneObject
    {
        public TileMapObject(TileMap asset)
        {
            this.asset = asset;
            this.Visible = true;
        }

        #region ISceneObject implementation

        public Point Position { get; set; }

        public void Update(GameTime gameTime, SceneState sceneState)
        {
            if (sceneState.Revision == this.cacheRevision)
            {
                return;
            }
            this.cacheRevision = sceneState.Revision;

            // To draw the visible area, the following is required:
            // + Starting indices of the top left visible tile
            // + Tile counts in both dimensions

            int cameraX = sceneState.CameraRectangle.X - this.Position.X;
            int cameraY = sceneState.CameraRectangle.Y - this.Position.Y;
            Point tileSize = this.asset.TileSize;
            // Calculate the starting indices
            // NOTE: Clip the camera offset to positive values only
            if (cameraX >= 0)
            {
                this.startTile.X = cameraX / tileSize.X;
            }
            else
            {
                this.startTile.X = 0;
            }
            if (cameraY >= 0)
            {
                this.startTile.Y = cameraY / tileSize.Y;
            }
            else
            {
                this.startTile.Y = 0;
            }
            // Calculate tile counts
            // NOTE: Clip tile counts to keep them within bounds
            Point tileMapDims = this.asset.Dimensions;
            this.tileCount.X = Math.Min(1 + (sceneState.CameraRectangle.Width / tileSize.X), tileMapDims.X - this.startTile.X);
            this.tileCount.Y = Math.Min(1 + (sceneState.CameraRectangle.Height / tileSize.Y), tileMapDims.Y - this.startTile.Y);
        }

        #endregion

        #region IDrawableSceneObject implementation

        public float Depth { get; set; }

        public bool Visible { get; set; }

        public bool IsVisible(Rectangle cameraRect)
        {
            Point tileSize = this.asset.TileSize;
            Point tileMapDims = this.asset.Dimensions;
            Rectangle boundingBox = new Rectangle(
                this.Position.X, this.Position.Y,
                tileMapDims.X * tileSize.X, tileMapDims.Y * tileSize.Y);
            return this.Visible && (cameraRect.Contains(boundingBox) || cameraRect.Intersects(boundingBox));
        }
        
        public void Draw(SpriteBatch spriteBatch, SceneState sceneState, float depth)
        {
            // Draw visible tiles
            int endCol = this.startTile.X + this.tileCount.X;
            int endRow = this.startTile.Y + this.tileCount.Y;
            Point tileSize = this.asset.TileSize;
            float positionY = this.Position.Y + this.startTile.Y * tileSize.Y;
            for (int rowIndex = this.startTile.Y; rowIndex < endRow; ++rowIndex)
            {
                float positionX = this.Position.X + this.startTile.X * tileSize.X;
                for (int colIndex = this.startTile.X; colIndex < endCol; ++colIndex)
                {
                    Tile tile = this.asset.GetTile(colIndex, rowIndex);
                    // Skip empty tiles
                    if (tile.SheetIndex != Tile.EMPTY_TILE)
                    {
                        SpriteSheet sheet = this.asset.SpriteSheets[tile.SheetIndex];
                        SpriteDefinition definition = sheet.Definitions[tile.SpriteIndex];
                        spriteBatch.Draw(
                            sheet.Texture,
                            new Vector2(positionX, positionY),
                            definition.SourceRectangle,
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            depth);
                    }
                    positionX += tileSize.X;
                }
                positionY += tileSize.Y;
            }
        }

        #endregion

        private TileMap asset;
        private uint cacheRevision;
        private Point startTile;
        private Point tileCount;
    }
}
