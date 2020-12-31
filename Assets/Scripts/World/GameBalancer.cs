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
            var minLevel = baseLevel <= 2 ? 1 : baseLevel - 2;
            var maxLevel = baseLevel <= 2 ? 3 : baseLevel + 3;

            return (short)R.RandomRange(minLevel, maxLevel);
        }
    }
}
