﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Items.Type.Controller;
using Assets.Scripts.Misc;
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
using UnityEngine;

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

        // -- Magic --
        public int[] Magic { get; set; }

        // -- User data --
        public ResourcesData Resources;
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
            Resources = ce.Resources;
            Attributes = ce.Attributes;
            Stats = ce.Stats;
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
            var ic = DI.Fetch<InventoryController>();
            if (ic == null)
                return;

            Inventory = new SavableItem[ic.InventoryGrid.Count];
            for (var i = 0; i < ic.InventoryGrid.Count; i++)
            {
                Inventory[i] = new SavableItem
                {
                    Durability = ic.InventoryGrid[i] is EquipableItem item ? item.Durability : 1,
                    Grade = ic.InventoryGrid[i].Grade,
                    ItemId = ic.InventoryGrid[i].Info.ItemId,
                    LocalId = ic.InventoryGrid[i].LocalId
                };
            }
        }

        public void Load()
        {
            LoadUserData();
            LoadMagicData();
            LoadInventory();
            LoadEquipment();
        }

        private void LoadUserData()
        {
            var ud = DI.Fetch<CharacterEntity>();
            ud.Resources = Resources ?? new ResourcesData();
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
            if (Inventory is null) return;

            var ic = DI.Fetch<InventoryController>();
            for (var i = 0; i < Inventory.Length; i++)
            {
                var item = ItemRepository.GetItemObjectFromId(Inventory[i].ItemId);
                item.Grade = Inventory[i].Grade;
                if (item is EquipableItem item1)
                    item1.Durability = Inventory[i].Durability;
                item.LocalId = Inventory[i].LocalId;
                ic.AddItem(item);
            }
        }
        private void LoadEquipment()
        {
            if (SavableEquipment is null) return;

            var ec = DI.Fetch<EquipmentController>();
            var ic = DI.Fetch<InventoryController>();

            DI.Fetch<FightingController>()?.Start();

            foreach (var localItemId in SavableEquipment)
            {
                var item = ic?.GetItemByItemLocalId(localItemId);
                if(item is EquipableItem item1)
                    ec?.EquipItem(item1);
            }

            DI.Fetch<InGameUiController>()?.OnEquipmentChanged(null, (0, null));
        }
    }

}