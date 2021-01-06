using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.User.Controller;
using Assets.Scripts.User.Messages;
using UnityEngine;

namespace Assets.Scripts.User.Resource
{
    /// <summary>
    /// Raw resource material, used for object resources
    /// that user can collect to increase their resource count 
    /// </summary>
    public class CollectableResource : MonoBehaviour
    {
        /// <summary>
        /// Targeted resource
        /// </summary>
        public int ResourceId;

        /// <summary>
        /// Resource design styles
        /// </summary>
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
            // Sets random design
            GetComponent<SpriteRenderer>().sprite = Designs[R.RandomRange(0, Designs.Length)];
            UIController.ActionKeyPress += OnActionKeyPress;
        }

        /// <summary>
        /// When character is close enough and action key is pressed,
        /// then user can collect the resource
        /// </summary>
        /// <param name="sender">null</param>
        /// <param name="characterPosition">Location of character</param>
        private void OnActionKeyPress(object sender, Vector2 characterPosition)
        {
            if (Vector2.Distance(transform.position, characterPosition) > 0.8f) return;
            CharacterEntity.Resources[ResourceId]++;
            DI.Fetch<MessageController>()?.AddMessage("+ 1 resource");
            CleanUp();
        }

        /// <summary>
        /// Removes event subscriptions to resolve scene switching
        /// </summary>
        private void CleanUp()
        {
            UIController.ActionKeyPress -= OnActionKeyPress;
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            CleanUp();
        }
    }
}
