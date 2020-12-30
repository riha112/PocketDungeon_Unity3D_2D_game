using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Weapons.Projectile;
using UnityEngine;

namespace Assets.Scripts.User.Resource
{
    public class ResourceObject : MonoBehaviour, IDamagable
    {
        public int Durability;
        public int ResourceId;

        private SpriteRenderer _renderer;
        private Color _startColor;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _startColor = _renderer.color;
        }

        public int TakeDamage(int hitPoints)
        {
            var realDamage = Random.Range(1, (hitPoints < 2) ? 2 : hitPoints);
            Durability -= realDamage;
            if (Durability < 0)
                Destroy(gameObject);

            _renderer.color = Color.red;
            Invoke(nameof(ResetColor), 0.08f);
            return realDamage;
        }

        private void ResetColor()
        {
            _renderer.color = _startColor;
        }
    }
}
