using Assets.Scripts.Items;
using Assets.Scripts.Items.Type.Controller;
using UnityEngine;

namespace Assets.Scripts.User.Equipment
{
    public class EquipmentSlot
    {
        public int Id { get; set; }
        public EquipableItem CurrentItem { get; set; }
        public ItemType AllowedItem { get; set; }
        public Rect Position { get; set; }
        public Texture2D EmptyTexture { get; set; }
        public GameObject[] EquipmentPoints { get; set; }
    }
}