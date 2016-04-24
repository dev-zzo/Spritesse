using System;
using System.IO;
using System.IO.Compression;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline;

// http://doc.mapeditor.org/reference/tmx-map-format/

namespace ThreeSheeps.Tiled
{
    [ContentImporter(".tmx", DisplayName = "Tiled Tile Map Importer - ThreeSheeps")]
    public sealed class TmxImporter : ContentImporter<TmxMapContent>
    {
        public override TmxMapContent Import(string filename, ContentImporterContext context)
        {
            this.sourcePath = Path.GetFullPath(filename);
            XmlDocument doc = new XmlDocument();
            doc.Load(this.sourcePath);
            return ImportMapContent(doc["map"], context);
        }

        private TmxMapContent ImportMapContent(XmlNode root, ContentImporterContext context)
        {
            if (root.Attributes["version"].Value != "1.0")
            {
                throw new InvalidContentException("only map data version 1.0 is supported");
            }

            TmxMapContent map = new TmxMapContent();
            this.map = map;
            map.Orientation = (TmxMapOrientation)Enum.Parse(typeof(TmxMapOrientation), root.Attributes["orientation"].Value, true);
            map.MapSize.X = root.ParseIntAttribute("width");
            map.MapSize.Y = root.ParseIntAttribute("height");
            map.TileSize.X = root.ParseIntAttribute("tilewidth");
            map.TileSize.Y = root.ParseIntAttribute("tileheight");
            map.Properties = TxxProperties.FromParentXml(root);

            foreach (XmlNode node in root.SelectNodes("tileset"))
            {
                TmxTileSetReference data = ImportTileSetContent(node, context);
                map.TileSets.Add(data);
            }
            if (map.TileSets.Count == 0)
            {
                throw new InvalidContentException("no tilesets referenced");
            }

            foreach (XmlNode node in root.SelectNodes("layer|objectgroup|imagelayer"))
            {
                if (node.Name == "layer")
                {
                    map.Layers.Add(ImportLayerContent(node));
                }
                else if (node.Name == "objectgroup")
                {
                    map.Layers.Add(TxxObjectGroupImportHelper.Import(node));
                }
                else
                {
                    // TODO: implement imagelayer handling.
                }
            }

            return map;
        }

        private TmxTileSetReference ImportTileSetContent(XmlNode root, ContentImporterContext context)
        {
            // Tileset information is not required during the import stage.
            // Avoid importing unrelated tileset data.
            // Tilesets should be handled as external resources.
            TmxTileSetReference reference;
            XmlAttribute sourceAttribute = root.Attributes["source"];
            // Load from disk?
            if (sourceAttribute != null)
            {
                TmxExternalTileSet externalReference = new TmxExternalTileSet();
                reference = externalReference;
                string path = sourceAttribute.Value;
                if (!Path.IsPathRooted(path))
                {
                    path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(this.sourcePath), path));
                }
                context.AddDependency(path);
                externalReference.Source = new ExternalReference<TsxTileSetContent>(path);
            }
            else
            {
                TmxEmbeddedTileSet embeddedReference = new TmxEmbeddedTileSet();
                reference = embeddedReference;
                TsxImporter importer = new TsxImporter();
                embeddedReference.TileSet = importer.Import(root, context);
            }
            reference.FirstGid = int.Parse(root.Attributes["firstgid"].Value);
            return reference;
        }

        private TmxTileLayerContent ImportLayerContent(XmlNode root)
        {
            TmxTileLayerContent layer = new TmxTileLayerContent();
            layer.Name = root.Attributes["name"].Value;
            layer.Dimensions.X = root.ParseIntAttribute("width");
            layer.Dimensions.Y = root.ParseIntAttribute("height");
            layer.Tiles = new TmxTileContent[layer.Dimensions.X, layer.Dimensions.Y];
            layer.Opacity = root.ParseFloatAttribute("opacity", 1.0f);
            layer.Visible = root.ParseBoolAttribute("visible", true);
            layer.RenderingOffset.X = root.ParseIntAttribute("offsetx");
            layer.RenderingOffset.Y = root.ParseIntAttribute("offsety");
            layer.Properties = TxxProperties.FromParentXml(root);

            // Read the tile data into a gid array
            XmlNode dataNode = root["data"];
            if (dataNode == null)
            {
                throw new InvalidContentException("a layer node must contain a data element");
            }
            int gidCount = layer.Dimensions.X * layer.Dimensions.Y;
            uint[] gids = new uint[gidCount];
            ImportGids(gidCount, gids, dataNode);

            // Convert data from gids to tileset:index pairs
            int i = 0;
            for (int y = 0; y < layer.Dimensions.Y; ++y)
            {
                for (int x = 0; x < layer.Dimensions.X; ++x)
                {
                    uint gid = gids[i++];
                    TmxTileContent tileData = new TmxTileContent();
                    tileData.FlippedDiagonally = (gid & FLIPPED_DIAGONALLY) != 0;
                    tileData.FlippedVertically = (gid & FLIPPED_VERTICALLY) != 0;
                    tileData.FlippedHorizontally = (gid & FLIPPED_HORIZONTALLY) != 0;
                    int tid = (int)(gid & 0x0FFFFFFF);
                    if (tid > 0)
                    {
                        TmxTileSetReference tileSetToUse = null;
                        tileSetToUse = this.map.TileSets[0];
                        for (int tileSetIndex = this.map.TileSets.Count - 1; tileSetIndex >= 0; --tileSetIndex)
                        {
                            TmxTileSetReference tileSet = this.map.TileSets[tileSetIndex];
                            if (tid >= tileSet.FirstGid)
                            {
                                tileData.TileSetId = tileSetIndex;
                                tileData.TileId = tid - tileSet.FirstGid;
                                break;
                            }
                        }
                    }
                    else
                    {
                        tileData.TileSetId = -1;
                        tileData.TileId = -1;
                    }
                    layer.Tiles[x, y] = tileData;
                }
            }
            return layer;
        }

        private static void ImportGids(int gidCount, uint[] gids, XmlNode dataNode)
        {
            string encoding = dataNode.GetStringAttribute("encoding");
            if (encoding == null)
            {
                // plain XML
                int nodeCount = dataNode.ChildNodes.Count;
                if (nodeCount != gidCount)
                {
                    throw new InvalidContentException(String.Format("unexpected gid count in XML (expected {0}, got {1})", gidCount, nodeCount));
                }
                int i = 0;
                foreach (XmlNode node in dataNode.ChildNodes)
                {
                    if (node.Name != "tile")
                    {
                        throw new InvalidContentException("only tile nodes allowed in data");
                    }
                    gids[i++] = node.ParseUIntAttribute("gid");
                }
            }
            else if (encoding == "base64")
            {
                byte[] data = Convert.FromBase64String(dataNode.InnerText);
                int byteCount = sizeof(UInt32) * gidCount;
                string compression = dataNode.GetStringAttribute("compression");
                if (compression == null)
                {
                    // Not compressed.
                }
                else if (compression == "zlib")
                {
                    byte[] decompressedData = new byte[byteCount];
                    using (MemoryStream stream = new MemoryStream(data))
                    {
                        stream.ReadByte(); stream.ReadByte();
                        using (DeflateStream decomp = new DeflateStream(stream, CompressionMode.Decompress))
                        {
                            if (decomp.Read(decompressedData, 0, byteCount) != byteCount)
                            {
                                throw new InvalidContentException("decompression produced less bytes than expected");
                            }
                        }
                    }
                    data = decompressedData;
                }
                else if (compression == "gzip")
                {
                    byte[] decompressedData = new byte[byteCount];
                    using (MemoryStream stream = new MemoryStream(data))
                    {
                        using (GZipStream decomp = new GZipStream(stream, CompressionMode.Decompress))
                        {
                            if (decomp.Read(decompressedData, 0, byteCount) != byteCount)
                            {
                                throw new InvalidContentException("decompression produced less bytes than expected");
                            }
                        }
                    }
                    data = decompressedData;
                }
                else
                {
                    throw new InvalidContentException(String.Format("compression {0} not supported", compression));
                }
                for (int i = 0; i < gidCount; ++i)
                {
                    gids[i] = BitConverter.ToUInt32(data, i * sizeof(UInt32));
                }
            }
            else if (encoding == "csv")
            {
                // plain CSV
                string[] parts = dataNode.InnerText.Split(",\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != gidCount)
                {
                    throw new InvalidContentException(String.Format("unexpected gid count in CSV (expected {0}, got {1})", gidCount, parts.Length));
                }
                for (int i = 0; i < gidCount; ++i)
                {
                    gids[i] = uint.Parse(parts[i]);
                }
            }
            else
            {
                throw new InvalidContentException(String.Format("encoding {0} not supported", encoding));
            }
        }

        private const uint FLIPPED_HORIZONTALLY = 0x80000000;
        private const uint FLIPPED_VERTICALLY = 0x40000000;
        private const uint FLIPPED_DIAGONALLY = 0x20000000;

        private string sourcePath;
        private TmxMapContent map;
    }
}
