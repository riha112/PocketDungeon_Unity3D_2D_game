using Assets.Scripts.Items.Type.Info;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Translator;
using Assets.Scripts.User.Attributes;
using Assets.Scripts.User.Equipment;
using Assets.Scripts.User.Inventory;

namespace Assets.Scripts.Items.Type.Controller
{
    /// <summary>
    /// Used for items that can be equipped onto character - weapons, armor, ...
    /// <seealso cref="EquipmentController"/>
    /// </summary>
    public class EquipableItem : SimpleItem
    {
        /// <inheritdoc cref="SimpleItem"/>
        public override ItemType[] Resolves()
        {
            return new[] {ItemType.Armor};
        }

        /// <summary>
        /// Converts current ItemData object into EquipableItemData object
        /// </summary>
        public EquipableItemData EquipableData
        {
            get
            {
                if (Info is EquipableItemData item)
                    return item;
                return null;
            }
        }

        public bool IsEquipped { get; set; }

        /// <summary>
        /// Holds items actual attribute increase rate, as they can differ
        /// from base rate because of items grade
        /// </summary>
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

        /// <summary>
        /// Called when equipping item
        /// </summary>
        /// <returns>Whether or not item was equipped</returns>
        public bool Mount()
        {
            IsEquipped = true;
            return EquipableData.Slot != ItemSlot.None && OnMount();
        }

        /// <summary>
        /// Called when dismounting item
        /// </summary>
        /// <returns>Whether or not item was dismounted</returns>
        public bool Dismount()
        {
            IsEquipped = false;
            return OnDismount();
        }

        /// <summary>
        /// Action that is happening while mounting item
        /// </summary>
        /// <returns>Whether or not item was equipped</returns>
        protected virtual bool OnMount()
        {
            return true;
        }

        /// <summary>
        /// Action that is happening while dismounting item
        /// </summary>
        /// <returns>Whether or not item was dismounted</returns>
        protected virtual bool OnDismount()
        {
            return true;
        }

        /// <inheritdoc cref="SimpleItem"/>
        public override void Use()
        {
            DI.Fetch<EquipmentController>()?.EquipItem(this);
        }

        /// <inheritdoc cref="SimpleItem"/>
        public override void UnUse()
        {
            var ec = DI.Fetch<EquipmentController>();
            ec?.UnEquipItem(ec.FindSlotForItem(this));
        }

        /// <summary>
        /// Called when durability is less or equal to 0
        /// - Removes item from users inventory/equipment
        /// </summary>
        protected virtual void Break()
        {
            InventoryManager.DropItem(this);
        }

        // TODO: Use enum names instead
        private static readonly string[] ATT_NAMES = { "Strength", "Vitality", "Agility", "Magic", "Resistance", "Luck" };

        /// <inheritdoc cref="SimpleItem"/>
        public override string GetDescription()
        {
            var output = base.GetDescription() +
                         $"\n<b>{T.Translate("Durability")}: </b>{Durability}";

            for (var i = 0; i < 6; i++) output += $"\n{T.Translate(ATT_NAMES[i])}: {Attribute[i]}/{EquipableData.BaseAttributes[i]}";

            return output;
        }
    }
}