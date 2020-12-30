using Assets.Scripts.Misc;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    /// <summary>
    /// Enemy AI logic for Bat like entities
    /// - Flying (ignoring collider)
    /// - Rotating around enemy
    /// - Attacking by swopping in
    /// </summary>
    public class BatEnemy : SimpleEnemy
    {
        /// <summary>
        /// Players center by default is at its feet,
        /// so Core is "actual" targets mid point 
        /// </summary>
        public Transform Core;

        /// <summary>
        /// When swooping in remembers previous position to
        /// which return after attack is done
        /// </summary>
        private bool _moveBack;
        private Vector2 _prevPosition;

        /// <inheritdoc cref="SimpleEnemy"/>
        protected override void Idle () { /* Do nothing */ }


        /// <inheritdoc cref="SimpleEnemy"/>
        protected override void SetBaseValues()
        {
            base.SetBaseValues();
            SetAttackCoolDown();
        }

        /// <summary>
        /// Sets random cooldown for attack
        /// </summary>
        private void SetAttackCoolDown()
        {
            _currentAttackCooldownTimer = Random.Range(Info.BaseCoolDownForAttack - 1, Info.BaseCoolDownForAttack + 3);
        }

        /// <inheritdoc cref="SimpleEnemy"/>
        public override int TakeDamage(int damage)
        {
            // If hit while attacking, then retreat.
            if (!_moveBack && _currentAttackCooldownTimer < 0)
            {
                _moveBack = true;
            }

            return base.TakeDamage(damage);
        }

        /// <inheritdoc cref="SimpleEnemy"/>
        protected override void Attack()
        {
            _currentAttackCooldownTimer -= Time.deltaTime;

            if (_currentAttackCooldownTimer < 0)
            {
                // After initializing attack towards player, bat returns back to correct 
                // distance to start circling player again
                if (_moveBack)
                {
                    transform.position = Vector2.MoveTowards(transform.position, _prevPosition, Stats.CurrentSpeed * Time.deltaTime * 3);
                    if (Vector2.Distance(Target.position, transform.position) >= Info.AttackDistance - 0.1f ||
                        Vector2.Distance(transform.position, _prevPosition) <= 0.1f)
                    {
                        _moveBack = false;
                        SetAttackCoolDown();
                    }
                }
                // Moves towards player to deal damage.
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, Target.position, Stats.CurrentSpeed * Time.deltaTime * 4);
                    if (Vector2.Distance(Target.position, transform.position) < 0.1f)
                    {
                        _moveBack = true;
                        TargetData.TakeDamage(Stats.HitPoints);
                    }
                }
            }
            // Circles player for x amount of time
            else
            {
                Core.transform.right = Util.LookAt2D(Core, Target);
                transform.position = Vector2.MoveTowards(transform.position, transform.position + Core.transform.up, Stats.CurrentSpeed * Time.deltaTime * 1.5f);
                _currentAttackCooldownTimer -= Time.deltaTime;
                _prevPosition = transform.position;
            }

            Animator?.SetBool("isWalking", true);
        }
    }
}
