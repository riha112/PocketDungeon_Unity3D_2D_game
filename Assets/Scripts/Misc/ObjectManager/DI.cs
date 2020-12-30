using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Assets.Scripts.Misc.ObjectManager
{
    /// <summary>
    /// Dependency Injection class
    /// - Responsible for fetching necessary classes within
    /// - active game environment
    ///
    /// Useful for accessing classes without searching them within
    /// game world. Or setting them in prefab data.
    /// </summary>
    public static class DI
    {
        /// <summary>
        /// List of all registered classes
        /// </summary>
        private static Dictionary<Type, object> Library { get; set; } = new Dictionary<Type, object>();

        /// <summary>
        /// Removes all entries
        /// </summary>
        public static void Flush() => Library = new Dictionary<Type, object>();

        /// <summary>
        /// Adds new class to register
        /// </summary>
        /// <param name="instance">Class to be added</param>
        public static void Register(object instance) => Library[instance.GetType()] = instance;

        /// <summary>
        /// Returns required instance by T
        /// </summary>
        /// <typeparam name="T">Class to be fetched</typeparam>
        /// <returns></returns>
        [CanBeNull]
        public static T Fetch<T>() where T : class => Library.ContainsKey(typeof(T)) ? Library[typeof(T)] as T : null;
    }
}
