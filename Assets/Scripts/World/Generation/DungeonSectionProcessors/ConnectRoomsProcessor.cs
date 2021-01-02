using System.Linq;
using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.World.Generation.Data;

namespace Assets.Scripts.World.Generation.DungeonSectionProcessors
{
    /// <summary>
    /// Dungeons pipeline process for connecting rooms via corridors
    /// </summary>
    public class ConnectRoomsProcessor : IPipelineProcess<DungeonSectionData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 550;

        private const int MIN_CORRIDOR_THICKNESS = 1;
        private const int MAX_CORRIDOR_THICKNESS = 2;

        /// <inheritdoc cref="IPipelineProcess{T}" />
        public DungeonSectionData Translate(DungeonSectionData data)
        {
            // Ignore last room, because if n - 1 room will be connected with n
            for (var i = 0; i < data.Rooms.Count - 1; i++)
                // var availableRooms = data.Rooms.Where(r => !r.IsConnected).ToList();
                // Excluding it self, thus starting at 1
                BuildCorridor(data.Rooms[i], data.Rooms[R.RandomRange(i + 1, data.Rooms.Count())], data);

            return data;
        }

        /// <summary>
        /// Joins two rooms via corridors
        /// </summary>
        /// <param name="from">Room to connect</param>
        /// <param name="to">Room to connect with</param>
        /// <param name="data">Dungeon data to register tile changes</param>
        private static void BuildCorridor(RoomData from, RoomData to, DungeonSectionData data)
        {
            var x = from.CenterX;
            var y = from.CenterY;
            var thickness = R.RandomRange(MIN_CORRIDOR_THICKNESS, MAX_CORRIDOR_THICKNESS);

            // Moves towards "to" room from "from" room in X axis
            while (x != to.CenterX)
            {
                PaintAt(x, y, thickness, data);
                x += x < to.CenterX ? 1 : -1;
            }

            // Moves towards "to" room from "from" room in Y axis
            while (y != to.CenterY)
            {
                PaintAt(x, y, thickness, data);
                y += y < to.CenterY ? 1 : -1;
            }
        }

        /// <summary>
        /// Fills dungeon at specific point with "mini-room" 4 walls & base floor
        /// </summary>
        /// <param name="x">Center of point X</param>
        /// <param name="y">Center of point Y</param>
        /// <param name="t">Radius of point</param>
        /// <param name="data">Data where to modify tiles</param>
        private static void PaintAt(int x, int y, int t, DungeonSectionData data)
        {
            t += 1;
            for (var xt = -t; xt <= t; xt++)
            for (var yt = -t; yt <= t; yt++)
            {
                var rx = x + xt;
                var ry = y + yt;

                if (rx < 0 || ry < 0 || rx >= data.Width || ry >= data.Height)
                    continue;

                if (xt == -t || xt == t || yt == -t || yt == t)
                {
                    if (data.DungeonGrid[rx, ry].Type == TileType.Floor)
                        continue;

                    data.DungeonGrid[rx, ry].Type = TileType.Wall;
                    continue;
                }

                data.DungeonGrid[rx, ry].Type = TileType.Floor;
            }
        }
    }
}