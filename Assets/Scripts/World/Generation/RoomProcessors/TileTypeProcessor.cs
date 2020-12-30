using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.World.Generation.Data;
using Color = UnityEngine.Color;
using TileData = Assets.Scripts.World.Generation.Data.TileData;

namespace Assets.Scripts.World.Generation.RoomProcessors
{
    public class TileTypeProcessor : IPipelineProcess<RoomData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 50;

        public RoomData Translate(RoomData room)
        {
            for (var x = 0; x < room.Width; x++)
            {
                for (var y = 0; y < room.Height; y++)
                {
                    room.Tiles[x, y] = new TileData
                    {
                        Type = (x == 0 || y == 0 || x == room.Width - 1 || y == room.Height - 1)
                            ? TileType.Wall
                            : TileType.Floor
                    };

               //     room.Tiles[x, y].Color = (room.Type == RoomType.Monster) ? Color.green :
                 //       (room.Type == RoomType.Treasure) ? Color.cyan : Color.red;
                }
            }

            return room;
        }
    }
}
