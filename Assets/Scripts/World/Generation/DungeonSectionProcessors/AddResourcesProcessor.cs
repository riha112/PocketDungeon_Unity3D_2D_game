using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.Repository;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;

namespace Assets.Scripts.World.Generation.DungeonSectionProcessors
{
    /// <summary>
    /// Dungeons pipeline process for populating it with resources
    /// - Adds Wood, Stone
    /// </summary>
    internal class AddResourcesProcessor : IPipelineProcess<DungeonSectionData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 20010;

        /// <inheritdoc cref="IPipelineProcess{T}" />
        public DungeonSectionData Translate(DungeonSectionData data)
        {
            // Calculation for amount of resources per map
            var resourceCount = (int) Mathf.Sqrt(data.Width * data.Height) / 2;

            for (var i = 0; i < resourceCount; i++)
            {
                PlaceResources(
                    R.RandomRange(0, data.Width),
                    R.RandomRange(0, data.Height),
                    GetResourcesPrefabId(),
                    R.RandomRange(0, 3),
                    data
                );
            }

            return data;
        }

        /// <summary>
        /// Generates random resource (prefabs) id
        /// </summary>
        /// <returns>Random resources prefabs id</returns>
        private static int GetResourcesPrefabId()
        {
            var resourcePercentage = R.RandomRange(0, 100);
            if (resourcePercentage < 50) return 6;  // Wood
            if (resourcePercentage < 90) return 7; // Stone
            return resourcePercentage < 99 ? 6 : 7; // Iron & Obsidian (not implemented, defaults to Wood & Stone)
        }

        /// <summary>
        /// Populates tiles with resource prefabs within specific radius 
        /// </summary>
        /// <param name="rx">Center position x</param>
        /// <param name="ry">Center position y</param>
        /// <param name="resourceId">Prefab to place</param>
        /// <param name="radius">Radius of placement</param>
        /// <param name="data">Dungeon data to populate</param>
        private static void PlaceResources(int rx, int ry, int resourceId, int radius, DungeonSectionData data)
        {
            for (var x = rx - radius; x <= rx + radius && x < data.Width; x++)
            {
                // Out of arrays range - continue
                if (x < 0) continue;
                for (var y = ry - radius; y <= ry + radius && y < data.Height; y++)
                {
                    // Out of arrays range - continue
                    if (y < 0) continue;

                    // 40% possibility of placing a resource
                    if (R.RandomRange(0, 10) <= 5) continue;

                    // If tile is not an empty floor - continue
                    if (data.DungeonGrid[x, y].Type != TileType.Floor ||
                        !(data.DungeonGrid[x, y].Child is null)) continue;

                    // Prevents from two-tile height wall overlapping with resource
                    if(y - 1 >= 0 && data.DungeonGrid[x, y - 1].Type == TileType.Wall)
                        continue;

                    // Loads resource prefab from repository
                    data.DungeonGrid[x, y].Child = PrefabRepository.Repository[resourceId].Prefab;
                }
            }
        }
    }
}