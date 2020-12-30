using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Items;

namespace Assets.Scripts.SaveLoad
{
    public class SavableItem
    {
        public int ItemId { get; set; }
        public int LocalId { get; set; }
        public ItemGrade Grade { get; set; }
        public float Durability { get; set; }
    }
}
