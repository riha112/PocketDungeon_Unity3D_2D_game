using Assets.Scripts.Misc;
using UnityEngine;

namespace Assets.Scripts.World.Items
{
    /// <summary>
    /// NOTE: IN PROGRESS
    /// </summary>
    public class SplashPotion : MonoBehaviour
    {
        public float Speed = 1;
        public float ScaleSpeed = 1;

        private float _maxDistance;

        private float _scale = 1;

        private Vector2 _targetPoint;

        private void Start()
        {
        }

        private bool _debug = false;
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.O))
            {
                transform.right = Util.GetDirectionTowardsCursor(transform);
                _maxDistance = Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) + 1;
                Debug.Log(_maxDistance);
                _targetPoint = transform.position + transform.right * _maxDistance;
                _debug = true;
            }

            if(!_debug) return;
            transform.position = Vector2.Lerp(transform.position, _targetPoint,Time.deltaTime * Speed);

            if (!(Vector2.Distance(_targetPoint, transform.position) < 0.1f))
                return;
        }
    }
}
