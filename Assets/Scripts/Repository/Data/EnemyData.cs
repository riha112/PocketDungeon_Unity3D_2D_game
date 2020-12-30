using Assets.Scripts.Misc.Translator;
using Assets.Scripts.User.Attributes;
using UnityEngine;

namespace Assets.Scripts.Repository.Data
{
    public class EnemyData : IIndexable
    {
        public int Id { get; set; }
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

        public AttributeData BaseAttributes { get; set; }
        public AttributeData AttributesMultiplierByLevel { get; set; }

        public int ExpToDrop { get; set; }
        public int ExpMultiplierByLevel { get; set; }

        public float AggroRange { get; set; } = 2;
        public bool KeepAggroIfAttacked { get; set; } = true;
        public bool KeepAggroAfterFirstInRange { get; set; } = true;
        public float AttackDistance { get; set; } = 1.25f;
        public float BaseCoolDownForAttack { get; set; } = 5;

        public GameObject[] Prefabs { get; private set; }

        public string[] PathToPrefabs
        {
            set
            {
                Prefabs = new GameObject[value.Length];
                for (var i = 0; i < Prefabs.Length; i++)
                    Prefabs[i] = Resources.Load<GameObject>($"Prefabs/Enemy/{value[i]}");
            }
        }

        public Texture2D Icon { get; private set; }

        public string PathToIcon
        {
            set => Icon = Resources.Load<Texture2D>($"Icons/Enemies/{value}");
        }
    }
}