using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;

namespace Assets.Scripts.World.MapDraw
{
    public class MapCreator : Injectable
    {
        public Color FloorColor;
        public Color WallColor;
        public Color WaterColor;
        public Color SpawnColor;
        public Color BaseColor;

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
