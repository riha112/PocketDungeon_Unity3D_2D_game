using Assets.Scripts.Misc;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Translator;

namespace Assets.Scripts.Items
{
    public class SimpleItem
    {
        /// <summary>
        /// Stores id of the las items _localId,
        /// used to generate _localId, by adding 1 to current counter.
        /// </summary>
        protected static int ItemCounter;

        /// <summary>
        /// Holds information about what type of items is this class responsible
        /// </summary>
        /// <returns></returns>
        // TODO: Change logic
        public virtual ItemType[] Resolves()
        {
            return new[] {ItemType.Misc};
        }

        /// <summary>
        /// Unique ID for item, differs from ItemData id, by
        /// that, that only one object instance can have this id
        /// </summary>
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


        /// <summary>
        /// As this is not an MonoBehaviour object and is instantiated via "new"
        /// keyword, we need a bridge to use Unity3D functions, for that reason we
        /// are using MonoUtil class.
        /// </summary>
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

        /// <summary>
        /// Sets _localId on creation
        /// </summary>
        public SimpleItem()
        {
            LocalId = ItemCounter++;
        }

        /// <summary>
        /// Called when executing inventory command "Use"
        /// </summary>
        public virtual void Use()
        {
        }

        /// <summary>
        /// Called when removing item from inventory aka "Drop" command
        /// </summary>
        public virtual void UnUse()
        {
        }

        /// <summary>
        /// Generates description for item
        /// </summary>
        /// <returns>Detailed description</returns>
        public virtual string GetDescription()
        {
            return $"{Info.Description}\n" +
                   $"<b>{T.Translate("Grade")}:</b>{Grade.ToString()}";
        }
    }
}