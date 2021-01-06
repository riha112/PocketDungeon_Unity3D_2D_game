using System.Collections.Generic;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Repository;
using Assets.Scripts.User.Messages;

namespace Assets.Scripts.User.Magic
{
    /// <summary>
    /// Class that manages magic for user
    /// </summary>
    public class MagicController : Injectable
    {
        /// <summary>
        /// List of users known magics
        /// </summary>
        public List<AbstractMagic> MyMagic { get; set; } = new List<AbstractMagic>();

        /// <summary>
        /// Learns new magic
        /// </summary>
        /// <param name="magicId">Id of magic</param>
        public void LearnMagic(int magicId)
        {
            var msg = DI.Fetch<MessageController>();
            
            // Checks if magic already was known
            if (MyMagic.Exists(m => m.Id == magicId))
            {
                msg?.AddMessage("Spell already learned!");
                return;
            }

            // Checks if magic exits
            if (!MagicRepository.Repository.Has(magicId))
            {
                msg?.AddMessage("Unknown spell!");
                return;
            }

            // Learns magic
            var magic = new AbstractMagic
            {
                Data = MagicRepository.Repository[magicId]
            };

            MyMagic.Add(magic);
            msg?.AddMessage($"Learned new spell: {magic.Title}");
        }
    }
}