using System;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    [Serializable]
    public sealed class SpriteDefinition
    {
        public Rectangle SourceRectangle { get; set; }

        public Point PivotOffset { get; set; }
    }

    [Serializable]
    public sealed class SpriteSheetContent
    {
        public string TextureName { get; set; }

        public SpriteDefinition[] Definitions { get; set; }
    }
}
