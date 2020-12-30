using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.Repository;
using Assets.Scripts.Repository.Data;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;

namespace Assets.Scripts.World.Generation.RoomProcessors
{
    internal class RoomsSection
    {
        public int Left;
        public int Top;
        public int Width;
        public int Height;

        public int Right => Left + Width - 1;
        public int Bottom => Top + Height - 1;

        public RoomLayoutData Layout;

        public bool CollidesWith(RoomsSection room)
        {
            if (Left > room.Right)
                return false;
            if (Top > room.Bottom)
                return false;
            if (Right < room.Left)
                return false;
            if (Bottom < room.Top)
                return false;

            return true;
        }
    }

    public class PopulateRoomProcessor : IPipelineProcess<RoomData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 100;

        private const int TRY_FAIL_COUNT = 25;
        private const int MAX_JOIN_COUNT = 6;

        public RoomData Translate(RoomData room)
        {
            var availableSections = RoomLayoutRepository.FilterForSizeAndType(
                room.Type, room.Width - 2, room.Height - 2
            ) ?? new List<RoomLayoutData>();

            var activeSections = new List<RoomsSection>();
            var joinCount = R.RandomRange(1, MAX_JOIN_COUNT);

            PlaceDefaultValues(room, activeSections);

            // Joins sections
            if (availableSections.Count > 0)
            {
                for (var s = 0; s < joinCount; s++)
                {
                    var currentLayout = availableSections[R.RandomRange(0, availableSections.Count)];
                    var currentTry = 0;
                    do
                    {
                        var section = new RoomsSection
                        {
                            Height = currentLayout.Height,
                            Width = currentLayout.Width,
                            Left = R.RandomRange(1, room.Width - currentLayout.Width),
                            Top = R.RandomRange(1, room.Width - currentLayout.Width),
                            Layout = currentLayout
                        };

                        if (!SectionCollides(section, activeSections))
                        {
                            activeSections.Add(section);
                            break;
                        }

                        currentTry++;
                    } while (currentTry < TRY_FAIL_COUNT);
                }
            }

            // Places prefabs
            foreach (var section in activeSections)
            {
                foreach (var layoutItemInfo in section.Layout.RoomLayout)
                {
                    var rx = layoutItemInfo.X + section.Left;
                    var ry = layoutItemInfo.Y + section.Height;
                    if(rx < 0 || ry < 0 || rx >= room.Width || ry >= room.Height) continue;
                    room.Tiles[rx, ry].Child = layoutItemInfo.GerPrefab();
                }
            }

            return room;
        }

        private static void PlaceDefaultValues(RoomData room, ICollection<RoomsSection> activeSections)
        {
            if (room.Type == RoomType.Monster)
            {
                if (room.Width < 11)
                {
                    activeSections.Add(new RoomsSection
                    {
                        Height = 3,
                        Width = 3,
                        Left = room.Width / 2,
                        Top = room.Height / 2,
                        Layout = new RoomLayoutData
                        {
                            RoomLayout = new[]
                            {
                                new RoomLayoutTileData
                                {
                                    X = 1,
                                    Y = 1,
                                    PrefabIds = new[] {1}
                                }
                            }
                        }
                    });
                }
                else
                {
                    activeSections.Add(new RoomsSection
                    {
                        Height = 3,
                        Width = 3,
                        Left = 2,
                        Top = room.Height / 2,
                        Layout = new RoomLayoutData
                        {
                            RoomLayout = new[]
                            {
                                new RoomLayoutTileData
                                {
                                    X = 1,
                                    Y = 1,
                                    PrefabIds = new[] {1}
                                }
                            }
                        }
                    });

                    activeSections.Add(new RoomsSection
                    {
                        Height = 3,
                        Width = 3,
                        Left = room.Width - 5,
                        Top = room.Height / 2,
                        Layout = new RoomLayoutData
                        {
                            RoomLayout = new[]
                            {
                                new RoomLayoutTileData
                                {
                                    X = 1,
                                    Y = 1,
                                    PrefabIds = new[] {1}
                                }
                            }
                        }
                    });
                }
            }
        }

        private static bool SectionCollides(RoomsSection section, List<RoomsSection> activeSections) => 
            activeSections.Any(section.CollidesWith);
    }
}
