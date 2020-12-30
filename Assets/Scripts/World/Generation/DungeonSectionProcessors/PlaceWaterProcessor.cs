using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.Repository;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;

namespace Assets.Scripts.World.Generation.DungeonSectionProcessors
{
    public class PlaceWaterProcessor : IPipelineProcess<DungeonSectionData>
    {
        public bool IsEnabled => true;
        public int PriorityId => 10000;

        private const float PERLIN_STRENGTH = 20;
        private static readonly Color WaterColor = new Color(0.67f, 0.67f, 0.67f, 0.5f);

        private int _offsetX;
        private int _offsetY;

        public DungeonSectionData Translate(DungeonSectionData data)
        {
            _offsetX = R.RandomRange(0, 1000);
            _offsetY = R.RandomRange(0, 1000);

            for (var x = 0; x < data.Width; x++)
            {
                for (var y = data.Height - 1; y >= 0; y--)
                {
                    if (data.DungeonGrid[x, y].Type == TileType.Wall &&
                        data.DungeonGrid[x, y].TileMapSectionTypeId == 0)
                    {
                        data.DungeonGrid[x, y].Sprite = TileMapRepository.Repository[data.TileSetId][1].TileTypes[99].GetRandomSprite();
                    }

                    if (data.DungeonGrid[x, y].Type == TileType.None || data.DungeonGrid[x, y].Type == TileType.Wall)
                        continue;

                    if (GetStrength(x, y, data) > 0.7f)
                    {
                        var id = data.DungeonGrid[x, y + 1].Type == TileType.Water ? 0 : 1;
                        data.DungeonGrid[x, y].Sprite = TileMapRepository.Repository[data.TileSetId][2].TileTypes[id].GetRandomSprite();
                        data.DungeonGrid[x, y].Type = TileType.Water;
                        data.DungeonGrid[x,y].Color = WaterColor;
                        data.DungeonGrid[x, y].Child = null;
                    }
                }
            }
            return data;
        }


        private float GetStrength(int x, int y, DungeonSectionData data)
        {
            var xCoordinate = (float)x / (data.Width * 6) * PERLIN_STRENGTH + _offsetX;
            var yCoordinate = (float)y / (data.Height * 6) * PERLIN_STRENGTH + _offsetY;
            var strength = Mathf.PerlinNoise(xCoordinate, yCoordinate);
            return strength;
        }
    }
}
