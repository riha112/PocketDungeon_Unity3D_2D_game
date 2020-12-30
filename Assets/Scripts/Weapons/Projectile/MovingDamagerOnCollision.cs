using UnityEngine;

namespace Assets.Scripts.Weapons.Projectile
{
    public class MovingDamagerOnCollision : DamageOnCollision
    {
        [Range(-25, 25)] public float Speed = 10;

        protected virtual void Update()
        {
            transform.position += transform.right * Time.deltaTime * Speed;
        }
    }
}