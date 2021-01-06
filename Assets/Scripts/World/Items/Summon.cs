using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.User;
using UnityEngine;

namespace Assets.Scripts.World.Items
{
    /// <summary>
    /// GameObject that initiates one of two summon type objects:
    /// - Fairy
    /// - Wizard
    /// Follows player and every X second adds ether Y amount of magic and or health
    /// </summary>
    public class Summon : MonoBehaviour
    {
        public float Speed;

        public float AddHealth;
        public float AddMagic;

        private float _timer = 2;
        private CharacterEntity _characterEntity;
        private Transform _followPoint;

        private void Start()
        {
            _characterEntity = DI.Fetch<CharacterEntity>();
            _followPoint = GameObject.FindGameObjectWithTag("SummonFollowPoint").transform;
            if (transform.parent.parent != null)
                transform.parent = transform.parent.parent;
        }

        private void Update()
        {
            // Updates timer and when its time adds health and/or magic
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                _characterEntity.Health = AddHealth;
                _characterEntity.Magic = AddMagic;
                _timer = 2;
            }

            // Moves towards player
            transform.position = Vector2.Lerp(transform.position, _followPoint.position, Time.deltaTime * Speed);
        }
    }
}