using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.User.Attributes;
using Assets.Scripts.User.Equipment;
using Assets.Scripts.User.Inventory;

namespace Assets.Scripts.Items.Type.Controller
{
    public class EquipableItem : SimpleItem
    {
        public override ItemType[] Resolves() => new[] { ItemType.Armor };

        public bool IsEquipped { get; set; } = false;
        public AttributeData Attribute { get; set; } = new AttributeData();

        private float _durability = 1;

        public float Durability
        {
            get => _durability;
            set
            {
                _durability = value;
                if (_durability < 0)
                    Break();
            }
        }

        public bool Mount()
        {
            IsEquipped = true;
            return Info.Slot != ItemSlot.None && OnMount();
        }

        public bool Dismount()
        {
            IsEquipped = false;
            return OnDismount();
        }

        protected virtual bool OnMount()
        {
            return true;
        }

        protected virtual bool OnDismount()
        {
            return true;
        }

        public override void Use()
        {
            DI.Fetch<EquipmentController>()?.EquipItem(this);
        }

        public override void UnUse()
        {
            var EC = DI.Fetch<EquipmentController>();
            EC?.UnEquipItem(EC.FindSlotForItem(this));
        }

        /// <summary>
        /// Called when durability is less or equal to 0
        /// - Removes item from users inventory/equipment
        /// </summary>
        protected virtual void Break()
        {
            InventoryManager.DropItem(this);
        }
    }
}
