using System;
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
            Write(output, value.StaticGeometry);
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

        private void Write(ContentWriter output, List<StaticGeometryContent> value)
        {
            output.Write(value.Count);
            foreach (StaticGeometryContent item in value)
            {
                Write(output, item);
            }
        }

        private void Write(ContentWriter output, StaticGeometryContent value)
        {
            StaticCircleContent maybeCircle = value as StaticCircleContent;
            StaticRectangleContent maybeRect = value as StaticRectangleContent;
            if (maybeCircle != null)
            {
                output.Write((byte)'C');
                output.Write(value.Position);
                output.Write(maybeCircle.Radius);
            }
            else if (maybeRect != null)
            {
                output.Write((byte)'R');
                output.Write(value.Position);
                output.Write(maybeRect.Dimensions);
            }
            else
            {
                throw new NotImplementedException();
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
