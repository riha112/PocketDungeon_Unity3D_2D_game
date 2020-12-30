using Assets.Scripts.Repository.Data;

namespace Assets.Scripts.Repository
{
    /// <summary>
    /// Holds all data for loadable prefabs
    /// <seealso cref="IndexedRepository{T}"/>
    /// </summary>
    public static class PrefabRepository
    {
        public static IndexedRepository<PrefabData> Repository = new IndexedRepository<PrefabData>("Register/Prefabs");
    }
}