using Assets.Scripts.Misc;
using Assets.Scripts.Misc.ObjectManager;
using UnityEngine;

namespace Assets.Scripts.User.Controller
{
    // TODO: REFACTOR WHOLE CLASS
    public class MovementController : MonoBehaviour
    {
        public float Speed = 2;
        public float Sprint = 4;

        public GameObject[] DustPrefab;
        private float _dustTimer = 1;

        private Rigidbody2D _rigidbody;
        private Transform _camera;
        private float _activeSpeed;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _camera = Camera.main.transform;

            DI.Register(this);
        }

        /**
         * Bouncing backwards from specific point
         */
        public void BounceOff()
        {
            _rigidbody.AddForce(Util.GetDirectionBackwardsCursor(transform) * 10);
        }

        private void InstallDust(int id = 0)
        {
            var d = Instantiate(DustPrefab[id]);
            d.transform.position = new Vector3(transform.position.x, transform.position.y, 5);
            d.transform.Rotate(0, 0, Random.Range(0, 360));
        }

        private void InstallWoff(int id = 2)
        {
            var d = Instantiate(DustPrefab[id]);
            d.transform.position = new Vector3(transform.position.x, transform.position.y, 5);
            d.transform.right = Util.GetDirectionTowardsCursor(d.transform);
        }

        /// <summary>
        /// Makes so that camera follows player
        /// </summary>
        private void CameraFollow()
        {
            _camera.transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                _camera.transform.position.z
            );
        }

        private void ImpulseMove()
        {
            _rigidbody.AddForce(Util.GetDirectionTowardsCursor(transform) * 250);
            InstallWoff();
            InstallDust(1);
        }

        private void Update()
        {
            CameraFollow();

            #region MyRegion
            // Initiates dust particle every X seconds
            if (_dustTimer < 0)
            {
                _dustTimer = 1;
                InstallDust(_activeSpeed == Sprint ? Random.Range(0, 2) : 0);
            }
            #endregion

            #region Impulse
            // Movement for impulse
            if (Input.GetMouseButtonUp(2))
            {
                ImpulseMove();
            }
            #endregion

            #region Speed change effect
            var newSpeed = Input.GetKey(KeyCode.LeftShift) ? Sprint : Speed;

            // If speed has changed then initiate dust particle
            if (newSpeed != _activeSpeed)
                InstallDust(1);

            _activeSpeed = newSpeed;
            #endregion

            #region Movement Axis
            var movementHorizontal = Input.GetAxis("Horizontal");
            var movementVertical = Input.GetAxis("Vertical");
            #endregion


            if (movementHorizontal != 0)
            {
                transform.position += Vector3.right * Time.deltaTime * movementHorizontal * _activeSpeed;

                var prev = transform.localScale.x;
                var newL = movementHorizontal < 0 ? 1 : -1;
                if (prev != newL) InstallDust();

                transform.localScale = new Vector3(newL, 1, 1);
            }

            if (movementVertical != 0)
                transform.position += Vector3.up * Time.deltaTime * movementVertical * _activeSpeed;

            if (movementVertical != 0 || movementHorizontal != 0)
                _dustTimer -= Time.deltaTime * (_activeSpeed / 2);

            _animator.SetBool("IsMoving", movementVertical != 0 || movementHorizontal != 0);
        }
    }
}