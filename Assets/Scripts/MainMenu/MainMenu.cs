﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.Misc;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Translator;
using Assets.Scripts.SaveLoad;
using Assets.Scripts.User.Messages;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    /// <summary>
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        public GUISkin Theme;
        public OptionsUi Options;

        private LocalMessageCache _lmc;
        private int _activeWindow = -1;

        private string[] _savedGameFiles;
        private static string _savedGameDir;

        private Vector2 _scrollView;

        private void Awake()
        {
            _savedGameDir = $"{Application.persistentDataPath}/Games/";
            BuildLmc();
        }

        private void BuildLmc()
        {
            _lmc = new LocalMessageCache
            {
                Messages = new List<string>
                {
                    "New Game", //0
                    "Load Game", //1
                    "Options", //2
                    "Exit", //3
                    "Back", //4
                    "Title", //5
                    "Seed", //6
                    "Leave empty to generate random", //7
                    "Create" //8
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

            if (_isStartGame) StartGame();
        }

        /// <summary>
        /// </summary>
        private void StartWindow()
        {
            GUI.BeginGroup(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200));

            for (var i = 0; i < 4; i++)
                if (GUI.Button(new Rect(0, 50 * i, 200, 40), _lmc[i]))
                {
                    _activeWindow = i;
                    switch (i)
                    {
                        case 3:
                            Application.Quit();
                            break;
                        case 1:
                            if (Directory.Exists(_savedGameDir))
                            {
                                var files = Directory.GetFiles(_savedGameDir, "*.json");
                                _savedGameFiles = files.Select(Path.GetFileNameWithoutExtension).ToArray();
                            }

                            break;
                    }
                }

            GUI.EndGroup();
        }

        private string _title;
        private string _seed;

        /// <summary>
        /// </summary>
        private void NewGameWindow()
        {
            GUI.Box(new Rect(Screen.width / 2 - 215, Screen.height / 2 - 175, 430, 350), "", "Holder");

            GUI.Label(new Rect(Screen.width / 2 - 175, Screen.height / 2 - 240, 350, 80), _lmc[0], "Title");

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

        /// <summary>
        /// </summary>
        private void LoadGameWindow()
        {
            GUI.Box(new Rect(Screen.width / 2 - 215, Screen.height / 2 - 175, 430, 350), "", "Holder");

            GUI.Label(new Rect(Screen.width / 2 - 175, Screen.height / 2 - 240, 350, 80), _lmc[1], "Title");
            GUI.BeginGroup(new Rect(Screen.width / 2 - 175, Screen.height / 2 - 125, 350, 260));

            if (_savedGameFiles != null)
            {
                _scrollView = GUI.BeginScrollView(new Rect(0, 0, 350, 210), _scrollView,
                    new Rect(0, 0, 320, 50 * _savedGameFiles.Length));
                for (var i = 0; i < _savedGameFiles.Length; i++)
                    if (GUI.Button(new Rect(0, 50 * i, 350, 45), _savedGameFiles[i], "LoadLevel"))
                        LoadGame(_savedGameFiles[i]);
                GUI.EndScrollView();
            }

            if (GUI.Button(new Rect(240, 220, 110, 40), _lmc[4]))
                _activeWindow = -1;
            GUI.EndGroup();
        }

        /// <summary>
        /// </summary>
        private void OptionsWindow()
        {
            GUI.Box(new Rect(Screen.width / 2 - 215, Screen.height / 2 - 175, 430, 350), "", "Holder");

            GUI.Label(new Rect(Screen.width / 2 - 175, Screen.height / 2 - 240, 350, 80), _lmc[2], "Title");
            GUI.BeginGroup(new Rect(Screen.width / 2 - 175, Screen.height / 2 - 125, 350, 260));

            Options.GuiDraw();

            if (GUI.Button(new Rect(240, 220, 110, 40), _lmc[4]))
                _activeWindow = -1;
            GUI.EndGroup();
        }

        /// <summary>
        /// </summary>
        /// <param name="title"></param>
        private void LoadGame(string title)
        {
            // Checks if object is loadable
            try
            {
                // Tries to load & convert file
                var data = File.ReadAllText($"{SavableGame.FileDirectory}{title}.json");
                var file = JsonConvert.DeserializeObject<SavableGame>(data);
                if (file == null)
                    throw new Exception("File not set");
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                DI.Fetch<MessageController>()?.AddMessage("Data is corrupted!");
                return;
            }

            PlayerPrefs.SetString("CurrentGame", title);
            _isStartGame = true;
            StartGame();
        }

        /// <summary>
        /// </summary>
        private void CreateNewGame()
        {
            // Checks if title is set
            if (string.IsNullOrEmpty(_title) || string.IsNullOrWhiteSpace(_title))
            {
                DI.Fetch<MessageController>()?.AddMessage("Title is empty!");
                return;
            }

            // Checks if file already exists
            if (File.Exists($"{SavableGame.FileDirectory}{_title}.json"))
            {
                DI.Fetch<MessageController>()?.AddMessage("Game with this title already exists");
                return;
            }

            var sg = new SavableGame();
            sg.CreateNewGame(_title, string.IsNullOrEmpty(_seed) ? null : _seed);
            _isStartGame = true;
            StartGame();
        }

        #region FadeIn effect variables

        private float _timer;
        private bool _isStartGame;
        private bool _started;
        public Texture2D FadeTexture;

        #endregion


        /// <summary>
        ///     FadeIn effect
        /// </summary>
        private void StartGame()
        {
            _timer += Time.deltaTime / 2;
            GUI.color = new Color(0, 0, 0, _timer);

            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeTexture);

            if (_timer >= 1 && !_started)
            {
                _timer = 1;
                _started = true;
                Util.ChangeScene("Dungeon");
            }
        }
    }
}