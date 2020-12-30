using Assets.Scripts.Misc;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class ThrowingDagger : MonoBehaviour
    {
        public int Distance = 2;

        public float Speed = 5;
        public int DeltaSpeed = 5;
        public int MinSpeed = 1;
        public int RevertSpeed = 5;

        private Vector2 _startPosition;
        private bool _moveBack;
        private Transform _parent;

        private void Start()
        {
            _startPosition = transform.position;
            _parent = Util.GetCharacterTransform();
            transform.position = _parent.transform.position;
        }

        private void Update()
        {
            if (!_moveBack)
            {
                if (Vector2.Distance(_startPosition, transform.position) < Distance)
                {
                    transform.position += transform.right * Time.deltaTime * Speed;
                    Speed -= Time.deltaTime * DeltaSpeed;
                    if (Speed < MinSpeed)
                        Speed = MinSpeed;
                }
                else
                {
                    _moveBack = true;
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, _parent.position, RevertSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, _parent.position) < 0.1f)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
