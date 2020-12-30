using UnityEngine;

namespace Assets.Scripts.Helper
{
    /// <summary>
    /// Helper class that destroys GameObject after specific time
    /// </summary>
    public class DestroyAfter : MonoBehaviour
    {
        public float Timer = 1;

        void Start()
        {
            Destroy(gameObject, Timer);
        }
    }
}
