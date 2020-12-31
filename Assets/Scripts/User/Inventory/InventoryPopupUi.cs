using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Items;
using Assets.Scripts.Misc.GUI;
using Assets.Scripts.Misc.ObjectManager;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.User.Inventory
{
    public class InventoryPopupUi : Popup
    {
        #region Config
        private const short INV_WIDTH = 4;
        private const short INV_HEIGHT = 20;
        #endregion

        private List<SimpleItem> FilteredOutInventoryItems { get; set; }
        private int _activeItemId = -1;
        private Vector2 _scrollPosition = Vector2.zero;

        protected override void Init()
        {
            LMC.BuildCachedMessages();
            RectConfig.Popup = new Rect(ScreenSize.x / 2 + 10, ScreenSize.y / 2 - 260, 360, 480);
            RectConfig.Body = new Rect(20, 30, 320, 420);
        }

        protected override void Start()
        {
            InventoryManager.InventoryUpdated += OnInventoryUpdated;
            Search(SearchMessage);
            ReactCache = GetRectCache();
            base.Start();
        }

        private void OnInventoryUpdated([CanBeNull] object sender, SimpleItem item)
        {
            Search(SearchMessage);
        }

        /**
         * Search function
         * - Implementation of parent classes "Searchable" methods "Search"
         * - Filters out inventory items based on their name
         * <param name="text">Searchable text</param>
         */
        protected override void Search(string text)
        {
            FilteredOutInventoryItems = string.IsNullOrEmpty(text)
                ? InventoryManager.InventoryGrid
                : InventoryManager.InventoryGrid.Where(item => item.Info.Title.ToLower().Contains(text.ToLower()))
                    .ToList();
            _activeItemId = -1;
        }

        protected static Rect[] GetRectCache()
        {
            var rectList = new List<Rect>
            {
                new Rect(10, 40, 70, 30),
                new Rect(92.5f, 40, 70, 30),
                new Rect(175, 40, 70, 30)
            };

            for (short y = 0; y < INV_HEIGHT; y++)
            for (short x = 0; x < INV_WIDTH; x++)
            {
                rectList.Add(new Rect(x * 65 + 22, y * 65 + 60, 20, 10));
                rectList.Add(new Rect(0, y * 65 + 70, 255, 80));
                rectList.Add(new Rect(x * 65, y * 65 + 90, 60, 60));
                rectList.Add(new Rect(x * 65, y * 65, 60, 60));
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
            _scrollPosition = GUI.BeginScrollView(new Rect(25, 25, 275, 340), _scrollPosition,
                new Rect(0, 0, 250, 65 * INV_HEIGHT + 15));
            for (short y = 0; y < INV_HEIGHT; y++)
            {
                var hasOffset = _activeItemId != -1 && y > _activeItemId / INV_WIDTH;

                for (short x = 0; x < INV_WIDTH; x++)
                {
                    // NOTE: May require performance caching 
                    var index = x + y * INV_WIDTH;
                    var style = "inv_item";
                    var iconReact = hasOffset ? ReactCache[index * 4 + 5] : ReactCache[index * 4 + 6];

                    if (FilteredOutInventoryItems != null && FilteredOutInventoryItems.Count > index)
                    {
                        if (_activeItemId == index)
                        {
                            style = "inv_item_active";
                            GUI.Box(ReactCache[index * 4 + 3], "", "inv_item_active_bg_top");
                            GUI.BeginGroup(ReactCache[index * 4 + 4], "", "inv_item_active_bg");

                            GUI.Label(new Rect(15, 10, 225, 20), FilteredOutInventoryItems[index].Info.Title,
                                "inv_item_title");

                            // Info
                            if (GUI.Button(ReactCache[0], LT("About"), "inv_item_active_btn"))
                                DI.Fetch<ItemInformation>()?.ShowInfoAbout(FilteredOutInventoryItems[index]);

                            // Use
                            if (GUI.Button(ReactCache[1], LT("Use"), "inv_item_active_btn"))
                                FilteredOutInventoryItems[index].Use();

                            // Drop
                            if (GUI.Button(ReactCache[2], LT("Drop"), "inv_item_active_btn"))
                                InventoryManager.DropItem(FilteredOutInventoryItems[index]);

                            GUI.EndGroup();
                        }

                        if (GUI.Button(iconReact, FilteredOutInventoryItems[index].Info.Icon.texture, style))
                            _activeItemId = _activeItemId == index ? -1 : index;
                    }
                    else
                    {
                        GUI.Box(iconReact, "", style);
                    }
                }
            }

            GUI.EndScrollView();
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
        protected override List<string> GetLmcLibrary()
        {
            return new List<string>
            {
                "About",
                "Use",
                "Drop"
            };
        }
    }
}