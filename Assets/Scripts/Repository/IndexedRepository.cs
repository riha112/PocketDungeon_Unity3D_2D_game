using System.Collections.Generic;
using Assets.Scripts.Misc;

namespace Assets.Scripts.Repository
{
    /// <summary>
    /// Repository class, that holds list of objects, loaded from
    /// json file, whom are accessible via Id property.
    /// </summary>
    /// <typeparam name="T">Data type with Id parameter</typeparam>
    public class IndexedRepository<T> where T : IIndexable
    {
        /// <summary>
        /// Json File location
        /// </summary>
        private readonly string _path;

        /// <summary>
        /// Shorthand method for getting data object by its Id
        /// </summary>
        /// <param name="id">Indexable objects Id</param>
        /// <returns>Object whose Id is same as passed</returns>
        public T this[int id] => Raw[id];

        /// <summary>
        /// Reloads data from json file
        /// </summary>
        private void Rebuild()
        {
            _repository = new Dictionary<int, T>();
            var items = Util.LoadJsonFromFile<List<T>>($"{_path}");
            foreach (var item in items) _repository.Add(item.Id, item);
        }

        /// <summary>
        /// Shorthand method for ContainsKey call
        /// </summary>
        /// <param name="id">Id of data object</param>
        /// <returns>Data object</returns>
        public bool Has(int id)
        {
            return Raw?.ContainsKey(id) ?? false;
        }

        public IndexedRepository(string path)
        {
            _path = path;
        }

        private Dictionary<int, T> _repository;

        public Dictionary<int, T> Raw
        {
            get
            {
                if(_repository is null)
                    Rebuild();
                return _repository;
            }
        }

        /// <summary>
        /// Shorthand for dictionary method Count
        /// </summary>
        public int Count => Raw?.Count ?? 0;
    }
}