using Microsoft.Xna.Framework.Content.Pipeline;

// http://www.windows-tech.info/5/e3e2848e6c622c37.php

namespace ThreeSheeps.Spritesse.PipelineExts
{
    /// <summary>
    /// Handles sprite sheets.
    /// </summary>
    [ContentProcessor(DisplayName = "Sprite Sheet Processor - ThreeSheeps")]
    public class SpriteSheetProcessor : ContentProcessor<SpriteSheet, SpriteSheet>
    {
        public override SpriteSheet Process(SpriteSheet input, ContentProcessorContext context)
        {
            // NULL processor
            return input;
        }
    }
}