using System.Collections.Generic;
using Assets.Scripts.Misc;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.Repository;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;

namespace Assets.Scripts.World.Generation
{
    /// <summary>
    /// Class that initiates dungeon generation and
    /// forwards dungeon sections towards Pipeline
    /// </summary>
    public class DungeonGenerator : MonoBehaviour
    {
        #region Config
        private const int MIN_SIZE = 24;
        private const int MAX_SIZE = 48;
        private const int MIN_ROOM_COUNT = 8;
        private const int MAX_ROOM_COUNT = 20;
        private const int DUNGEON_SECTION_COUNT = 1;
        private const float TILE_SIZE = 0.8f;
        #endregion

        public string Seed;
        public bool UseRandomSeed;

        public GameObject TilePrefab;
        public GameObject SolidTilePrefab;

        private List<DungeonSectionData> _dungeonSections;
        private static Pipeline<DungeonSectionData> _dungeonPipeline;

        public void Generate()
        {
            R.SetSeed(UseRandomSeed ? null : Seed);
            PopulateSections();
            PipeData();
            BuildGraphics();
        }

        /// <summary>
        /// Converts data to actual GameObject instances
        /// NOTE: Maybe move to new pipeline process
        /// </summary>
        private void BuildGraphics()
        {
            DI.Register(_dungeonSections[0]);
            for (var i = 0; i < DUNGEON_SECTION_COUNT; i++)
            {
                var offsetX = _dungeonSections[i].Width * TILE_SIZE * -0.5f;
                var offsetY = _dungeonSections[i].Height * TILE_SIZE * -0.5f;

                for (var x = 0; x < _dungeonSections[i].Width; x++)
                for (var y = 0; y < _dungeonSections[i].Height; y++)
                {
                    var tileData = _dungeonSections[i].DungeonGrid[x, y];
                    if (tileData == null || tileData.Type == TileType.None)
                        continue;

                    var tile = Instantiate(tileData.Prefab == null
                        ? tileData.Type == TileType.Wall || tileData.Type == TileType.Water ? SolidTilePrefab :
                        TilePrefab
                        : tileData.Prefab);

                    tile.transform.position = new Vector3(
                        offsetX + x * TILE_SIZE,
                        offsetY + y * TILE_SIZE,
                        y * TILE_SIZE * -0.01f * (tileData.Type == TileType.Wall ? -1 : 1)
                    );

                    tile.name = $"tile[{x},{y}]::{tileData.Sprite?.name}";
                    _dungeonSections[i].DungeonGrid[x, y].Instance = tile;

                    var sr = tile.GetComponent<SpriteRenderer>();
                    sr.sprite = tileData.Sprite;
                    sr.color = tileData.Color ?? new Color(1, 1, 1, 1);

                    if (tileData.Child != null)
                    {
                        var child = Instantiate(tileData.Child);
                        child.transform.position = tile.transform.position;
                        child.transform.position -= tile.transform.forward * 0.1f;
                        child.transform.parent = tile.transform;
                    }

                    tile.transform.parent = transform;
                }

                SetDungeonStartPoint(_dungeonSections[i]);
            }
        }

        /// <summary>
        /// Sets characters location in dungeon
        /// when dungeon is generated
        /// </summary>
        /// <param name="dungeon"></param>
        private void SetDungeonStartPoint(DungeonSectionData dungeon)
        {
            for (var x = dungeon.Width - 1; x > 0; x--)
            for (var y = dungeon.Height - 1; y > 0; y--)
                // Finds location in dungeon where upper to walls are front-facing and bottom
                // two rows are empty floor tiles
                if (dungeon.DungeonGrid[x, y].Type == TileType.Wall &&
                    dungeon.DungeonGrid[x - 1, y].Type == TileType.Wall &&
                    dungeon.DungeonGrid[x, y].TileMapSectionTypeId == (int) WallType.Top &&
                    dungeon.DungeonGrid[x, y].TileMapSectionTypeId == (int) WallType.Top &&
                    dungeon.DungeonGrid[x, y - 1].Type == TileType.Floor &&
                    dungeon.DungeonGrid[x - 1, y - 1].Type == TileType.Floor &&
                    dungeon.DungeonGrid[x - 1, y - 1].Child is null &&
                    dungeon.DungeonGrid[x, y - 1].Child is null)
                {
                    if (dungeon.DungeonGrid[x, y].Instance.transform.childCount > 0 ||
                        dungeon.DungeonGrid[x - 1, y].Instance.transform.childCount > 0)
                        continue;

                    Util.GetCharacterTransform().position =
                        dungeon.DungeonGrid[x, y - 1].Instance.transform.position - Vector3.right * 0.4f;

                    // Loads door design
                    for (var i = 0; i < 2; i++)
                    {
                        var doorPart = Instantiate(TilePrefab);
                        doorPart.transform.position =
                            dungeon.DungeonGrid[x - i, y].Instance.transform.position - Vector3.forward;
                        doorPart.GetComponent<SpriteRenderer>().sortingOrder = 1;
                        doorPart.GetComponent<SpriteRenderer>().color =
                            dungeon.DungeonGrid[x - i, y].Instance.GetComponent<SpriteRenderer>().color;
                        doorPart.GetComponent<SpriteRenderer>().sprite =
                            TileMapRepository.Repository[dungeon.TileSetId][3].TileTypes[i].GetRandomSprite();
                        doorPart.transform.parent = dungeon.DungeonGrid[x - i, y].Instance.transform;
                    }

                    return;
                }
        }

        /// <summary>
        /// Populates data with dungeon sections
        /// </summary>
        private void PopulateSections()
        {
            _dungeonSections = new List<DungeonSectionData>();
            for (var i = 0; i < DUNGEON_SECTION_COUNT; i++)
            {
                var width = R.RandomRange(MIN_SIZE, MAX_SIZE);
                var height = R.RandomRange(MIN_SIZE, MAX_SIZE);
                _dungeonSections.Add(new DungeonSectionData(width, height)
                {
                    RoomCount = R.RandomRange(MIN_ROOM_COUNT, MAX_ROOM_COUNT),
                    RoomSizeRange = (R.RandomRange(5, 7), R.RandomRange(7, 14)),
                    TileSetId = R.RandomRange(0, TileMapRepository.Repository.Count)
                });
            }
        }

        /// <summary>
        /// Runs dungeon data through pipeline
        /// </summary>
        private void PipeData()
        {
            if (_dungeonPipeline == null)
                _dungeonPipeline = new Pipeline<DungeonSectionData>();

            foreach (var section in _dungeonSections) _dungeonPipeline.RunThroughPipeline(section);
        }
    }
}