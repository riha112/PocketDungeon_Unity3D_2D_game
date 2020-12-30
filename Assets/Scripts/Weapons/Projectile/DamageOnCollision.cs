using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.User;
using UnityEngine;

namespace Assets.Scripts.Weapons.Projectile
{
    public class DamageOnCollision : MonoBehaviour
    {
        public int HitPoints;

        public bool GlueOnHit = true;
        public bool DisableOnHit = true;
        public Vector2 Offset;

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

        protected virtual void HitTile(Collider2D col)
        {
        }

        protected virtual void HitResource(Collider2D col)
        {
            if (GlueOnHit)
                GlueToCollider(col);

            col.GetComponent<IDamagable>()?.TakeDamage(HitPoints);

            if (DisableOnHit)
                Disable();
        }

        protected virtual void HitTarget(Collider2D col)
        {
            if (GlueOnHit)
                GlueToCollider(col);

            col.GetComponent<IDamagable>()?.TakeDamage(HitPoints);

            if (DisableOnHit)
                Disable();
        }

        protected virtual void Disable()
        {
            GetComponent<BoxCollider2D>().enabled = false;
            enabled = false;
        }

        protected virtual void GlueToCollider(Collider2D col)
        {
            transform.parent = col.transform;
            transform.localPosition = Vector3.zero;
            transform.position += transform.right * Offset.x;
            transform.position += col.transform.up * Offset.y;
        }
    }
}