using Assets.Scripts.Misc.Random;
using UnityEngine;

namespace Assets.Scripts.Repository.Data
{
    public class LootTableData : IIndexable
    {
        public int Id { get; set; }

        public LootTableItemData[] Items { get; set; }
        public (int min, int max) ItemCount { get; set; }
        public (int min, int max) SpawnOnLevelsInRangeOf { get; set; }

        public int GetRandomItem()
        {
            // TODO: Move logic to IRandomizable
            var percentage = R.RandomRange(0, 100);
            var curr = 0;
            for (var i = 0; i < Items.Length - 1; i++)
            {
                curr += Items[i].PossibilityOfSpawning;
                if (curr > percentage)
                    return Items[i].ItemId;
            }

            return Items[Items.Length - 1].ItemId;
        }
    }
}