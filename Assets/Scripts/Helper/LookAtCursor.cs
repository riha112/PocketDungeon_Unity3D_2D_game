using Assets.Scripts.Misc;
using UnityEngine;

namespace Assets.Scripts.Helper
{
    /// <summary>
    /// Helper class that transforms ones rotation, so that it "Looks At cursor"
    /// </summary>
    public class LookAtCursor : MonoBehaviour
    {
        private void Update()
        {
            transform.right = Util.GetDirectionTowardsCursor(transform);
        }
    }
}
