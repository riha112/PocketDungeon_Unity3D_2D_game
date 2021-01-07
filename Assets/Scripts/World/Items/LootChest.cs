using System.Collections.Generic;
using Assets.Scripts.Items;
using Assets.Scripts.Items.Type.Controller;
using Assets.Scripts.Misc.GUI;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.Misc.Translator;
using Assets.Scripts.Repository;
using Assets.Scripts.Repository.Data;
using Assets.Scripts.User;
using Assets.Scripts.User.Controller;
using Assets.Scripts.User.Inventory;
using UnityEngine;

namespace Assets.Scripts.World.Items
{
    /// <summary>
    /// Contains logic for item: Loot Chest
    /// - Loads random items,
    /// - Allows player to collect them when in range
    /// - Animates between open/closed state
    /// </summary>
    public class LootChest : Popup
    {
        private const float ACCESS_RANGE = 1f;

        #region Animaton Data
        /// <summary>
        /// Designs for open & closed states
        /// </summary>
        public Sprite[] ChestStates;
        private SpriteRenderer _spriteRenderer;
        #endregion

        #region Info
        private LootTableData _lootTableData;
        private List<SimpleItem> _chestItems;

        private bool _isVisible;

        private bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                if (_spriteRenderer != null)
                    _spriteRenderer.sprite = ChestStates[!_isVisible ? 0 : 1];
            }
        }

        public override int Depth => 5;
        #endregion

        /// <summary>
        /// Configuration for Popup UI
        /// <seealso cref="Popup"/>
        /// <seealso cref="UI"/>
        /// </summary>
        protected override void Init()
        {
            RectConfig.Title = T.Translate("Loot Chest");
            RectConfig.Popup = new Rect(ScreenSize.x / 2 - 370, ScreenSize.y / 2 - 260, 360, 480);
            RectConfig.Body = new Rect(25, 30, 310, 420);
            RectConfig.TitleRect = new Rect(360 / 2 - 100, 0, 200, 80);

            RectConfig.ShowBackground = false;
        }

        /// <summary>
        /// Called on start of object
        /// - Subscribes to events
        /// - Populates loot table
        /// - Loads components
        /// </summary>
        protected override void Start()
        {
            _lootTableData = LootTableRepository.GetLootTable();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            BuildLootItems();
            UIController.ActionKeyPress += OnActionKeyPress;
            ToggleEvent += OnTogglePopup;
            base.Start();
        }

        /// <summary>
        /// Event subscription for hiding Loot Table Chest UI, when
        /// other UI windows are visible
        /// </summary>
        /// <param name="sender">UI object that is toggled</param>
        /// <param name="state">UI objects new state</param>
        private void OnTogglePopup(object sender, bool state)
        {
            // Hide when any other popup is active (except for inventory)
            if (sender is Popup && state && !(sender is InventoryPopupUi))
                IsVisible = false;
        }

        /// <summary>
        /// Event subscription to check distance between chest and player
        /// when "Action Key" is pressed - F, if player is within the renge
        /// toggles UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="characterPosition">Location of character in game</param>
        private void OnActionKeyPress(object sender, Vector2 characterPosition)
        {
            if (IsVisible)
            {
                Toggle(false);
                return;
            }

            if (Vector2.Distance(transform.position, characterPosition) < ACCESS_RANGE)
                ShowChest();
        }

        private void OnDestroy()
        {
            UIController.ActionKeyPress -= OnActionKeyPress;
        }

        /// <summary>
        /// Populates Loot chest with random items
        /// </summary>
        protected void BuildLootItems()
        {
            var itemCount = R.RandomRange(_lootTableData.ItemCount.min, _lootTableData.ItemCount.max);

            var userData = DI.Fetch<CharacterEntity>();
            var luck = userData is null ? 1 : (int) userData.Stats.CurrentLuck;
            _chestItems = new List<SimpleItem>();

            for (var i = 0; i < itemCount; i++)
            {
                var item = ItemRepository.GetItemObjectFromId(
                    _lootTableData.GetRandomItem()
                );

                ItemManager.AssignRandomGrade(item, luck);
                if(item is EquipableItem item1)
                    ItemManager.AssignRandomDurability(item1, luck);

                _chestItems.Add(item);
            }
        }

        /// <inheritdoc cref="Popup"/>
        public override void Toggle(bool state)
        {
            base.Toggle(state);
            IsVisible = state;
            enabled = true;

            // Hides other Popups when chest UI is visible,
            // except inventory, as we want to see it when accessing
            // loot table
            DI.Fetch<UIController>()?.HideAllSections();
            DI.Fetch<InventoryPopupUi>()?.Toggle(state);
            DI.Fetch<UIController>()?.SetToggleState<InventoryPopupUi>(state);
        }



        /// <summary>
        /// Moves item from chest to users inventory
        /// </summary>
        /// <param name="id">Items id in list</param>
        protected void CollectItem(int id)
        {
            InventoryManager.AddItem(_chestItems[id]);
            _chestItems.RemoveAt(id);
        }

        #region GUI
        public override void GuiDraw()
        {
            if (!IsVisible)
                return;

            Design();
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
            for (var y = 0; y < Mathf.CeilToInt(_chestItems.Count / 4.0f); y++)
            {
                var realId = x + y * 4;
                if (realId >= _chestItems.Count)
                    break;

                if (GUI.Button(new Rect(25 + x * 65, 65 + y * 65, 60, 60), _chestItems[realId].Info.Icon.texture,
                    "inv_item"))
                {
                    CollectItem(realId);
                    break;
                }
            }
        }
        #endregion

    }
}