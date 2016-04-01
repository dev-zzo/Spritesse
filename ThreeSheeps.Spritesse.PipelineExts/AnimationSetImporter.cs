using Microsoft.Xna.Framework.Content.Pipeline;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    [ContentImporter(".animset", DefaultProcessor = "AnimationSetProcessor", DisplayName = "Animation Set Importer - ThreeSheeps")]
    public sealed class AnimationSetImporter : XmlImporter
    {
    }
}
