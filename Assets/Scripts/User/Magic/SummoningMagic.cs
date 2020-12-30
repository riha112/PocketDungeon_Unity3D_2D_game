using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Enemy;
using Assets.Scripts.Misc;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.User.Party;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;

namespace Assets.Scripts.User.Magic
{
    public class SummoningMagic : MonoBehaviour
    {
        public float LifeLength = 0;
        public float SummonCount = 0;
        public GameObject Summon;
        public GameObject SummonEffect;

        private List<GameObject> _summons = new List<GameObject>();

        private void Start()
        {
            var angle = 0.0f;
            var step = (2 * Mathf.PI) / SummonCount;
            var dsd = DI.Fetch<DungeonSectionData>();
            var pc = DI.Fetch<PartyController>();

            for (var i = 0; i < SummonCount; i++)
            {
                var x = transform.position.x + Math.Cos(angle);
                var y = transform.position.y + Math.Sin(angle);
                var coords = new Vector2((float)x, (float)y);
                angle += step;

                var tile = dsd.GetTileByCoords(coords);
                if (tile == null || tile.Type != TileType.Floor)
                    continue;

                if (SummonEffect != null)
                {
                    var effect = Instantiate(SummonEffect);
                    effect.transform.position = coords;
                }

                var summon = Instantiate(Summon);
                summon.transform.position = coords;
                summon.name = "summon";
                summon.GetComponent<SimpleEnemy>().Target = GetClosestEnemy(summon.transform.position);
                pc.PartyMembers.Add(summon.GetComponent<SimpleEnemy>());

                _summons.Add(summon);
            }

            Invoke(nameof(CleanUp), LifeLength);
        }

        private void CleanUp()
        {
            var pc = DI.Fetch<PartyController>();
            foreach (var summon in _summons)
            {
                if(summon == null) continue;
                pc.PartyMembers.Remove(summon.GetComponent<SimpleEnemy>());
                Destroy(summon);
            }
            Destroy(gameObject);
        }

        private Transform GetClosestEnemy(Vector2 point)
        {
            (Transform closest, float distance) target = (null, 0);

            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies)
            {
                if(enemy.name == "summon") continue;

                var dist = Vector2.Distance(point, enemy.transform.position);
                if (target.closest != null && !(dist < target.distance)) continue;
                target.distance = dist;
                target.closest = enemy.transform;
            }

            return target.closest;
        }
    }
}
