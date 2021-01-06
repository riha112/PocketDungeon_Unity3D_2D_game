using UnityEngine;

namespace Assets.Scripts.Items.Type.Controller.Weapons
{
    /// <summary>
    /// Used for weapons that are stored in separate object,
    /// meaning that when equipping item, the new gameObject
    /// instance is created that do not use FightingController,
    /// <example>Wrecking Ball</example>
    /// </summary>
    public class InstantiableWeapon : WeaponItem
    {
        /// <inheritdoc cref="WeaponItem"/>
        public override ItemType[] Resolves()
        {
            return new[] {ItemType.WeaponInstantiable};
        }

        private GameObject _cachedInstance;

        /// <summary>
        /// Loads object into game world when mounted
        /// </summary>
        /// <returns>Whether or not item was successfully mounted</returns>
        protected override bool OnMount()
        {
            if (Info.Prefabs.Length > 1)
            {
                _cachedInstance = MonoUtil.Load(Info.Prefabs[1]);
                _cachedInstance.transform.parent = Camera.main.transform;
                _cachedInstance.GetComponent<InstantiableWeaponController>().Item = this;
                _cachedInstance.transform.localPosition = new Vector3(0, 0, _cachedInstance.transform.localPosition.z);
            }

            return base.OnMount();
        }

        /// <summary>
        /// Destroys object that was loaded into game world when mounted,
        /// called on dismount
        /// </summary>
        /// <returns>Whether or not item was successfully dismounted</returns>
        protected override bool OnDismount()
        {
            if (_cachedInstance != null) MonoUtil.Remove(_cachedInstance);
            return base.OnDismount();
        }
    }
}