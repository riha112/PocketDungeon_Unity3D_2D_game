using System.Collections.Generic;
using Assets.Scripts.Misc;
using Assets.Scripts.Misc.Translator;
using UnityEngine;

namespace Assets.Scripts.Items
{
    /**
     * Holds information about games Item
     */
    public class ItemData
    {
        public virtual string GetDataLoadPath()
        {
            return "Items";
        }

        public int ItemId { get; set; }

        private string _title;

        public string Title
        {
            get => _title;
            set => _title = T.Translate(value);
        }

        private string _description;

        public string Description
        {
            get => _description;
            set => _description = T.Translate(value);
        }

        public GameObject[] Prefabs { private set; get; }

        public string[] PathToPrefabs
        {
            set
            {
                Prefabs = new GameObject[value.Length];
                for (var i = 0; i < value.Length; i++)
                    Prefabs[i] = Resources.Load<GameObject>($"Prefabs/Items/{value[i]}");
            }
        }

        public Sprite Icon { private set; get; }

        public string PathToIcon
        {
            set => Icon = Resources.Load<Sprite>($"Icons/Items/{value}");
        }

        public ItemType Type { get; set; }
        public ItemSlot Slot { get; set; }

        public float DurabilityReducer { get; set; }

        public virtual List<object> LoadObjects(string path)
        {
            return new List<object>(
                Util.LoadJsonFromFile<List<ItemData>>($"{path}{GetDataLoadPath()}")
            );
        }
    }
}