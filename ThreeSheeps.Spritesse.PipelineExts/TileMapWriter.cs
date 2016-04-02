using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using ThreeSheeps.Spritesse.Content;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    [ContentTypeWriter]
    public class TileMapWriter : ContentTypeWriter<TileMapContent>
    {
        protected override void Write(ContentWriter output, TileMapContent value)
        {
            output.Write(value.TileSize.X);
            output.Write(value.TileSize.Y);
            output.Write((byte)value.SpriteSheets.Length);
            
            foreach (var sheet in value.SpriteSheets)
            {
                output.Write(sheet);
            }
            output.Write(value.TileRows.Length);
            output.Write(value.TileRows[0].Tiles.Length);
            foreach (TileRowContent row in value.TileRows)
            {
                foreach (TileContent tile in row.Tiles)
                {
                    if (tile != null)
                    {
                        output.Write((byte)tile.SheetIndex);
                        output.Write((ushort)tile.SpriteIndex);
                    }
                    else
                    {
                        output.Write((byte)0xFF);
                    }
                }
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(TileMapReader).AssemblyQualifiedName;
        }
    }
}
