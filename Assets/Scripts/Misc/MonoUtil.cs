using Assets.Scripts.Misc.ObjectManager;
using UnityEngine;

namespace Assets.Scripts.Misc
{
    /// <summary>
    /// Resolver class for adding support for non-unity initialized classes,
    /// so that they can use Unity methods
    /// </summary>
    public class MonoUtil : Injectable
    {
        /// <summary>
        /// Unity Destroy method
        /// </summary>
        /// <param name="obj">Object to destroy</param>
        /// <param name="timer">Time after which to destroy</param>
        public void Remove(GameObject obj, float timer = 0)
        {
            Destroy(obj, timer);
        }

        /// <summary>
        /// Unity Instantiate method
        /// </summary>
        /// <param name="obj">Object to Instantiate</param>
        /// <returns>Instantiated object</returns>
        public GameObject Load(GameObject obj)
        {
            return Instantiate(obj);
        }
    }
}