using System.Collections.Generic;
using Assets.Scripts.Repository;

namespace Assets.Scripts.User.Resource
{
    public class ResourcesData
    {
        public Dictionary<int, int> Resources { get; set; }

        /// <summary>
        /// Helper operation to transform class to array like access
        /// </summary>
        /// <param name="id">Id of resource</param>
        /// <returns>Resource value, on fail - 0</returns>
        public int this[int id]
        {
            get
            {
                if (Resources is null)
                    Resources = GetDefaultResources();
                return Resources.ContainsKey(id) ? Resources[id] : 0;
            }
            set
            {
                if (Resources is null)
                    Resources = GetDefaultResources();
                if (!Resources.ContainsKey(id))
                    Resources.Add(id, value);
                else
                    Resources[id] = value;
            }
        }

        /// <summary>
        /// Generates default resources
        /// </summary>
        /// <returns>Dictionary with default resources</returns>
        private static Dictionary<int, int> GetDefaultResources()
        {
            var resources = new Dictionary<int, int>();
            foreach (var resourceData in ResourceRepository.Repository.Raw)
            {
                resources.Add(resourceData.Key, 5);
            }
            return resources;
        }
    }
}
