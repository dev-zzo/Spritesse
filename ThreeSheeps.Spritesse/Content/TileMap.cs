using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ThreeSheeps.Spritesse.Content
{
    /// <summary>
    /// An element of the tile layer.
    /// </summary>
    public struct Tile
    {
        public ushort SheetIndex;
        public ushort SpriteIndex;
    }

    /// <summary>
    /// A simple tile map layer
    /// </summary>
    public class TileMap
    {
        /// <summary>
        /// Constructs the object.
        /// </summary>
        /// <param name="spriteSheets"></param>
        /// <param name="tiles"></param>
        public TileMap(Point tileSize, SpriteSheet[] spriteSheets, Tile[,] tiles)
        {
            this.spriteSheets = spriteSheets;
            this.tiles = tiles;
        }

        /// <summary>
        /// Size of each individual tile, in pixels
        /// </summary>
        public Point TileSize
        {
            get { return this.tileSize; }
        }

        /// <summary>
        /// Palette of sprite sheets to choose from
        /// </summary>
        public IList<SpriteSheet> SpriteSheets
        {
            get { return this.spriteSheets; }
        }

        /// <summary>
        /// Tile array
        /// </summary>
        public Tile[,] Tiles
        {
            get { return this.tiles; }
        }

        private Point tileSize;
        private SpriteSheet[] spriteSheets;
        private Tile[,] tiles;
    }

    public sealed class TileMapReader : ContentTypeReader<TileMap>
    {
        protected override TileMap Read(ContentReader input, TileMap existingInstance)
        {
            throw new System.NotImplementedException();
        }
    }
}
