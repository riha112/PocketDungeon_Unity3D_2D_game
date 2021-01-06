using Assets.Scripts.Items.Type.Controller;
using UnityEngine;

namespace Assets.Scripts.Items
{
    /// <summary>
    /// Reduces Weapons Durability for weapon without active "Use" function
    /// <seealso cref="Type.Controller.Weapons.InstantiableWeapon"/>
    /// </summary>
    public class InstantiableWeaponController : MonoBehaviour
    {
        public EquipableItem Item;

        private float _durabilityTimer = 2;

        private void Update()
        {
            // Removes durability every X seconds
            _durabilityTimer -= Time.deltaTime;
            if (_durabilityTimer > 0) return;

            _durabilityTimer = 2;
            Item.Durability -= Item.EquipableData.DurabilityReducer;
        }
    }
}