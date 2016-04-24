using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using ThreeSheeps.Spritesse.Content;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    [ContentTypeWriter]
    public class LocationWriter : ContentTypeWriter<LocationContent>
    {
        protected override void Write(ContentWriter output, LocationContent value)
        {
            output.WriteObject<List<ExternalReference<SpriteSheetContent>>>(value.SpriteSheetRefs);
            Write(output, value.BackgroundLayers);
            Write(output, value.ForegroundLayers);
        }

        private void Write(ContentWriter output, List<TileMapContent> value)
        {
            output.Write((byte)value.Count);
            foreach (TileMapContent layer in value)
            {
                this.Write(output, layer);
            }
        }

        private void Write(ContentWriter output, TileMapContent value)
        {
            output.Write(value.TileSize.X);
            output.Write(value.TileSize.Y);
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
            return typeof(LocationReader).AssemblyQualifiedName;
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(Location).AssemblyQualifiedName;
        }
    }
}
