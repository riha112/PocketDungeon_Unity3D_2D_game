using Assets.Scripts.User.Magic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Items.Type.Controller
{
    public class GemItem : EquipableItem, IPinnable
    {
        public override ItemType[] Resolves() => new[] { ItemType.Gem };

        public int Id => LocalId;
        public bool CanUse => true;
        public bool IsSingleUse => true;
        public float CoolDownTimer => 0;
        public Texture2D Icon => Info.Icon.texture;
        public string Title => Info.Title;

        public override void Use()
        {
            base.Use();

            var particle = MonoUtil.Load(Info.Prefabs[0]);
            var parent = GameObject.FindGameObjectWithTag("Player");
            particle.transform.parent = parent.transform;
            particle.transform.localPosition = Vector3.zero;
        }


        public void OnPinned()
        {
        }

        public void OnUnPinned()
        {
        }

        public bool OnUsed()
        {
            Use();
            return true;
        }
    }
}
