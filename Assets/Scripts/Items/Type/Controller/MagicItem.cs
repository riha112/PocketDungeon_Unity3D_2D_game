using Assets.Scripts.Items.Type.Info;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.User.Inventory;
using Assets.Scripts.User.Magic;

namespace Assets.Scripts.Items.Type.Controller
{
    /// <summary>
    /// Holds real items data for items with which user can use
    /// to learn new magic
    /// </summary>
    public class MagicItem : SimpleItem
    {
        /// <inheritdoc cref="SimpleItem"/>
        public override ItemType[] Resolves()
        {
            return new[] {ItemType.SpellBook};
        }

        /// <summary>
        /// Converts current ItemData object into BookItemData object
        /// </summary>
        public BookItemData BookInfo
        {
            get
            {
                if (Info is BookItemData item)
                    return item;
                return null;
            }
        }

        /// <inheritdoc cref="SimpleItem"/>
        public override void Use()
        {
            DI.Fetch<MagicController>()?.LearnMagic(BookInfo.MagicId);
            InventoryManager.DropItem(this);
        }
    }
}