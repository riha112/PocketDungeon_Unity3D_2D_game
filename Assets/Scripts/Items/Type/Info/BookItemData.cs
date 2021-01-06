using System.Collections.Generic;
using Assets.Scripts.Misc;

namespace Assets.Scripts.Items.Type.Info
{
    /// <summary>
    /// Holds information about items that player
    /// can use to learn new spells.
    /// </summary>
    public class BookItemData : ItemData
    {
        /// <inheritdoc cref="ItemData"/>
        public override string GetDataLoadPath()
        {
            return "Books";
        }

        /// <summary>
        /// Holds information about which magic can be learned when used
        /// </summary>
        public int MagicId;

        /// <inheritdoc cref="ItemData"/>
        public override List<object> LoadObjects(string path)
        {
            return new List<object>(
                Util.LoadJsonFromFile<List<BookItemData>>($"{path}{GetDataLoadPath()}")
            );
        }
    }
}