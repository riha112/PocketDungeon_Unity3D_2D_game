using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.World.Generation;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;

namespace Assets.Scripts.World.Items
{
    [System.Serializable]
    public struct MonsterSpawnerSettings
    {
        public GameObject Monster;
        [Range(0, 100)]
        public int PossibilityOfSpawning;
    }

    public class MonsterSpawner : MonoBehaviour
    {
        public List<MonsterSpawnerSettings> Monsters;
        [Range(1, 50)]
        public int SpawnPerBatch;
        [Range(1, 10)]
        public int Radius;
        [Range(1, 120)]
        public float CoolDownTimer;

        public GameObject SpawningEffect;

        private void Start()
        {
            CoolDownTimer = R.RandomRange(30, 120);
            SpawnPerBatch = R.RandomRange(2, 10);

            InvokeRepeating(nameof(SpawnMonsters), CoolDownTimer / 2, CoolDownTimer);
        }

        private void SpawnMonsters()
        {
            for (var i = 0; i < SpawnPerBatch; i++)
            {
                var position = GetSpawnPoint();
                if (position == null) continue;

                var monster = Instantiate(GetMonsterToSpawn());

                monster.transform.position = position.Value;

                var particle = Instantiate(SpawningEffect);
                particle.transform.position = monster.transform.position;
            }
        }

        private Vector2? GetSpawnPoint()
        {
            for (var c = 0; c < 10; c++)
            {
                var position = new Vector2(
                    transform.position.x + R.RandomRange(-Radius, Radius + 1) * 0.8f,
                    transform.position.y + R.RandomRange(-Radius, Radius + 1) * 0.8f
                );

                var tileData = DI.Fetch<DungeonSectionData>().GetTileByCoords(position);

                if (tileData != null && tileData.Type == TileType.Floor &&
                    tileData.Instance.transform.childCount == 0)
                {
                    return position;
                }
            }

            return null;
        }

        private GameObject GetMonsterToSpawn()
        {
            var monsterPrc = R.RandomRange(0, 100);
            var curr = 0;
            for (var i = 0; i < Monsters.Count - 1; i++)
            {
                curr += Monsters[i].PossibilityOfSpawning;
                if (curr > monsterPrc)
                    return Monsters[i].Monster;
            }
            return Monsters[Monsters.Count - 1].Monster;
        }

    }
}
