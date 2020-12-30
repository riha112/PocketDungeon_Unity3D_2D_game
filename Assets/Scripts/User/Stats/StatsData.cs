namespace Assets.Scripts.User.Stats
{
    /// <summary>
    /// Contains actual information of entity data
    /// Should not be set manually, instead should be
    /// calculated by attributes using Entity class as
    /// proxy
    /// </summary>
    public class StatsData
    {
        public float MaxHealth { get; set; } = 100;
        public float MaxMagic { get; set; } = 20;
        public int HitPoints { get; set; } = 1;

        public float CurrentLuck { get; set; } = 1;
        public float CurrentDefense { get; set; } = 1;
        public float CurrentSpeed { get; set; } = 1;
        public float CurrentStrength { get; set; } = 1;

        public short CurrentLevel { get; set; } = 1;
        public int CurrentExp { get; set; } = 0;
        public int RequiredExp => CurrentLevel * 10;

        public void LevelUp()
        {
            CurrentLevel++;
            CurrentExp = 0;
        }
    }
}
