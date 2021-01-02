using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.Repository;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;

namespace Assets.Scripts.World.Generation.DungeonSectionProcessors
{
    /// <summary>
    /// Dungeons pipeline process for populating room with water via perlin noise algorithm
    /// - Replaces only floor tiles
    /// </summary>
    public class PlaceWaterProcessor : IPipelineProcess<DungeonSectionData>
    {
        public bool IsEnabled => true;
        public int PriorityId => 10000;

        private const float PERLIN_STRENGTH = 20;

        // Waters color - darker & semi transparent
        private static readonly Color WaterColor = new Color(0.67f, 0.67f, 0.67f, 0.5f);

        private int _offsetX;
        private int _offsetY;

        /// <inheritdoc cref="IPipelineProcess{T}" />
        public DungeonSectionData Translate(DungeonSectionData data)
        {
            _offsetX = R.RandomRange(0, 1000);
            _offsetY = R.RandomRange(0, 1000);

            for (var x = 0; x < data.Width; x++)
            for (var y = data.Height - 1; y >= 0; y--)
            {
                // TODO: Move somewhere.
                // Loads ERROR texture for non-supported wall joins
                if (data.DungeonGrid[x, y].Type == TileType.Wall &&
                    data.DungeonGrid[x, y].TileMapSectionTypeId == 0)
                    data.DungeonGrid[x, y].Sprite =
                        TileMapRepository.Repository[data.TileSetId][1].TileTypes[99].GetRandomSprite();

                // If tile is not floor -> continue
                if (data.DungeonGrid[x, y].Type == TileType.None || data.DungeonGrid[x, y].Type == TileType.Wall)
                    continue;

                // Checks perlin noise so that gray-scale of it is > .7f 
                if (!(GetStrength(x, y, data) > 0.7f)) continue;

                // Sets tile as water
                // If tile above is wall then uses water sprite with shadow effect
                var id = data.DungeonGrid[x, y + 1].Type == TileType.Water ? 0 : 1;
                data.DungeonGrid[x, y].Sprite =
                    TileMapRepository.Repository[data.TileSetId][2].TileTypes[id].GetRandomSprite();
                data.DungeonGrid[x, y].Type = TileType.Water;
                data.DungeonGrid[x, y].Color = WaterColor;
                data.DungeonGrid[x, y].Child = null;
            }
            return data;
        }

        /// <summary>
        /// Returns gray-scale density of perlin noise at X,Y point
        /// </summary>
        /// <param name="x">X point</param>
        /// <param name="y">Y point</param>
        /// <param name="data">Dungeon to get size</param>
        /// <returns>Density of perlin noise in range of 0 - 1</returns>
        private float GetStrength(int x, int y, DungeonSectionData data)
        {
            var xCoordinate = (float) x / (data.Width * 6) * PERLIN_STRENGTH + _offsetX;
            var yCoordinate = (float) y / (data.Height * 6) * PERLIN_STRENGTH + _offsetY;
            var strength = Mathf.PerlinNoise(xCoordinate, yCoordinate);
            return strength;
        }
    }
}