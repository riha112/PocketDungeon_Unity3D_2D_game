using UnityEngine;

namespace Assets.Scripts.Misc.GUI
{
    /// <summary>
    /// Shows popup box with title...
    /// <seealso cref="UI"/>
    /// </summary>
    public class Popup : Searchable
    {
        /// <inheritdoc cref="PopupConfig"/>
        protected PopupConfig RectConfig { get; set; } = new PopupConfig()
        {
            ShowBackground = true,
            Popup = new Rect(0, 145, 380, 480),
            Body = new Rect(20, 30, 340, 420),
            TitleRect = new Rect(90, 0, 200, 80)
        };

        /// <inheritdoc cref="UI"/>
        public override int Depth { get; set; } = 10;

        /// <inheritdoc cref="UI"/>
        protected override void Awake()
        {
            ScreenSize = new Vector2(Screen.width, Screen.height);
            RectConfig.Popup.x = ScreenSize.x / 2 - RectConfig.Popup.width / 2;
            RectConfig.Popup.y = ScreenSize.y / 2 - RectConfig.Popup.height / 2 - 40;
            base.Awake();
        }

        /// <inheritdoc cref="UI"/>
        protected override void Design()
        {
            DrawBackground();
            DrawPopup();
        }

        /// <summary>
        /// Contains design that is INSIDE popups main body
        /// </summary>
        protected virtual void DrawBody()
        {

        }

        /// <summary>
        /// Contains design that is OUTSIDE popups main body
        /// </summary>
        protected virtual void DrawOverlay()
        {
        }

        /// <summary>
        /// Contains main structure of Popup all segments via GUI.BeginGroup
        /// </summary>
        protected virtual void DrawPopup()
        {
            UnityEngine.GUI.BeginGroup(RectConfig.Popup);
            UnityEngine.GUI.BeginGroup(RectConfig.Body, "", "popup_body");
            DrawBody();
            UnityEngine.GUI.EndGroup();
            DrawOverlay();
            DrawTitle();
            UnityEngine.GUI.EndGroup();
        }

        /// <summary>
        /// If title is set then outputs title via GUI.Box
        /// </summary>
        protected virtual void DrawTitle()
        {
            if (string.IsNullOrEmpty(RectConfig.Title))
                return;
            UnityEngine.GUI.Box(RectConfig.TitleRect, RectConfig.Title, "popup_title");
        }

        /// <summary>
        /// Draws background via GUI.Box
        /// </summary>
        protected virtual void DrawBackground()
        {
            if (RectConfig.ShowBackground)
            {
                UnityEngine.GUI.Box(new Rect(0, 0, ScreenSize.x, ScreenSize.y), "", "popup_bg");
            }
        }
    }
}
