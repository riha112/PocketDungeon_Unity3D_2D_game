using Assets.Scripts.Repository.Data;

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
    }
}