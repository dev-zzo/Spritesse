using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    public sealed class TileContent
    {
        public int SheetIndex;
        public int SpriteIndex;
    }

    public sealed class TileRowContent
    {
        public TileContent[] Tiles;
    }

    public sealed class TileMapContent
    {
        public Point TileSize;

        [ContentSerializer(CollectionItemName = "Row")]
        public TileRowContent[] TileRows;
    }

    [ContentTypeSerializer]
    class TileRowSerializer : ContentTypeSerializer<TileRowContent>
    {
        protected override TileRowContent Deserialize(IntermediateReader input, ContentSerializerAttribute format, TileRowContent existingInstance)
        {
            List<TileContent> tiles = new List<TileContent>();
            string data = input.Xml.ReadString();
            string[] parts = data.Split(null as char[], StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in parts)
            {
                if (item == "x")
                {
                    tiles.Add(null);
                    continue;
                }
                int separatorIndex = item.IndexOf(':');
                if (separatorIndex != -1)
                {
                    TileContent tile = new TileContent();
                    tile.SheetIndex = Byte.Parse(item.Substring(0, separatorIndex));
                    tile.SpriteIndex = UInt16.Parse(item.Substring(separatorIndex + 1));
                    tiles.Add(tile);
                    continue;
                }
                throw new InvalidContentException(String.Format("cannot parse tile row element: '{0}'", item));
            }

            TileRowContent row = new TileRowContent();
            row.Tiles = tiles.ToArray();
            return row;
        }

        protected override void Serialize(IntermediateWriter output, TileRowContent value, ContentSerializerAttribute format)
        {
            StringBuilder sb = new StringBuilder();
            foreach (TileContent t in value.Tiles)
            {
                if (t != null)
                {
                    sb.AppendFormat("{0}:{1} ", t.SheetIndex, t.SpriteIndex);
                }
                else
                {
                    sb.Append("x ");
                }
            }
            if (sb.Length > 0 && sb[sb.Length - 1] == ' ')
            {
                sb.Length -= 1;
            }
            output.Xml.WriteString(sb.ToString());
        }
    }
}
