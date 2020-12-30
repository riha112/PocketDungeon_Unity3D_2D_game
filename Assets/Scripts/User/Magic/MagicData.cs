using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Items;
using Assets.Scripts.Repository;
using UnityEngine;

namespace Assets.Scripts.User.Magic
{
    public class MagicData : IIndexable
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public float MagicUsage { get; set; } = 5;
        public float Cooldown { get; set; } = 1;
        public int MinLevelRequirement { get; set; } = 0;
        public int[] EquippedItemRequirements { get; set; }

        public GameObject Prefab { get; set; }
        private string _pathToObject;
        public string PathToObject
        {
            get => _pathToObject;
            set
            {
                _pathToObject = value;
                Prefab = Resources.Load<GameObject>($"Prefabs/Magic/{_pathToObject}");
            }
        }

        public Sprite Icon { get; set; }
        private string _pathToIcon;
        public string PathToIcon
        {
            get => _pathToIcon;
            set
            {
                _pathToIcon = value;
                Icon = Resources.Load<Sprite>($"Icons/Magic/{_pathToIcon}");
            }
        }
    }
}
