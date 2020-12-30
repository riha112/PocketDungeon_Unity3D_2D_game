using UnityEngine;

namespace Assets.Scripts.World.Generation.Data
{
    public class TileData
    {
        public TileType Type = TileType.None;
        public int TileMapSectionTypeId = 0;
        public Color? Color = null;
        public Sprite Sprite;
        public GameObject Prefab;
        public GameObject Instance;
        public GameObject Child;
    }
}
