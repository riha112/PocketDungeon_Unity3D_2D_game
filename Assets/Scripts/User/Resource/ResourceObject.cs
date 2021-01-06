using Assets.Scripts.Entity;
using Assets.Scripts.Weapons.Projectile;
using UnityEngine;

namespace Assets.Scripts.User.Resource
{
    /// <summary>
    /// Class for placed resource object
    /// </summary>
    public class ResourceObject : MonoBehaviour, IDamagable
    {
        public int Durability;

        private SpriteRenderer _renderer;
        private Color _startColor;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _startColor = _renderer.color;
        }

        /// <summary>
        /// Reduces durability for block...
        /// If Durability is less or equal then 0 ... destroys the gameObject
        /// </summary>
        /// <param name="hitPoints">Max damage to take</param>
        /// <returns>Real damage taken</returns>
        public int TakeDamage(int hitPoints)
        {
            var realDamage = Random.Range(1, hitPoints < 2 ? 2 : hitPoints);
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