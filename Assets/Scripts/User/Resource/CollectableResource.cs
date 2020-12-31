using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.User.Controller;
using Assets.Scripts.User.Messages;
using UnityEngine;

namespace Assets.Scripts.User.Resource
{
    public class CollectableResource : MonoBehaviour
    {
        public int ResourceId;
        public Sprite[] Designs;

        private static CharacterEntity _characterEntity;
        private static CharacterEntity CharacterEntity
        {
            get
            {
                if (_characterEntity == null)
                    _characterEntity = DI.Fetch<CharacterEntity>();
                return _characterEntity;
            }
        }

        private void Start()
        {
            GetComponent<SpriteRenderer>().sprite = Designs[R.RandomRange(0, Designs.Length)];
            UIController.ActionKeyPress += OnActionKeyPress;
        }

        private void OnActionKeyPress(object sender, Vector2 characterPosition)
        {
            if (Vector2.Distance(transform.position, characterPosition) > 0.8f) return;

            CharacterEntity.Resources[ResourceId]++;
            UIController.ActionKeyPress -= OnActionKeyPress;
            DI.Fetch<MessageController>().AddMessage("+ 1 resource");
            Destroy(gameObject);
        }
    }
}
