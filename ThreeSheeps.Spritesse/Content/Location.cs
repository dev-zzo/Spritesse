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
        public Location(List<SpriteSheet> spriteSheets, List<TileMap> tileMaps)
        {
            this.spriteSheets = spriteSheets;
            this.tileMaps = tileMaps;
        }

        public IList<SpriteSheet> SpriteSheets { get { return this.spriteSheets; } }

        public int BackgroundLayersCount { get; private set; }

        public IList<TileMap> Layers { get { return this.tileMaps; } }

        private List<SpriteSheet> spriteSheets;
        private List<TileMap> tileMaps;
    }

    public sealed class LocationReader : ContentTypeReader<Location>
    {
        protected override Location Read(ContentReader input, Location existingInstance)
        {
            List<SpriteSheet> spriteSheets = input.ReadObject<List<SpriteSheet>>();
            byte backgroundLayersCount = input.ReadByte();
            byte layerCount = input.ReadByte();
            List<TileMap> layers = new List<TileMap>();
            while (layerCount-- > 0)
            {
                layers.Add(this.ReadLayer(input, spriteSheets));
            }

            return new Location(spriteSheets, layers);
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
