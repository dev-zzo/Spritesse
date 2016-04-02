using Microsoft.Xna.Framework.Content.Pipeline;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    [ContentImporter(".tilemap", DefaultProcessor = "TileMapProcessor", DisplayName = "Tile Map Importer - ThreeSheeps")]
    public sealed class TileMapImporter : XmlImporter
    {
    }
}
