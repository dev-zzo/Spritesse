using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    public class LocationContent
    {
        public List<ExternalReference<SpriteSheetContent>> SpriteSheetRefs;

        public int BackgroundLayerCount;

        public List<TileMapContent> Layers;
    }
}
