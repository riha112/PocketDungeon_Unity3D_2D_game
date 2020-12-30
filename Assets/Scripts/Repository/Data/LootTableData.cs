namespace Assets.Scripts.Repository.Data
{
    public class LootTableData : IIndexable
    {
        public int Id { get; set; }
        public (int itemId, int percentage)[] LootItems { get; set; }
        public int MinItems { get; set; }
        public int MaxItems { get; set; }
    }
}