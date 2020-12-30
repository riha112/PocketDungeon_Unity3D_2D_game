using System;
using System.Collections.Generic;
using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;

namespace Assets.Scripts.World.Generation.DungeonProcessors
{
    public class PostProcessProcessor : IPipelineProcess<DungeonSectionData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 5000;

        private readonly List<int> FrontalTallWallIds = new List<int>
        {
            (int) WallType.Top,
            (int) WallType.BottomLeftInv,
            (int) WallType.BottomRightInv,
            (int) WallType.SingleWidthVerticalBottom
        };

        private static readonly List<int>TallWallIds = new List<int>
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

        public DungeonSectionData Translate(DungeonSectionData data)
        {
            var tallWall = (GameObject) Resources.Load("Prefabs/World/Generation/Tiles/prefab_generation_tile_tall_wall");
            var tallWallWithTorch = (GameObject) Resources.Load("Prefabs/World/Generation/Tiles/prefab_generation_tile_tall_wall_with_torch");

            for (var x = 0; x < data.Width; x++)
            {
                for (var y = 0; y < data.Height; y++)
                {
                    if (data.DungeonGrid[x, y].Type != TileType.Wall) continue;

                    if (TallWallIds.Contains(data.DungeonGrid[x, y].TileMapSectionTypeId))
                    {
                        data.DungeonGrid[x, y].Prefab = (R.RandomRange(0, 100) > 25 && (x + y) % 4 == 0 && FrontalTallWallIds.Contains(data.DungeonGrid[x, y].TileMapSectionTypeId))
                            ? tallWallWithTorch
                            : tallWall;
                    }
                }
            }
            return data;
        }
    }
}
