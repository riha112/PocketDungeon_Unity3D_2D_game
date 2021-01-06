using System.Collections.Generic;
using Assets.Scripts.Misc;
using Assets.Scripts.Misc.GUI;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    /// <summary>
    /// Controller for In-Game main menu... menu that
    /// is available while player is actually in game instead
    /// of main menu which is in the start of the game
    /// </summary>
    public class InGameMainMenu : Popup
    {
        public override int Depth => -100;

        protected override void Awake()
        {
            RectConfig.Body = new Rect(0, 0, 280, 220);
            RectConfig.Popup = new Rect(0, 0, 280, 220);
            base.Awake();
        }

        /// <summary>
        /// Changes timeScale to 0 when screen is visible to pause the game.
        /// </summary>
        /// <param name="state">New state of UI</param>
        public override void Toggle(bool state)
        {
            Time.timeScale = state ? 0 : 1;
            base.Toggle(state);
        }

        protected override void DrawBody()
        {
            // Resume game action
            if (GUI.Button(new Rect(40, 40, 200, 40), LT("Resume"))) Toggle(false);

            // Restart dungeon level action
            if (GUI.Button(new Rect(40, 90, 200, 40), LT("Restart level"))) Util.ChangeScene("Dungeon");

            // Return to main menu screen action
            if (GUI.Button(new Rect(40, 140, 200, 40), LT("To main menu"))) Util.ChangeScene("Start");
        }

        protected override List<string> GetLmcLibrary()
        {
            return new List<string>
            {
                "Resume",
                "Restart level",
                "To main menu"
            };
        }
    }
}