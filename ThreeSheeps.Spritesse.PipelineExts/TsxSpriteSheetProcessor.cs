using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using ThreeSheeps.Tiled;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    /// <summary>
    /// Handles sprite sheets.
    /// </summary>
    [ContentProcessor(DisplayName = "TSX -> Sprite Sheet Processor - ThreeSheeps")]
    public class TsxSpriteSheetProcessor : ContentProcessor<TsxTileSetContent, SpriteSheetContent>
    {
        public override SpriteSheetContent Process(TsxTileSetContent input, ContentProcessorContext context)
        {
            SpriteSheetContent output = new SpriteSheetContent();

            output.Texture = input.Image.Image;
            output.TextureObject = context.BuildAndLoadAsset<TextureContent, TextureContent>(output.Texture, "TextureProcessor");

            List<SpriteDefinition> defs = new List<SpriteDefinition>();
            int strideX = input.TileSize.X + input.Spacing;
            int strideY = input.TileSize.Y + input.Spacing;
            int width = input.Image.ImageSize.X - strideX;
            int height = input.Image.ImageSize.Y - strideY;
            for (int y = input.Margin; y < height; y += strideY)
            {
                for (int x = input.Margin; x < width; x += strideX)
                {
                    SpriteDefinition def = new SpriteDefinition();
                    def.SourceRectangle.X = x;
                    def.SourceRectangle.Y = y;
                    def.SourceRectangle.Width = input.TileSize.X;
                    def.SourceRectangle.Height = input.TileSize.Y;
                    defs.Add(def);
                }
            }
            output.Definitions = defs.ToArray();

            return output;
        }
    }
}
