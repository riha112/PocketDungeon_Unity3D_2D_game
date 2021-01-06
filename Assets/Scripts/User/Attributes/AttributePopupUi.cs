using System.Collections.Generic;
using Assets.Scripts.Misc.GUI;
using Assets.Scripts.Misc.Translator;
using UnityEngine;

namespace Assets.Scripts.User.Attributes
{
    public class AttributePopupUi : Popup
    {
        private int _activeItemId = -1;
        private Vector2 _scrollPosition = Vector2.zero;

        protected override void Init()
        {
            AttributeManager.Reset();
            RectConfig.Title = T.Translate("ATTRIBUTES");
            RectConfig.Popup.y = ScreenSize.y / 2 - 283;
            RectConfig.Popup.height = 515;
        }

        protected override void DrawBody()
        {
            GUI.Box(new Rect(65 ,55, 100, 30), $"Level: {AttributeManager.Level}", "att_points");
            GUI.Box(new Rect(175, 55, 100, 30), $"Points: {AttributeManager.Points}", "att_points");

            _scrollPosition = GUI.BeginScrollView(
                new Rect(35, 95, 285, 295), 
                _scrollPosition, 
                new Rect(0, 0, 250, 295 + (_activeItemId == -1 ? 0 : 60))
            );

            for (short i = 0; i < 6; i++)
            {
                var offsetY = (_activeItemId != -1 && i > _activeItemId) ? 60 : 0;
                var top = 50 * i + offsetY;

                GUI.Box(new Rect(0, top, 240, 40), LMC[i], "att_label");
                GUI.Box(new Rect(140, top, 80, 40), $"{ AttributeManager.GetPointsFor(i) }", "att_label_dark");
                if (GUI.Button(new Rect(200, top, 40, 40), "+", "att_btn")) { AttributeManager.AddPointTo(i); }
                if (GUI.Button(new Rect(120, top, 40, 40), "-", "att_btn")) { AttributeManager.RemovePointFrom(i); }

                // Info
                if (GUI.Button(new Rect(245, top + 10, 20, 20), "?", "att_info_btn"))
                {
                    _activeItemId = (_activeItemId == i) ? -1 : i;
                }

                if (_activeItemId != i) continue;
                GUI.Box(new Rect(115, top + 40, 20, 10), "", "inv_item_active_bg_top");
                GUI.Box(new Rect(0, top + 50, 265, 50), LMC[i + 6], "inv_item_active_bg");
            }

            GUI.EndScrollView();
        }

        /**
         * Confirm & Reset BTN
         */
        protected override void DrawOverlay()
        {
            if (!AttributeManager.IsModified)
                return;

            if (GUI.Button(new Rect(10, 465, 170, 50), LT("Cancel"), "att_action_btn"))
            {
                AttributeManager.Cancel();
            }

            if (GUI.Button(new Rect(200, 465, 170, 50), LT("Accept"), "att_action_btn"))
            {
                AttributeManager.Save();
            }
        }


        public override void Toggle(bool state)
        {
            if (state)
            {
                AttributeManager.Reload();
            }
            else
            {
                AttributeManager.Cancel();
                _activeItemId = -1;
            }

            base.Toggle(state);
        }

        protected override List<string> GetLmcLibrary() => new List<string>()
        {
            "Strength",     // 0
            "Vitality",     // 1
            "Agility",      // 2
            "Magic",        // 3
            "Resistance",   // 4
            "Luck",         // 5

            "Increases character attack power.",                    // 6
            "Increases character HP amount.",                       // 7
            "Increases characters speed - casting, movement, etc.", // 8
            "Increases characters MP amount",                       // 9
            "Increases characters defense",                         // 10
            "Increases quality and items of chests & loot bags",    // 11

            "Accept", // 12
            "Cancel", // 13
        };
    }
}
