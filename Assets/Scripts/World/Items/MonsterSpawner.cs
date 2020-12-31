using System;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.Repository;
using Assets.Scripts.Repository.Data;
using Assets.Scripts.User.FloorSwitcher;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;

namespace Assets.Scripts.World.Items
{
    /// <summary>
    /// Contains logic for item: Monster Spawner
    /// - Initiates random monster, based on json data config,
    ///   within given range...
    /// - Spawns X monsters Y times, in interval of Z
    /// </summary>
    public class MonsterSpawner : MonoBehaviour
    {
        private const int SPAWN_RADIUS = 3;

        #region Config
        private MonsterSpawnerData _data;
        private int _coolDownTimer;
        private int _spawnPerBatch;
        private int _spawnBatchCount;
        private int _currentBatchId = 0;
        #endregion

        public GameObject SpawningEffect;
        public GameObject EndEffect;
        public EventHandler<bool> FinishedSpawningMonsters;

        /// <summary>
        /// Called on start of the object instance
        /// </summary>
        private void Start()
        {
            _data = MonsterSpawnerRepository.GetMonsterSpawner();
            if (_data == null)
            {
                EndOfSpawner();
                return;
            }

            _coolDownTimer = R.RandomRange(_data.CoolDownRange.min, _data.CoolDownRange.max);
            _spawnPerBatch = R.RandomRange(_data.SpawnAmountPerBatch.min, _data.SpawnAmountPerBatch.max);
            _spawnBatchCount = R.RandomRange(_data.BatchRange.min, _data.BatchRange.max);

            InvokeRepeating(nameof(SpawnMonsters), _coolDownTimer - 10, _coolDownTimer);
            DI.Fetch<FloorSwitcher>()?.Register(this);
        }

        /// <summary>
        /// Loads monsters into game
        /// </summary>
        private void SpawnMonsters()
        {
            _currentBatchId++;

            for (var i = 0; i < _spawnPerBatch; i++)
            {
                var position = GetSpawnPoint();
                if (position == null) continue;

                var monster = Instantiate(_data.GetRandomMonster());

                monster.transform.position = position.Value;

                var particle = Instantiate(SpawningEffect);
                particle.transform.position = monster.transform.position;
            }

            if (_currentBatchId >= _spawnBatchCount)
            {
                EndOfSpawner();
            }
        }

        /// <summary>
        /// Tries 10 times to find spawn point within specific radius of the spawner,
        /// so that spawning tile is Floor.
        /// </summary>
        /// <returns>Vector2 point for spawn location | on fail null</returns>
        private Vector2? GetSpawnPoint()
        {
            // Allows up to 10 tries to find suitable spawn location for monster
            for (var c = 0; c < 10; c++)
            {
                // Random position around spawner
                var position = new Vector2(
                    transform.position.x + R.RandomRange(-SPAWN_RADIUS, SPAWN_RADIUS + 1) * 0.8f,
                    transform.position.y + R.RandomRange(-SPAWN_RADIUS, SPAWN_RADIUS + 1) * 0.8f
                );

                var tileData = DI.Fetch<DungeonSectionData>()?.GetTileByCoords(position);

                // If tile is floor returns spawn position
                if (tileData != null && tileData.Type == TileType.Floor &&
                    tileData.Instance.transform.childCount == 0)
                    return position;
            }

            return null;
        }

        /// <summary>
        /// Method that is executed when all monsters are spawned
        /// </summary>
        private void EndOfSpawner()
        {
            // TODO: Add animation for ending of the spawners life (shrinking (then puff) or exploding)
            FinishedSpawningMonsters?.Invoke(this, true);

            var effect = Instantiate(EndEffect);
            effect.transform.position = transform.position;

            DI.Fetch<FloorSwitcher>()?.Remove(this);
            Destroy(gameObject);
        }
    }
}