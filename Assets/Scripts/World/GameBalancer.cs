using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Random;
using Assets.Scripts.SaveLoad;

namespace Assets.Scripts.World
{
    /// <summary>
    /// Responsible for generating balanced loadable values
    /// * Enemy level
    /// </summary>
    public static class GameBalancer
    {
        private static SavableCharacter _characterStartData;
        private static SavableCharacter CharacterStartData => _characterStartData ?? (_characterStartData = DI.Fetch<SavableCharacter>());

        /// <summary>
        /// Generates balanced LVL for enemy by selecting users LVL at start
        /// of the dungeons floor and adding range between -2 to + 2 levels
        /// </summary>
        /// <returns>Balanced enemies LVL</returns>
        public static short GetBalancedMonsterLevel()
        {
            var baseLevel = CharacterStartData?.Stats?.CurrentLevel ?? 1;
            var minLevel = baseLevel <= 5 ? 1 : baseLevel - 5;
            var maxLevel = baseLevel <= 2 ? 3 : baseLevel + 3;

            return (short)R.RandomRange(minLevel, maxLevel);
        }

        /// <summary>
        /// Returns balanced room type percentage based on room size and dungeon
        /// size
        /// </summary>
        /// <param name="size">(int treasure percentage, int design percentage)</param>
        /// <returns></returns>
        public static (int, int) GetBalancedRoomType(int size)
        {
            var baseLevel = CharacterStartData?.Stats?.CurrentLevel ?? 1;
            
            // Small Room percentage
            if (size < 50)
            {
                return (baseLevel > 5) ? (13, 60) : (30, 70);
            }

            // Medium Room percentage
            if (size < 90)
            {
                return (baseLevel > 5) ? (10, 35) : (30, 50);
            }

            // Large room percentage
            return (baseLevel > 5) ? (5, 25) : (15, 30);
        }
    }
}
