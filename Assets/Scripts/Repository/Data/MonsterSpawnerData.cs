using Assets.Scripts.Misc.Random;
using UnityEngine;

namespace Assets.Scripts.Repository.Data
{
    public class MonsterSpawnerData : IIndexable
    {
        public int Id { get; set; }

        public MonsterSpawnersMonsterData[] Monsters { get; set; }
        public (int min, int max) SpawnAmountPerBatch { get; set; }
        public (int min, int max) SpawnOnLevelsInRangeOf { get; set; }
        public (int min, int max) BatchRange { get; set; }
        public (int min, int max) CoolDownRange { get; set; }

        // NOT IMPLEMENTED:
        public bool IsAquatic { get; set; }

        public GameObject GetRandomMonster()
        {
            // TODO: Move logic to IRandomizable
            var percentage = R.RandomRange(0, 100);
            var curr = 0;
            for (var i = 0; i < Monsters.Length - 1; i++)
            {
                curr += Monsters[i].PossibilityOfSpawning;
                if (curr > percentage)
                    return EnemyRepository.Repository[Monsters[i].MonsterId].Prefabs[0];
            }

            return EnemyRepository.Repository[Monsters[Monsters.Length - 1].MonsterId].Prefabs[0];
        }
    }
}
