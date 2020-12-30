using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.User.Attributes;
using Assets.Scripts.User.Equipment;

namespace Assets.Scripts.Items.Type.Controller
{
    public class EquipableItem : SimpleItem
    {
        public override ItemType[] Resolves() => new[] { ItemType.Armor };

        public bool IsEquipped { get; set; } = false;
        public AttributeData Attribute { get; set; } = new AttributeData();
        public float Durability { get; set; } = 1;

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
    }
}
