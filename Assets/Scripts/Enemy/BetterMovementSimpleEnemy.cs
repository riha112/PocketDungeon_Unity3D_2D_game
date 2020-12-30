using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    /// <summary>
    /// Extension of <see cref="SimpleEnemy"/>, adds
    /// weight based movement system.
    ///
    /// Weight based movement system:
    /// - Uses rays to check surrounding objects
    /// - Assigns weight to each ray, based by DOT product of two Vectors
    /// - Corrects weights by taking distance, hit object type etc. into account
    /// - Moves towards ray with largest weight
    /// </summary>
    public class BetterMovementSimpleEnemy : SimpleEnemy
    {
        /// <summary>
        /// Directions of Rays (12-way check around entity)
        /// Circle type check
        /// </summary>
        private static readonly Vector2[] Directions = new[]
        {
            Vector2.up,
            -Vector2.up,
            Vector2.right,
            -Vector2.right,
            (Vector2.right * 0.866f + Vector2.up * 0.5f),
            (Vector2.right * 0.5f + Vector2.up * 0.866f),

            (Vector2.right * 0.866f - Vector2.up * 0.5f),
            (Vector2.right * 0.5f - Vector2.up * 0.866f),

            (-Vector2.right * 0.866f + Vector2.up * 0.5f),
            (-Vector2.right * 0.5f + Vector2.up * 0.866f),

            (-Vector2.right * 0.866f - Vector2.up * 0.5f),
            (-Vector2.right * 0.5f - Vector2.up * 0.866f)
        };

        /// <summary>
        /// Weights of each of ray
        /// </summary>
        private readonly List<float> _weights = new List<float>() {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        private Vector2 _targetDirection;
        private Vector2 _activeDirection;

        /// <summary>
        /// Weight recalculation time
        /// </summary>
        private float _movementTimer = 0;


        /// <inheritdoc cref="SimpleEnemy"/>
        protected override void Attack()
        {
            WalkTowardsTarget();
            base.Attack();
        }

        /// <inheritdoc cref="SimpleEnemy"/>
        protected override void WalkTowardsTarget()
        {
            // If timer < 0 then recalculates weights
            _movementTimer -= Time.deltaTime;
            if (_movementTimer < 0)
            {
                UpdateMovement();
                _movementTimer = 0.25f;
            }

            // To prevent from "jagged" movement, meaning that enemy is moving towards one point
            // and then all of the sudden moving to complete different point. I added Lerp function
            // between previous point and current point to make smooth transaction
            _activeDirection = Vector2.Lerp(_activeDirection, _targetDirection, Time.deltaTime);
            transform.position = Vector2.MoveTowards(transform.position, _activeDirection, Stats.CurrentSpeed * Time.deltaTime);
            Animator?.SetBool("isWalking", true);
        }

        /// <summary>
        /// Recalculates weights & sets new movement point based
        /// on previously calculated weights 
        /// </summary>
        private void UpdateMovement()
        {
            CalculateWeights();
            SetActiveDirection();
        }

        /// <summary>
        /// Sets active movement point to be 3.2m away from current point towards
        /// direction with largest weight
        /// </summary>
        private void SetActiveDirection()
        {
            // Finds weights id with larges value
            var maxId = 0;
            for (var i = 1; i < _weights.Count; i++)
                if (_weights[i] > _weights[maxId])
                    maxId = i;

            // Sets current target direction as previous (for smooth transaction)
            _activeDirection = _targetDirection;

            // Sets target direction as point that's 3.2m away from enemy
            // 3.2m are used as its adequate amount of distance for enemy to
            // make within given refresh time.
            _targetDirection = new Vector2(transform.position.x + Directions[maxId].x * 3.2f, transform.position.y + Directions[maxId].y * 3.2f);
        }

        /// <summary>
        /// Calculates direction weights by DOT product & collider distance & type
        /// </summary>
        private void CalculateWeights()
        {
            for (var i = 0; i < _weights.Count; i++)
            {
                _weights[i] = 1f;

                var hit = Physics2D.Raycast(transform.position, Directions[i]);
                var hitAnEnemy = false;
                var modifier = 1f;

                // Makes this enemy a center point of grid (0, 0)
                var to = new Vector2(Target.position.x - transform.position.x, Target.position.y - transform.position.y);

                if (IsInAttackRange)
                {
                    var distance = Vector2.Distance(Directions[i], to);
                    modifier =  distance < Info.AttackDistance ? distance / Info.AttackDistance : 1;
                }

                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Player" && IsInAttackRange)
                    {
                        _weights[i] = 0;
                    }
                    else
                    {
                        var distance = Vector2.Distance(hit.point, transform.position);
                        if (distance < 1)
                        {
                            _weights[i] = 0;
                            hitAnEnemy = hit.collider.tag == "Enemy";
                        }
                        else if (distance < 2)
                        {
                            hitAnEnemy = hit.collider.tag == "Enemy";
                            _weights[i] = 0.8f;
                        }
                    }
                }

                // Calculates Dot product from possible movement direction to target
                // + 1 / 2 => Dot product range is from -1 to 1, but we need range from 0 to 1
                var dot = (Vector2.Dot(Directions[i].normalized, to.normalized) + 1) / 2;
                _weights[i] *= (hitAnEnemy? 1 - Math.Abs(dot - 0.65f) : dot) * modifier;
            }
        }
    }
}
