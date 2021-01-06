using UnityEngine;

namespace Assets.Scripts.Weapons
{
    /// <summary>
    /// Controller for Wrecking ball
    /// - Extends to specific size based on distance between character and mouse
    /// - Rotates wrecking ball
    /// </summary>
    public class RotatingWreckingBall : MonoBehaviour
    {
        public Transform Ball;
        public int MaxDistance = 2;
        public float Delta = 0.5f;
        public float LerpSpeed = 5;
        public float RotationSpeed = 5;

        public SpriteRenderer Chain;
        public float ChainSizeDelta = 0.19f;

        private Vector2 _targetPosition;
        private float _chainSizeX;

        private void Start()
        {
            if (Ball == null && transform.childCount > 0)
                Ball = transform.GetChild(0);

            _chainSizeX = Chain.size.x;
        }

        private void Update()
        {
            // Rotate parent itself
            transform.Rotate(0, 0, RotationSpeed * Time.deltaTime);

            // Move ball (distance from center)
            if (Vector2.Distance(_targetPosition, Ball.transform.position) > 0.1f)
                Ball.localPosition = Vector2.Lerp(Ball.localPosition, _targetPosition, Time.deltaTime * LerpSpeed);

            // Calculate distance
            _targetPosition =
                new Vector2(
                    Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) * Delta,
                    0);
            if (_targetPosition.x > MaxDistance)
                _targetPosition.x = MaxDistance;


            // Change of chains size
            Chain.size = new Vector2(
                _chainSizeX,
                Ball.transform.localPosition.x * ChainSizeDelta
            );
        }
    }
}