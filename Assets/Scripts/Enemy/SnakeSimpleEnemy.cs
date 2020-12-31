using Assets.Scripts.Misc;
using Assets.Scripts.Weapons.Projectile;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    /// <summary>
    /// Implements support for enemies that haw multiple
    /// part bodies in where each part follows each other
    /// </summary>
    public class SnakeSimpleEnemy : SimpleEnemy
    {
        /// <summary>
        /// Body parts... part n follow n - 1
        /// </summary>
        public Transform[] BodyParts;

        /// <summary>
        /// Follow speed for body parts
        /// </summary>
        public float DeltaFollowSpeed;

        /// <summary>
        /// Timer at which to update movement direction
        /// (_updateLookAtTimer is reset back to this value)
        /// </summary>
        public float UpdateSpeed = 1;

        /// <summary>
        /// Speed of heads rotation
        /// </summary>
        public float RotationSpeedDelta = 5;


        /// <summary>
        /// Actual timer for movement direction change
        /// </summary>
        private float _updateLookAtTimer;


        /// <inheritdoc cref="SimpleEnemy"/>
        protected override void Start()
        {
            base.Start();

            // Fixes Z-indexes for body parts
            for (var i = 0; i < BodyParts.Length; i++)
            {
                BodyParts[i].transform.localPosition = new Vector3(0, 0, i * 0.1f);
            }

            GetComponent<DamageOnCollisionPlayer>().HitPoints = Stats.HitPoints;
        }

        /// <inheritdoc cref="SimpleEnemy"/>
        protected override void Idle()
        {
            // Rotates around itself
            transform.position += transform.right * Stats.CurrentSpeed * Time.deltaTime;
            transform.Rotate(0, 0, Time.deltaTime * Stats.CurrentSpeed * 30);
        }

        /// <inheritdoc cref="SimpleEnemy"/>
        protected override void Attack()
        {
            // Enemy deal damage by collision, thus it's just moving always
            // towards its target.
            WalkTowardsTarget();
        }

        /// <inheritdoc cref="SimpleEnemy"/>
        protected override void WalkTowardsTarget()
        {
            _updateLookAtTimer -= Time.deltaTime;
            if (_updateLookAtTimer < 0)
            {
                transform.right = Util.LookAt2D(Target, transform);
                _updateLookAtTimer = UpdateSpeed;
            }

            transform.right = Vector2.Lerp(transform.right, Util.LookAt2D(Target, transform), RotationSpeedDelta * Time.deltaTime);
            transform.position += transform.right * Stats.CurrentSpeed * Time.deltaTime;
        }

        /// <inheritdoc cref="SimpleEnemy"/>
        protected override void Death()
        {
            // Loads exp points for each body part
            foreach (var bodyPart in BodyParts)
            {
                InitializeExpPoint(bodyPart);
            }
            Destroy(transform.parent.gameObject);
        }

        /// <summary>
        /// Moves each n body part towards n - 1 body part
        /// </summary>
        private void MoveBodyParts()
        {
            // Start at 1, because of logic n - 1
            for (var i = 1; i < BodyParts.Length; i++)
            {
                // Give some distance between body parts, to prevent overlapping
                if (!(Vector2.Distance(BodyParts[i].position, BodyParts[i - 1].position) > 0.5f))
                    continue;

                // Smooth movement towards specific point
                BodyParts[i].position = Vector2.Lerp(
                    BodyParts[i].position, BodyParts[i - 1].position,
                    DeltaFollowSpeed * Time.deltaTime
                );

                // Sets rotation
                BodyParts[i].right = Util.LookAt2D(BodyParts[i], BodyParts[i - 1]);
            }
        }

        /// <inheritdoc cref="SimpleEnemy"/>
        protected new void Update()
        {
            base.Update();
            MoveBodyParts();
        }
    }
}
