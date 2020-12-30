using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.World.Generation.Data;

namespace Assets.Scripts.World.Generation.DungeonSectionProcessors
{
    public class ConnectRoomsProcessor : IPipelineProcess<DungeonSectionData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 550;

        private const int MIN_CORRIDOR_THICKNESS = 1;
        private const int MAX_CORRIDOR_THICKNESS = 2;

        public DungeonSectionData Translate(DungeonSectionData data)
        {
            // Ignore last room, because if n - 1 room will be connected with n
            for (var i = 0; i < data.Rooms.Count - 1; i++)
            {
                // var availableRooms = data.Rooms.Where(r => !r.IsConnected).ToList();
                // Excluding it self, thus starting at 1
                BuildCorridor(data.Rooms[i], data.Rooms[R.RandomRange(i + 1, data.Rooms.Count())], data);
            }

            return data;
        }

        private static void BuildCorridor(RoomData from, RoomData to, DungeonSectionData data)
        {
            var x = from.CenterX;
            var y = from.CenterY;
            var thickness = R.RandomRange(MIN_CORRIDOR_THICKNESS, MAX_CORRIDOR_THICKNESS);

            while (x != to.CenterX)
            {
                PaintAt(x, y, thickness, data);
                x += x < to.CenterX ? 1 : -1;
            }

            while (y != to.CenterY)
            {
                PaintAt(x, y, thickness, data);
                y += y < to.CenterY ? 1 : -1;
            }
        }

        private static void PaintAt(int x, int y, int t, DungeonSectionData data)
        {
            t += 1;
            for (var xt = -t; xt <= t; xt++)
            {
                for (var yt = -t; yt <= t; yt++)
                {
                    var rx = x + xt;
                    var ry = y + yt;

                    if (rx < 0 || ry < 0 || rx >= data.Width || ry >= data.Height)
                        continue;

                    if (xt == -t || xt == t || yt == -t || yt == t)
                    {
                        if(data.DungeonGrid[rx, ry].Type == TileType.Floor)
                            continue;

                        data.DungeonGrid[rx, ry].Type = TileType.Wall;
                        continue;
                    }

                    //if (data.DungeonGrid[rx, ry].Type == TileType.Wall)
                    //    continue;

                    data.DungeonGrid[rx, ry].Type = TileType.Floor;
                }
            }
        }
    }
}
