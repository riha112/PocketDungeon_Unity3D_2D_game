using Assets.Scripts.Misc;
using UnityEngine;

namespace Assets.Scripts.Items.Type.Controller.Weapons
{
    /// <summary>
    /// Weapon class that instantiates projectile,
    /// towards the mouse
    /// </summary>
    public class BowWeaponItem : WeaponItem
    {
        /// <inheritdoc cref="SimpleItem"/>
        public override ItemType[] Resolves()
        {
            return new[] {ItemType.WeaponBow};
        }

        protected virtual string PATH_TO_DEFAULT_PROJECTILE => "prefab_weapon_projectile_bow_default_arrow";

        private GameObject _projectile;

        protected GameObject Projectile
        {
            get
            {
                if (_projectile is null)
                    _projectile = Info.Prefabs.Length > 1
                        ? Info.Prefabs[2]
                        : Resources.Load<GameObject>($"Prefabs/Items/Weapons/Projectiles/{PATH_TO_DEFAULT_PROJECTILE}");
                return _projectile;
            }
        }

        /// <summary>
        /// Initiates projectile towards mouse
        /// </summary>
        public override void Attack()
        {
            if (Projectile == null || !CanAttack)
                return;

            var projectile = MonoUtil.Load(Projectile);
            projectile.transform.position = Util.GetCharacterTransform().position;
            projectile.transform.right = Util.GetDirectionTowardsCursor(projectile.transform);
            MonoUtil.Remove(projectile, 12);

            SetCollDown();

            base.Attack();
        }
    }
}
