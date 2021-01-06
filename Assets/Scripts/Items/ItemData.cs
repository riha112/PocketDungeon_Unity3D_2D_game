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
        public int ItemId { get; set; }

        #region Title
        private string _title;
        public string Title
        {
            get => _title;
            set => _title = T.Translate(value);
        }
        #endregion

        #region Description
        private string _description;
        public string Description
        {
            get => string.IsNullOrEmpty(_description) ? T.Translate("[No description]") : _description;
            set => _description = T.Translate(value);
        }
        #endregion

        #region Prefabs
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
        #endregion

        #region Icon
        public Sprite Icon { private set; get; }
        public string PathToIcon
        {
            set => Icon = Resources.Load<Sprite>($"Icons/Items/{value}");
        }

        #endregion

        public ItemType Type { get; set; }

        /// <returns>Returns JSONs file name from which to get information</returns>
        public virtual string GetDataLoadPath()
        {
            return "Items";
        }

        /// <summary>
        /// Converts file to specific type of objects
        /// - Used to resolve multi type object loading
        /// </summary>
        /// <param name="path">Directory where files are stored</param>
        /// <returns>List of objects containing ItemData objects with type of this class</returns>
        public virtual List<object> LoadObjects(string path)
        {
            return new List<object>(
                Util.LoadJsonFromFile<List<ItemData>>($"{path}{GetDataLoadPath()}")
            );
        }
    }
}