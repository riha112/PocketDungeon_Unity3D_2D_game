using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.Repository;
using Assets.Scripts.Repository.Data;
using Assets.Scripts.World.Generation.Data;

namespace Assets.Scripts.World.Generation.DungeonSectionProcessors
{
    public class CreateRoomProcessor : IPipelineProcess<DungeonSectionData>
    {
        private const short TRY_FAIL_COUNT = 100;
        private static Pipeline<RoomData> _roomPipeline;

        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 10;

        public DungeonSectionData Translate(DungeonSectionData data)
        {
            data.Rooms = new List<RoomData>();

            for (var i = 0; i < data.RoomCount; i++)
            {
                var tryId = 0;
                do
                {
                    var rsx = R.RandomRange(data.RoomSizeRange.min, data.RoomSizeRange.max);
                    var rsy = R.RandomRange(data.RoomSizeRange.min, data.RoomSizeRange.max);

                    var room = new RoomData(rsx, rsy)
                    {
                        Left = R.RandomRange(1, data.Width - rsx - 1),
                        Top = R.RandomRange(1, data.Height - rsy - 1),
                    };

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

        private static RoomType GeRoomType(int size)
        {
            var percentage = R.RandomRange(0, 100);
            // Small Room percentage
            if (size < 50)
            {
                if (percentage < 13) return RoomType.Treasure;
                return percentage < 60 ? RoomType.Design : RoomType.Monster;
            }
            // Medium Room percentage
            if (size < 90)
            {
                if (percentage < 10) return RoomType.Treasure;
                return percentage < 35 ? RoomType.Design : RoomType.Monster;
            }

            // Large room percentage
            if (percentage < 5) return RoomType.Treasure;
            return percentage < 25 ? RoomType.Design : RoomType.Monster;
        }
        
        private static void PipeRooms(IEnumerable<RoomData> rooms)
        {
            if(_roomPipeline == null)
                _roomPipeline = new Pipeline<RoomData>();

            foreach (var room in rooms)
            {
                _roomPipeline.RunThroughPipeline(room);
            }
        }


        private static bool RoomCollides(RoomData room, IEnumerable<RoomData> rooms)
        {
            return rooms.Any(room.CollidesWith);
        }
    }
}
