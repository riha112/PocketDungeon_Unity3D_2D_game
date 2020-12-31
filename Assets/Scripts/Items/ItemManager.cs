using Assets.Scripts.Items.Type.Controller;
using Assets.Scripts.Misc.Random;

namespace Assets.Scripts.Items
{
    public static class ItemManager
    {
        /// <summary>
        /// Assigns random durability based on characters luck
        /// </summary>
        /// <param name="item">Item to whom assign the durability</param>
        /// <param name="luck">Luck amount</param>
        public static void AssignRandomDurability(EquipableItem item, int luck = 1)
        {
            item.Durability = R.RandomRange(luck, 101) / 100.0f;
        }

        /// <summary>
        /// Assigns items grade based on characters luck
        /// </summary>
        /// <param name="item">Item to whom assign the class</param>
        /// <param name="luck">Luck amount</param>
        public static void AssignRandomGrade(SimpleItem item, int luck = 1)
        {
            var percentage = R.RandomRange(luck, 100);

            if (percentage < 40)
                item.Grade = ItemGrade.D;
            else if (percentage < 65)
                item.Grade = ItemGrade.C;
            else if (percentage < 80)
                item.Grade = ItemGrade.B;
            else if (percentage < 94)
                item.Grade = ItemGrade.A;
            else if (percentage < 99)
                item.Grade = ItemGrade.S;
            else
                item.Grade = ItemGrade.SS;
        }
    }
}
