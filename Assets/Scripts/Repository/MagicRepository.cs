using Assets.Scripts.User.Magic;

namespace Assets.Scripts.Repository
{
    /// <summary>
    /// Holds all data for magic data
    /// <seealso cref="IndexedRepository{T}"/>
    /// </summary>
    public static class MagicRepository
    {
        public static IndexedRepository<MagicData> Repository = new IndexedRepository<MagicData>("Register/Magic");
    }
}
