using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.Items;
using Assets.Scripts.Misc.ObjectManager;
using UnityEngine;
using Assets.Scripts.UI;
using Newtonsoft.Json;

namespace Assets.Scripts.User.Inventory
{
    public class InventoryController : Popup
    {
        private const short INV_WIDTH = 4;
        private const short INV_HEIGHT = 20;

        public List<SimpleItem> InventoryGrid { get; private set; } = new List<SimpleItem>();
        private List<SimpleItem> FilteredOutInventoryItems { get; set; }
        private int ActiveItemId { get; set; } = -1;

        private Vector2 scrollPosition = Vector2.zero;
        private GameObject ItemDropPrefab;
        private bool ShowInfoScreen { get; set; }

        protected override void Init()
        {
            LMC.BuildCachedMessages();
            RectConfig.Popup = new Rect(ScreenSize.x / 2 + 10, ScreenSize.y / 2 - 260, 360, 480);
            RectConfig.Body = new Rect(20, 30, 320, 420);

            //InventoryGrid = InventoryTestData.GetTesItems();

            Search(SearchMessage);

            ItemDropPrefab = Resources.Load<GameObject>("Prefabs/Player/Misc/DroppedItem");

            ReactCache = GetRectCache();
        }

        /**
         * Search function
         * - Implementation of parent classes "Searchable" methods "Search"
         * - Filters out inventory items based on their name
         *
         * <param name="text">Searchable text</param>
         */
        protected override void Search(string text)
        {
            InventoryGrid.Sort((x, y) =>
                string.Compare(x.Info.Title, y.Info.Title, StringComparison.OrdinalIgnoreCase));

            FilteredOutInventoryItems = (string.IsNullOrEmpty(text)) ? InventoryGrid :
                InventoryGrid.Where(item => item.Info.Title.ToLower().Contains(text.ToLower())).ToList();
            ActiveItemId = -1;
            ShowInfoScreen = false;
        }

        public void RemoveItem(int id)
        {
            var itemId = InventoryGrid.FindIndex(i => i.LocalId == id);
            if(itemId >= 0 && itemId < InventoryGrid.Count)
                InventoryGrid.RemoveAt(itemId);
            Search(SearchMessage);
        }

        public void AddItem(SimpleItem item)
        {
            InventoryGrid.Add(item);
            Search(SearchMessage);
        }

        public override void Toggle(bool state)
        {
            ShowInfoScreen = false;
            base.Toggle(state);
        }

        public List<T> FetchFilteredItems<T>()
        {
            var output = new List<T>();
            foreach (var item in InventoryGrid)
            {
                if (item is T item1)
                    output.Add(item1);
            }
            Debug.Log(output.Count);
            return output;
            // InventoryGrid.Where(item => item.GetType() == typeof(T)).ToList();
        }


        protected static Rect[] GetRectCache()
        {
            var rectList = new List<Rect>()
            {
                new Rect(10, 40, 70, 30),
                new Rect(92.5f, 40, 70, 30),
                new Rect(175, 40, 70, 30)
            };

            for (short y = 0; y < INV_HEIGHT; y++)
            {
                for (short x = 0; x < INV_WIDTH; x++)
                {
                    rectList.Add(new Rect(x * 65 + 22, y * 65 + 60, 20, 10));
                    rectList.Add(new Rect(0, y * 65 + 70, 255, 80));
                    rectList.Add(new Rect(x * 65, y * 65 + 90, 60, 60));
                    rectList.Add(new Rect(x * 65, y * 65, 60, 60));
                }
            }

            return rectList.ToArray();
        }

        /**
         * Outputs GUI
         * - Implementation of parent classes "Popup" methods "DrawBody"
         * - Draw UI design
         */
        protected override void DrawBody()
        { 
            scrollPosition = GUI.BeginScrollView(new Rect(25, 25, 275, 340), scrollPosition, new Rect(0, 0, 250, 65 * INV_HEIGHT + 15));
            for (short y = 0; y < INV_HEIGHT; y++)
            {
                var hasOffset = ActiveItemId != -1 && y > ActiveItemId / INV_WIDTH;

                for (short x = 0; x < INV_WIDTH; x++)
                {
                    // NOTE: May require performance caching 
                    var index = x + y * INV_WIDTH;
                    var style = "inv_item";
                    var iconReact = hasOffset ? ReactCache[index * 4 + 5] : ReactCache[index * 4 + 6];

                    if (FilteredOutInventoryItems != null && FilteredOutInventoryItems.Count > index)
                    {
                        if (ActiveItemId == index)
                        {
                            style = "inv_item_active";
                            GUI.Box(ReactCache[index * 4 + 3], "", "inv_item_active_bg_top");
                            GUI.BeginGroup(ReactCache[index * 4 + 4], "", "inv_item_active_bg");

                            GUI.Label(new Rect(15, 10, 225, 20), FilteredOutInventoryItems[index].Info.Title, "inv_item_title");

                            // Info
                            if (GUI.Button(ReactCache[0], LMC.CachedMessages[0], "inv_item_active_btn"))
                            {
                                ShowInfoScreen = !ShowInfoScreen;
                            }

                            // Use
                            if (GUI.Button(ReactCache[1], LMC.CachedMessages[1], "inv_item_active_btn"))
                            {
                                Debug.Log(FilteredOutInventoryItems[index].Info.Title);
                                Debug.Log(FilteredOutInventoryItems[index].LocalId);
                                Debug.Log(FilteredOutInventoryItems[index].GetType());
                                FilteredOutInventoryItems[index].Use();
                            }

                            // Drop
                            if (GUI.Button(ReactCache[2], LMC.CachedMessages[2], "inv_item_active_btn"))
                            {
                                DropItem(FilteredOutInventoryItems[index]);
                            }

                            GUI.EndGroup();
                        }

                        if (GUI.Button(iconReact, FilteredOutInventoryItems[index].Info.Icon.texture, style))
                        {
                            ActiveItemId = (ActiveItemId == index) ? -1 : index;
                        }
                    }
                    else
                    {
                        GUI.Box(iconReact, "", style);
                    }
                }
            }
            GUI.EndScrollView();
        }

        protected override void DrawPopup()
        {
            base.DrawPopup();
            if (!ShowInfoScreen) return;
            DrawInfoScreen();
        }

        protected void DropItem(SimpleItem item)
        {
            RemoveItem(item.LocalId);
            item.UnUse();
          //  return;

            //var itemGo = Instantiate(ItemDropPrefab);
            //itemGo.transform.position = transform.position;

            //itemGo.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.Info.Icon;
        }

        protected void DrawInfoScreen()
        {
            GUI.depth = 0;
            if(GUI.Button(new Rect(0, 0, ScreenSize.x, ScreenSize.y), "", "popup_bg"))
            {
                ShowInfoScreen = false;
            }

            GUI.BeginGroup(new Rect(ScreenSize.x / 2 - 200, ScreenSize.y / 2 - 170, 400, 300), "", "popup_body");
            GUI.EndGroup();
        }

        public SimpleItem GetItemByItemLocalId(int itemLocalId)
        {
            var itemId = InventoryGrid.FindIndex(i => i.LocalId == itemLocalId);
            if (itemId >= 0 && itemId < InventoryGrid.Count)
                return InventoryGrid[itemId];
            return null;
        }

        /**
         * Makes search message
         */
        protected override void DrawOverlay()
        {
            SearchMessage = GUI.TextField(new Rect(0, 375, 360, 75), SearchMessage, "search");
        }

        /**
         * Populates Cached Translated library
         */
        protected override List<string> GetLmcLibrary() => new List<string>()
        {
            "About",
            "Use",
            "Drop"
        };
    }
}
