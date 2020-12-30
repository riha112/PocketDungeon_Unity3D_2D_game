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
    public class ApplyRoomTilesProcessor : IPipelineProcess<DungeonSectionData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 500;

        public DungeonSectionData Translate(DungeonSectionData data)
        {
            foreach (var room in data.Rooms)
            {
                for (var x = room.Left; x < room.Left + room.Width; x++)
                {
                    for (var y = room.Top; y < room.Top + room.Height; y++)
                    {
                     //   if(data.DungeonGrid[x, y].Type == TileType.None)
                            data.DungeonGrid[x, y] = room.Tiles[x - room.Left, y - room.Top];
                    }
                }
            }

            return data;
        }

    }
}
