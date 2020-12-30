using UnityEngine;

namespace Assets.Scripts.World
{
    public class FadeIn : MonoBehaviour
    {
        public float Speed = 1;
        public Texture2D FadeTexture;

        private float _timer = 1.3f;

        private void OnGUI()
        {
            GUI.depth = -100;
            GUI.color = new Color(0, 0, 0, _timer);

            _timer -= Time.deltaTime / Speed;
            if (_timer < 0)
                enabled = false;

            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeTexture);
        }
    }
}