using Assets.Scripts.Items.Type.Controller;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class InstantiableWeaponController : MonoBehaviour
    {
        public EquipableItem Item;

        private float _durabilityTimer = 2;

        private void Update()
        {
            _durabilityTimer -= Time.deltaTime;
            if (_durabilityTimer > 0) return;

            _durabilityTimer = 2;
            Item.Durability -= Item.Info.DurabilityReducer;
        }
    }
}