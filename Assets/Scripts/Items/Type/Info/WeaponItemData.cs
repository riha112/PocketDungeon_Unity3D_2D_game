using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Misc;

namespace Assets.Scripts.Items.Type.Info
{
    public class WeaponItemData : ItemData
    {
        public override string GetDataLoadPath() => "Weapons";

        public float AttackCoolDown { get; set; } = 0.5f;

        public override List<object> LoadObjects(string path) => new List<object>(
            Util.LoadJsonFromFile<List<WeaponItemData>>($"{path}{GetDataLoadPath()}")
        );
    }
}
