using Assets.Scripts.Items.Type.Info;
using Assets.Scripts.Misc;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.User;
using Assets.Scripts.User.Inventory;
using Assets.Scripts.User.Magic;
using UnityEngine;

namespace Assets.Scripts.Items.Type.Controller
{
    /// <summary>
    /// Holds real items data for items with which user can use
    /// to update its attribute/stats values
    /// </summary>
    public class PotionItem : SimpleItem, IPinnable
    {
        /// <inheritdoc cref="SimpleItem"/>
        public override ItemType[] Resolves()
        {
            return new[] {ItemType.Potion};
        }

        /// <summary>
        /// Converts current ItemData object into PotionItemData object
        /// </summary>
        public PotionItemData PotionInfo
        {
            get
            {
                if (Info is PotionItemData item)
                    return item;
                return null;
            }
        }

        /// <inheritdoc cref="SimpleItem"/>
        public override void Use()
        {
            var userData = DI.Fetch<CharacterEntity>();
            if (userData != null && PotionInfo != null)
            {
                userData.Health = PotionInfo.EffectAmount.Vitality;
                userData.Magic = PotionInfo.EffectAmount.Magic;

                LoadEffect();
            }

            InventoryManager.DropItem(this);
        }

        /// <summary>
        /// Adds visual effect onto player when used
        /// TODO: Better effect fetcher
        /// </summary>
        protected virtual void LoadEffect()
        {
            var effectName = "healing";
            if (PotionInfo.EffectAmount.Magic > 0) effectName = "healing_mp";

            var effectPrefab = Resources.Load<GameObject>($"Prefabs/Player/Effects/prefab_player_effect_{effectName}");
            var effect = MonoUtil.Load(effectPrefab);
            effect.transform.position = Util.GetCharacterTransform().position;
            effect.transform.parent = Camera.main.transform;
            MonoUtil.Remove(effect, 3);
        }

        #region IPinnable
        public int Id => LocalId;
        public bool CanUse => true;
        public float CoolDownTimer => 1;
        public Texture2D Icon => Info.Icon.texture;
        public string Title => Info.Title;
        public bool IsSingleUse => true;

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
        #endregion

    }
}
