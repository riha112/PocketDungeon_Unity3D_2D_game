using System;
using Assets.Scripts.Misc;
using Assets.Scripts.Misc.GUI;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.Repository.Data;
using Assets.Scripts.SaveLoad;
using Assets.Scripts.User;
using Assets.Scripts.User.Attributes;
using Assets.Scripts.User.Controller;
using Assets.Scripts.User.Equipment;
using Assets.Scripts.User.Inventory;
using Assets.Scripts.User.Magic;
using Assets.Scripts.User.Map;
using Assets.Scripts.User.Messages;
using Assets.Scripts.User.Party;
using Assets.Scripts.User.Resource;
using Assets.Scripts.World.Generation;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Assets.Scripts.World
{
    public class WorldController : Injectable
    {
        public WorldData Data = new WorldData();
        public DungeonGenerator DungeonGenerator;
        public SavableGame SaveGame = new SavableGame();

        protected override void Init()
        {
            RegisterDi();
            SaveGame.Load();
            StartGame();
        }

        private void RegisterDi()
        {
            DI.Register(this);
            DI.Register(GetComponent<MonoUtil>());
            DI.Register(GetComponent<Gui>());

            var character = Util.GetCharacterTransform();
            DI.Register(character.GetComponent<CharacterEntity>());
            DI.Register(character.GetComponent<MovementController>());
            DI.Register(character.GetComponent<FightingController>());
            DI.Register(character.GetComponent<InventoryPopupUi>());
            DI.Register(character.GetComponent<MagicController>());
            DI.Register(character.GetComponent<ResourceController>());
            DI.Register(character.GetComponent<MessageController>());
            DI.Register(character.GetComponent<PartyController>());

            var mainCamera = Camera.main;
            DI.Register(mainCamera.GetComponent<AttributePopupUi>());
            DI.Register(mainCamera.GetComponent<EquipmentController>());
            DI.Register(mainCamera.GetComponent<MapController>());
            DI.Register(mainCamera.GetComponent<InGameUiController>());
            DI.Register(mainCamera.GetComponent<UIController>());
        }

        public void StartGame()
        {
            if (Data.DungeonFloorSeeds == null || Data.DungeonFloorSeeds.Length == 0)
                PopulateFloorSeeds(Data);

            GenerateDungeon(Data.DungeonFloor);
        }

        public static void PopulateFloorSeeds(WorldData data, [CanBeNull] string seed = null)
        {
            data.DungeonFloorSeeds = new string[100];
            var baseSeed = seed ?? DateTime.Now.ToString();
            R.SetSeed(baseSeed);

            data.DungeonFloorSeeds[0] = baseSeed;
            for (var i = 1; i < 100; i++)
            {
                data.DungeonFloorSeeds[i] = $"{R.RandomRange(0, 999999)}_{baseSeed}_{i}_{R.RandomRange(0, 999999)}";
            }
        }

        public void GenerateDungeon(int level)
        {
            DungeonGenerator.Seed = Data.DungeonFloorSeeds[level - 1];
            DungeonGenerator.UseRandomSeed = false;
            DungeonGenerator.Generate();
        }

        public void EndGame()
        {
            Data.DungeonFloor++;
            SaveGame.Save();

            // Reload level.
            SceneManager.LoadScene("Dungeon");
        }
    }
}