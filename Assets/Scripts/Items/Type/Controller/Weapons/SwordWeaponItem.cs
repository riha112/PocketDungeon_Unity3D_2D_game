using System;
using Assets.Scripts.Misc;
using UnityEngine;

namespace Assets.Scripts.Items.Type.Controller.Weapons
{
    public class SwordWeaponItem : WeaponItem
    {
        public override ItemType[] Resolves() => new[] { ItemType.WeaponSword };

        protected virtual string PATH_TO_DEFAULT_ATTACK => "prefab_weapon_attack_sword_default";
        private bool _usedDefault;

        private GameObject _attack;
        protected GameObject AttackPrefab
        {
            get
            {
                if (_attack is null)
                {
                    _attack = Info.Prefabs.Length > 1 ? Info.Prefabs[2] : Resources.Load<GameObject>($"Prefabs/Items/Weapons/Attacks/{PATH_TO_DEFAULT_ATTACK}");
                    _usedDefault = Info.Prefabs.Length <= 1;
                }
                return _attack;
            }
        }

        public override void Attack()
        {
            if (AttackPrefab == null || !CanAttack)
                return;

            var attack = MonoUtil.Load(AttackPrefab);
            attack.transform.position = Util.GetCharacterTransform().position;
            attack.transform.right = Util.GetDirectionTowardsCursor(attack.transform);
            attack.transform.parent = Camera.main.transform;

            var character = Util.GetCharacter;
            character.GetComponent<Rigidbody2D>().AddForce(Util.GetDirectionTowardsCursor(character.transform) * 50);

            if (!_usedDefault) return;

            try
            {
                attack.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite =
                    Info.Prefabs[0].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
            }
            catch (Exception)
            {
                // Ignored
            }

            SetCollDown();

            base.Attack();
        }
    }
}
