using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;

namespace Assets.Scripts.World.Generation.DungeonSectionProcessors
{
    public class SetFloorTypeProcessor : IPipelineProcess<DungeonSectionData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 700;

        private int _width;
        private int _height;
        private bool[,] _heightMap;

        private static readonly Dictionary<int, int> Translator = new Dictionary<int, int>()
        {
            { 3, (int)WallType.Top },
            { 1, (int)WallType.Left },
            { 2, (int)WallType.Right },
            { 8, (int)WallType.Top },
            { 4, (int)WallType.Bottom },
            { 12, (int)WallType.Right },
            { 5, (int)WallType.TopRight },
            { 15, (int)WallType.All },
            { 10, (int)WallType.BottomLeft },
            { 9, (int)WallType.TopLeft },
            { 6, (int)WallType.BottomRight }
        };

        public DungeonSectionData Translate(DungeonSectionData data)
        {
            _width = data.Width;
            _height = data.Height;
            _heightMap = new bool[_width, _height];

            RandomFillMap();
            for (var i = 0; i < R.RandomRange(2, 4); i++)
                SmoothenHeightMap();


            for (var x = 0; x < data.Width; x++)
            {
                for (var y = 0; y < data.Height; y++)
                {
                    // Not floor
                    if (data.DungeonGrid[x, y].Type != TileType.Floor)
                        continue;

                    // Already modified
                    if (data.DungeonGrid[x, y].TileMapSectionTypeId != 0)
                        continue;

                    data.DungeonGrid[x, y].TileMapSectionTypeId = GetTileMapSectionTypeId(x, y, data);
                }
            }

            return data;
        }

        private int GetTileMapSectionTypeId(int x, int y, DungeonSectionData data)
        {
            var refVal = _heightMap[x, y];

            var id = 0;
            if (x - 1 < 0 || _heightMap[x - 1, y] == refVal) id += 1;
            if (x + 1 >= _width || _heightMap[x + 1, y] == refVal) id += 2;
            if (y - 1 < 0 || _heightMap[x, y - 1] == refVal) id += 4;
            if (y + 1 >= _height || _heightMap[x, y + 1] == refVal) id += 8;

            return Translator.ContainsKey(id) ? Translator[id] : 0;
        }

        private void RandomFillMap()
        {
            var fillPercent = R.RandomRange(45, 65);

            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    // Room walls
                    if (x == 0 || y == 0 || x == _width - 1 || y == _height - 1)
                    {
                        _heightMap[x, y] = true;
                        continue;
                    }

                    _heightMap[x, y] = R.RandomRange(0, 100) < fillPercent;
                }
            }
        }

        private void SmoothenHeightMap()
        {
            var soothRoom = new bool[_width, _height];
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var neighboringWalls = GetNeighborWallCount(x, y);
                    soothRoom[x, y] = neighboringWalls > 4;
                }
            }

            _heightMap = soothRoom;
        }

        private int GetNeighborWallCount(int posX, int posY)
        {
            var counter = 0;
            for (var x = posX - 1; x < posX + 2; x++)
            {
                for (var y = posY - 1; y < posY + 2; y++)
                {
                    if (x >= 0 && x < _width && y >= 0 && y < _height)
                    {
                        if (x == posX && y == posY)
                            continue;
                        if (_heightMap[x, y])
                            counter++;
                    }
                    else
                    {
                        counter++;
                    }

                }
            }
            return counter;
        }
    }
}
