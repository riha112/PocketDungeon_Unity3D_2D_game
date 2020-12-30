using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class PopupConfig
    {
        public bool ShowBackground;
        public Rect Popup;
        public Rect Body;
        public string Title;
        public Rect TitleRect;
    }

    public class Popup : Searchable
    {
        protected PopupConfig RectConfig { get; set; } = new PopupConfig()
        {
            ShowBackground = true,
            Popup = new Rect(0, 145, 380, 480),
            Body = new Rect(20, 30, 340, 420),
            TitleRect = new Rect(90, 0, 200, 80)
        };

        protected override int Depth { get; set; } = 10;

        protected override void Awake()
        {
            ScreenSize = new Vector2(Screen.width, Screen.height);
            RectConfig.Popup.x = ScreenSize.x / 2 - RectConfig.Popup.width / 2;
            RectConfig.Popup.y = ScreenSize.y / 2 - RectConfig.Popup.height / 2 - 40;
            base.Awake();
        }

        protected override void Design()
        {
            DrawBackground();
            DrawPopup();
        }

        protected virtual void DrawBody()
        {
        }

        protected virtual void DrawOverlay()
        {
        }

        protected virtual void DrawPopup()
        {
            GUI.BeginGroup(RectConfig.Popup);
            GUI.BeginGroup(RectConfig.Body, "", "popup_body");
            DrawBody();
            GUI.EndGroup();
            DrawOverlay();
            DrawTitle();
            GUI.EndGroup();
        }

        protected virtual void DrawTitle()
        {
            if (string.IsNullOrEmpty(RectConfig.Title))
                return;
            GUI.Box(RectConfig.TitleRect, RectConfig.Title, "popup_title");
        }

        protected virtual void DrawBackground()
        {
            if (RectConfig.ShowBackground)
            {
                GUI.Box(new Rect(0, 0, ScreenSize.x, ScreenSize.y), "", "popup_bg");
            }
        }
    }
}
