using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    public sealed class SpriteDefinition
    {
        public Rectangle SourceRectangle;

        public Point PivotOffset;
    }

    public sealed class SpriteSheetContent
    {
        public ExternalReference<TextureContent> Texture;

        public SpriteDefinition[] Definitions;

        internal TextureContent TextureObject;
    }
}
