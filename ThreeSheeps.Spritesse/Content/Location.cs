using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ThreeSheeps.Spritesse.Content
{
    /// <summary>
    /// Represents a game location.
    /// </summary>
    public class Location
    {
        public Location(List<SpriteSheet> spriteSheets, List<TileMap> bgTileMaps, List<TileMap> fgTileMaps)
        {
            this.spriteSheets = spriteSheets;
            this.bgTileMaps = bgTileMaps;
            this.fgTileMaps = fgTileMaps;
        }

        public IList<SpriteSheet> SpriteSheets { get { return this.spriteSheets; } }

        public IList<TileMap> BackgroundLayers { get { return this.bgTileMaps; } }
        public IList<TileMap> ForegroundLayers { get { return this.fgTileMaps; } }

        private List<SpriteSheet> spriteSheets;
        private List<TileMap> bgTileMaps;
        private List<TileMap> fgTileMaps;
    }

    public sealed class LocationReader : ContentTypeReader<Location>
    {
        protected override Location Read(ContentReader input, Location existingInstance)
        {
            List<SpriteSheet> spriteSheets = input.ReadObject<List<SpriteSheet>>();
            List<TileMap> bgLayers = new List<TileMap>();
            for (int layerCount = input.ReadByte(); layerCount > 0; layerCount--)
            {
                bgLayers.Add(this.ReadLayer(input, spriteSheets));
            }
            List<TileMap> fgLayers = new List<TileMap>();
            for (int layerCount = input.ReadByte(); layerCount > 0; layerCount--)
            {
                fgLayers.Add(this.ReadLayer(input, spriteSheets));
            }
            return new Location(spriteSheets, bgLayers, fgLayers);
        }

        private TileMap ReadLayer(ContentReader input, List<SpriteSheet> spriteSheets)
        {
            Point tileSize;
            tileSize.X = input.ReadInt32();
            tileSize.Y = input.ReadInt32();
            int rowCount = input.ReadInt32();
            int colCount = input.ReadInt32();
            Tile[,] tiles = new Tile[rowCount, colCount];
            for (int row = 0; row < rowCount; ++row)
            {
                for (int col = 0; col < colCount; ++col)
                {
                    byte sheetIndex = input.ReadByte();
                    if (sheetIndex != byte.MaxValue)
                    {
                        tiles[row, col].SheetIndex = sheetIndex;
                        tiles[row, col].SpriteIndex = input.ReadUInt16();
                    }
                    else
                    {
                        tiles[row, col].SheetIndex = Tile.EMPTY_TILE;
                        tiles[row, col].SpriteIndex = Tile.EMPTY_TILE;
                    }
                }
            }
            return new TileMap(tileSize, spriteSheets, tiles);
        }
    }
}
