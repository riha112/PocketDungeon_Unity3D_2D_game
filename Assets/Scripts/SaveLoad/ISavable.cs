namespace Assets.Scripts.SaveLoad
{
    /// <summary>
    /// Interface that is used for objects that can be
    /// saved and loaded.
    /// </summary>
    public interface ISavable
    {
        /// <summary>
        /// Called when saving object
        /// </summary>
        void Save();

        /// <summary>
        /// Called when loading object
        /// </summary>
        void Load();
    }
}
