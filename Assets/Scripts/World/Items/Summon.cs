using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.User;
using UnityEngine;

namespace Assets.Scripts.World.Items
{
    /// <summary>
    /// 
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
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                _characterEntity.Health = AddHealth;
                _characterEntity.Magic = AddMagic;
                _timer = 2;
            }

            transform.position = Vector2.Lerp(transform.position, _followPoint.position, Time.deltaTime * Speed);
        }
    }
}