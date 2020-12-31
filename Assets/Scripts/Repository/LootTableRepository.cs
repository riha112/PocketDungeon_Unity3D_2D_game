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


        public static LootTableData GetLootTable()
        {
            // TODO: Move to ILevelFiltrable
            var level = DI.Fetch<SavableGame>()?.World.DungeonFloor ?? 1;
            var filteredOutTables = Repository.Raw.Where(s =>
                s.Value.SpawnOnLevelsInRangeOf.min <= level && s.Value.SpawnOnLevelsInRangeOf.max >= level).ToList();

            return filteredOutTables.Count == 0
                ? null
                : filteredOutTables[R.RandomRange(0, filteredOutTables.Count)].Value;
        }
    }
}