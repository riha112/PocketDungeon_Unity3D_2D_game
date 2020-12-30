using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Misc;

namespace Assets.Scripts.Repository.Data
{
    public static class RoomLayoutRepository
    {
        private static Dictionary<RoomType, List<RoomLayoutData>> _roomLayoutLibrary;

        public static Dictionary<RoomType, List<RoomLayoutData>> RoomLayoutLibrary
        {
            get
            {
                if (_roomLayoutLibrary == null)
                {
                    var roomLayoutItems = Util.LoadJsonFromFile<List<RoomLayoutData>>("Register/RoomLayouts");
                    _roomLayoutLibrary = new Dictionary<RoomType, List<RoomLayoutData>>();
                    foreach (var item in roomLayoutItems)
                    {
                        if (!_roomLayoutLibrary.ContainsKey(item.RoomType))
                            _roomLayoutLibrary.Add(item.RoomType, new List<RoomLayoutData>());
                        _roomLayoutLibrary[item.RoomType].Add(item);
                    }
                }

                return _roomLayoutLibrary;
            }
        }

        public static List<RoomLayoutData> FilterForSizeAndType(RoomType type, int width, int height)
        {
            if (!RoomLayoutLibrary.ContainsKey(type))
                return null;

            // Will not be called if used correctly, but extra check never did any
            // harm
            if (RoomLayoutLibrary[type].Count == 0)
                return null;

            return RoomLayoutLibrary[type].Where(r => r.Width <= width && r.Height <= height).ToList();
        }
    }
}