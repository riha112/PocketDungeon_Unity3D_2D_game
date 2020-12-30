using System;
using Assets.Scripts.Misc;
using UnityEngine;

namespace Assets.Scripts.Items.Type.Controller.Weapons
{
    public class ThrowableWeaponItem : BowWeaponItem
    {
        public override ItemType[] Resolves() => new[] { ItemType.WeaponThrowable };

        protected override string PATH_TO_DEFAULT_PROJECTILE => "prefab_weapon_projectile_throwable_default_dagger";

        public override void Attack()
        {
            if (Projectile == null || !CanAttack)
                return;

            var projectile = MonoUtil.Load(Projectile);
            projectile.transform.position = Util.GetCharacterTransform().position;
            projectile.transform.right = Util.GetDirectionTowardsCursor(projectile.transform);

            try
            {
                projectile.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite =
                    Info.Prefabs[0].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
            }
            catch (Exception)
            {
                // ignored
            }

            SetCollDown();
            MonoUtil.Remove(projectile, 6);

            Durability -= Info.DurabilityReducer;
        }
    }
}
