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
        private static Dictionary<int, ItemData> _itemLibrary;

        public static Dictionary<int, ItemData> Repository
        {
            get
            {
                if (_itemLibrary == null) BuildItemLibrary();
                return _itemLibrary;
            }
        }

        public static void BuildItemLibrary()
        {
            _itemLibrary = new Dictionary<int, ItemData>();

            var itemInfoTypes =
                Assembly.GetExecutingAssembly().GetTypes().Where(t =>
                    t.IsClass && t.IsSubclassOf(typeof(ItemData)) || t == typeof(ItemData));

            foreach (var typeInfoHolder in itemInfoTypes)
            {
                var instance = Activator.CreateInstance(typeInfoHolder);
                dynamic items = typeInfoHolder.GetMethod("LoadObjects")?.Invoke(instance,
                    new[]
                    {
                        "Register/Items/"
                    });

                if (!(items is List<object> list)) continue;
                foreach (var item in list)
                    if (item is ItemData item1)
                        _itemLibrary.Add(item1.ItemId, item1);
            }
        }

        public static SimpleItem GetItemObjectFromId(int id)
        {
            if (_typeClassReferenceTable == null)
                BuildTypeClassReferenceTable();

            if (!Repository.ContainsKey(id))
                return null;

            var info = Repository[id];
            var type = typeof(SimpleItem);

            if (_typeClassReferenceTable != null && _typeClassReferenceTable.ContainsKey(info.Type))
                type = _typeClassReferenceTable[info.Type];

            var instance = (SimpleItem) Activator.CreateInstance(type);
            instance.Info = info;
            return instance;
        }

        private static void BuildTypeClassReferenceTable()
        {
            _typeClassReferenceTable = new Dictionary<ItemType, Type>();
            var itemTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && t.IsSubclassOf(typeof(SimpleItem)));
            foreach (var itemType in itemTypes)
            {
                var instance = (SimpleItem) Activator.CreateInstance(itemType);
                foreach (var resolvedType in instance.Resolves()) _typeClassReferenceTable.Add(resolvedType, itemType);
            }
        }
    }
}