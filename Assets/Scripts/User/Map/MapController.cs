using Assets.Scripts.Misc;
using Assets.Scripts.Misc.GUI;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.User.Map.MapDraw;
using UnityEngine;

namespace Assets.Scripts.User.Map
{
    /// <summary>
    /// Outputs mini-map of current dungeons floor
    /// </summary>
    public class MapController : UI
    {
        public Texture2D MapTexture2D;

        #region Foreground data
        public Texture2D CharacterTexture;
        private Texture2D _mapForeground;
        
        /// <summary>
        /// Calculated size of one pixel in foreground image
        /// </summary>
        private float _pixelSize;

        /// <summary>
        /// New foreground height based on pixel size
        /// </summary>
        private float _mapHeight;

        private Texture2D MapForeground
        {
            get
            {
                if (_mapForeground == null)
                {
                    _mapForeground = DI.Fetch<MapCreator>()?.GeneratedTexture;
                    _pixelSize = 300.0f / _mapForeground.width;
                    _mapHeight = _pixelSize * _mapForeground.height;
                }

                return _mapForeground;
            }
        }
        #endregion 

        private Transform _character;

        private Transform Character
        {
            get
            {
                if (_character == null) _character = Util.GetCharacterTransform();
                return _character;
            }
        }

        public override int Depth => 10;

        protected override void Design()
        {
            DrawBackground();

            // Map background
            GUI.DrawTexture(new Rect(ScreenSize.x / 2 - 225, ScreenSize.y / 2 - 225, 450, 450), MapTexture2D);

            // Dungeon texture
            GUI.DrawTexture(new Rect(ScreenSize.x / 2 - 150, ScreenSize.y / 2 - _mapHeight / 2, 300, _mapHeight),
                MapForeground);

            // Character position
            GUI.DrawTexture(new Rect(
                ScreenSize.x / 2 + Character.position.x / 0.8f * _pixelSize,
                ScreenSize.y / 2 - Character.position.y / 0.8f * _pixelSize,
                _pixelSize,
                _pixelSize
            ), CharacterTexture);
        }

        protected virtual void DrawBackground()
        {
            GUI.Box(new Rect(0, 0, ScreenSize.x, ScreenSize.y), "", "popup_bg");
        }
    }
}