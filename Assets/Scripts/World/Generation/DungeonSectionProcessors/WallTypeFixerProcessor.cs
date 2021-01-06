using System.Collections.Generic;
using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.World.Generation.Data;

namespace Assets.Scripts.World.Generation.DungeonSectionProcessors
{
    public class WallTypeFixerProcessor : IPipelineProcess<DungeonSectionData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 2500;

        private DungeonSectionData _data;
        private bool[,] _wallMap;

        // 0 - wildcard, 1 - wall, 2 - floor, 3 - none, 4 - floor/none, 5 - none/wall 9 - tile itself
        private static readonly List<(WallType, int[,])> BitMask = new List<(WallType, int[,])>
        {
            (WallType.Top, new[,]
            {
                {0, 1, 0},
                {2, 9, 4},
                {0, 1, 0}
            }),
            (WallType.Bottom, new[,]
            {
                {0, 1, 0},
                {3, 9, 2},
                {0, 1, 0}
            }),
            (WallType.Right, new[,]
            {
                {0, 2, 0},
                {1, 9, 1},
                {0, 3, 0}
            }),
            (WallType.Left, new[,]
            {
                {0, 3, 0},
                {1, 9, 1},
                {0, 2, 0}
            }),
            (WallType.BottomLeft, new[,]
            {
                {0, 3, 0},
                {3, 9, 1},
                {0, 1, 0}
            }),
            (WallType.BottomRight, new[,]
            {
                {0, 1, 0},
                {3, 9, 1},
                {0, 3, 0}
            }),
            (WallType.TopRight, new[,]
            {
                {0, 1, 0},
                {1, 9, 3},
                {0, 3, 0}
            }),
            (WallType.TopLeft, new[,]
            {
                {0, 3, 0},
                {1, 9, 3},
                {0, 1, 0}
            }),
            (WallType.TopLeftInv, new[,]
            {
                {0, 2, 0},
                {1, 9, 2},
                {0, 1, 0}
            }),
            (WallType.TopRightInv, new[,]
            {
                {0, 1, 0},
                {1, 9, 2},
                {0, 2, 0}
            }),
            (WallType.BottomLeftInv, new[,]
            {
                {0, 2, 0},
                {2, 9, 1},
                {0, 1, 0}
            }),
            (WallType.BottomRightInv, new[,]
            {
                {0, 1, 0},
                {2, 9, 1},
                {0, 2, 0}
            }),
            (WallType.Top, new[,]
            {
                {0, 1, 0},
                {2, 9, 2},
                {0, 2, 0}
            }),
            (WallType.Top, new[,]
            {
                {0, 2, 0},
                {2, 9, 2},
                {0, 1, 0}
            }),
            (WallType.Top, new[,]
            {
                {0, 2, 0},
                {2, 9, 2},
                {0, 2, 0}
            }),
            (WallType.SingleWidthVerticalBottom, new[,]
            {
                {2, 2, 2},
                {2, 9, 1},
                {2, 2, 2}
            }),
            (WallType.SingleWidthVerticalMiddle, new[,]
            {
                {2, 2, 2},
                {1, 9, 1},
                {2, 2, 2}
            }),
            (WallType.SingleWidthVerticalTop, new[,]
            {
                {2, 2, 2},
                {1, 9, 2},
                {2, 2, 2}
            }),
            (WallType.Left, new[,]
            {
                {1, 1, 1},
                {1, 9, 1},
                {2, 2, 2}
            }),
            (WallType.Right, new[,]
            {
                {2, 2, 2},
                {1, 9, 1},
                {1, 1, 1}
            }),
            (WallType.Top, new[,]
            {
                {2, 1, 0},
                {2, 9, 1},
                {2, 1, 0}
            }),
            (WallType.SingleWidthVerticalMiddle, new[,]
            {
                {2, 2, 2},
                {1, 9, 1},
                {1, 2, 2}
            }),
            (WallType.SingleWidthVerticalMiddle, new[,]
            {
                {1, 2, 2},
                {1, 9, 1},
                {2, 2, 2}
            }),
            (WallType.Top, new[,]
            {
                {1, 1, 0},
                {2, 9, 1},
                {2, 1, 0}
            }),
            (WallType.Right, new[,]
            {
                {2, 1, 0},
                {1, 9, 3},
                {2, 1, 0}
            }),
            (WallType.SingleWidthVerticalMiddle, new[,]
            {
                {2, 2, 2},
                {1, 9, 1},
                {2, 2, 1}
            }),
            (WallType.SingleWidthVerticalMiddle, new[,]
            {
                {2, 2, 1},
                {1, 9, 1},
                {2, 2, 2}
            }),
            (WallType.SingleWidthVerticalMiddle, new[,]
            {
                {2, 2, 2},
                {1, 9, 1},
                {2, 2, 1}
            }),
            (WallType.TopLeft, new[,]
            {
                {2, 1, 0},
                {1, 9, 2},
                {0, 1, 2}
            }),
            (WallType.Right, new[,]
            {
                {1, 1, 1},
                {2, 9, 2},
                {0, 2, 0}
            }),
            (WallType.SingleWidthVerticalMiddle, new[,]
            {
                {0, 2, 0},
                {1, 9, 1},
                {2, 1, 0}
            }),
            (WallType.SingleWidthVerticalMiddle, new[,]
            {
                {0, 2, 0},
                {1, 9, 1},
                {0, 2, 0}
            }),
            (WallType.Top, new[,]
            {
                {2, 2, 2},
                {1, 9, 1},
                {0, 2, 0}
            }),
            (WallType.TopRight, new[,]
            {
                {3, 1, 0},
                {1, 9, 2},
                {2, 0, 0}
            }),
            (WallType.TopRightInv, new[,]
            {
                {2, 2, 0},
                {2, 9, 1},
                {1, 2, 1}
            }),
            (WallType.Left, new[,]
            {
                {0, 2, 0},
                {2, 9, 2},
                {1, 1, 2}
            }),
            (WallType.Left, new[,]
            {
                {0, 2, 0},
                {2, 9, 2},
                {2, 1, 1}
            }),
            (WallType.TopLeftInv, new[,]
            {
                {2, 1, 0},
                {1, 9, 0},
                {3, 1, 1}
            }),
            (WallType.BottomRight, new[,]
            {
                {0, 1, 2},
                {3, 9, 1},
                {0, 0, 0}
            })
        };

        public DungeonSectionData Translate(DungeonSectionData data)
        {
            _data = data;
            _wallMap = new bool[data.Width, data.Height];

            for (var x = 0; x < data.Width; x++)
            for (var y = data.Height - 1; y >= 0; y--)
            {
                if (data.DungeonGrid[x, y].Type != TileType.Wall)
                    continue;

                var type = (int) GetWallType(x, y);
                if (type != (int) WallType.None)
                    data.DungeonGrid[x, y].TileMapSectionTypeId = type;
                _wallMap[x, y] = true;
            }

            return data;
        }

        public WallType GetWallType(int px, int py)
        {
            foreach (var bitMask in BitMask)
            {
                var (type, mask) = (bitMask.Item1, bitMask.Item2);
                var isOk = true;
                for (var x = 0; x < 3; x++)
                {
                    if (!isOk) break;

                    for (var y = 0; y < 3; y++)
                    {
                        if (y == 1 && x == 1) continue;
                        if (mask[x, y] == 0) continue;

                        var targetType = T(px + x - 1, py + y - 1);
                        if (mask[x, y] == 1 && targetType != TileType.Wall ||
                            mask[x, y] == 2 && targetType != TileType.Floor ||
                            mask[x, y] == 3 && targetType != TileType.None ||
                            mask[x, y] == 4 && targetType != TileType.None && targetType != TileType.Floor)
                        {
                            isOk = false;
                            break;
                        }
                    }
                }

                if (isOk)
                    return type;
            }

            return WallType.None;
        }

        private TileType T(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _data.Width || y >= _data.Height) return TileType.None;
            return _data.DungeonGrid[x, y].Type;
        }
    }
}