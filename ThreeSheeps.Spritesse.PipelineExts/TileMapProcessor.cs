using Microsoft.Xna.Framework.Content.Pipeline;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    /// <summary>
    /// Handles animation sets.
    /// </summary>
    [ContentProcessor(DisplayName = "Tile Map Processor - ThreeSheeps")]
    public class TileMapProcessor : ContentProcessor<TileMapContent, TileMapContent>
    {
        public override TileMapContent Process(TileMapContent input, ContentProcessorContext context)
        {
            // Validate sprite sheet references
            if (input.SpriteSheets.Length >= byte.MaxValue)
            {
                throw new InvalidContentException("too many sprite sheets referenced (max: 254)");
            }
            
            // Validate rows
            int width = input.TileRows[0].Tiles.Length;
            for (int i = 1; i < input.TileRows.Length; ++i)
            {
                if (input.TileRows[i].Tiles.Length != width)
                {
                    throw new InvalidContentException(string.Format("row {0} has incorrect length", i));
                }
            }
            // No actual changes needed
            return input;
        }
    }
}