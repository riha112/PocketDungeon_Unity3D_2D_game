using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entity;
using Assets.Scripts.Items.Type.Controller;
using Assets.Scripts.MainMenu;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.User.Attributes;
using Assets.Scripts.User.Controller;
using Assets.Scripts.User.Equipment;
using Assets.Scripts.User.Messages;
using Assets.Scripts.User.Resource;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.User
{
    public class CharacterEntity : Entity.Entity
    {
        public ResourcesData Resources { get; set; }

        #region Damage Effects
        public GameObject[] Effects;

        /// <summary>
        /// Contains list of all body part renderers, that
        /// colors are changed when taking a damage
        /// </summary>
        public SpriteRenderer[] BodyParts;

        /// <summary>
        /// Color to which renderer color is changed when
        /// taking a damage
        /// </summary>
        public Color DamageColor;
        #endregion

        /// <inheritdoc cref="Entity"/>
        protected override void LevelUp()
        {
            base.LevelUp();

            // Loads leveling up effect
            var effectLevelUp = Instantiate(Effects[0]);
            effectLevelUp.transform.position = transform.position;
            effectLevelUp.transform.position += effectLevelUp.transform.forward * -0.5f;
            effectLevelUp.transform.parent = Camera.main.transform;
        }

        /// <summary>
        /// Updates characters stats when attributes are changed
        /// Includes equipment calculations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        public override void OnAttributesChange([CanBeNull] object sender, (int, short) data)
        {
            base.OnAttributesChange(sender, data);

            // Calculates attribute sum of equipped items
            var equipmentAttributes = new AttributeData();
            var equippedItems = DI.Fetch<EquipmentController>()?.EquippedItems() ?? new List<EquipableItem>();
            foreach (var item in equippedItems)
            {
                equipmentAttributes += item.Attribute;
            }

            // Applies equipment attributes to character
            Stats.MaxHealth += 100 + equipmentAttributes.Vitality;
            Stats.MaxMagic += 20 + equipmentAttributes.Magic;
            Stats.CurrentDefense += equipmentAttributes.Resistance;
            Stats.CurrentSpeed += equipmentAttributes.Agility / 50.0f;
            Stats.CurrentLuck += equipmentAttributes.Luck / 10.0f;
            Stats.CurrentStrength += equipmentAttributes.Strength;
            Stats.HitPoints += Attributes.Strength / 4;
        }

        /// <inheritdoc cref="Entity"/>
        public override int TakeDamage(int hitPoints)
        {
            var realDamage = base.TakeDamage(hitPoints);

            // Adds damage effect
            ChangeBodyColor(DamageColor);
            Invoke(nameof(ResetColor), 0.15f);

            // Pushes character away
            DI.Fetch<MovementController>()?.BounceOff();

            // Outputs msg about taken damage
            DI.Fetch<MessageController>()?.AddMessage($"Took damage: {realDamage}");

            return realDamage;
        }

        /// <summary>
        /// Fix for unity Invoke... Parameter based methods not supported
        /// </summary>
        private void ResetColor()
        {
            ChangeBodyColor();
        }

        /// <summary>
        /// Changes color of characters body
        /// </summary>
        /// <param name="color">Color of body (by default resets to base)</param>
        private void ChangeBodyColor(Color? color = null)
        {
            var bodyColor = color ?? Color.white;

            foreach (var bodyPart in BodyParts)
            {
                bodyPart.color = bodyColor;
            }
        }

        /// <inheritdoc cref="Entity"/>
        protected override void Death()
        {
            Time.timeScale = 0;
            DI.Fetch<FightingController>().enabled = false;
            DI.Fetch<MovementController>().enabled = false;
            DI.Fetch<UIController>().enabled = false;
            DI.Fetch<InGameMainMenu>().Toggle(true);
        }

        private void ReloadDungeon()
        {
            SceneManager.LoadScene("Dungeon");
        }
    }
}
