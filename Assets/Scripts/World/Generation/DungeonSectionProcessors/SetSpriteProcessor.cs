using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.Repository;
using Assets.Scripts.World.Generation.Data;

namespace Assets.Scripts.World.Generation.DungeonSectionProcessors
{
    /// <summary>
    /// Dungeons pipeline process for loading sprites for tiles from tile map
    /// </summary>
    public class SetSpriteProcessor : IPipelineProcess<DungeonSectionData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 3000;

        /// <inheritdoc cref="IPipelineProcess{T}" />
        public DungeonSectionData Translate(DungeonSectionData data)
        {
            for (var x = 0; x < data.Width; x++)
            for (var y = 0; y < data.Height; y++)
            {
                var t = data.DungeonGrid[x, y];

                // Tile is empty => continue
                if (t.Type == TileType.None)
                    continue;

                // Tile map subsets: 0 - floor tiles, 1 - wall tiles, 2 - water
                var tileMapSubTypeId = t.Type == TileType.Wall ? 1 : 0;
                t.Sprite = TileMapRepository.Repository[data.TileSetId][tileMapSubTypeId]
                    .TileTypes[t.TileMapSectionTypeId].GetRandomSprite();
            }

            return data;
        }
    }
}