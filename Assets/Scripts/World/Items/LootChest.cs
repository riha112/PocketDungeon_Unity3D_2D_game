using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Items;
using Assets.Scripts.Misc;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.Misc.Translator;
using Assets.Scripts.Repository;
using Assets.Scripts.Repository.Data;
using Assets.Scripts.UI;
using Assets.Scripts.User;
using Assets.Scripts.User.Controller;
using Assets.Scripts.User.Equipment;
using Assets.Scripts.User.Inventory;
using Assets.Scripts.User.Stats;
using UnityEngine;

namespace Assets.Scripts.World.Items
{
    public class LootChest : Popup
    {
        public int LootTableId;
        public int AccessRange = 2;
        public Sprite[] ChestStates;

        private List<SimpleItem> _chestItems;
        private LootTableData _lootTableData;
        private SpriteRenderer _spriteRenderer;

        private bool _isVisible;
        private bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                if(_spriteRenderer != null)
                    _spriteRenderer.sprite = ChestStates[!_isVisible ? 0 : 1];
            }
        }

        protected override int Depth { get; set; } = 5;

        protected override void Init()
        {
            RectConfig.Title = T.Translate("Loot Chest");
            RectConfig.Popup = new Rect(ScreenSize.x / 2 - 370, ScreenSize.y / 2 - 260, 360, 480);
            RectConfig.Body = new Rect(25, 30, 310, 420);
            RectConfig.TitleRect = new Rect(360 / 2 - 100, 0, 200, 80);

            RectConfig.ShowBackground = false;

            _lootTableData = LootTableRepository.Repository[LootTableId];
        }

        protected void Start()
        {
            BuildLootItems();
            UIController.ActionKeyPress += OnActionKeyPress;
            ToggleEvent += OnTogglePopup;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTogglePopup(object sender, bool state)
        {
            // Hide when any other popup is active (except for inventory)
            if (sender is Popup && state && !(sender is InventoryController))
                IsVisible = false;
        }

        private void OnActionKeyPress(object sender, Vector2 characterPosition)
        {
            if (IsVisible)
            {
                Toggle(false);
                return;
            }

            if (Vector2.Distance(transform.position, characterPosition) < AccessRange)
                ShowChest();
        }

        protected void BuildLootItems()
        {
            var itemCount = R.RandomRange(_lootTableData.MinItems, _lootTableData.MaxItems);

            var userData = DI.Fetch<CharacterEntity>();
            var luck = userData is null ? 0 : (int)userData.Stats.CurrentLuck;
            _chestItems = new List<SimpleItem>();

            for (var i = 0; i < itemCount; i++)
            {
                var item = ItemRepository.GetItemObjectFromId(
                    FetchItemFromLootTable()
                );

                SetItemGrade(item, luck);
                _chestItems.Add(item);
            }
        }

        public override void Toggle(bool state)
        {
            base.Toggle(state);
            IsVisible = state;
            enabled = true;

            DI.Fetch<UIController>()?.HideAllSections();
            DI.Fetch<InventoryController>()?.Toggle(state);
            DI.Fetch<UIController>()?.SetToggleState<InventoryController>(state);
        }

        public override void OnGUI()
        {
            if(!IsVisible)
                return;

            // TODO: Some issue with setting depth
            GUI.skin = Theme;
            GUI.depth = Depth;
            Design();
        }

        private int FetchItemFromLootTable()
        {
            var percentage = R.RandomRange(0, 100);
            var curr = 0;

            for (var i = 0; i < _lootTableData.LootItems.Length - 1; i++)
            {
                curr += _lootTableData.LootItems[i].percentage;
                if (curr > percentage)
                    return _lootTableData.LootItems[i].itemId;
            }

            return _lootTableData.LootItems[_lootTableData.LootItems.Length - 1].itemId;
        }

        private static void SetItemGrade(SimpleItem item, int luck)
        {
            var percentage = R.RandomRange(0, 100);
            percentage += luck;

            if (percentage < 40)
                item.Grade = ItemGrade.D;
            else if (percentage < 65)
                item.Grade = ItemGrade.C;
            else if (percentage < 80)
                item.Grade = ItemGrade.B;
            else if (percentage < 94)
                item.Grade = ItemGrade.A;
            else if (percentage < 99)
                item.Grade = ItemGrade.S;
            else
                item.Grade = ItemGrade.SS;
        }

        public void ShowChest()
        {
            if (_chestItems == null)
                BuildLootItems();

            Toggle(true);
        }

        protected override void DrawBody()
        {
            for (var x = 0; x < 4; x++)
            {
                for (var y = 0; y < Mathf.CeilToInt(_chestItems.Count / 4.0f); y++)
                {
                    var realId = x + y * 4;
                    if (realId >= _chestItems.Count)
                        break;

                    if (GUI.Button(new Rect(25 + x * 65, 65 + y * 65, 60, 60), _chestItems[realId].Info.Icon.texture, "inv_item"))
                    {
                        CollectItem(realId);
                        break;
                    }
                }
            }
        }

        protected void CollectItem(int id)
        {
            DI.Fetch<InventoryController>()?.AddItem(_chestItems[id]);
            _chestItems.RemoveAt(id);
        }
    }
}
