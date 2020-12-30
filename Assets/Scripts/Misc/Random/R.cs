﻿using System;
using Assets.Scripts.Misc.ObjectManager;
using JetBrains.Annotations;

namespace Assets.Scripts.Misc.Random
{
    /// <summary>
    /// Class that is used within whole project to
    /// add support for seed based generation, via creating
    /// one instance of System.Random object (instead of Unity.Random)
    /// and assigning it specific seed
    /// </summary>
    public static class R
    {
        private static System.Random _random;
        public static string Seed { get; private set; }

        /// <summary>
        /// Sets seed for Randomizer
        /// </summary>
        /// <param name="seed">Randomizers seed, if null then generated by taking current datetime</param>
        public static void SetSeed([CanBeNull] string seed = null)
        {
            _random = new System.Random(GetSeed(seed));
            DI.Register(_random);
        }

        /// <summary>
        /// Gets random number within the range
        /// </summary>
        /// <param name="min">Min number (included)</param>
        /// <param name="max">Max number (excluded)</param>
        /// <returns>Random number between min and max value</returns>
        public static int RandomRange(int min, int max)
        {
            if (_random == null)
               SetSeed();

            return _random.Next(min, max);
        }

        /// <returns>Randomizers seed</returns>
        public static int GetSeed([CanBeNull] string value)
        {
            Seed = value ?? DateTime.Now.ToString();
            UnityEngine.Debug.Log($"Seed: {Seed}");
            return Seed.GetHashCode();
        }
    }
}