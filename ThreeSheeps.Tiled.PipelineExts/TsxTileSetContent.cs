using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace ThreeSheeps.Tiled
{
    /// <summary>
    /// Terrain descriptor
    /// </summary>
    public sealed class TsxTerrainContent
    {
        /// <summary>
        /// Terrain name; used in the editor only.
        /// </summary>
        public string Name;
        /// <summary>
        /// Contains any custom properties set for this object.
        /// </summary>
        public TxxProperties Properties;
        /// <summary>
        /// Local tile id used to represent this terrain in the editor
        /// </summary>
        public int TileId;
    }

    /// <summary>
    /// Defines properties of a single tile within a tileset.
    /// </summary>
    public sealed class TsxTileContent
    {
        public TsxTileContent()
        {
            this.Probability = 0.0f;
            this.Terrain = new TsxTerrainContent[4];
        }

        /// <summary>
        /// Defines the terrain type of each corner of the tile in the order 
        /// top-left, top-right, bottom-left, bottom-right.
        /// </summary>
        public readonly TsxTerrainContent[] Terrain;

        /// <summary>
        /// A percentage indicating the probability that this tile is chosen 
        /// when it competes with others while editing with the terrain tool.
        /// </summary>
        public float Probability;

        /// <summary>
        /// Associated objects (seems to be used for collision stuff only).
        /// </summary>
        public TxxObjectGroupContent Objects;

        // TODO: animation

        /// <summary>
        /// Contains any custom properties set for this node.
        /// </summary>
        public TxxProperties Properties;
    }

    /// <summary>
    /// Encapsulates data associated with an image underlyting the tileset.
    /// </summary>
    public sealed class TsxImageContent
    {
        /// <summary>
        /// Image to be used.
        /// </summary>
        /// <remarks>
        /// Embedded images are not currently supported.
        /// </remarks>
        public ExternalReference<TextureContent> Image;
        /// <summary>
        /// Image size (for reference)
        /// </summary>
        public Point ImageSize;
        /// <summary>
        /// Defines the color marking transparent pixels.
        /// </summary>
        public Color ColorKey;
    }

    /// <summary>
    /// Represents a tileset, external or embedded.
    /// </summary>
    public sealed class TsxTileSetContent : ContentItem
    {
        /// <summary>
        /// Associated image data.
        /// </summary>
        public TsxImageContent Image;
        /// <summary>
        /// The (maximum) dimensions of the tiles, in pixels.
        /// </summary>
        public Point TileSize;
        /// <summary>
        /// Offset to be applied when drawing a tile from the related tileset, in pixels.
        /// </summary>
        public Point TileOffset;
        /// <summary>
        /// The spacing between adjacent tiles, in pixels.
        /// </summary>
        public int Spacing;
        /// <summary>
        /// Distance from the image's border to the first tile data, in pixels.
        /// </summary>
        public int Margin;
        /// <summary>
        /// The number of tiles in this tileset
        /// </summary>
        public int TileCount;
        /// <summary>
        /// Terrain definitions
        /// </summary>
        public TsxTerrainContent[] Terrains;
        /// <summary>
        /// Tile definintions
        /// </summary>
        public TsxTileContent[] Tiles;
        /// <summary>
        /// Contains any custom properties set for this node.
        /// </summary>
        public TxxProperties Properties;
    }
}
