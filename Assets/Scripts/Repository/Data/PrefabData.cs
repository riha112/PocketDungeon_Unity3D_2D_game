using Assets.Scripts.Misc.Translator;
using UnityEngine;

namespace Assets.Scripts.Repository.Data
{
    public class PrefabData : IIndexable
    {
        public int Id { get; set; }

        private string _title;

        public string Title
        {
            get => _title;
            set => _title = T.Translate(value);
        }

        public GameObject Prefab;

        public string PathToPrefab
        {
            set => Prefab = Resources.Load<GameObject>($"Prefabs/World/Items/{value}");
        }
    }
}