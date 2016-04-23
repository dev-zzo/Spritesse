using Microsoft.Xna.Framework.Content.Pipeline;

namespace ThreeSheeps.Tiled
{
    /// <summary>
    /// This is used to refer to tilesets
    /// </summary>
    public class TmxTileSetReference
    {
        /// <summary>
        /// Offset to be added to each local tile ID in the tileset.
        /// </summary>
        public int FirstGid;
    }

    public class TmxExternalTileSet : TmxTileSetReference
    {
        /// <summary>
        /// If referring to an external tile set, the path to the source .tsx.
        /// </summary>
        public ExternalReference<TsxTileSetContent> Source;
    }

    public class TmxEmbeddedTileSet : TmxTileSetReference
    {
        /// <summary>
        /// Tileset object reference.
        /// </summary>
        public TsxTileSetContent TileSet;
    }
}
