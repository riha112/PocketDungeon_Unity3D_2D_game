using UnityEngine;

namespace Assets.Scripts.World
{
    /// <summary>
    /// Sets main cursors texture
    /// </summary>
    public class CursorStyle : MonoBehaviour
    {
        public Texture2D CursorTexture2D;

        private void Awake()
        {
            Cursor.SetCursor(CursorTexture2D, Vector2.zero, CursorMode.Auto);
        }
    }
}