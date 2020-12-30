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
    class AddTrapsProcessor : IPipelineProcess<DungeonSectionData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 20020;

        public DungeonSectionData Translate(DungeonSectionData data)
        {
            var resourceCount = (int)Mathf.Sqrt(data.Width * data.Height) / 3;
            Debug.Log(resourceCount);
            for (var i = 0; i < resourceCount; i++)
            {
                PlaceTraps(
                    R.RandomRange(0, data.Width),
                    R.RandomRange(0, data.Height),
                    R.RandomRange(0, 2),
                    data
                );
            }

            return data;
        }

        private static void PlaceTraps(int rx, int ry, int radius, DungeonSectionData data)
        {
            for (var x = rx - radius; x <= rx + radius && x < data.Width; x++)
            {
                if (x < 0) continue;
                for (var y = ry - radius; y <= ry + radius && y < data.Height; y++)
                {
                    if (y < 0) continue;
                    if(R.RandomRange(0, 10) <= 5) continue;
                    if (data.DungeonGrid[x, y].Type != TileType.Floor || !(data.DungeonGrid[x, y].Child is null)) continue;

                    data.DungeonGrid[x, y].Child = PrefabRepository.Repository[5].Prefab;
                }
            }
        }
    }
}
