using System.Collections.Generic;
using Assets.Scripts.Misc;
using Assets.Scripts.Repository.Data;

namespace Assets.Scripts.Repository
{
    /// <summary>
    /// Holds all data for TileMaps
    /// <seealso cref="IndexedRepository{T}"/>
    /// </summary>
    public static class TileMapRepository
    {
        private static Dictionary<int, TileMapData[]> _tileMapLibrary;

        public static Dictionary<int, TileMapData[]> Repository =>
            _tileMapLibrary ?? (_tileMapLibrary =
                Util.LoadJsonFromFile<Dictionary<int, TileMapData[]>>("Register/TileMap"));
    }
}