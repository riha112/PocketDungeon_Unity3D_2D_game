using Assets.Scripts.User.Magic;
using UnityEngine;

namespace Assets.Scripts.Items.Type.Controller
{
    public class GemItem : EquipableItem, IPinnable
    {
        /// <inheritdoc cref="SimpleItem"/>
        public override ItemType[] Resolves()
        {
            return new[] {ItemType.Gem};
        }

        #region IPinnable

        public int Id => LocalId;
        public bool CanUse => true;
        public bool IsSingleUse => true;
        public float CoolDownTimer => 0;
        public Texture2D Icon => Info.Icon.texture;
        public string Title => Info.Title;

        #endregion

        /// <inheritdoc cref="SimpleItem"/>
        public override void Use()
        {
            base.Use();

            var particle = MonoUtil.Load(Info.Prefabs[0]);
            var parent = GameObject.FindGameObjectWithTag("Player");
            particle.transform.parent = parent.transform;
            particle.transform.localPosition = Vector3.zero;
        }

        /// <inheritdoc cref="IPinnable"/>
        public void OnPinned()
        {
        }

        /// <inheritdoc cref="IPinnable"/>
        public void OnUnPinned()
        {
        }

        /// <inheritdoc cref="IPinnable"/>
        public bool OnUsed()
        {
            Use();
            return true;
        }
    }
}