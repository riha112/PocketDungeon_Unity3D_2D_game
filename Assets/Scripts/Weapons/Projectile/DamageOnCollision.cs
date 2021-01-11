using Assets.Scripts.Entity;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.User;
using UnityEngine;

namespace Assets.Scripts.Weapons.Projectile
{
    /// <summary>
    /// Controller for objects that adds damage to IDamagable objects
    /// when they collide.
    /// </summary>
    public class DamageOnCollision : MonoBehaviour
    {
        /// <summary>
        /// Amount of damage to give
        /// </summary>
        public int HitPoints;

        /// <summary>
        /// Set this object as the part of the hit object when
        /// collided
        /// </summary>
        public bool GlueOnHit = true;

        /// <summary>
        /// OffsetY for object from the targets center when "gluing" it
        /// onto target.
        /// </summary>
        public Vector2 Offset;

        /// <summary>
        /// Give damage only once
        /// </summary>
        public bool DisableOnHit = true;

        protected virtual string GetTargetTag()
        {
            return "Enemy";
        }

        protected void Start()
        {
            if (HitPoints < 0) HitPoints = DI.Fetch<CharacterEntity>()?.Stats.HitPoints ?? 0;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == GetTargetTag()) HitTarget(col);
            else if (col.tag == "Tile") HitTile(col);
            else if (col.tag == "Resource") HitResource(col);
        }

        /// <summary>
        /// Called when objects hits world Tile
        /// </summary>
        /// <param name="col">Tiles collider</param>
        protected virtual void HitTile(Collider2D col)
        {
        }

        /// <summary>
        /// Called when object hits resource object
        /// </summary>
        /// <param name="col">Resources collider</param>
        protected virtual void HitResource(Collider2D col)
        {
            if (GlueOnHit)
                GlueToCollider(col);

            col.GetComponent<IDamagable>()?.TakeDamage(HitPoints);

            if (DisableOnHit)
                Disable();
        }

        /// <summary>
        /// Called when object hits target
        /// </summary>
        /// <param name="col"></param>
        protected virtual void HitTarget(Collider2D col)
        {
            if (GlueOnHit)
                GlueToCollider(col);

            col.GetComponent<IDamagable>()?.TakeDamage(HitPoints);

            if (DisableOnHit)
                Disable();
        }

        /// <summary>
        /// Disables object
        /// </summary>
        protected virtual void Disable()
        {
            GetComponent<BoxCollider2D>().enabled = false;
            enabled = false;
        }

        /// <summary>
        /// Attaches object onto its target
        /// </summary>
        /// <param name="col">targets collider</param>
        protected virtual void GlueToCollider(Collider2D col)
        {
            transform.parent = col.transform;
            transform.localPosition = Vector3.zero;
            transform.position += transform.right * Offset.x;
            transform.position += col.transform.up * Offset.y;
        }
    }
}