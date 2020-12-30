using UnityEngine;

namespace Assets.Scripts.Items.Type.Controller.Weapons
{
    public class InstantiableWeapon : WeaponItem
    {
        public override ItemType[] Resolves() => new[] { ItemType.WeaponInstantiable };

        private GameObject _cachedInstance;

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

        protected override bool OnDismount()
        {
            if (_cachedInstance != null)
            {
                MonoUtil.Remove(_cachedInstance);
            }
            return base.OnDismount();
        }
    }

}
