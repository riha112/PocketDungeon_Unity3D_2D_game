namespace Assets.Scripts.Repository
{
    /// <summary>
    /// Interface to implement indexing for objects,
    /// used for IndexedRepository
    /// <seealso cref="IndexedRepository{T}"/>
    /// </summary>
    public interface IIndexable
    {
        int Id { get; set; }
    }
}