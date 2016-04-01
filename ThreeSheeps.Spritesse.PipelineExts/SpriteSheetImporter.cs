using Microsoft.Xna.Framework.Content.Pipeline;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    [ContentImporter(".spritesheet", DefaultProcessor = "SpriteSheetProcessor", DisplayName = "Sprite Sheet Importer - ThreeSheeps")]
    public sealed class SpriteSheetImporter : XmlImporter
    {
    }
}
