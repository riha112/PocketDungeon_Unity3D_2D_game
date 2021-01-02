using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.Repository;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;

namespace Assets.Scripts.World.Generation.DungeonSectionProcessors
{
    /// <summary>
    /// Dungeons pipeline process for populating dungeon with non-monster-spawner mobs
    /// - Adds bats
    /// </summary>
    internal class AddMobsProcessor : IPipelineProcess<DungeonSectionData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 20000;

        /// <inheritdoc cref="IPipelineProcess{T}" />
        public DungeonSectionData Translate(DungeonSectionData data)
        {
            for (var x = 0; x < data.Width; x++)
            for (var y = data.Height - 1; y >= 0; y--)
            {
                if (data.DungeonGrid[x, y].Type != TileType.Wall)
                    continue;

                // Adds Bat to corners of wall
                if (data.DungeonGrid[x, y].TileMapSectionTypeId == (int) WallType.TopRight)
                {
                    AddBat(x - 1, y, data);
                    AddBat(x - 2, y, data);
                }
                else if (data.DungeonGrid[x, y].TileMapSectionTypeId == (int) WallType.TopLeft)
                {
                    AddBat(x + 1, y, data);
                    AddBat(x + 2, y, data);
                }
            }

            return data;
        }

        /// <summary>
        /// Places "Bat" mob into dungeon by chance
        /// </summary>
        /// <param name="x">Tile position x</param>
        /// <param name="y">Tile position y</param>
        /// <param name="data">Dungeons data container</param>
        private static void AddBat(int x, int y, DungeonSectionData data)
        {
            // Tile is not wall or percentage is less then 7 then return,
            // meaning that there is 30% of placing a bat
            if (R.RandomRange(0, 10) <= 6) return;
            if (data.DungeonGrid[x, y].Type != TileType.Wall) return;

            var rX = x * 0.8f - data.Width * 0.4f;
            var rY = y * 0.8f - data.Height * 0.4f + 0.4f;

            var bat = Object.Instantiate(EnemyRepository.Repository[5].Prefabs[0]);
            bat.transform.position = new Vector2(rX, rY);
        }
    }
}