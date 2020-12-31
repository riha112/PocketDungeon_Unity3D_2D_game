using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Items;
using Assets.Scripts.Items.Type.Controller;
using Assets.Scripts.Misc.GUI;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Translator;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.User.Equipment
{
    public class EquipmentController : Popup
    {
        private const short SLOT_SIZE = 75;

        private List<EquipmentSlot> _slots;

        public List<EquipmentSlot> Slots
        {
            get
            {
                if (_slots is null)
                    BuildEmptySlots();
                return _slots;
            }
            protected set => _slots = value;
        }

        public List<Texture2D> DefaultIcons;

        public override int Depth => 10;

        public event EventHandler<(int, EquipableItem)> EquipmentChanged;

        protected override void Init()
        {
            RectConfig.Popup = new Rect(ScreenSize.x / 2 - 370, ScreenSize.y / 2 - 260, 360, 480);
            RectConfig.Body = new Rect(25, 30, 310, 420);
            RectConfig.TitleRect = new Rect(360 / 2 - 100, 0, 200, 80);

            RectConfig.ShowBackground = false;
            RectConfig.Title = T.Translate("INVENTORY");
        }

        protected override void DrawBody()
        {
            foreach (var slot in Slots)
            {
                var isEmpty = IsEmptyItem(slot.CurrentItem);

                if (GUI.Button(slot.Position,
                    isEmpty ? slot.EmptyTexture : slot.CurrentItem.Info.Icon.texture,
                    "eqpm_item"))
                    UnEquipItem(slot);
            }
        }

        public List<EquipableItem> EquippedItems()
        {
            return (from slot in Slots where !IsEmptyItem(slot.CurrentItem) select slot.CurrentItem).ToList();
        }

        /**
         * Loads textures into games character to visualize equipped item
         * <param name="slot">Slot to which update</param>
         */
        protected void UpdateEquipmentVisual(EquipmentSlot slot)
        {
            if (slot.EquipmentPoints == null) return;

            // Removes old Design items
            foreach (var slotEquipmentPoint in slot.EquipmentPoints)
                if (slotEquipmentPoint.transform.childCount > 0)
                    Destroy(slotEquipmentPoint.transform.GetChild(0).gameObject);

            if (slot.CurrentItem?.Info.Prefabs == null) return;

            // Applies new Design items
            var counter = 0;
            foreach (var mount in slot.CurrentItem?.Info.Prefabs)
            {
                if (counter >= slot.EquipmentPoints.Length)
                    break;

                var design = Instantiate(mount);
                design.transform.parent = slot.EquipmentPoints[counter].transform;
                design.transform.localPosition = Vector3.zero;
                design.transform.localEulerAngles = Vector3.zero;
                design.transform.localScale = Vector3.one;
                counter++;
            }
        }

        /**
         * Un-equips item from item UI slot
         * <param name="slot">Slot to clear</param>
         */
        public void UnEquipItem([CanBeNull] EquipmentSlot slot)
        {
            if (slot == null || IsEmptyItem(slot.CurrentItem))
                return;

            slot.CurrentItem.Dismount();
            slot.CurrentItem = null;

            EquipmentChanged?.Invoke(this, (slot.Id, null));
            DI.Fetch<CharacterEntity>()?.OnAttributesChange(null, (0, 0));
            UpdateEquipmentVisual(slot);
        }

        [CanBeNull]
        public EquipmentSlot FindSlotForItem(EquipableItem item)
        {
            return Slots.Find(slot => slot.CurrentItem?.LocalId == item.LocalId);
        }

        /**
         * Equips item into UI
         * <param name="item">Item to equip</param>
         */
        public void EquipItem(EquipableItem item)
        {
            // If item is ether equipped or is un-equipable then return
            if (item.IsEquipped || item.Info.Slot == ItemSlot.None)
                return;

            // Catches slot ID based on slot type
            // - Some items can be equipped into different slots, so
            // - we are preforming priority check
            var slotId = (int) item.Info.Slot;
            switch (item.Info.Slot)
            {
                case ItemSlot.PrimarySecondary:
                    slotId = IsEmptyItem(Slots[2].CurrentItem) || !IsEmptyItem(Slots[4].CurrentItem) ? 2 : 4;
                    break;
                case ItemSlot.Socket:
                    slotId = IsEmptyItem(Slots[7].CurrentItem) || !IsEmptyItem(Slots[8].CurrentItem) ? 7 : 8;
                    break;
            }

            // Clears slot before equipping 
            UnEquipItem(Slots[slotId]);

            // Equip item
            if (item is EquipableItem item1)
                Slots[slotId].CurrentItem = item1;
            item.Mount();

            EquipmentChanged?.Invoke(this, (slotId, Slots[slotId].CurrentItem));
            DI.Fetch<CharacterEntity>()?.OnAttributesChange(null, (0, 0));
            UpdateEquipmentVisual(Slots[slotId]);
        }

        /**
         * Checks weather or not slot contains equipped item
         * <param name="item">Slot</param>
         * <returns type="bool">True if slot is empty</returns>
         */
        private static bool IsEmptyItem(EquipableItem item)
        {
            return item?.Equals(null) == null;
        }

        public bool IsItemWithIdEquipped(int id)
        {
            return Slots.Where(slot => !IsEmptyItem(slot.CurrentItem)).Any(slot => slot.CurrentItem.Info.ItemId == id);
        }

        /**
         * Configuration setup for equipment UI slots
         */
        private void BuildEmptySlots()
        {
            Slots = new List<EquipmentSlot>
            {
                // Head
                new EquipmentSlot
                {
                    Id = (int) ItemSlot.Head,
                    CurrentItem = null,
                    AllowedItem = ItemType.Armor,
                    Position = new Rect(310 / 2 - SLOT_SIZE / 2, 60, SLOT_SIZE, SLOT_SIZE),
                    EmptyTexture = DefaultIcons[0],
                    EquipmentPoints = new GameObject[1]
                    {
                        GameObject.FindGameObjectWithTag("slot_head")
                    }
                },
                // Neck
                new EquipmentSlot
                {
                    Id = (int) ItemSlot.Neck,
                    CurrentItem = null,
                    AllowedItem = ItemType.Armor,
                    Position = new Rect(310 / 2 - SLOT_SIZE / 2 + 10 + SLOT_SIZE, 60, SLOT_SIZE, SLOT_SIZE),
                    EmptyTexture = DefaultIcons[1]
                },
                // Sword
                new EquipmentSlot
                {
                    Id = (int) ItemSlot.Primary,
                    CurrentItem = null,
                    AllowedItem = ItemType.Armor,
                    Position = new Rect(310 / 2 - SLOT_SIZE / 2 - SLOT_SIZE - 10, 60 + SLOT_SIZE * 1 + 10, SLOT_SIZE,
                        SLOT_SIZE),
                    EmptyTexture = DefaultIcons[2],
                    EquipmentPoints = new GameObject[1]
                    {
                        GameObject.FindGameObjectWithTag("slot_primary")
                    }
                },
                // Armor
                new EquipmentSlot
                {
                    Id = (int) ItemSlot.Body,
                    CurrentItem = null,
                    AllowedItem = ItemType.Armor,
                    Position = new Rect(310 / 2 - SLOT_SIZE / 2, 60 + SLOT_SIZE * 1 + 10, SLOT_SIZE, SLOT_SIZE),
                    EmptyTexture = DefaultIcons[3],
                    EquipmentPoints = new GameObject[3]
                    {
                        GameObject.FindGameObjectWithTag("slot_body"),
                        GameObject.FindGameObjectWithTag("slot_arm_0"),
                        GameObject.FindGameObjectWithTag("slot_arm_1")
                    }
                },
                // Secondary Item
                new EquipmentSlot
                {
                    Id = (int) ItemSlot.Secondary,
                    CurrentItem = null,
                    AllowedItem = ItemType.Armor,
                    Position = new Rect(310 / 2 - SLOT_SIZE / 2 + SLOT_SIZE + 10, 60 + SLOT_SIZE * 1 + 10, SLOT_SIZE,
                        SLOT_SIZE),
                    EmptyTexture = DefaultIcons[4],
                    EquipmentPoints = new GameObject[1]
                    {
                        GameObject.FindGameObjectWithTag("slot_secondary")
                    }
                },
                // Pants
                new EquipmentSlot
                {
                    Id = (int) ItemSlot.Pants,
                    CurrentItem = null,
                    AllowedItem = ItemType.Armor,
                    Position = new Rect(310 / 2 - SLOT_SIZE / 2, 60 + SLOT_SIZE * 2 + 10 * 2, SLOT_SIZE, SLOT_SIZE),
                    EmptyTexture = DefaultIcons[5]
                },
                // Shoes
                new EquipmentSlot
                {
                    Id = (int) ItemSlot.Shoes,
                    CurrentItem = null,
                    AllowedItem = ItemType.Armor,
                    Position = new Rect(310 / 2 - SLOT_SIZE / 2, 60 + SLOT_SIZE * 3 + 10 * 3, SLOT_SIZE, SLOT_SIZE),
                    EmptyTexture = DefaultIcons[6],
                    EquipmentPoints = new GameObject[2]
                    {
                        GameObject.FindGameObjectWithTag("slot_shoe_0"),
                        GameObject.FindGameObjectWithTag("slot_shoe_1")
                    }
                },
                // Modifier 1
                new EquipmentSlot
                {
                    Id = (int) ItemSlot.Socket,
                    CurrentItem = null,
                    AllowedItem = ItemType.Armor,
                    Position = new Rect(310 / 2 - SLOT_SIZE / 2 - SLOT_SIZE - 10, 60 + SLOT_SIZE * 2 + 10 * 2,
                        SLOT_SIZE, SLOT_SIZE),
                    EmptyTexture = DefaultIcons[7]
                },
                // Modifier 2
                new EquipmentSlot
                {
                    Id = (int) ItemSlot.Socket + 1,
                    CurrentItem = null,
                    AllowedItem = ItemType.Armor,
                    Position = new Rect(310 / 2 - SLOT_SIZE / 2 - SLOT_SIZE - 10, 60 + SLOT_SIZE * 3 + 10 * 3,
                        SLOT_SIZE, SLOT_SIZE),
                    EmptyTexture = DefaultIcons[8]
                }
            };
        }
    }
}