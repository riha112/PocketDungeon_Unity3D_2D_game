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

        public static MonsterSpawnerData GetMonsterSpawner()
        {
            // TODO: Move to ILevelFiltrable
            var level = DI.Fetch<SavableGame>()?.World.DungeonFloor ?? 1;
            var filteredOutSpwaners = Repository.Raw.Where(s =>
                s.Value.SpawnOnLevelsInRangeOf.min <= level && s.Value.SpawnOnLevelsInRangeOf.max >= level).ToList();

            return filteredOutSpwaners.Count == 0
                ? null
                : filteredOutSpwaners[R.RandomRange(0, filteredOutSpwaners.Count)].Value;
        }
    }
} 