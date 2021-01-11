namespace Assets.Scripts.SaveLoad
{
    /// <summary>
    /// Stores data that is pinned on in-game menu or and in
    /// characters inventory
    /// </summary>
    public class SavableEquipment
    {
        public int SlotId { get; set; }
        public int ItemLocalId { get; set; }
    }
}
