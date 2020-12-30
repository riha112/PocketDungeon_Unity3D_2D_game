using Assets.Scripts.Repository.Data;

namespace Assets.Scripts.Repository
{
    /// <summary>
    /// Holds all data for resources AKA materials
    /// <seealso cref="IndexedRepository{T}"/>
    /// </summary>
    public static class ResourceRepository
    {
        public static IndexedRepository<ResourceData> Repository =
            new IndexedRepository<ResourceData>("Register/Resources");
    }
}