using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.Repository;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.World.Generation.DungeonSectionProcessors
{
    class AddMobsProcessor : IPipelineProcess<DungeonSectionData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 20000;

        public DungeonSectionData Translate(DungeonSectionData data)
        {
            for (var x = 0; x < data.Width; x++)
            {
                for (var y = data.Height - 1; y >= 0; y--)
                {
                    if(data.DungeonGrid[x, y].Type != TileType.Wall)
                        continue;

                    if (data.DungeonGrid[x, y].TileMapSectionTypeId == (int)WallType.TopRight)
                    {
                        AddBat(x - 1, y, data);
                        AddBat(x - 2, y, data);
                    }
                    else if (data.DungeonGrid[x, y].TileMapSectionTypeId == (int)WallType.TopLeft)
                    {
                        AddBat(x + 1, y, data);
                        AddBat(x + 2, y, data);
                    }
                }
            }


            return data;
        }

        private static void AddBat(int x, int y, DungeonSectionData data)
        {
            if (R.RandomRange(0, 10) <= 6) return;
            if(data.DungeonGrid[x,y].Type != TileType.Wall) return;


            var rX = x * 0.8f - data.Width * 0.4f;
            var rY = y * 0.8f - data.Height * 0.4f + 0.4f;

            var bat = Object.Instantiate(EnemyRepository.Repository[5].Prefabs[0]);
            bat.transform.position = new Vector2(rX, rY);
        }
    }
}
