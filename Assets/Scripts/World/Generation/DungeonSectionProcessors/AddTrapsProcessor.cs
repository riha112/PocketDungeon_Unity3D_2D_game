using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.Repository;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;

namespace Assets.Scripts.World.Generation.DungeonSectionProcessors
{
    /// <summary>
    /// Dungeons pipeline process for placing traps:
    /// - Adds spike traps
    /// </summary>
    internal class AddTrapsProcessor : IPipelineProcess<DungeonSectionData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 20020;

        /// <inheritdoc cref="IPipelineProcess{T}" />
        public DungeonSectionData Translate(DungeonSectionData data)
        {
            // Calculation for trap count
            var trapCount = (int) Mathf.Sqrt(data.Width * data.Height) / 3;

            for (var i = 0; i < trapCount; i++)
                PlaceTraps(
                    R.RandomRange(0, data.Width),
                    R.RandomRange(0, data.Height),
                    R.RandomRange(0, 2),
                    data
                );

            return data;
        }

        /// <summary>
        /// Populates tiles with trap prefabs within specific radius 
        /// </summary>
        /// <param name="rx">Center position x</param>
        /// <param name="ry">Center position y</param>
        /// <param name="radius">Radius of placement</param>
        /// <param name="data">Dungeon data to populate</param>
        private static void PlaceTraps(int rx, int ry, int radius, DungeonSectionData data)
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

                    // Loads trap prefab from repository
                    data.DungeonGrid[x, y].Child = PrefabRepository.Repository[5].Prefab;
                }
            }
        }
    }
}