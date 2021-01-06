using System.Collections.Generic;
using Assets.Scripts.Items.Type.Controller;
using Assets.Scripts.Misc;
using Assets.Scripts.Misc.GUI;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.User.Equipment;
using Assets.Scripts.User.Inventory;
using Assets.Scripts.User.Magic;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.User.Controller
{
    public class PinnableSlotConfiguration
    {
        public IPinnable Pinnable { get; set; }
        public float CooldownTimer { get; set; } = 0;
        public KeyCode Key { get; set; }
    }

    public class PinnableSlotUiController : UI
    {
        private const short ICON_SIZE = 60;
        private const short FAN_SLOT_SIZE = 40;
        private const short MAGIC_SLOT_COUNT = 4;
        private const short OTHER_SLOT_COUNT = 2;

        /// <summary>
        /// Placeholder texture for empty slot
        /// </summary>
        public Texture2D[] Placeholder;

        /// <summary>
        /// List of slots
        /// </summary>
        public PinnableSlotConfiguration[] PinnedSlots { get; protected set; } = new PinnableSlotConfiguration[MAGIC_SLOT_COUNT + OTHER_SLOT_COUNT];

        /// <summary>
        /// List of pinnable items that are not already pinned
        /// </summary>
        private List<IPinnable> _filteredOutPinnables;

        /// <summary>
        /// Active pinnable slot
        /// </summary>
        private short _idActiveSlot = -1;

        private Transform _character;
        private static CharacterEntity _characterEntity;
        private static MagicController _magicController;

        /// <summary>
        /// Draws fan for items with durability
        /// </summary>
        private List<EquipableItem> _durableItems;

        protected override void Init()
        {
            BuildSlotConfig();;
            ToggleEvent += OnUiToggle;

            _character = Util.GetCharacterTransform();
            _characterEntity = _character.GetComponent<CharacterEntity>();
            _magicController = _character.GetComponent<MagicController>();

            DI.Fetch<EquipmentController>().EquipmentChanged += OnEquipmentChanged;
        }

        /// <summary>
        /// Hides fan when Popup is visible
        /// </summary>
        /// <param name="sender">UI that is changing state</param>
        /// <param name="state">new state of UI</param>
        private void OnUiToggle([CanBeNull] object sender, bool state)
        {
            if (sender is Popup && state)
            {
                _idActiveSlot = -1;
            }
        }

        /// <summary>
        /// Populates list of items that are equipped, so that durability window
        /// can be shown in game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        public void OnEquipmentChanged([CanBeNull] object sender, (int slotId, EquipableItem item) data)
        {
            _durableItems = DI.Fetch<EquipmentController>()?.EquippedItems() ?? new List<EquipableItem>();
        }

        /// <summary>
        /// Filters out already pinned items & splits them by:
        /// Magic and items
        /// </summary>
        /// <param name="slotId">For which slot to show items</param>
        /// <returns></returns>
        protected bool FilterOut(short slotId)
        {
            if (_idActiveSlot == slotId)
                return false;

            if (slotId < MAGIC_SLOT_COUNT)
            {
                // Magic filtration
                if (_magicController.MyMagic.Count == 0) return false;

                _filteredOutPinnables = new List<IPinnable>();
                _filteredOutPinnables.AddRange(_magicController.MyMagic);
            }
            else
            {
                // Item filtration
                _filteredOutPinnables = InventoryManager.FetchFilteredItems<IPinnable>();
            }

            if (_filteredOutPinnables.Count == 0 && PinnedSlots[slotId].Pinnable == null)
                return false;

            _filteredOutPinnables = RemovePinned(_filteredOutPinnables);
            _idActiveSlot = slotId;
            return true;
        }

        private List<IPinnable> RemovePinned(IEnumerable<IPinnable> items)
        {
            var output = new List<IPinnable>();
            foreach (var item in items)
            {
                var canAdd = true;
                foreach (var slot in PinnedSlots)
                {
                    if(slot.Pinnable == null)
                        continue;

                    if (slot.Pinnable.Id == item.Id)
                    {
                        canAdd = false;
                        break;
                    }
                }

                if (canAdd) output.Add(item);
            }
            return output;
        }

        protected override void Design()
        {
            DrawStatData();
            DrawDurabilityDesign();

            GUI.BeginGroup(new Rect(25, ScreenSize.y - 45 - ICON_SIZE, ICON_SIZE * MAGIC_SLOT_COUNT + 50,ICON_SIZE + 20), "", "magic_bg");
            for (short i = 0; i < MAGIC_SLOT_COUNT; i++)
            {
                if (GUI.Button(new Rect(10 + (ICON_SIZE + 10) * i, 10, ICON_SIZE, ICON_SIZE), PinnedSlots[i].Pinnable == null ? Placeholder[1] : PinnedSlots[i].Pinnable.Icon,
                    "magic_slot"))
                {
                    FilterOut(i);
                }

                if (PinnedSlots[i].CooldownTimer > 0)
                {
                    var timerHeight = ICON_SIZE * (PinnedSlots[i].CooldownTimer / PinnedSlots[i].Pinnable.CoolDownTimer);
                    if (timerHeight < 5) timerHeight = 5;
                    GUI.Box(new Rect(10 + (ICON_SIZE + 10) * i, 10 + ICON_SIZE - timerHeight, ICON_SIZE, timerHeight), "", "magic_timer");
                }
                GUI.Box(new Rect(ICON_SIZE - 10 + (ICON_SIZE + 10) * i, ICON_SIZE - 10, 20, 20), $"{i + 1}", "magic_btn_indicator");
            }
            GUI.EndGroup();

            GUI.BeginGroup(new Rect(ScreenSize.x - 175, ScreenSize.y - 45 - ICON_SIZE, ICON_SIZE * 2 + 30, ICON_SIZE + 20), "", "magic_bg");
            for (short i = MAGIC_SLOT_COUNT; i < MAGIC_SLOT_COUNT + OTHER_SLOT_COUNT; i++)
            {
                if (GUI.Button(new Rect(10 + (ICON_SIZE + 10) * (i - MAGIC_SLOT_COUNT), 10, ICON_SIZE, ICON_SIZE),
                    PinnedSlots[i].Pinnable == null ? Placeholder[0] : PinnedSlots[i].Pinnable.Icon, "magic_slot"))
                {
                    FilterOut(i);
                }

                GUI.Box(new Rect(ICON_SIZE - 10 + (ICON_SIZE + 10) * (i - MAGIC_SLOT_COUNT), ICON_SIZE - 10, 20, 20), $"{PinnedSlots[i].Key}", "magic_btn_indicator");
            }
            GUI.EndGroup();


            if (_idActiveSlot != -1)
            {
                DrawFan();
            }

        }

        protected void DrawFan()
        {
            var x = (_idActiveSlot < MAGIC_SLOT_COUNT)
                ? 45 + (ICON_SIZE + 10) * _idActiveSlot
                : ScreenSize.x - 160 + (ICON_SIZE + 10) * (_idActiveSlot - MAGIC_SLOT_COUNT);

            GUI.BeginGroup(new Rect(
                x, 
                ScreenSize.y - 50 - ICON_SIZE - FAN_SLOT_SIZE * (_filteredOutPinnables.Count + 1),
                FAN_SLOT_SIZE, 
                (_filteredOutPinnables.Count + 1) * FAN_SLOT_SIZE
            ), "");

            // Set
            for (var i = 0; i < _filteredOutPinnables.Count; i++)
            {
                if (GUI.Button(new Rect(0, i * FAN_SLOT_SIZE, FAN_SLOT_SIZE, FAN_SLOT_SIZE), _filteredOutPinnables[i].Icon, "magic_fan"))
                {
                    PinnedSlots[_idActiveSlot].Pinnable = _filteredOutPinnables[i];
                    _idActiveSlot = -1;
                }
            }

            // Remove
            if (GUI.Button(new Rect(0, _filteredOutPinnables.Count * FAN_SLOT_SIZE, FAN_SLOT_SIZE, FAN_SLOT_SIZE), "X", "magic_fan"))
            {
                PinnedSlots[_idActiveSlot].Pinnable = null;
                _idActiveSlot = -1;
            }
            GUI.EndGroup();
        }

        private void DrawDurabilityDesign()
        {
            if(_durableItems == null)
                return;

            GUI.BeginGroup(new Rect(20, 20, 130, 55 * _durableItems.Count));
            for(var i =0; i < _durableItems.Count; i++)
            {
                GUI.Box(new Rect(0, 55 * i, 45, 45), _durableItems[i].Info.Icon.texture, "durability_item_bg");
                GUI.Box(new Rect(50, 15 + 55 * i, 80, 15), "", "durability_bar_bg");
                GUI.Box(new Rect(55, 20 + 55 * i, 70 * _durableItems[i].Durability, 5), "", "durability_bar_fg");
            }
            GUI.EndGroup();
        }

        private const int STATS_BAR_SIZE = ICON_SIZE * MAGIC_SLOT_COUNT + 50;
        private const int STATS_BAR_HALF_SIZE = STATS_BAR_SIZE / 2 - 5;
        private void DrawStatData()
        {
            GUI.BeginGroup(new Rect(25, ScreenSize.y - 160, STATS_BAR_SIZE, 50));

            // Health
            GUI.Box(new Rect(0, 0, STATS_BAR_HALF_SIZE, 30), "", "magic_bg");
            GUI.Box(new Rect(5, 5, (STATS_BAR_HALF_SIZE - 10) * _characterEntity.Health / _characterEntity.Stats.MaxHealth, 20), "", "hp_bar_fg");
            GUI.Box(new Rect(0, 0, STATS_BAR_HALF_SIZE, 30), "", "stats_fg");

            // Magic
            GUI.Box(new Rect(STATS_BAR_HALF_SIZE + 10, 0, STATS_BAR_HALF_SIZE, 30), "", "magic_bg");
            GUI.Box(new Rect(STATS_BAR_HALF_SIZE + 15, 5, (STATS_BAR_HALF_SIZE - 10) * _characterEntity.Magic / _characterEntity.Stats.MaxMagic, 20), "", "mp_bar_fg");
            GUI.Box(new Rect(STATS_BAR_HALF_SIZE + 10, 0, STATS_BAR_HALF_SIZE, 30), "", "stats_fg");

            // Exp
            GUI.Box(new Rect(0, 35, STATS_BAR_SIZE, 15), "", "exp_bar_bg");
            GUI.Box(new Rect(10, 40, (STATS_BAR_SIZE - 20) * _characterEntity.Stats.CurrentExp / _characterEntity.Stats.RequiredExp, 5), "", "exp_bar_fg");

            GUI.EndGroup();
        }

        private void Update()
        {
            foreach (var pinnableSlot in PinnedSlots)
            {
                // Magic is set.
                if (pinnableSlot.Pinnable == null) 
                    continue;

                // Magic requirements are met.
                if (!pinnableSlot.Pinnable.CanUse)
                    continue;

                // Magic is not on cooldown.
                pinnableSlot.CooldownTimer -= Time.deltaTime;
                if (!(pinnableSlot.CooldownTimer < 0) || !Input.GetKeyUp(pinnableSlot.Key))
                    continue;

                if (pinnableSlot.Pinnable.OnUsed())
                {
                    if (pinnableSlot.Pinnable.IsSingleUse)
                    {
                        pinnableSlot.Pinnable = null;
                        continue;
                    }
                    pinnableSlot.CooldownTimer = pinnableSlot.Pinnable.CoolDownTimer;
                }
            }
        }

        private void BuildSlotConfig()
        {
            var keyMap = new[]
            {
                KeyCode.Alpha1,
                KeyCode.Alpha2,
                KeyCode.Alpha3,
                KeyCode.Alpha4,
                KeyCode.Z,
                KeyCode.X,
            };
            for (var i = 0; i < MAGIC_SLOT_COUNT + OTHER_SLOT_COUNT; i++)
            {
                PinnedSlots[i] = new PinnableSlotConfiguration()
                {
                    Key =  keyMap[i]
                };
            }
        }
    }
}
