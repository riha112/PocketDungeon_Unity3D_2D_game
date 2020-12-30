using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Misc;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.World.MapDraw;
using UnityEngine;

namespace Assets.Scripts.User.Map
{
    public class MapController : UI.UI
    {
        public Texture2D MapTexture2D;
        public Texture2D CharacterTexture;

        private Texture2D _mapForeground;
        private float _pixelSize;
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

        private Transform _character;

        private Transform Character
        {
            get
            {
                if (_character == null)
                {
                    _character = Util.GetCharacterTransform();
                }
                return _character;
            }
        }

        protected override int Depth { get; set; } = 10;

        protected override void Design()
        {
            DrawBackground();
            GUI.DrawTexture(new Rect(ScreenSize.x / 2 - 225, ScreenSize.y / 2 - 225, 450, 450), MapTexture2D);
            GUI.DrawTexture(new Rect(ScreenSize.x / 2 - 150, ScreenSize.y / 2 - _mapHeight / 2, 300, _mapHeight), MapForeground);
            GUI.DrawTexture(new Rect(
                ScreenSize.x / 2 + (Character.position.x / 0.8f) * _pixelSize, 
                ScreenSize.y / 2 - (Character.position.y / 0.8f) * _pixelSize,
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
