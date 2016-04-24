using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline;
using ThreeSheeps.Tiled;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    [ContentProcessor(DisplayName = "TMX -> Location Processor - ThreeSheeps")]
    public class TmxLocationProcessor : ContentProcessor<TmxMapContent, LocationContent>
    {
        public override LocationContent Process(TmxMapContent input, ContentProcessorContext context)
        {
            LocationContent content = new LocationContent();

            List<ExternalReference<SpriteSheetContent>> spriteSheets = BuildSpriteSheetRefs(input, context);
            content.SpriteSheetRefs = spriteSheets;

            List<TileMapContent> bgLayers = new List<TileMapContent>();
            List<TileMapContent> fgLayers = new List<TileMapContent>();
            List<TileMapContent> layers = bgLayers;
            foreach (TxxLayerContent layer in input.Layers)
            {
                if (layer is TmxTileLayerContent)
                {
                    TmxTileLayerContent tileLayer = layer as TmxTileLayerContent;
                    TileMapContent layerContent = BuildTileMap(input, tileLayer);
                    layers.Add(layerContent);
                }
                else if (layer is TxxObjectGroupContent)
                {
                    // Switch to foreground layers now
                    layers = fgLayers;
                    // TODO: handle objects.
                }
            }
            content.BackgroundLayers = bgLayers;
            content.ForegroundLayers = fgLayers;

            return content;
        }

        private List<ExternalReference<SpriteSheetContent>> BuildSpriteSheetRefs(TmxMapContent input, ContentProcessorContext context)
        {
            // https://social.msdn.microsoft.com/Forums/en-US/f2aac788-6aa0-496a-a41b-108a6f23d637/dependent-content-builds-with-custom-contentprocessor?forum=xnagamestudioexpress

            List<ExternalReference<SpriteSheetContent>> spriteSheets = new List<ExternalReference<SpriteSheetContent>>();
            foreach (TmxTileSetReference reference in input.TileSets)
            {
                TmxExternalTileSet castReference = reference as TmxExternalTileSet;
                if (castReference == null)
                {
                    throw new InvalidContentException("a location can only contain external tileset references");
                }
                ExternalReference<SpriteSheetContent> ssRef = context.BuildAsset<TsxTileSetContent, SpriteSheetContent>(castReference.Source, "TsxSpriteSheetProcessor");
                // Replace the run-time content type to match.
                // TODO: Is this the correct way?..
                //ExternalReference<SpriteSheet> fixedRef = new ExternalReference<SpriteSheet>(ssRef.Filename);
                spriteSheets.Add(ssRef);
            }
            return spriteSheets;
        }

        private TileMapContent BuildTileMap(TmxMapContent input, TmxTileLayerContent layer)
        {
            TileMapContent content = new TileMapContent();

            content.TileSize = input.TileSize;
            content.TileRows = new TileRowContent[layer.Dimensions.Y];

            for (int rowIndex = 0; rowIndex < content.TileRows.Length; ++rowIndex)
            {
                TileRowContent row = new TileRowContent();
                row.Tiles = new TileContent[layer.Dimensions.X];
                for (int colIndex = 0; colIndex < row.Tiles.Length; ++colIndex)
                {
                    TmxTileContent sourceTile = layer.Tiles[colIndex, rowIndex];
                    if (sourceTile.TileId != -1)
                    {
                        TileContent tile = new TileContent();
                        tile.SheetIndex = sourceTile.TileSetId;
                        tile.SpriteIndex = sourceTile.TileId;
                        row.Tiles[colIndex] = tile;
                    }
                }
                content.TileRows[rowIndex] = row;
            }

            return content;
        }
    }
}
