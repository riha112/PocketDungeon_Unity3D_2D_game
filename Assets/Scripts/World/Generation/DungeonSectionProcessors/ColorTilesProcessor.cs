using System;
using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;

namespace Assets.Scripts.World.Generation.DungeonProcessors
{
    public class ColorTilesProcessor : IPipelineProcess<DungeonSectionData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 600;

        private const float PERLIN_STRENGTH = 20;
        private const float COLOR_DELTA = 0.8f;

        private int _offsetX;
        private int _offsetY;

        public DungeonSectionData Translate(DungeonSectionData data)
        {
            _offsetX = R.RandomRange(0, 1000);
            _offsetY = R.RandomRange(0, 1000);

            for (var x = 0; x < data.Width; x++)
            {
                for (var y = 0; y < data.Height; y++)
                {
                    if (data.DungeonGrid[x, y].Type == TileType.None)
                        continue;

                   // if (data.DungeonGrid[x, y].TileMapSectionTypeId == 1)
                   //     continue;

                    if (data.DungeonGrid[x,y].Color == null) 
                        data.DungeonGrid[x, y].Color = GetTileColor(x, y, data);
                }
            }
            return data;
        }


        private Color GetTileColor(int x, int y, DungeonSectionData data)
        {
            var xCoordinate = (float)x / data.Width * PERLIN_STRENGTH + _offsetX;
            var yCoordinate = (float)y / data.Height * PERLIN_STRENGTH + _offsetY;
            var strength = Mathf.PerlinNoise(xCoordinate, yCoordinate) * COLOR_DELTA;

            return new Color(1 - strength, 1 - strength, 1 - strength, 1);
        }
    }
}
