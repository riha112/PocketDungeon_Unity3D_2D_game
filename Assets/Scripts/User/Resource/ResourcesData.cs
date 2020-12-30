using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Repository;

namespace Assets.Scripts.User.Resource
{
    public class ResourcesData
    {
        private Dictionary<int, int> _resources;

        /// <summary>
        /// Helper operation to transform class to array like access
        /// </summary>
        /// <param name="id">Id of resource</param>
        /// <returns>Resource value, on fail - 0</returns>
        public int this[int id]
        {
            get
            {
                if (_resources is null)
                    _resources = GetDefaultResources();
                return _resources.ContainsKey(id) ? _resources[id] : 0;
            }
            set
            {
                if (_resources is null)
                    _resources = GetDefaultResources();
                if (!_resources.ContainsKey(id))
                    _resources.Add(id, value);
                else
                    _resources[id] = value;
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
