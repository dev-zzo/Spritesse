using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace ThreeSheeps.Tiled
{
    public enum TmxMapOrientation
    {
        Orthogonal,
        Isometric,
        Hexagonal,
    }

    public sealed class TmxMapContent : ContentItem
    {
        public TmxMapOrientation Orientation;
        /// <summary>
        /// Map dimensions, in tiles.
        /// </summary>
        public Point MapSize;
        /// <summary>
        /// Dimensions of each tile on the map, in pixels.
        /// </summary>
        public Point TileSize;
        /// <summary>
        /// References to associated tilesets.
        /// </summary>
        public readonly List<TmxTileSetReference> TileSets = new List<TmxTileSetReference>();
        /// <summary>
        /// Layer stack.
        /// </summary>
        public readonly List<TxxLayerContent> Layers = new List<TxxLayerContent>();
        /// <summary>
        /// Contains any custom properties set for this node.
        /// </summary>
        public TxxProperties Properties;
    }
}
