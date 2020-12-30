using System.Collections.Generic;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Translator;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.User.Attributes
{
    public class AttributeController : Popup
    {
        private static CharacterEntity _characterEntity;
        private static CharacterEntity CharacterEntity => _characterEntity ?? (_characterEntity = DI.Fetch<CharacterEntity>());

        private static AttributeData Attribute => CharacterEntity.Attributes;

        private static int Points {
            get => Attribute.Points;
            set => Attribute.Points = value;
        }

        private static int Level => CharacterEntity.Stats.CurrentLevel;

        private AttributeData _backupAttributeData;
        private int _backupPoints;
        private bool _isModified;

        private int _activeItemId = -1;
        private Vector2 _scrollPosition = Vector2.zero;

        protected override void Init()
        {
            RectConfig.Title = T.Translate("ATTRIBUTES");
            RectConfig.Popup.y = ScreenSize.y / 2 - 283;
            RectConfig.Popup.height = 515;
        }

        protected override void DrawBody()
        {
            GUI.Box(new Rect(65 ,55, 100, 30), $"Level: {Level}", "att_points");
            GUI.Box(new Rect(175, 55, 100, 30), $"Points: {Points}", "att_points");

            _scrollPosition = GUI.BeginScrollView(
                new Rect(35, 95, 285, 295), 
                _scrollPosition, 
                new Rect(0, 0, 250, 295 + (_activeItemId == -1 ? 0 : 60))
            );

            for (short i = 0; i < 6; i++)
            {
                var offsetY = (_activeItemId != -1 && i > _activeItemId) ? 60 : 0;
                var top = 50 * i + offsetY;

                GUI.Box(new Rect(0, top, 240, 40), LMC.CachedMessages[i], "att_label");
                GUI.Box(new Rect(140, top, 80, 40), $"{ _backupAttributeData[i] }", "att_label_dark");
                if (GUI.Button(new Rect(200, top, 40, 40), "+", "att_btn")) { AddPoint(i); }
                if (GUI.Button(new Rect(120, top, 40, 40), "-", "att_btn")) { RemovePoint(i); }

                // Info
                if (GUI.Button(new Rect(245, top + 10, 20, 20), "?", "att_info_btn"))
                {
                    _activeItemId = (_activeItemId == i) ? -1 : i;
                }

                if (_activeItemId != i) continue;
                GUI.Box(new Rect(115, top + 40, 20, 10), "", "inv_item_active_bg_top");
                GUI.Box(new Rect(0, top + 50, 265, 50), LMC.CachedMessages[i + 6], "inv_item_active_bg");
            }

            GUI.EndScrollView();
        }

        /**
         * Confirm & Reset BTN
         */
        protected override void DrawOverlay()
        {
            if (!_isModified) return;

            if (GUI.Button(new Rect(10, 465, 170, 50), LMC.CachedMessages[13], "att_action_btn"))
            {
                Points = _backupPoints;
                Reset();
            }
            if (GUI.Button(new Rect(200, 465, 170, 50), LMC.CachedMessages[12], "att_action_btn") ) { Confirm(); }
        }


        public override void Toggle(bool state)
        {
            if (state)
            {
                Reset();
            }
            else
            {
                Points = _backupPoints;
                _activeItemId = -1;
            }

            base.Toggle(state);
        }

        private void Reset()
        {
            _isModified = false;
            _backupPoints = Points;
            _backupAttributeData = new AttributeData();
            AttributeData.MoveData(_backupAttributeData, Attribute);
        }

        private void Confirm()
        {
            AttributeData.MoveData(Attribute, _backupAttributeData);
            Reset();
        }

        protected void AddPoint(int attributeId)
        {
            if (Points <= 0) return;

            _backupAttributeData[attributeId]++;
            _isModified = true;
            Points--;
        }

        protected void RemovePoint(int attributeId)
        {
            var attValue = _backupAttributeData[attributeId];
            if (attValue <= 0) return;

            var originalValue = Attribute[attributeId];
            if (attValue <= originalValue) return;

            _backupAttributeData[attributeId]--;
            _isModified = true;
            Points++;
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
