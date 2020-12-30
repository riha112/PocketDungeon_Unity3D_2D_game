using Assets.Scripts.Misc;
using UnityEngine;

namespace Assets.Scripts.Helper
{
    public class LookAtCursor : MonoBehaviour
    {
        private void Update()
        {
            transform.right = Util.GetDirectionTowardsCursor(transform);
        }
    }
}
