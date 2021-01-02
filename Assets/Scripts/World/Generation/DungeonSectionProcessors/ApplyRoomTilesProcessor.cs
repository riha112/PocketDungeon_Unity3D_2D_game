using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.World.Generation.Data;

namespace Assets.Scripts.World.Generation.DungeonSectionProcessors
{
    /// <summary>
    /// Dungeons pipeline process fo moving all room tiles into dungeon tiles
    /// </summary>
    public class ApplyRoomTilesProcessor : IPipelineProcess<DungeonSectionData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 500;

        /// <inheritdoc cref="IPipelineProcess{T}" />
        public DungeonSectionData Translate(DungeonSectionData data)
        {
            // Copies each rooms tile into dungeon tile
            foreach (var room in data.Rooms)
                for (var x = room.Left; x < room.Left + room.Width; x++)
                for (var y = room.Top; y < room.Top + room.Height; y++)
                    data.DungeonGrid[x, y] = room.Tiles[x - room.Left, y - room.Top];

            return data;
        }
    }
}