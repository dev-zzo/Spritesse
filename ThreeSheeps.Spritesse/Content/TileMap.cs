using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Content
{
    /// <summary>
    /// An element of the tile layer.
    /// </summary>
    public struct Tile
    {
        /// <summary>
        /// Used to indicate that the tile is empty.
        /// </summary>
        public const ushort EMPTY_TILE = UInt16.MaxValue;

        /// <summary>
        /// Index of a spite sheet to use.
        /// </summary>
        public ushort SheetIndex;

        /// <summary>
        /// Index of a sprite in the sprite sheet.
        /// </summary>
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
        internal TileMap(Point tileSize, IList<SpriteSheet> spriteSheets, Tile[,] tiles)
        {
            this.tileSize = tileSize;
            this.spriteSheets = spriteSheets;
            this.tiles = tiles;
        }

        /// <summary>
        /// Dimensions of the tile map, in tiles
        /// </summary>
        public Point Dimensions
        {
            get { return new Point(this.tiles.GetLength(1), this.tiles.GetLength(0)); }
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
        /// Access the tile
        /// </summary>
        public Tile GetTile(int x, int y)
        {
            return this.tiles[y, x];
        }

        private Point tileSize;
        private IList<SpriteSheet> spriteSheets;
        private Tile[,] tiles;
    }
}
