using UnityEngine;

namespace Assets.Scripts.Helper
{
    /// <summary>
    /// Helper class that moves GameObject 
    /// </summary>
    public class Move : MonoBehaviour
    {
        public Vector2 Speed;

        public void Update()
        {
            transform.position += transform.right * Time.deltaTime * Speed.x;
            transform.position += transform.up * Time.deltaTime * Speed.y;
        }
    }
}