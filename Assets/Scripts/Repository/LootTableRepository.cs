using System.Linq;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.Repository.Data;
using Assets.Scripts.SaveLoad;

namespace Assets.Scripts.Repository
{
    /// <summary>
    /// Holds all data for chest loot tables
    /// <seealso cref="IndexedRepository{T}"/>
    /// </summary>
    public static class LootTableRepository
    {
        public static IndexedRepository<LootTableData> Repository =
            new IndexedRepository<LootTableData>("Register/LootTables");

        /// <returns>Returns random room within specific level range</returns>
        // TODO: Move to ILevelFiltrable
        public static LootTableData GetLootTable()
        {
            // Get current level of dungeon, defaults to 1
            var level = DI.Fetch<SavableGame>()?.World.DungeonFloor ?? 1;

            // Fetches all rooms that are within the range of current level
            var filteredOutTables = Repository.Raw.Where(s =>
                s.Value.SpawnOnLevelsInRangeOf.min <= level && s.Value.SpawnOnLevelsInRangeOf.max >= level).ToList();

            // Returns random room, or null on fail
            return filteredOutTables.Count == 0
                ? null
                : filteredOutTables[R.RandomRange(0, filteredOutTables.Count)].Value;
        }
    }
}