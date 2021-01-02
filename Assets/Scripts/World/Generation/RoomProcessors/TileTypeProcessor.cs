using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.World.Generation.Data;

namespace Assets.Scripts.World.Generation.RoomProcessors
{
    /// <summary>
    /// Room pipeline process for room base tile setup:
    /// - Sets borders as walls
    /// - Sets main body as floor
    /// </summary>
    public class TileTypeProcessor : IPipelineProcess<RoomData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 50;

        public RoomData Translate(RoomData room)
        {
            for (var x = 0; x < room.Width; x++)
            for (var y = 0; y < room.Height; y++)
                // If x & y are edges of array => wall otherwise floor
                room.Tiles[x, y] = new TileData
                {
                    Type = x == 0 || y == 0 || x == room.Width - 1 || y == room.Height - 1
                        ? TileType.Wall
                        : TileType.Floor
                };

            return room;
        }
    }
}