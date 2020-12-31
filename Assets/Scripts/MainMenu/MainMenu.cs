using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Misc.Translator;
using Assets.Scripts.SaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        public GUISkin Theme;

        private LocalMessageCache _lmc;
        private int _activeWindow = -1;

        private string[] _savedGameFiles;
        private static string _savedGameDir;

        private void Awake()
        {
            _savedGameDir = $"{Application.persistentDataPath}/Games/";
            BuildLmc();
        }

        private void BuildLmc()
        {
            _lmc = new LocalMessageCache()
            {
                Messages = new List<string>
                {
                    "New Game",         //0
                    "Load Game",        //1
                    "Options",          //2
                    "Exit",             //3
                    "Back",             //4
                    "Title",            //5
                    "Seed",             //6
                    "Leave empty to generate random",   //7
                    "Create"            //8
                }
            };
            _lmc.BuildCachedMessages();
        }

        private void OnGUI()
        {
            GUI.skin = Theme;

            switch (_activeWindow)
            {
                case 0: 
                    NewGameWindow();
                    break;
                case 1:
                    LoadGameWindow();
                    break;
                case 2:
                    OptionsWindow();
                    break;
                default:
                    StartWindow();
                    break;
            }

            if (_isStartGame)
            {
                StartGame();
            }
        }

        private void StartWindow()
        {
            GUI.BeginGroup(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200));

            for (var i = 0; i < 4; i++)
            {
                if (GUI.Button(new Rect(0, 50 * i, 200, 40), _lmc[i]))
                {
                    _activeWindow = i;
                    switch (i)
                    {
                        case 3:
                            Application.Quit();
                            break;
                        case 1:
                            var files =  Directory.GetFiles(_savedGameDir, "*.json");
                            _savedGameFiles = files.Select(Path.GetFileNameWithoutExtension).ToArray();
                            break;
                    }
                }
            }

            GUI.EndGroup();
        }

        private string _title;
        private string _seed;
        private void NewGameWindow()
        {
            GUI.Box(new Rect(Screen.width / 2 - 215, Screen.height / 2- 175, 430, 350), "", "Holder");

            GUI.Label(new Rect(Screen.width / 2 -175, Screen.height / 2 - 250, 350, 80), _lmc[0], "Title");

            GUI.BeginGroup(new Rect(Screen.width / 2 - 175, Screen.height / 2 - 130, 350, 260));

            GUI.Label(new Rect(0, 0, 100, 25), _lmc[5]);
            _title = GUI.TextField(new Rect(0, 25, 350, 50), _title);

            GUI.Label(new Rect(0, 85, 100, 25), _lmc[6]);
            _seed = GUI.TextField(new Rect(0, 110, 350, 50), _seed);
            GUI.Label(new Rect(0, 170, 350, 25), _lmc[7], "Label2");

            if (GUI.Button(new Rect(50, 220, 110, 40), _lmc[8]))
                CreateNewGame();

            if (GUI.Button(new Rect(190, 220, 110, 40), _lmc[4]))
                _activeWindow = -1;

            GUI.EndGroup();
        }

        private void LoadGameWindow()
        {
            GUI.Box(new Rect(Screen.width / 2 - 215, Screen.height / 2 - 175, 430, 350), "", "Holder");

            GUI.Label(new Rect(Screen.width / 2 - 175, Screen.height / 2 - 250, 350, 80), _lmc[1], "Title");

            GUI.BeginGroup(new Rect(Screen.width / 2 - 175, Screen.height / 2 - 125, 350, 260));
            for(var i = 0; i < _savedGameFiles.Length; i++)
            {
                if (GUI.Button(new Rect(0, 50 * i, 350, 45), _savedGameFiles[i], "LoadLevel"))
                {
                    LoadGame(_savedGameFiles[i]);
                }
            }
            if (GUI.Button(new Rect(240, 220, 110, 40), _lmc[4]))
                _activeWindow = -1;
            GUI.EndGroup();
        }

        private void OptionsWindow()
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), _lmc[4]))
                _activeWindow = -1;
        }

        private void LoadGame(string title)
        {
            PlayerPrefs.SetString("CurrentGame", title);
            _isStartGame = true;
            StartGame();
        }

        private void CreateNewGame()
        {
            if(string.IsNullOrEmpty(_title) || string.IsNullOrWhiteSpace(_title))
                return;

            var sg = new SavableGame();
            sg.CreateNewGame(_title, string.IsNullOrEmpty(_seed) ? null : _seed);
            _isStartGame = true;
            StartGame();
        }

        private float _timer = 0;
        private bool _isStartGame = false;
        private bool _started = false;
        public Texture2D FadeTexture;
        private void StartGame()
        {
            _timer += Time.deltaTime / 2;
            GUI.color = new Color(0, 0, 0, _timer);

            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeTexture);

            if (_timer >= 1 && !_started)
            {
                _timer = 1;
                _started = true;
                SceneManager.LoadScene("Dungeon");
            }
        }
    }
}
