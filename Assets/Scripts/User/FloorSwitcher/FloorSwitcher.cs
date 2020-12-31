using System.Collections.Generic;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.World;
using Assets.Scripts.World.Items;
using UnityEngine;

namespace Assets.Scripts.User.FloorSwitcher
{
    /// <summary>
    /// UI interface that outputs button for
    /// switching onto next floor
    ///
    /// - If there are no monster spawner on the floor
    /// - user can switch to next floor
    /// </summary>
    public class FloorSwitcher : Injectable
    {
        private List<MonsterSpawner> _spawners;

        /// <summary>
        /// Shorthand method for adding monster spawner to list
        /// </summary>
        /// <param name="spawner"></param>
        public void Register(MonsterSpawner spawner)
        {
            if(_spawners == null)
                _spawners = new List<MonsterSpawner>();
            _spawners.Add(spawner);
        }

        /// <summary>
        /// Shorthand method for removing monster spawner from list
        /// </summary>
        /// <param name="spawner"></param>
        public void Remove(MonsterSpawner spawner)
        {
            _spawners?.Remove(spawner);
        }

        /// <summary>
        /// UI Design
        /// </summary>
        private void OnGUI()
        {
            if (_spawners == null || _spawners.Count > 0)
                return;

            GUI.depth = -5;

            if (GUI.Button(new Rect(Screen.width - 150, Screen.height - 60, 130, 40), "Next floor"))
            {
                DI.Fetch<WorldController>()?.EndGame();
            }
        }
    }
}
