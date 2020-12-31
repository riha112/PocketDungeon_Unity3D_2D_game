using Assets.Scripts.Items.Type.Controller;
using Assets.Scripts.Misc.GUI;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.User.Equipment;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.User.Controller
{
    /// <summary>
    /// Controls weapon usage for player
    /// </summary>
    public class FightingController : MonoBehaviour
    {
        /// <summary>
        /// Primary & Secondary weapon instances
        /// </summary>
        private readonly WeaponItem[] _weapons = new WeaponItem[2];

        /// <summary>
        /// When Popups are open makes, so that user can not attack
        /// </summary>
        private bool _canAttack = true;

        /// <summary>
        /// Called on object initialization
        /// </summary>
        public void Start()
        {
            DI.Fetch<EquipmentController>().EquipmentChanged += OnWeaponChange;
            UI.ToggleEvent += OnUiToggle;
        }

        /// <summary>
        /// Sets attack state opposite of popup state...
        /// <example>If inventory is active then attack is inactive</example>
        /// </summary>
        /// <param name="sender">UI who changes state</param>
        /// <param name="state">New state</param>
        private void OnUiToggle(object sender, bool state)
        {
            if (sender is Popup)
                _canAttack = !state;
        }

        /// <summary>
        /// Updates local weapon instances when player changes equipment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data">slotId and item that is equipped/un-equipped</param>
        private void OnWeaponChange([CanBeNull] object sender, (int slotId, EquipableItem item) data)
        {
            var (slotId, item) = data;

            // If item is not Weapon or null then we don't care,
            // null is kept to un-equip weapon
            if (item != null && !(item is WeaponItem))
                return;

            switch (slotId)
            {
                // Primary weapon slot
                case 2:
                    _weapons[0] = (WeaponItem) item;
                    break;
                // Secondary weapon slot
                case 4:
                    _weapons[1] = (WeaponItem) item;
                    break;
            }
        }

        private void Update()
        {
            if (!_canAttack)
                return;

            // If click left click attack with primary weapon
            // If click right click attack with secondary attack
            for (var i = 0; i < 2; i++)
            {
                if (!Input.GetMouseButtonUp(i) || _weapons[i] is null) continue;
                _weapons[i].Attack();
            }
        }
    }
}