using System.Collections.Generic;
using Assets.Scripts.Misc;
using Assets.Scripts.User.Attributes;

namespace Assets.Scripts.Items.Type.Info
{
    /// <summary>
    /// Holds information about items that player
    /// can equip.
    /// </summary>
    public class EquipableItemData : ItemData
    {
        /// <summary>
        /// Holds information about how much item adds to
        /// players attributes when equipped.
        /// </summary>
        public AttributeData BaseAttributes { get; set; }

        /// <summary>
        /// Holds information about which slot can this item
        /// be equipped.
        /// </summary>
        public ItemSlot Slot { get; set; }

        /// <summary>
        /// Holds information about how much durability is
        /// reduced per use.
        /// </summary>
        public float DurabilityReducer { get; set; }

        /// <inheritdoc cref="ItemData"/>
        public override string GetDataLoadPath()
        {
            return "Equipable";
        }

        /// <inheritdoc cref="ItemData"/>
        public override List<object> LoadObjects(string path)
        {
            return new List<object>(
                Util.LoadJsonFromFile<List<EquipableItemData>>($"{path}{GetDataLoadPath()}")
            );
        }
    }
}