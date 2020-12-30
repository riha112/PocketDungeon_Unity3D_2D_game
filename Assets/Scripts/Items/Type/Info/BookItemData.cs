using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Misc;

namespace Assets.Scripts.Items.Type.Info
{
    public class BookItemData : ItemData
    {
        public override string GetDataLoadPath() => "Books";
        public int MagicId;

        public override List<object> LoadObjects(string path) => new List<object>(
            Util.LoadJsonFromFile<List<BookItemData>>($"{path}{GetDataLoadPath()}")
        );
    }
}
