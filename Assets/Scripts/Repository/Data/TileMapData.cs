using System.Collections.Generic;

namespace Assets.Scripts.Repository.Data
{
    public class TileMapData : IIndexable
    {
        public int Id { get; set; }
        public Dictionary<int, TileData> TileTypes { get; set; }
    }
}