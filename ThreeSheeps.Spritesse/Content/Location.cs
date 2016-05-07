using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ThreeSheeps.Spritesse.Physics;

namespace ThreeSheeps.Spritesse.Content
{
    /// <summary>
    /// Represents a game location.
    /// </summary>
    public class Location
    {
        public Location(
            List<SpriteSheet> spriteSheets,
            List<TileMap> bgTileMaps,
            List<TileMap> fgTileMaps,
            List<PhysicalShape.CreationInfo> staticShapes)
        {
            this.spriteSheets = spriteSheets;
            this.bgTileMaps = bgTileMaps;
            this.fgTileMaps = fgTileMaps;
            this.staticShapes = staticShapes;
        }

        public IList<SpriteSheet> SpriteSheets { get { return this.spriteSheets; } }

        public IList<TileMap> BackgroundLayers { get { return this.bgTileMaps; } }
        public IList<TileMap> ForegroundLayers { get { return this.fgTileMaps; } }

        public IList<PhysicalShape.CreationInfo> StaticShapes { get { return this.staticShapes; } }

        private List<SpriteSheet> spriteSheets;
        private List<TileMap> bgTileMaps;
        private List<TileMap> fgTileMaps;
        private List<PhysicalShape.CreationInfo> staticShapes;
    }

    public sealed class LocationReader : ContentTypeReader<Location>
    {
        protected override Location Read(ContentReader input, Location existingInstance)
        {
            List<SpriteSheet> spriteSheets = input.ReadObject<List<SpriteSheet>>();
            List<TileMap> bgLayers = new List<TileMap>();
            for (int layerCount = input.ReadByte(); layerCount > 0; layerCount--)
            {
                bgLayers.Add(ReadLayer(input, spriteSheets));
            }
            List<TileMap> fgLayers = new List<TileMap>();
            for (int layerCount = input.ReadByte(); layerCount > 0; layerCount--)
            {
                fgLayers.Add(ReadLayer(input, spriteSheets));
            }
            List<PhysicalShape.CreationInfo> staticShapes = new List<PhysicalShape.CreationInfo>();
            for (int shapeCount = input.ReadInt32(); shapeCount > 0; shapeCount--)
            {
                staticShapes.Add(ReadShape(input));
            }
            return new Location(spriteSheets, bgLayers, fgLayers, staticShapes);
        }

        private static TileMap ReadLayer(ContentReader input, List<SpriteSheet> spriteSheets)
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

        private static PhysicalShape.CreationInfo ReadShape(ContentReader input)
        {
            PhysicalShape.CreationInfo info;
            char typeMark = (char)input.ReadByte();
            switch (typeMark)
            {
                case 'C':
                    info = ReadCircle(input);
                    break;
                case 'R':
                    info = ReadRectangle(input);
                    break;
                default:
                    throw new ContentLoadException("unexpected shape type");
            }
            info.ReceiveCollisions = false;
            info.SendCollisions = true;
            return info;
        }

        private static PhysicalShape.CreationInfo ReadCircle(ContentReader input)
        {
            PhysicalCircle.CreationInfo info = new PhysicalCircle.CreationInfo();
            info.Position = input.ReadVector2();
            info.Radius = input.ReadSingle();
            return info;
        }

        private static PhysicalShape.CreationInfo ReadRectangle(ContentReader input)
        {
            PhysicalAxisAlignedBox.CreationInfo info = new PhysicalAxisAlignedBox.CreationInfo();
            info.Position = input.ReadVector2();
            info.Dimensions = input.ReadVector2();
            return info;
        }
    }
}
