using Assets.Scripts.Helper;
using Assets.Scripts.Misc;
using Assets.Scripts.Weapons.Projectile;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    /// <summary>
    /// Adds support for enemies that use ranged attacks
    /// </summary>
    public class SimpleRangedEnemy : BetterMovementSimpleEnemy
    {
        /// <summary>
        /// Projectile that is initialized when attacking (arrow / spell)
        /// </summary>
        public GameObject Projectile;

        /// <summary>
        /// Players center by default is at its feet,
        /// so _targetPoint is "actual" targets mid point 
        /// </summary>
        private Transform _targetPoint;


        /// <inheritdoc cref="BetterMovementSimpleEnemy"/>
        protected override void SetComponents()
        {
            base.SetComponents();
            _targetPoint = Util.GetCharacterCenter.transform;
        }

        /// <inheritdoc cref="BetterMovementSimpleEnemy"/>
        protected override void Attack()
        {
            // If enemy is getting closer start moving away from him
            if (Vector2.Distance(transform.position, Target.position) < Info.AttackDistance / 2f)
            {
                WalkTowardsTarget();
            }

            // Cool down timer for attack
            Animator?.SetBool("isCasting", true);
            _currentAttackCooldownTimer -= Time.deltaTime;
            if (_currentAttackCooldownTimer > 0) return;
            _currentAttackCooldownTimer = Info.BaseCoolDownForAttack;

            // Loads projectile
            var projectile = Instantiate(Projectile);
            projectile.transform.position = transform.position;
            projectile.transform.right = Util.LookAt2D(projectile.transform, _targetPoint);
            projectile.GetComponent<EnemyProjectile>().HitPoints = Stats.HitPoints;
            Destroy(projectile, 10);
        }
    }
}
