using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Misc.GUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.MainMenu
{
    public class InGameMainMenu : Popup
    {
        public override int Depth => -100;

        protected override void Awake()
        {
            RectConfig.Body = new Rect(0, 0, 280, 220);
            RectConfig.Popup = new Rect(0, 0, 280, 220);
            base.Awake();
        }

        public override void Toggle(bool state)
        {
            Time.timeScale = state ? 0 : 1;
            base.Toggle(state);
        }

        protected override void DrawBody()
        {
            if(GUI.Button(new Rect(40, 40, 200, 40), LT("Resume")))
            {
                Toggle(false);
            }

            if (GUI.Button(new Rect(40, 90, 200, 40), LT("Restart level")))
            {
                SceneManager.LoadScene("Dungeon");
            }

            if (GUI.Button(new Rect(40, 140, 200, 40), LT("To main menu")))
            {
                SceneManager.LoadScene("Start");
            }
        }
    }
}
