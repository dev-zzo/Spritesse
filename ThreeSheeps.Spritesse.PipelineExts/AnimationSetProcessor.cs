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
            foreach (AnimationSequence seq in input.Animations)
            {
                uint totalLength = 0;
                foreach (AnimationFrame frame in seq.Frames)
                {
                    totalLength += frame.Delay;
                }
                seq.TotalLength = totalLength;
            }

            return input;
        }
    }
}