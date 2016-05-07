using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline;
using ThreeSheeps.Tiled;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    [ContentProcessor(DisplayName = "TMX -> Location Processor - ThreeSheeps")]
    public class TmxLocationProcessor : ContentProcessor<TmxMapContent, LocationContent>
    {
        public override LocationContent Process(TmxMapContent input, ContentProcessorContext context)
        {
            LocationContent content = new LocationContent();

            List<ExternalReference<SpriteSheetContent>> spriteSheets = this.BuildSpriteSheetRefs(input, context);
            content.SpriteSheetRefs = spriteSheets;

            List<TileMapContent> layers = this.bgLayers;
            foreach (TxxLayerContent layer in input.Layers)
            {
                if (layer is TmxTileLayerContent)
                {
                    TmxTileLayerContent tileLayer = layer as TmxTileLayerContent;
                    TileMapContent layerContent = this.BuildTileMap(input, tileLayer);
                    layers.Add(layerContent);
                }
                else if (layer is TxxObjectGroupContent)
                {
                    // Switch to foreground layers now
                    layers = this.fgLayers;
                    
                    TxxObjectGroupContent objectLayer = layer as TxxObjectGroupContent;
                    foreach (TxxObjectContent obj in objectLayer.Objects)
                    {
                        this.ProcessObject(obj);
                    }
                }
            }
            content.BackgroundLayers = this.bgLayers;
            content.ForegroundLayers = this.fgLayers;
            content.StaticGeometry = this.staticGeometry;

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

        private void ProcessObject(TxxObjectContent obj)
        {
            string objectType = obj.Type;

            switch (objectType)
            {
                case "static":
                    this.ProcessStaticGeometry(obj);
                    break;

                default:
                    break;
            }
        }

        private void ProcessStaticGeometry(TxxObjectContent obj)
        {
            TxxEllipseObjectContent maybeEllipse = obj as TxxEllipseObjectContent;
            TxxRectangleObjectContent maybeRect = obj as TxxRectangleObjectContent;
            if (maybeEllipse != null)
            {
                if (maybeEllipse.Dimensions.X != maybeEllipse.Dimensions.Y)
                {
                    throw new InvalidContentException("only circles are supported as static geometry");
                }
                StaticCircleContent content = new StaticCircleContent();
                content.Radius = maybeEllipse.Dimensions.X * 0.5f;
                content.Position = new Vector2(
                    maybeEllipse.Position.X + content.Radius,
                    maybeEllipse.Position.Y + content.Radius);
                this.staticGeometry.Add(content);
            }
            else if (maybeRect != null)
            {
                StaticRectangleContent content = new StaticRectangleContent();
                content.Dimensions = new Vector2(maybeRect.Dimensions.X, maybeRect.Dimensions.Y);
                content.Position = new Vector2(maybeRect.Position.X, maybeRect.Position.Y) + content.Dimensions * 0.5f;
                this.staticGeometry.Add(content);
            }
            else
            {
                throw new InvalidContentException("static geometry other than circle or rectangle is not supported");
            }
        }

        private readonly List<TileMapContent> bgLayers = new List<TileMapContent>();
        private readonly List<TileMapContent> fgLayers = new List<TileMapContent>();
        private readonly List<StaticGeometryContent> staticGeometry = new List<StaticGeometryContent>();
    }
}
