using Assets.Scripts.Misc.Pipeline;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;

namespace Assets.Scripts.World.Generation.DungeonSectionProcessors
{
    /// <summary>
    /// Dungeon pipeline process for adding gray-scale effect for each tile
    /// - Sets darkness of the tile using perlin noise algorithm.
    /// </summary>
    public class ColorTilesProcessor : IPipelineProcess<DungeonSectionData>
    {
        public bool IsEnabled { get; } = true;
        public int PriorityId { get; } = 600;

        private const float PERLIN_STRENGTH = 20;
        private const float COLOR_DELTA = 0.8f;

        private int _offsetX;
        private int _offsetY;

        /// <inheritdoc cref="IPipelineProcess{T}" />
        public DungeonSectionData Translate(DungeonSectionData data)
        {
            // Offset for perlin noise 
            _offsetX = R.RandomRange(0, 1000);
            _offsetY = R.RandomRange(0, 1000);

            for (var x = 0; x < data.Width; x++)
            for (var y = 0; y < data.Height; y++)
            {
                // If tile is empty, then skip
                if (data.DungeonGrid[x, y].Type == TileType.None)
                    continue;

                // Assigns color
                if (data.DungeonGrid[x, y].Color == null)
                    data.DungeonGrid[x, y].Color = GetTileColor(x, y, data);
            }

            return data;
        }

        /// <summary>
        /// Generates random gray-scale color via perlin noise
        /// </summary>
        /// <param name="x">Tile x position</param>
        /// <param name="y">Tile y position</param>
        /// <param name="data">Dungeon data to get size dimensions</param>
        /// <returns>Gray-scale color in range of 0 - 0.8f</returns>
        private Color GetTileColor(int x, int y, DungeonSectionData data)
        {
            var xCoordinate = (float) x / data.Width * PERLIN_STRENGTH + _offsetX;
            var yCoordinate = (float) y / data.Height * PERLIN_STRENGTH + _offsetY;
            var strength = Mathf.PerlinNoise(xCoordinate, yCoordinate) * COLOR_DELTA;

            return new Color(1 - strength, 1 - strength, 1 - strength, 1);
        }
    }
}