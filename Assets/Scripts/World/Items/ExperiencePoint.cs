using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.User;
using UnityEngine;

namespace Assets.Scripts.World.Items
{
    public class ExperiencePoint : MonoBehaviour
    {
        public int ExpToAdd = 1;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag != "Player") return;
            DI.Fetch<CharacterEntity>()?.AddExp(ExpToAdd);
            Destroy(gameObject);
        }
    }
}
