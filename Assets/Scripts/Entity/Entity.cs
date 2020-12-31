using System;
using Assets.Scripts.User.Attributes;
using Assets.Scripts.User.Messages;
using Assets.Scripts.User.Stats;
using Assets.Scripts.Weapons.Projectile;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entity
{
    /// <summary>
    /// Class for basic Entity - any living object within game,
    /// contains base parameters like Stats & Attributes
    /// </summary>
    public abstract class Entity : MonoBehaviour, IDamagable
    {
        private const int POINTS_PER_LEVEL = 5;

        public StatsData Stats = new StatsData();

        public AttributeData Attributes = new AttributeData();

        #region Health
        private float _health;
        public float Health
        {
            get => _health;
            set
            {
                _health += value;
                if (_health > Stats.MaxHealth)
                {
                    _health = Stats.MaxHealth;
                }
                else if (_health <= 0)
                {
                    _health = 0;
                    Death();
                }
                HealthUpdated?.Invoke(this, _health);
            }
        }
        public event EventHandler<float> HealthUpdated;
        #endregion

        #region Magic
        private float _magic;
        public float Magic
        {
            get => _magic;
            set
            {
                _magic += value;
                if (_magic > Stats.MaxMagic)
                {
                    _magic = Stats.MaxMagic;
                }
                else if (_magic <= 0)
                {
                    _magic = 0;
                }
                MagicUpdated?.Invoke(this, _magic);
            }
        }
        public event EventHandler<float> MagicUpdated;
        #endregion

        /// <summary>
        /// Called on initialization of entity (replaces constructor in Unity)
        /// </summary>
        protected virtual void Awake()
        {
            Attributes.AttributeUpdate += OnAttributesChange;
        }

        /// <summary>
        /// Recalculates stats when attributes change for entity
        /// </summary>
        /// <param name="sender">What was updated</param>
        /// <param name="data">Id of attribute & value of new attribute</param>
        public virtual void OnAttributesChange([CanBeNull] object sender, (int, short) data)
        {
            Stats.MaxHealth = Attributes.Vitality;
            Stats.MaxMagic = Attributes.Magic;
            Stats.CurrentDefense = Attributes.Resistance;
            Stats.CurrentSpeed = 1 + Attributes.Agility / 50.0f;
            Stats.CurrentLuck = 1 + Attributes.Luck / 10.0f;
            Stats.CurrentStrength = Attributes.Strength;
            Stats.HitPoints = 1 + Attributes.Strength / 4;
        }

        /// <summary>
        /// Adds EXP to current entity, when exceeding required
        /// EXP count performs leveling up
        /// </summary>
        /// <param name="amount">Exp amount to be added</param>
        public virtual void AddExp(int amount)
        {
            Stats.CurrentExp += amount;
            if (Stats.CurrentExp < Stats.RequiredExp) return;

            // If added EXP exceeds required EXP for this LVL, then
            // move tailing EXP to the next LVL progress bar
            var expDifference = Stats.CurrentExp - Stats.RequiredExp;
            LevelUp();
            AddExp(expDifference);
        }

        /// <summary>
        /// Updates Entities LVL by 1
        /// </summary>
        protected virtual void LevelUp()
        {
            Attributes.Points += POINTS_PER_LEVEL;
            Stats.LevelUp();
            Health = Stats.MaxHealth;
            Magic = Stats.MaxMagic;
        }

        /// <summary>
        /// Takes damage AKA removes calculated amount of points
        /// from entities HP
        /// </summary>
        /// <param name="hitPoints">RAW hit point count</param>
        /// <returns>Real damage</returns>
        public virtual int TakeDamage(int hitPoints)
        {
            // Calculates REAL hit points, by taking entities
            // damage into account
            var maxDamage = hitPoints - Stats.CurrentDefense / 4 + 1;
            var realDamage = Random.Range(1, (maxDamage < 2) ? 2 : (int) maxDamage);
            Health = -realDamage;
            return realDamage;
        }

        /// <summary>
        /// Action that is performed when entities health is less or
        /// equal to 0 AKA when entity is dead
        /// </summary>
        protected virtual void Death()
        {
            throw new NotImplementedException();
        }
    }
}
