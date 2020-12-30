using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Misc;
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
            DI.Register(Util.GetCharacterTransform().GetComponent<CharacterEntity>());
            DI.Register(Util.GetCharacterTransform().GetComponent<MovementController>());
            DI.Register(Util.GetCharacterTransform().GetComponent<FightingController>());
            DI.Register(Util.GetCharacterTransform().GetComponent<InventoryController>());
            DI.Register(Util.GetCharacterTransform().GetComponent<MagicController>());
            DI.Register(Util.GetCharacterTransform().GetComponent<ResourceController>());
            DI.Register(Util.GetCharacterTransform().GetComponent<MessageController>());
            DI.Register(Util.GetCharacterTransform().GetComponent<PartyController>());

            DI.Register(Camera.main.GetComponent<AttributeController>());
            DI.Register(Camera.main.GetComponent<EquipmentController>());
            DI.Register(Camera.main.GetComponent<UIController>());
            DI.Register(Camera.main.GetComponent<MapController>());
            DI.Register(Camera.main.GetComponent<InGameUiController>());
        }

        public void StartGame()
        {
            if(Data.DungeonFloorSeeds == null || Data.DungeonFloorSeeds.Length == 0)
                PopulateFloorSeeds(Data);

            GenerateDungeon(Data.DungeonFloor);
        }

        public static void PopulateFloorSeeds(WorldData data, [CanBeNull] string seed = null)
        {
            data.DungeonFloorSeeds = new string[100];
            var baseSeed = seed ?? DateTime.Now.ToString();
            data.DungeonFloorSeeds[0] = baseSeed;
            for (var i = 1; i < 100; i++)
                data.DungeonFloorSeeds[i] = $"{Random.Range(0, 100)}_{baseSeed}_{i}_{Random.Range(0, 999999999)}";
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

            // Destroy all GO.

            // Reload level.
            SceneManager.LoadScene("Dungeon");
        }
    }
}
