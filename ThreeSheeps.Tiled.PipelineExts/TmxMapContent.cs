using System;
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

        public readonly List<TmxTileLayerContent> TileLayers = new List<TmxTileLayerContent>();

        public readonly List<TxxObjectGroupContent> ObjectLayers = new List<TxxObjectGroupContent>();

        /// <summary>
        /// Contains any custom properties set for this node.
        /// </summary>
        public TxxProperties Properties;

        public void TranslateGid(int gid, out TmxTileSetReference tileSet, out int tileIndex)
        {
            throw new IndexOutOfRangeException("gid is outside the expected range");
        }
    }
}
