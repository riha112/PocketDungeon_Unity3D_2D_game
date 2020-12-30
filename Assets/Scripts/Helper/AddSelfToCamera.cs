using UnityEngine;

namespace Assets.Scripts.Helper
{
    /// <summary>
    /// Helper class that adds GameObject to Main camera on start
    /// </summary>
    public class AddSelfToCamera : MonoBehaviour
    {
        void Awake()
        {
            transform.parent = Camera.main.transform;
        }
    }
}
