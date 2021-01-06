using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Items;

namespace Assets.Scripts.Repository
{
    /// <summary>
    /// Repository that holds all item configurations for Item type objects
    /// - Supports dynamic Data holders
    /// </summary>
    public static class ItemRepository
    {
        private static Dictionary<ItemType, Type> _typeClassReferenceTable;
        private static Dictionary<int, ItemData> _itemRepository;

        public static Dictionary<int, ItemData> Repository
        {
            get
            {
                if (_itemRepository == null) BuildItemLibrary();
                return _itemRepository;
            }
        }

        /// <summary>
        /// Populates repository with item global ids and their info class instances
        /// </summary>
        public static void BuildItemLibrary()
        {
            _itemRepository = new Dictionary<int, ItemData>();

            // Loads all class types that are derived from ItemData class
            var itemInfoTypes =
                Assembly.GetExecutingAssembly().GetTypes().Where(t =>
                    t.IsClass && t.IsSubclassOf(typeof(ItemData)) || t == typeof(ItemData));

            foreach (var typeInfoHolder in itemInfoTypes)
            {
                // Creates new instance of the data object class, to access its functions
                var instance = Activator.CreateInstance(typeInfoHolder);

                // Fetches list of items with same type as previously created object
                dynamic items = typeInfoHolder.GetMethod("LoadObjects")?.Invoke(instance,
                    new object[]
                    {
                        "Register/Items/"
                    });

                if (!(items is List<object> list)) continue;

                // Registers all items into repository
                foreach (var item in list)
                    if (item is ItemData item1)
                        _itemRepository.Add(item1.ItemId, item1);
            }
        }

        /// <summary>
        /// Creates item from items global Id
        /// </summary>
        /// <param name="id">Item Id (global)</param>
        /// <returns>Instance of items object</returns>
        public static SimpleItem GetItemObjectFromId(int id)
        {
            // Rebuild Reference table
            if (_typeClassReferenceTable == null)
                BuildTypeClassReferenceTable();

            // Checks if item with this global id exists
            if (!Repository.ContainsKey(id))
                return null;

            // Loads ItemData object from repository
            var info = Repository[id];

            // Sets default type to SimpleItem
            var type = typeof(SimpleItem);

            // Tries to fetch correct class type based on requested items ItemType value
            if (_typeClassReferenceTable != null && _typeClassReferenceTable.ContainsKey(info.Type))
                type = _typeClassReferenceTable[info.Type];

            // Creates instance of object with correct type
            var instance = (SimpleItem) Activator.CreateInstance(type);
            instance.Info = info;

            return instance;
        }

        /// <summary>
        /// Build reference table for enum ItemType and assigns
        /// each of its values an class that is ether SimpleItem or Derived from SimpleItem
        /// class, so that later it can be used to create correct object with correct type of
        /// class.
        /// </summary>
        private static void BuildTypeClassReferenceTable()
        {
            _typeClassReferenceTable = new Dictionary<ItemType, Type>();

            // Loads all class types that are derived from Simple item
            var itemTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && t.IsSubclassOf(typeof(SimpleItem)));

            foreach (var itemType in itemTypes)
            {
                // Creates new instance for this class so that we can access
                // its method Resolves that contains list of ItemTypes that this
                // class can be used for.
                var instance = (SimpleItem) Activator.CreateInstance(itemType);

                // Creates reference table - read summary for function
                foreach (var resolvedType in instance.Resolves())
                    _typeClassReferenceTable.Add(resolvedType, itemType);
            }
        }
    }
}