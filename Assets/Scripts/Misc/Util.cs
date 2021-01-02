using Assets.Scripts.Misc.ObjectManager;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Misc
{
    /// <summary>
    /// Holds many different functions that are commonly used
    /// between other classes and are not linked to one specific type
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Switches between Unity 3D scenes
        /// </summary>
        /// <param name="name">Scene name</param>
        public static void ChangeScene(string name)
        {
            DI.Flush();
            Time.timeScale = 1;
            SceneManager.LoadScene(name);
        }

        /// <summary>
        /// Loads objects from JSON file within Resource directory
        /// </summary>
        /// <typeparam name="T">Type to which convert the json file</typeparam>
        /// <param name="path">Location within Resource directory</param>
        /// <returns>Converted object</returns>
        public static T LoadJsonFromFile<T>(string path)
        {
            var resource = (TextAsset) Resources.Load($"Data/{path}");
            if (resource == null)
                return default;

            var json = resource.text;
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Returns vectorized direction towards mouse cursor
        /// </summary>
        /// <param name="baseTransform">Point from</param>
        /// <returns>Vectorized direction towards cursor</returns>
        public static Vector2 GetDirectionTowardsCursor(Transform baseTransform)
        {
            var mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return new Vector2(mouseWorld.x, mouseWorld.y) -
                   new Vector2(baseTransform.position.x, baseTransform.position.y);
        }

        /// <summary>
        /// Returns vectorized direction away from cursor
        /// </summary>
        /// <param name="baseTransform">Point from</param>
        /// <returns>Vectorized direction away from cursor</returns>
        public static Vector2 GetDirectionBackwardsCursor(Transform baseTransform)
        {
            var mouseWorld = Camera.main.ScreenToWorldPoint(-Input.mousePosition);
            return new Vector2(mouseWorld.x, mouseWorld.y) -
                   new Vector2(baseTransform.position.x, baseTransform.position.y);
        }

        private static GameObject _character;

        public static GameObject GetCharacter
        {
            get
            {
                if (_character == null) _character = GameObject.FindGameObjectWithTag("Player");

                return _character;
            }
        }

        private static GameObject _characterCenter;

        public static GameObject GetCharacterCenter
        {
            get
            {
                if (_characterCenter == null) _characterCenter = GameObject.FindGameObjectWithTag("CharacterCenter");

                return _characterCenter;
            }
        }

        public static Transform GetCharacterTransform()
        {
            return GetCharacter.transform;
        }

        /// <summary>
        /// 2D implementation of LookAtt by freezing Z axis
        /// </summary>
        /// <param name="from">Who looks</param>
        /// <param name="to">At whom it looks</param>
        /// <returns>Vectorized direction towards target</returns>
        public static Vector2 LookAt2D(Transform from, Transform to)
        {
            return new Vector2(from.position.x, from.position.y) -
                   new Vector2(to.position.x, to.position.y);
        }
    }
}