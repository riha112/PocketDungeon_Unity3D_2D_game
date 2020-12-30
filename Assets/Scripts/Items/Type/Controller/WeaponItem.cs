using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.Items.Type.Info;
using UnityEngine;

namespace Assets.Scripts.Items.Type.Controller
{
    public class WeaponItem : EquipableItem
    {
        public override ItemType[] Resolves() => new[] { ItemType.Weapon };
        public WeaponItemData WeaponInfo
        {
            get
            {
                if (Info is WeaponItemData item)
                    return item;
                return null;
            }
        }

        protected bool CanAttack = true;

        protected virtual void SetCollDown()
        {
            CanAttack = false;

            // Little Unity hack to replace MonoBehaviour.Invoke for non attached classes
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep((int)(WeaponInfo.AttackCoolDown * 1000));
                CanAttack = true;
            });
        }

        public virtual void Attack()
        {
            Durability -= Info.DurabilityReducer;
        }
    }
}
