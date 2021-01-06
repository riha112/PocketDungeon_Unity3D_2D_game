using System.Linq;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.Repository.Data;
using Assets.Scripts.SaveLoad;

namespace Assets.Scripts.Repository
{
    /// <summary>
    /// Holds all data for monster spawners
    /// <seealso cref="IndexedRepository{T}" />
    /// </summary>
    public static class MonsterSpawnerRepository
    {
        public static IndexedRepository<MonsterSpawnerData> Repository =
            new IndexedRepository<MonsterSpawnerData>("Register/MonsterSpawner");

        /// <returns>Returns random monster spawner within specific level range</returns>
        // TODO: Move to ILevelFiltrable
        public static MonsterSpawnerData GetMonsterSpawner()
        {
            // Get current level of dungeon, defaults to 1
            var level = DI.Fetch<SavableGame>()?.World.DungeonFloor ?? 1;

            // Fetches all spawners that are within the range of current level
            var filteredOutSpwaners = Repository.Raw.Where(s =>
                s.Value.SpawnOnLevelsInRangeOf.min <= level && s.Value.SpawnOnLevelsInRangeOf.max >= level).ToList();

            // Returns random spawner, or null on fail
            return filteredOutSpwaners.Count == 0
                ? null
                : filteredOutSpwaners[R.RandomRange(0, filteredOutSpwaners.Count)].Value;
        }
    }
} 