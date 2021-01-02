using System.Collections.Generic;
using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;

namespace Assets.Scripts.World.Generation.DungeonSectionProcessors
{
    /// <summary>
    /// Dungeons pipeline process that is executed after dungeon & room processing
    /// - Sets prefab for walls with 16x32
    /// - Adds torches to walls
    /// </summary>
    public class PostProcessProcessor : IPipelineProcess<DungeonSectionData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 5000;

        // Walls that can contain torches
        private static readonly List<int> FrontalTallWallIds = new List<int>
        {
            (int) WallType.Top,
            (int) WallType.BottomLeftInv,
            (int) WallType.BottomRightInv,
            (int) WallType.SingleWidthVerticalBottom
        };

        // Walls that use 16x32 tiles
        private static readonly List<int> TallWallIds = new List<int>
        {
            (int) WallType.Top,
            (int) WallType.TopRight,
            (int) WallType.TopLeft,
            (int) WallType.BottomLeftInv,
            (int) WallType.BottomRightInv,
            (int) WallType.SingleWidthVerticalBottom,
            (int) WallType.SingleWidthHorizontalLeft,
            (int) WallType.SingleWidthHorizontalRight,
            (int) WallType.SingleWidthHorizontalMiddle
        };

        /// <inheritdoc cref="IPipelineProcess{T}" />
        public DungeonSectionData Translate(DungeonSectionData data)
        {
            // TODO: Replace with PrefabManager
            var tallWall =
                (GameObject) Resources.Load("Prefabs/World/Generation/Tiles/prefab_generation_tile_tall_wall");

            var tallWallWithTorch =
                (GameObject) Resources.Load(
                    "Prefabs/World/Generation/Tiles/prefab_generation_tile_tall_wall_with_torch");

            for (var x = 0; x < data.Width; x++)
            for (var y = 0; y < data.Height; y++)
            {
                if (data.DungeonGrid[x, y].Type != TileType.Wall) continue;

                if (!TallWallIds.Contains(data.DungeonGrid[x, y].TileMapSectionTypeId)) continue;

                if (!FrontalTallWallIds.Contains(data.DungeonGrid[x, y].TileMapSectionTypeId)) continue;

                data.DungeonGrid[x, y].Prefab = R.RandomRange(0, 100) > 25 && (x + y) % 4 == 0
                    ? tallWallWithTorch
                    : tallWall;
            }

            return data;
        }
    }
}