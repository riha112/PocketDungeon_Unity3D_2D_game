using Assets.Scripts.Misc;
using Assets.Scripts.Misc.ObjectManager;
using UnityEngine;

namespace Assets.Scripts.User.Controller
{
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
         * Two-dimensional movement up-down / left-right
         */
        protected void Move()
        {

        }

        /**
         * Movement int up-down direction via ladder
         */
        protected void Climb()
        {

        }

        /**
         * Climbing onto elements with max-height of 1.0
         */
        protected void Jump()
        {

        }

        /**
         * Rush towards AKA adding rigidbody force toward specific point
         */
        protected void ForceTowards()
        {

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

        void Update()
        {
            if (_dustTimer < 0)
            {
                _dustTimer = 1;
                InstallDust(_activeSpeed == Sprint ? Random.Range(0, 2) : 0);
            }

            _camera.transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                _camera.transform.position.z
            );

            if (Input.GetMouseButtonUp(2))
            {
                _rigidbody.AddForce(Util.GetDirectionTowardsCursor(transform) * 250);
                InstallWoff();
                InstallDust(1);
            }

            var newSpeed = Input.GetKey(KeyCode.LeftShift) ? Sprint : Speed;
            if(newSpeed != _activeSpeed)
                InstallDust(1);

            _activeSpeed = newSpeed;

            var movementHorizontal = Input.GetAxis("Horizontal");
            var movementVertical = Input.GetAxis("Vertical");
            if (movementHorizontal != 0)
            {
                transform.position += Vector3.right * Time.deltaTime * movementHorizontal * _activeSpeed;

                var prev = transform.localScale.x;
                var newL = movementHorizontal < 0 ? 1 : -1;
                if (prev != newL)
                {
                    InstallDust();
                }

                transform.localScale = new Vector3(newL, 1, 1);
            }

            if (movementVertical != 0)
            {
                transform.position += Vector3.up * Time.deltaTime * movementVertical * _activeSpeed;
            }

            if (movementVertical != 0 || movementHorizontal != 0)
            {
                _dustTimer -= Time.deltaTime * (_activeSpeed / 2);
            }

            _animator.SetBool("IsMoving", movementVertical != 0 || movementHorizontal != 0);
        }
    }
}
