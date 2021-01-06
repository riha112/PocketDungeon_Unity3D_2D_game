using Assets.Scripts.Misc;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    /// <summary>
    /// Controller for Throwing dagger:
    /// - Moves to specific distance from character
    /// - When distance is met, returns back to character
    /// </summary>
    public class ThrowingDagger : MonoBehaviour
    {
        /// <summary>
        /// Maximum throwing distance from
        /// character to dagger, before returning
        /// </summary>
        public int Distance = 2;

        /// <summary>
        /// Start speed for when dagger is moving forward
        /// </summary>
        public float Speed = 5;

        /// <summary>
        /// Speed reduced by time while dagger is moving forward, to give
        /// slowing effect
        /// </summary>
        public int DeltaSpeed = 5;

        /// <summary>
        /// Minimum amount of speed that can be reached while moving
        /// forwards (protects from speed of 0, or less that 0 while moving
        /// forward as its speed is damped by time)
        /// </summary>
        public int MinSpeed = 1;

        /// <summary>
        /// Speed with which dagger returns back to player
        /// </summary>
        public int RevertSpeed = 5;

        /// <summary>
        /// Start position, used to calculate moved distance, as user
        /// can be moving, thus changing distance calculations
        /// </summary>
        private Vector2 _startPosition;

        /// <summary>
        /// Is the dagger moving forward or back to player
        /// </summary>
        private bool _moveBack;

        /// <summary>
        /// Who trowed the knife;
        /// </summary>
        private Transform _parent;

        private void Start()
        {
            _startPosition = transform.position;
            _parent = Util.GetCharacterTransform();
            transform.position = _parent.transform.position;
        }

        private void Update()
        {
            // Moving forward
            if (!_moveBack)
            {
                if (Vector2.Distance(_startPosition, transform.position) < Distance)
                {
                    // Moves forward
                    transform.position += transform.right * Time.deltaTime * Speed;

                    // Dampens speed per time
                    Speed -= Time.deltaTime * DeltaSpeed;

                    // Locks speed into range
                    if (Speed < MinSpeed)
                        Speed = MinSpeed;
                }
                else
                {
                    _moveBack = true;
                }
            }
            // Returning back to player
            else
            {
                transform.position =
                    Vector2.MoveTowards(transform.position, _parent.position, RevertSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, _parent.position) < 0.1f) Destroy(gameObject);
            }
        }
    }
}