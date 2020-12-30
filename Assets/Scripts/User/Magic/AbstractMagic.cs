using Assets.Scripts.Misc;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Translator;
using Assets.Scripts.Repository;
using Assets.Scripts.User.Equipment;
using Assets.Scripts.User.Messages;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.User.Magic
{
    public class AbstractMagic : IPinnable
    {
        private static Transform _character;
        protected static Transform Character
        {
            get {
                if (_character == null)
                    _character = Util.GetCharacterTransform();
                return _character;
            }
        }

        private static CharacterEntity _characterEntity;
        protected static CharacterEntity CharacterEntity
        {
            get
            {
                if (_characterEntity == null)
                    _characterEntity = DI.Fetch<CharacterEntity>();
                return _characterEntity;
            }
        }

        private static EquipmentController _equipmentController;
        protected static EquipmentController EquipmentController
        {
            get
            {
                if (_equipmentController == null)
                    _equipmentController = DI.Fetch<EquipmentController>();
                return _equipmentController;
            }
        }


        public MagicData Data { get; set; } = new MagicData();

        public int Id => Data.Id;
        public bool CanUse => true;
        public float CoolDownTimer => Data.Cooldown;
        public Texture2D Icon => Data.Icon.texture;
        public string Title => Data.Title;
        public bool IsSingleUse => false;

        public virtual void OnPinned() { }

        public virtual void OnUnPinned() { }

        public virtual bool OnUsed()
        {
            // Nothing to load
            if (Data.Prefab == null)
                return false;

            // Checks if user has sufficient amount of magic left
            if (CharacterEntity.Magic < Data.MagicUsage)
            {
                DI.Fetch<MessageController>()?.AddMessage(T.Translate("Not enough magic!"));
                return false;
            }

            // Checks if min required LVL is reached
            if (CharacterEntity.Stats.CurrentLevel < Data.MinLevelRequirement)
            {
                DI.Fetch<MessageController>()?.AddMessage(T.Translate("Minimum required level is: ") + Data.MinLevelRequirement);
                return false;
            }

            // Could move to trigger based system... when equipping item 
            // Check if required items are equipped
            if (Data.EquippedItemRequirements != null && Data.EquippedItemRequirements.Length > 0)
            {
                foreach (var requiredItemId in Data.EquippedItemRequirements)
                {
                    if (!EquipmentController.IsItemWithIdEquipped(requiredItemId))
                    {
                        DI.Fetch<MessageController>()?.AddMessage(T.Translate("To use this spell, required item: ") + ItemRepository.Repository[requiredItemId].Title);
                        return false;
                    }
                }
            }

            var magicGo = Object.Instantiate(Data.Prefab);
            magicGo.transform.position = Character.position;

            CharacterEntity.Magic = -Data.MagicUsage;
            return true;
        }
    }
}
