using Assets.Scripts.Items;
using Assets.Scripts.Misc.GUI;
using Assets.Scripts.Misc.Translator;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.User.Inventory
{
    /// <summary>
    /// UI for items information
    /// <seealso cref="InventoryPopupUi"/>
    /// </summary>
    public class ItemInformation : Popup
    {
        private bool _isVisible;
        private SimpleItem _item;

        public override int Depth => 5;

        /// <summary>
        /// Called on start of object
        /// </summary>
        protected override void Init()
        {
            ToggleEvent += OnTogglePopup;
            RectConfig.Title = T.Translate("Info");
        }

        /// <summary>
        /// Hide popup if any other popup is toggled
        /// </summary>
        /// <param name="sender">Popup that is toggled</param>
        /// <param name="state">State of toggled popup</param>
        private void OnTogglePopup([CanBeNull] object sender, bool state)
        {
            if (sender is Popup && state && !(sender is ItemInformation))
                _isVisible = false;
        }

        /// <summary>
        /// Toggles information screen & displays info about item
        /// </summary>
        /// <param name="item">Item about whom to show info</param>
        public void ShowInfoAbout(SimpleItem item)
        {
            _isVisible = item != null;
            _item = item;
        }

        /// <inheritdoc cref="UI"/>
        public override void GuiDraw()
        {
            if (!_isVisible)
                return;

            // Hides when left click is pressed
            if (Input.GetMouseButtonDown(0))
                _isVisible = false;

            base.GuiDraw();
        }

        protected override void DrawBody()
        {
            GUI.Box(new Rect(125, 55, 90, 105), _item.Info.Icon.texture, "info_icon");
            GUI.Label(new Rect(85 ,165, 170, 50), _item.Info.Title, "info_title");
            GUI.Label(new Rect(42.5f, 215, 255, 165), _item.GetDescription(), "info_description");
        }
    }
}
