using Assets.Scripts.Misc;
using Assets.Scripts.Misc.ObjectManager;

namespace Assets.Scripts.Items
{
    public class SimpleItem
    {
        // Change This
        public virtual ItemType[] Resolves()
        {
            return new[] {ItemType.Misc};
        }

        protected static int ItemCounter;

        private int _localId;

        public int LocalId
        {
            get => _localId;
            set
            {
                // Prevents from overlapping local ids
                if (ItemCounter <= value)
                    ItemCounter = value + 1;
                _localId = value;
            }
        }

        private MonoUtil _monoUtil;

        protected MonoUtil MonoUtil
        {
            get
            {
                if (_monoUtil is null)
                    _monoUtil = DI.Fetch<MonoUtil>();
                return _monoUtil;
            }
        }

        public ItemData Info { get; set; }
        public ItemGrade Grade { get; set; }

        public SimpleItem()
        {
            LocalId = ItemCounter++;
        }

        public virtual void Use()
        {
        }

        public virtual void UnUse()
        {
        }

        public virtual string GetDescription()
        {
            return $"{Info.Description}\n" +
                   $"<b>GRADE:</b>{Grade.ToString()}";
        }
    }
}