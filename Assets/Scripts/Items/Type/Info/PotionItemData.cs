using System.Collections.Generic;
using Assets.Scripts.Misc;
using Assets.Scripts.User.Attributes;

namespace Assets.Scripts.Items.Type.Info
{
    /// <summary>
    /// Holds information about items that player can
    /// use to change its attributes & stats
    /// </summary>
    public class PotionItemData : ItemData
    {
        /// <inheritdoc cref="ItemData"/>
        public override string GetDataLoadPath()
        {
            return "Potions";
        }

        /// <summary>
        /// Information about what attributes are increased / decreased
        /// </summary>
        public AttributeData EffectAmount { get; set; }

        /// <summary>
        /// Whether or not potion is throwable (not implemented yet)
        /// </summary>
        public bool IsSplash { get; set; } = false;

        /// <summary>
        /// Counter for how many second attributes are increased,
        /// if value is 0 then permanent.
        /// </summary>
        public float BuffTime { get; set; } = 0;

        /// <inheritdoc cref="ItemData"/>
        public override List<object> LoadObjects(string path)
        {
            return new List<object>(
                Util.LoadJsonFromFile<List<PotionItemData>>($"{path}{GetDataLoadPath()}")
            );
        }
    }
}