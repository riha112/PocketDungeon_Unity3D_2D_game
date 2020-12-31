using UnityEngine;

namespace Assets.Scripts.Misc.GUI
{
    /// <summary>
    /// Configurations for popup type UI elements
    /// </summary>
    public class PopupConfig
    {
        /// <summary>
        /// Title of popup, if empty not shown
        /// </summary>
        public string Title;

        public bool ShowBackground;

        public Rect Popup;
        public Rect Body;
        public Rect TitleRect;
    }
}