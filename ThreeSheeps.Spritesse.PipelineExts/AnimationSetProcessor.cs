using Microsoft.Xna.Framework.Content.Pipeline;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    /// <summary>
    /// Handles animation sets.
    /// </summary>
    [ContentProcessor(DisplayName = "Animation Set Processor - ThreeSheeps")]
    public class AnimationSetProcessor : ContentProcessor<AnimationSetContent, AnimationSetContent>
    {
        public override AnimationSetContent Process(AnimationSetContent input, ContentProcessorContext context)
        {
            // NULL processor
            return input;
        }
    }
}