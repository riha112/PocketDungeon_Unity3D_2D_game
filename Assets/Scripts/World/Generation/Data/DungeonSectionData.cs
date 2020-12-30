using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.World.Generation.Data
{
    public class DungeonSectionData
    {
        public int Width;
        public int Height;

        public int RoomCount;
        public (int min, int max) RoomSizeRange;
        public List<RoomData> Rooms;

        public int TileSetId = 0;

        public TileData[,] DungeonGrid;

        public DungeonSectionData(int width, int height)
        {
            Width = width;
            Height = height;
            DungeonGrid = new TileData[Width, Height];
            for(var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                    DungeonGrid[x, y] = new TileData();
                
        }

        public TileData GetTileByCoords(Vector2 point)
        {
            var x = Mathf.RoundToInt(point.x * 1.25f + Width / 2.0f);
            var y = Mathf.RoundToInt(point.y * 1.25f + Height / 2.0f);

            if (x >= 0 && y >= 0 && x < Width && y < Height)
                return DungeonGrid[x, y];
            return null;
        }
    }
}
