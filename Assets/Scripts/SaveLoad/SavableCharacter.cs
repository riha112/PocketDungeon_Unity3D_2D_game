using System;
using System.Collections.Generic;
using Assets.Scripts.Items;
using Assets.Scripts.Items.Type.Controller;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Repository;
using Assets.Scripts.User;
using Assets.Scripts.User.Attributes;
using Assets.Scripts.User.Controller;
using Assets.Scripts.User.Equipment;
using Assets.Scripts.User.Inventory;
using Assets.Scripts.User.Magic;
using Assets.Scripts.User.Resource;
using Assets.Scripts.User.Stats;

namespace Assets.Scripts.SaveLoad
{
    /// <summary>
    /// Manages saving & loading of character entity
    /// </summary>
    public class SavableCharacter : ISavable
    {
        // -- Inventory --
        public SavableItem[] Inventory { get; set; }

        // -- Equipment --
        public List<int> SavableEquipment { get; set; }

        // -- In Game UI -- 
        public SavableEquipment[] PinnedEquipment { get; set; }
        public SavableEquipment[] PinnedMagic { get; set; }

        // -- Magic --
        public int[] Magic { get; set; }

        // -- User data --
        public Dictionary<int, int> Resources;
        public AttributeData Attributes;
        public StatsData Stats;

        public float? HP = 100;
        public float? MP = 20;
        
        public void Save()
        {
            SaveUserData();
            SaveInventory();
            SaveMagic();
            SaveUserEquipment();
            SavePinnedData();
        }

        private void SaveMagic()
        {
            var mc = DI.Fetch<MagicController>();
            if(mc == null)
                return;

            Magic = new int[mc.MyMagic.Count];
            for (var i = 0; i < Magic.Length; i++)
            {
                Magic[i] = mc.MyMagic[i].Data.Id;
            }
        }

        private void SaveUserData()
        {
            var ce = DI.Fetch<CharacterEntity>();
            if(ce == null)
                return;

            HP = ce.Health;
            MP = ce.Magic;
            Resources = ce.Resources.Resources;
            Attributes = ce.Attributes;
            Stats = ce.Stats;
        }

        private void SavePinnedData()
        {
            var pc = DI.Fetch<PinnableSlotUiController>();
            if(pc == null)
                return;

            PinnedMagic = new SavableEquipment[PinnableSlotUiController.MAGIC_SLOT_COUNT];
            PinnedEquipment = new SavableEquipment[PinnableSlotUiController.OTHER_SLOT_COUNT];
            for (var i = 0; i < PinnableSlotUiController.MAGIC_SLOT_COUNT + PinnableSlotUiController.OTHER_SLOT_COUNT; i++)
            {
                var element = new SavableEquipment
                {
                    ItemLocalId = pc.PinnedSlots[i].Pinnable?.Id ?? -1,
                    SlotId = i
                };

                if (i >= PinnableSlotUiController.MAGIC_SLOT_COUNT)
                    PinnedEquipment[i - PinnableSlotUiController.MAGIC_SLOT_COUNT] = element;
                else
                    PinnedMagic[i] = element;
            }
        }

        private void SaveUserEquipment()
        {
            var ec = DI.Fetch<EquipmentController>();
            if (ec == null)
                return;

            SavableEquipment = new List<int>();
            foreach (var item in ec.EquippedItems())
            {
               SavableEquipment.Add(item.LocalId);
            }
        }

        private void SaveInventory()
        {
            if(InventoryManager.InventoryGrid.Count == 0)
                return;

            Inventory = new SavableItem[InventoryManager.InventoryGrid.Count];
            for (var i = 0; i < InventoryManager.InventoryGrid.Count; i++)
            {
                Inventory[i] = new SavableItem
                {
                    Durability = InventoryManager.InventoryGrid[i] is EquipableItem item ? item.Durability : 1,
                    Grade = InventoryManager.InventoryGrid[i].Grade,
                    ItemId = InventoryManager.InventoryGrid[i].Info.ItemId,
                    LocalId = InventoryManager.InventoryGrid[i].LocalId
                };
            }
        }

        public void Load()
        {
            LoadUserData();
            LoadMagicData();
            LoadInventory();
            LoadEquipment();
            LoadPinnedData();

            DI.Register(this);
        }

        private void LoadUserData()
        {
            var ud = DI.Fetch<CharacterEntity>();
            ud.Resources = new ResourcesData()
            {
                Resources = Resources
            };
            ud.Attributes = Attributes ?? new AttributeData();
            ud.Stats = Stats ?? new StatsData();
            ud.Health = HP ?? 100;
            ud.Magic = MP ?? 20;
        }

        private void LoadMagicData()
        {
            if(Magic is null) return;

            var mc = DI.Fetch<MagicController>();
            foreach (var magicId in Magic)
            {
                mc.LearnMagic(magicId);
            }
        }

        private void LoadInventory()
        {
            InventoryManager.InventoryGrid = new List<SimpleItem>();

            if (Inventory is null) return;

            for (var i = 0; i < Inventory.Length; i++)
            {
                var item = ItemRepository.GetItemObjectFromId(Inventory[i].ItemId);
                item.Grade = Inventory[i].Grade;
                if (item is EquipableItem item1)
                    item1.Durability = Inventory[i].Durability;
                item.LocalId = Inventory[i].LocalId;
                InventoryManager.AddItem(item);
            }
        }

        private void LoadEquipment()
        {
            if (SavableEquipment is null) return;

            var ec = DI.Fetch<EquipmentController>();
            DI.Fetch<FightingController>()?.Start();

            foreach (var localItemId in SavableEquipment)
            {
                var item = InventoryManager.GetItemByItemLocalId(localItemId);
                if(item is EquipableItem item1)
                    ec?.EquipItem(item1);
            }

            DI.Fetch<PinnableSlotUiController>()?.OnEquipmentChanged(null, (0, null));
        }

        private void LoadPinnedData()
        {
            var pc = DI.Fetch<PinnableSlotUiController>();
            if (pc == null)
                return;

            if (PinnedEquipment != null)
            {
                foreach (var item in PinnedEquipment)
                {
                    if (item.ItemLocalId == -1)
                        continue;

                    pc.PinnedSlots[item.SlotId].Pinnable =
                        (IPinnable)InventoryManager.GetItemByItemLocalId(item.ItemLocalId);
                }
            }

            if (PinnedMagic == null) return;

            var mc = DI.Fetch<MagicController>();
            if (mc == null)
                return;

            foreach (var magic in PinnedMagic)
            {
                if (magic.ItemLocalId == -1)
                    continue;

                pc.PinnedSlots[magic.SlotId].Pinnable = 
                    mc.MyMagic.Find(m => m.Id == magic.ItemLocalId);
            }
        }
    }

}
