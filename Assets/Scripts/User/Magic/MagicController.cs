using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Repository;

namespace Assets.Scripts.User.Magic
{
    public class MagicController : Injectable
    {
        public List<AbstractMagic> MyMagic { get; set; } = new List<AbstractMagic>();

        public void LearnMagic(int magicId)
        {
            if (MyMagic.Exists(m => m.Id == magicId))
                return;

            if (!MagicRepository.Repository.Has(magicId))
                return;

            var magic = new AbstractMagic()
            {
                Data = MagicRepository.Repository[magicId]
            };

            MyMagic.Add(magic);
        }
    }
}
