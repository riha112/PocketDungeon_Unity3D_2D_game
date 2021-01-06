using System.Collections.Generic;
using Assets.Scripts.Misc;

namespace Assets.Scripts.Items.Type.Info
{
    /// <summary>
    /// Holds information about items that player can
    /// attack with
    /// </summary>
    public class WeaponItemData : EquipableItemData
    {
        /// <inheritdoc cref="ItemData"/>
        public override string GetDataLoadPath()
        {
            return "Weapons";
        }

        /// <summary>
        /// Holds information about how much time must player
        /// wait between attacks
        /// </summary>
        public float AttackCoolDown { get; set; } = 0.5f;

        /// <inheritdoc cref="ItemData"/>
        public override List<object> LoadObjects(string path)
        {
            return new List<object>(
                Util.LoadJsonFromFile<List<WeaponItemData>>($"{path}{GetDataLoadPath()}")
            );
        }
    }
}
