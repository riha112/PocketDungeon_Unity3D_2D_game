using Assets.Scripts.Misc.Random;
using UnityEngine;

namespace Assets.Scripts.Repository.Data
{
    public class RoomLayoutTileData
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int[] PrefabIds { get; set; }

        public GameObject GerPrefab()
        {
            return PrefabRepository.Repository[PrefabIds[R.RandomRange(0, PrefabIds.Length)]].Prefab;
        }
    }
}