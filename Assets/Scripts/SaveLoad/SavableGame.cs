using System;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Items;
using Assets.Scripts.Misc;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Repository.Data;
using Assets.Scripts.User.Attributes;
using Assets.Scripts.User.Stats;
using Assets.Scripts.World;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.SaveLoad
{
    public class SavableGame : ISavable
    {
        public WorldData World { get; set; }
        public SavableCharacter CharacterData { get; set; } = new SavableCharacter();

        private string FileLocation() => $"{Application.persistentDataPath}/Games/{PlayerPrefs.GetString("CurrentGame")}.json";

        public void CreateNewGame(string title, [CanBeNull] string seed = null)
        {
            World = new WorldData
            {
                Title = title,
                DungeonFloor = 1,
            };

            CharacterData = new SavableCharacter
            {
                HP = 100,
                MP = 20,
                Inventory = new []
                {
                    new SavableItem
                    {
                        Durability = 1,
                        Grade = ItemGrade.E,
                        ItemId = 214,
                        LocalId = 1
                    },
                    new SavableItem
                    {
                        Durability = 1,
                        Grade = ItemGrade.E,
                        ItemId = 500,
                        LocalId = 2
                    },
                    new SavableItem
                    {
                        Durability = 1,
                        Grade = ItemGrade.E,
                        ItemId = 501,
                        LocalId = 3
                    }
                },
                SavableEquipment = new List<int>()
                {
                    1
                },
                Attributes = new AttributeData()
                {
                    Agility = 0,
                    Luck = 0,
                    Magic = 0,
                    Points = 5,
                    Resistance = 0,
                    Strength = 0,
                    Vitality = 0
                },
            };

            PlayerPrefs.SetString("CurrentGame", title);
            WorldController.PopulateFloorSeeds(World, seed);
            Save();
        }

        public void Save()
        {
            if (World is null)
            {
                World = DI.Fetch<WorldController>()?.Data;
            }

            CharacterData.Save();

            CheckFileDir();

            var savableJsonText = JsonConvert.SerializeObject(this, Formatting.Indented);
            //if (!File.Exists(FileLocation()))
            //    File.Create(FileLocation());
            File.WriteAllText(FileLocation(), savableJsonText);
        }

        public void Load()
        {
            CheckFileDir();

            var jsonData = File.ReadAllText(FileLocation());
            var loadedData = JsonConvert.DeserializeObject<SavableGame>(jsonData);
            World = loadedData.World;
            CharacterData = loadedData.CharacterData;

            CharacterData.Load();
            if (!File.Exists(FileLocation()))
                throw new Exception($"Save file not found at: {FileLocation()}");

            var wc = DI.Fetch<WorldController>();
            wc.Data = World;

            DI.Register(this);
        }

        private void CheckFileDir()
        {
            if (!PlayerPrefs.HasKey("CurrentGame"))
                throw new Exception("No game selected");

            var savedGameDirectory = $"{Application.persistentDataPath}/Games";
            if (!Directory.Exists(savedGameDirectory))
                Directory.CreateDirectory(savedGameDirectory);
        }
    }
}
