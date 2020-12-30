using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Items;
using Assets.Scripts.Items.Type;
using Assets.Scripts.Items.Type.Controller;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.UI;
using Assets.Scripts.User.Equipment;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.User.Controller
{
    public class FightingController : MonoBehaviour
    {
        private readonly WeaponItem[] _weapons = new WeaponItem[2];
        private bool _canAttack = true;
        private Animator _animator;

        public void Start()
        {
            DI.Fetch<EquipmentController>().EquipmentChanged += OnWeaponChange;
            UI.UI.ToggleEvent += OnUiToggle;

            _animator = GetComponent<Animator>();
        }

        private void OnUiToggle(object sender, bool state)
        {
            if (sender is Popup)
                _canAttack = !state;
        }

        private void OnWeaponChange([CanBeNull] object sender, (int slotId, EquipableItem item) data)
        {
            var (slotId, item) = data;


            if (item != null && !(item is WeaponItem))
                return;

            switch (slotId)
            {
                case 2:
                    _weapons[0] = (WeaponItem)item;
                    break;
                case 4:
                    _weapons[1] = (WeaponItem)item;
                    break;
            }
        }

        private void Update()
        {
            if (!_canAttack)
                return;

            for (var i = 0; i < 2; i++)
            {
                if (!Input.GetMouseButtonUp(i) || _weapons[i] is null) continue;
                _weapons[i].Attack();
            }

            // No weapon combat
            if (_weapons[0] is null && _weapons[1] is null && Input.GetMouseButtonDown(0))
            {
                _animator.SetTrigger("PrimaryAttack");
            }
        }
    }
}
