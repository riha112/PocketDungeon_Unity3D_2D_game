using System;
using System.Collections.Generic;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.User.Inventory
{
    public static class InventoryManager
    {
        private static List<SimpleItem> _inventoryGrid = new List<SimpleItem>();

        public static List<SimpleItem> InventoryGrid
        {
            get => _inventoryGrid;
            set
            {
                _inventoryGrid = value;
                _inventoryGrid.Sort((x, y) =>
                    string.Compare(x.Info.Title, y.Info.Title, StringComparison.OrdinalIgnoreCase));
            }
        }

        public static EventHandler<SimpleItem> InventoryUpdated;

        public static void Reset()
        {
            _inventoryGrid = new List<SimpleItem>();
        }

        /// <summary>
        /// Adds item to inventory
        /// </summary>
        /// <param name="item">Item to add</param>
        public static void AddItem(SimpleItem item)
        {
            InventoryUpdated?.Invoke(null, item);
            InventoryGrid.Add(item);
        }

        /// <summary>
        /// Fetches list of T type items within inventory
        /// </summary>
        /// <typeparam name="T">Type of items to fetch</typeparam>
        /// <returns>Lift of T type items</returns>
        public static List<T> FetchFilteredItems<T>()
        {
            var output = new List<T>();
            foreach (var item in InventoryGrid)
            {
                if (item is T item1)
                    output.Add(item1);
            }
            return output;
        }

        /// <summary>
        /// Fetches item from inventory by Items local ID
        /// </summary>
        /// <param name="itemLocalId">Local Id of item</param>
        /// <returns>Item/Null</returns>
        public static SimpleItem GetItemByItemLocalId(int itemLocalId)
        {
            var itemId = InventoryGrid.FindIndex(i => i.LocalId == itemLocalId);
            if (itemId >= 0 && itemId < InventoryGrid.Count)
                return InventoryGrid[itemId];
            return null;
        }

        /// <summary>
        /// Removes item from inventory by object
        /// </summary>
        /// <param name="item">Item to remove</param>
        public static void DropItem(SimpleItem item)
        {
            RemoveItem(item.LocalId);
            item.UnUse();
        }

        /// <summary>
        /// Removes item from inventory by local id
        /// </summary>
        /// <param name="id">Local item id to removed</param>
        public static void RemoveItem(int id)
        {
            var itemId = InventoryGrid.FindIndex(i => i.LocalId == id);
            if (itemId < 0 || itemId >= InventoryGrid.Count) return;
            InventoryUpdated?.Invoke(null, InventoryGrid[itemId]);
            InventoryGrid.RemoveAt(itemId);
        }
    }
}
