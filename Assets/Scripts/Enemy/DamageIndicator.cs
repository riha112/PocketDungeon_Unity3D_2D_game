using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Misc;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Repository;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    /// <summary>
    /// Contains damage indicator sprites
    /// </summary>
    public static class DamageIndicator
    {
        #region Prefab
        // Caches damage indicators prefab locally
        private static GameObject _damageIndicatorObject;
        private static GameObject DamageIndicatorObject
        {
            get
            {
                if (_damageIndicatorObject is null)
                {
                    _damageIndicatorObject = PrefabRepository.Repository[14].Prefab;
                }

                return _damageIndicatorObject;
            }
        }
        #endregion

        #region Sprites
        // Caches damage indicators sprites locally
        private static Sprite[] _damageIndicatorSprites;
        private static Sprite[] DamageIndicatorSprites
        {
            get
            {
                if (_damageIndicatorSprites is null)
                {
                    _damageIndicatorSprites = new Sprite[10];
                    for (var i = 0; i < 10; i++)
                    {
                        _damageIndicatorSprites[i] = Resources.Load<Sprite>($"Textures/Particles/number_{i}");
                    }
                }

                return _damageIndicatorSprites;
            }
        }
        #endregion

        /// <summary>
        /// Returns sprite that is equal to given damage,
        /// if damage exceeds 9 then returns 9 if less that 0
        /// then returns 0
        /// </summary>
        /// <param name="damage">Real damage</param>
        /// <returns>Sprite with number of damage</returns>
        public static Sprite GetSprite (int damage)
        {
            if (damage < 0) damage = 0;
            else if (damage > 9) damage = 9;
            return DamageIndicatorSprites[damage];
        }


        /// <summary>
        /// Initializes damage indicators effect at specific point,
        /// removes after specific time if necessary.
        /// </summary>
        /// <param name="transform">Where to load</param>
        /// <param name="damage">Real damage amount</param>
        /// <param name="remove">Delete indicator</param>
        /// <param name="timer">After what time should indicator be deleted</param>
        /// <returns></returns>
        public static GameObject LoadDamageIndicator(Transform transform, int damage, bool remove = true, float timer = 1)
        {
            // As this is not instance of MonoBehaviour we are
            // using helper class MonoUtil to call Unity methods for Loading &
            // removing GameObjects
            var mu = DI.Fetch<MonoUtil>();
            if (mu is null) return null;

            // Creates damage indicator
            var damageIndicator = mu.Load(DamageIndicatorObject);
            damageIndicator.transform.position = transform.position;
            damageIndicator.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GetSprite(damage);

            // If required, then deletes new indicator after x amount of time
            if (remove) mu.Remove(damageIndicator, timer);

            return damageIndicator;
        }
    }
}
