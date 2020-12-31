using Assets.Scripts.Items.Type.Info;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.User.Inventory;
using Assets.Scripts.User.Magic;

namespace Assets.Scripts.Items.Type.Controller
{
    public class MagicItem : SimpleItem
    {
        public override ItemType[] Resolves() => new[] { ItemType.SpellBook };

        public BookItemData BookInfo
        {
            get
            {
                if (Info is BookItemData item)
                    return item;
                return null;
            }
        }

        public override void Use()
        {
            DI.Fetch<MagicController>()?.LearnMagic(BookInfo.MagicId);
            InventoryManager.DropItem(this);
        }
    }
}
