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
    class AddResourcesProcessor : IPipelineProcess<DungeonSectionData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 20010;

        public DungeonSectionData Translate(DungeonSectionData data)
        {
            var resourceCount = (int)Mathf.Sqrt(data.Width * data.Height) / 2;
            for (var i = 0; i < resourceCount; i++)
            {
                var resourceId = R.RandomRange(0, 100);
                if (resourceId < 50) resourceId = 6;
                else if (resourceId < 90) resourceId = 7;
                else if (resourceId < 99) resourceId = 6;
                else resourceId = 7;

                PlaceResources(
                    R.RandomRange(0, data.Width), 
                    R.RandomRange(0, data.Height),
                    resourceId,
                    R.RandomRange(0, 3), 
                    data
                );
            }

            return data;
        }

        private static void PlaceResources(int rx, int ry, int resourceId, int radius, DungeonSectionData data)
        {
            for (var x = rx - radius; x <= rx + radius && x < data.Width; x++)
            {
                if(x < 0) continue;
                for (var y = ry - radius; y <= ry + radius && y < data.Height; y++)
                {
                    if (y < 0) continue;
                    if (R.RandomRange(0, 10) <= 5) continue;
                    if (data.DungeonGrid[x,y].Type != TileType.Floor || !(data.DungeonGrid[x,y].Child is null)) continue;

                    data.DungeonGrid[x, y].Child = PrefabRepository.Repository[resourceId].Prefab;

                }
            }
        }
    }
}