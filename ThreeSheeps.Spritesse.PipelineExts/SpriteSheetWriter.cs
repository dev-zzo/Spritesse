using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using ThreeSheeps.Spritesse.Content;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    [ContentTypeWriter]
    public class SpriteSheetWriter : ContentTypeWriter<SpriteSheetContent>
    {
        protected override void Write(ContentWriter output, SpriteSheetContent value)
        {
            output.Write(value.TextureName);
            output.Write(value.Definitions.Length);
            foreach (SpriteDefinition def in value.Definitions)
            {
                output.Write((short)def.SourceRectangle.X);
                output.Write((short)def.SourceRectangle.Y);
                output.Write((short)def.SourceRectangle.Width);
                output.Write((short)def.SourceRectangle.Height);

                output.Write((short)def.PivotOffset.X);
                output.Write((short)def.PivotOffset.Y);
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SpriteSheetReader).AssemblyQualifiedName;
        }
    }
}
