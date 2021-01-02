using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.Repository.Data;
using Assets.Scripts.World.Generation.Data;

namespace Assets.Scripts.World.Generation.DungeonSectionProcessors
{
    /// <summary>
    /// Dungeons pipeline process for room generation:
    /// - Populates dungeon with randomly created rooms,
    /// - Assigns room type: treasure, design, monster
    /// - Runs each room through Room pipeline
    /// </summary>
    public class CreateRoomProcessor : IPipelineProcess<DungeonSectionData>
    {
        private const short TRY_FAIL_COUNT = 100;
        private static Pipeline<RoomData> _roomPipeline;

        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 10;

        /// <inheritdoc cref="IPipelineProcess{T}" />
        public DungeonSectionData Translate(DungeonSectionData data)
        {
            data.Rooms = new List<RoomData>();

            for (var i = 0; i < data.RoomCount; i++)
            {
                // As we are performing try/fail principle with room placement
                // we are using counter/breaker to prevent from infinity-loop
                var tryId = 0;
                do
                {
                    // Real x position of the room
                    var rsx = R.RandomRange(data.RoomSizeRange.min, data.RoomSizeRange.max);

                    // Real y position of the room
                    var rsy = R.RandomRange(data.RoomSizeRange.min, data.RoomSizeRange.max);

                    var room = new RoomData(rsx, rsy)
                    {
                        Left = R.RandomRange(1, data.Width - rsx - 1),
                        Top = R.RandomRange(1, data.Height - rsy - 1)
                    };

                    // If room does not collide with other rooms, then
                    // add it to dungeon
                    if (!RoomCollides(room, data.Rooms))
                    {
                        room.Type = GeRoomType(room.Width * room.Height);
                        data.Rooms.Add(room);
                        break;
                    }

                    tryId++;
                } while (tryId < TRY_FAIL_COUNT);
            }

            PipeRooms(data.Rooms);
            return data;
        }

        /// <summary>
        /// Returns random room type
        /// </summary>
        /// <param name="size">Size of the room</param>
        /// <returns>Room type</returns>
        private static RoomType GeRoomType(int size)
        {
            var percentage = R.RandomRange(0, 100);
            var (treasurePercentage, designPercentage) = GameBalancer.GetBalancedRoomType(size);

            if (percentage < treasurePercentage) return RoomType.Treasure;
            return percentage < designPercentage ? RoomType.Design : RoomType.Monster;
        }

        /// <summary>
        /// Runs each room through Room creations pipeline 
        /// </summary>
        /// <param name="rooms">List of rooms to modify</param>
        private static void PipeRooms(IEnumerable<RoomData> rooms)
        {
            if (_roomPipeline == null)
                _roomPipeline = new Pipeline<RoomData>();

            foreach (var room in rooms) _roomPipeline.RunThroughPipeline(room);
        }

        /// <summary>
        /// Checks whether or not room collides with other rooms
        /// </summary>
        /// <param name="room">Room to check</param>
        /// <param name="rooms">Rooms to check with</param>
        /// <returns>True - if room collides</returns>
        private static bool RoomCollides(RoomData room, IEnumerable<RoomData> rooms)
        {
            return rooms.Any(room.CollidesWith);
        }
    }
}