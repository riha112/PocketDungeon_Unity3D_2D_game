using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;

namespace Assets.Scripts.User.Map.MapDraw
{
    /// <summary>
    /// Generates texture that represents
    /// current dungeons floor...
    /// </summary>
    public class MapCreator : Injectable
    {
        #region Color configurations
        public Color FloorColor;
        public Color WallColor;
        public Color WaterColor;
        public Color BaseColor;
        #endregion

        private Texture2D _generatedTexture;

        public Texture2D GeneratedTexture
        {
            get
            {
                if (_generatedTexture == null)
                    GenerateTexture();
                return _generatedTexture;
            }
            set => _generatedTexture = value;
        }

        /// <summary>
        /// Generates 2D texture that represents dungeons floor
        /// </summary>
        /// <param name="dungeon">Targeted floor | By default uses current</param>
        public void GenerateTexture(DungeonSectionData dungeon = null)
        {
            if (dungeon == null)
                dungeon = DI.Fetch<DungeonSectionData>();

            if (dungeon == null)
                return;

            GeneratedTexture = new Texture2D(dungeon.Width, dungeon.Height);

            for (var x = 0; x < dungeon.Width; x++)
            {
                for (var y = 0; y < dungeon.Height; y++)
                {
                    GeneratedTexture.SetPixel(x, y, TypeToColor(dungeon.DungeonGrid[x,y].Type));
                }
            }

            GeneratedTexture.filterMode = FilterMode.Point;
            GeneratedTexture.wrapMode = TextureWrapMode.Clamp;
            GeneratedTexture.Apply();
        }
        
        /// <summary>
        /// Converts tile type into color
        /// </summary>
        /// <param name="type">Tile type</param>
        /// <returns>color</returns>
        private Color TypeToColor(TileType type)
        {
            switch (type)
            {
                case TileType.None:
                    return BaseColor;
                case TileType.Floor:
                    return FloorColor;
                case TileType.Wall:
                    return WallColor;
                case TileType.Water:
                    return WaterColor;
                case TileType.Other:
                    return BaseColor;
                default:
                    return BaseColor;
            }
        }

    }
}
