using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.User;
using Assets.Scripts.User.Messages;
using UnityEngine;

namespace Assets.Scripts.World.Items
{
    /// <summary>
    /// When character collides with object, adds exp to collided character
    /// </summary>
    public class ExperiencePoint : MonoBehaviour
    {
        public int ExpToAdd = 1;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag != "Player") return;
            DI.Fetch<MessageController>()?.AddMessage($"+ {ExpToAdd} exp points");
            DI.Fetch<CharacterEntity>()?.AddExp(ExpToAdd);
            Destroy(gameObject);
        }
    }
}