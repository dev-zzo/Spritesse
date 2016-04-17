using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

// http://www.windows-tech.info/5/e3e2848e6c622c37.php

namespace ThreeSheeps.Spritesse.PipelineExts
{
    /// <summary>
    /// Handles sprite sheets.
    /// </summary>
    [ContentProcessor(DisplayName = "Sprite Sheet Processor - ThreeSheeps")]
    public class SpriteSheetProcessor : ContentProcessor<SpriteSheetContent, SpriteSheetContent>
    {
        public override SpriteSheetContent Process(SpriteSheetContent input, ContentProcessorContext context)
        {
            input.TextureObject = context.BuildAndLoadAsset<TextureContent, TextureContent>(input.Texture, "TextureProcessor");
            return input;
        }
    }
}