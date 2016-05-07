﻿using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace ThreeSheeps.Tiled
{
    /// <summary>
    /// Imports .tsx files generated by Tiled.
    /// </summary>
    [ContentImporter(".tsx", DisplayName="Tiled Tile Set Importer - ThreeSheeps")]
    public sealed class TsxImporter : ContentImporter<TsxTileSetContent>
    {
        public TsxImporter()
            : base()
        {
        }

        public override TsxTileSetContent Import(string filename, ContentImporterContext context)
        {
            TsxTileSetContent content = new TsxTileSetContent();
            this.tileset = content;
            content.Identity = new ContentIdentity(filename);
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            return this.ImportTileSet(doc["tileset"], context);
        }

        public TsxTileSetContent Import(XmlNode root, ContentImporterContext context)
        {
            TsxTileSetContent content = new TsxTileSetContent();
            this.tileset = content;
            content.Identity = new ContentIdentity();
            return this.ImportTileSet(root, context);
        }

        private TsxTileSetContent ImportTileSet(XmlNode root, ContentImporterContext context)
        {
            TsxTileSetContent content = this.tileset;
            content.Properties = TxxProperties.FromParentXml(root);
            content.Name = root.GetStringAttribute("name");
            content.TileSize.X = root.ParseIntAttribute("tilewidth");
            content.TileSize.Y = root.ParseIntAttribute("tileheight");
            content.Spacing = root.ParseIntAttribute("spacing");
            content.Margin = root.ParseIntAttribute("margin");

            content.Image = this.ImportImageData(root["image"], context);
            int tileCount = root.ParseIntAttribute("tilecount");
            if (tileCount == 0)
            {
                // TODO: Calculate this somehow.
                throw new InvalidContentException("tile count is missing");
            }
            content.TileCount = tileCount;

            // Fetch terrain types
            List<TsxTerrainContent> terrains = new List<TsxTerrainContent>();
            XmlNode terrainTypes = root["terraintypes"];
            if (terrainTypes != null)
            {
                foreach (XmlNode node in terrainTypes.SelectNodes("terrain"))
                {
                    TsxTerrainContent terrain = new TsxTerrainContent();
                    terrain.Name = root.GetStringAttribute("name");
                    terrain.TileId = root.ParseIntAttribute("tile");
                    terrain.Properties = TxxProperties.FromParentXml(node);
                    terrains.Add(terrain);
                }
            }
            content.Terrains = terrains.ToArray();

            // Create default tile descriptors
            TsxTileContent[] tiles = new TsxTileContent[tileCount];
            for (int tileIndex = 0; tileIndex < tileCount; ++tileIndex)
            {
                tiles[tileIndex] = new TsxTileContent();
            }

            // Apply customisations
            foreach (XmlNode node in root.SelectNodes("tile"))
            {
                int id = node.ParseIntAttribute("id", -1);
                if (id < tiles.GetLowerBound(0) || id > tiles.GetUpperBound(0))
                {
                    throw new InvalidContentException("a tile element with absent or incorrect id found");
                }
                TsxTileContent tile = tiles[id];
                tile.Probability = node.ParseFloatAttribute("probability");
                string terrain = node.GetStringAttribute("terrain");
                if (terrain != null)
                {
                    string[] terrainIds = terrain.Split(",".ToCharArray());
                    for (int terrainIdIndex = 0; terrainIdIndex < 4; ++terrainIdIndex)
                    {
                        if (!string.IsNullOrWhiteSpace(terrainIds[terrainIdIndex]))
                        {
                            int terrainId = int.Parse(terrainIds[terrainIdIndex]);
                            tile.Terrain[terrainIdIndex] = terrains[terrainId];
                        }
                    }
                }
                XmlNode objectsNode = node.SelectSingleNode("objectgroup");
                if (objectsNode != null)
                {
                    tile.Objects = TxxObjectGroupImportHelper.Import(objectsNode);
                }
            }

            return content;
        }

        private TsxImageContent ImportImageData(XmlNode root, ContentImporterContext context)
        {
            if (root == null)
            {
                throw new InvalidContentException("a tileset node must contain an image element");
            }

            TsxImageContent content = new TsxImageContent();

            content.ImageSize.X = root.ParseIntAttribute("width");
            content.ImageSize.Y = root.ParseIntAttribute("height");
            XmlAttribute sourceAttribute = root.Attributes["source"];
            if (sourceAttribute == null)
            {
                throw new InvalidContentException("a tileset image must have a source");
            }
            content.Image = new ExternalReference<TextureContent>(sourceAttribute.Value, this.tileset.Identity);

            return content;
        }

        private TsxTileSetContent tileset;
    }
}
