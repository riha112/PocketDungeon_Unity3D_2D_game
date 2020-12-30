using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Misc;
using Assets.Scripts.Misc.Translator;
using Assets.Scripts.User.Attributes;
using Assets.Scripts.User.Stats;

namespace Assets.Scripts.Items.Type.Info
{
    public class PotionItemData : ItemData
    {
        public override string GetDataLoadPath() => "Potions";

        public AttributeData EffectAmount { get; set; }
        public bool IsSplash { get; set; } = false;
        public float BuffTime { get; set; } = 0;

        public override List<object> LoadObjects(string path) => new List<object>(
            Util.LoadJsonFromFile<List<PotionItemData>>($"{path}{GetDataLoadPath()}")
        );
    }
}
