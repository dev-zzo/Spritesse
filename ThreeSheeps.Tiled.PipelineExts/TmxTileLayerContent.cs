using Microsoft.Xna.Framework;

namespace ThreeSheeps.Tiled
{
    /// <summary>
    /// Describes a single tile within a layer.
    /// </summary>
    public struct TmxTileContent
    {
        /// <summary>
        /// Local tile ID; -1 means no tile present.
        /// </summary>
        public int TileId;
        /// <summary>
        /// Tile set to be used; -1 means no tile present.
        /// </summary>
        public int TileSetId;
        /// <summary>
        /// Whether to flip the source tile diagonally (swap X and Y).
        /// </summary>
        public bool FlippedDiagonally;
        /// <summary>
        /// Whether to flip the source tile vertically.
        /// </summary>
        public bool FlippedVertically;
        /// <summary>
        /// Whether to flip the source tile horizontally.
        /// </summary>
        public bool FlippedHorizontally;
    }

    /// <summary>
    /// Describes a tile layer.
    /// </summary>
    public sealed class TmxTileLayerContent
    {
        /// <summary>
        /// Object name.
        /// </summary>
        public string Name;
        /// <summary>
        /// Whether this layer is visible
        /// </summary>
        public bool Visible;
        /// <summary>
        /// Opacity of this layer
        /// </summary>
        public float Opacity;
        /// <summary>
        /// Rendering offset for this layer, in pixels.
        /// </summary>
        public Point RenderingOffset;
        /// <summary>
        /// The dimensions of the layer, in tiles.
        /// Traditionally required, but as of Tiled Qt always the same as the map width.
        /// </summary>
        public Point Dimensions;
        /// <summary>
        /// The 2D array containing the tile information. Coords = (x, y).
        /// </summary>
        public TmxTileContent[,] Tiles;
        /// <summary>
        /// Contains any custom properties set for this node.
        /// </summary>
        public TxxProperties Properties;
    }
}
