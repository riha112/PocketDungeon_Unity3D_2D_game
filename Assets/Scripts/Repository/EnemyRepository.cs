using Assets.Scripts.Repository.Data;

namespace Assets.Scripts.Repository
{
    /// <summary>
    /// Holds all data enemies
    /// <seealso cref="IndexedRepository{T}"/>
    /// </summary>
    public static class EnemyRepository
    {
        public static IndexedRepository<EnemyData> Repository = new IndexedRepository<EnemyData>("Register/Enemies");
    }
}