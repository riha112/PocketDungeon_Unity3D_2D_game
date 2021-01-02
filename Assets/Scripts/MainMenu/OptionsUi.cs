using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Misc.GUI;
using Assets.Scripts.Misc.Translator;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    /// <summary>
    /// GUI for managing games options:
    /// - Resolution
    /// - Graphics
    /// - Language
    /// - Display
    /// </summary>
    public class OptionsUi : UI
    {
        #region Resolution
        private static Resolution[] _resolutions;
        private static Resolution _currentResolution;
        #endregion

        #region Language
        private static readonly Dictionary<string, string> _languages = new Dictionary<string, string>
        {
            { "en_EN", "English" },
            { "jp_JP", "日本語" },
            { "lv_LV", "Latviešu" }
        };
        private static readonly string[] _languageCodes = _languages.Keys.ToArray();
        private static string _currentLanguage;
        #endregion

        #region Quality
        private static string[] _qualityNames;
        private static int _currentQualityLevel;
        #endregion

        #region Display
        //private static Display[] _displays = Display.displays;
        //private static Display _currentDisplay = Display.main;
        #endregion

        #region Calculations
        private Vector2 _scrollView;
        private static int _calculatedResolutionHeight;
        private static int _calculatedLanguageHeight;
        private static int _calculatedQualityHeight;
        private static int _calculatedHeight;
        #endregion

        public static EventHandler<Resolution> ResolutionChanged;

        protected override void Awake()
        {
            _resolutions = Screen.resolutions;
            _currentResolution = Screen.currentResolution;
            _qualityNames = QualitySettings.names;
            _currentQualityLevel = QualitySettings.GetQualityLevel();
            ReCalculate();
            LoadConfiguration();

            base.Awake();
        }

        private static void ReCalculate()
        {
            _calculatedResolutionHeight = _resolutions.Length * 40 + 50;
            _calculatedLanguageHeight = _languages.Count * 40 + 50;
            _calculatedQualityHeight = _qualityNames.Length * 40 + 50;
            _calculatedHeight = _calculatedLanguageHeight + _calculatedQualityHeight + _calculatedResolutionHeight;
        }

        private static void LoadConfiguration()
        {
            if (!PlayerPrefs.HasKey("option_language"))
            {
                PlayerPrefs.SetString("option_language", _languageCodes[0]);
            }
            _currentLanguage = PlayerPrefs.GetString("option_language");
        }

        private static void ChangeLanguage(string langCode)
        {
            PlayerPrefs.SetString("option_language", langCode);
            T.SetLanguage(langCode);
            _currentLanguage = langCode;
        }

        private static void ChangeResolution(Resolution resolution)
        {
            Screen.SetResolution(resolution.width, resolution.height, true);
            _currentResolution = resolution;
            ResolutionChanged?.Invoke(null, resolution);
        }

        private void DrawResolutionSelector()
        {
            GUI.Label(new Rect(0, 0, 340, 50), LT("Resolution"), "option_label");
            for (var i =0; i < _resolutions.Length; i++)
            {
                var resolution = _resolutions[i];
                var style = (resolution.width == _currentResolution.width &&
                             resolution.height == _currentResolution.height)
                    ? "option_selected"
                    : "option_btn";

                if (GUI.Button(new Rect(0, 50 +40 * i, 340, 35), $"{resolution.width}x{resolution.height}", style))
                {
                    ChangeResolution(resolution);
                }
            }
        }

        private void DrawGraphicsSelector()
        {
            GUI.Label(new Rect(0, 0, 340, 50), LT("Quality"), "option_label");
            for (var i = 0; i < _qualityNames.Length; i++)
            {
                var style = (i == _currentQualityLevel) ? "option_selected" : "option_btn";
                if (GUI.Button(new Rect(0, 50 + 40 * i, 340, 35), T.Translate(_qualityNames[i]), style))
                {
                    _currentQualityLevel = i;
                    QualitySettings.SetQualityLevel(i);
                }
            }
        }

        private void DrawLanguageSelector()
        {
            GUI.Label(new Rect(0, 0, 340, 50), LT("Language"), "option_label");
            for (var i = 0; i < _languageCodes.Length; i++)
            {
                var language = _languageCodes[i];
                var style = (language == _currentLanguage) ? "option_selected" : "option_btn";
                if (GUI.Button(new Rect(0, 50 + 40 * i, 340, 35), _languages[language], style))
                {
                    ChangeLanguage(language);
                }
            }
        }

        private void DrawDisplaySelector()
        {
            throw new NotImplementedException("Not yet supported");
        }

        protected override void Design()
        {
            _scrollView = GUI.BeginScrollView(new Rect(0, 0, 350, 200), _scrollView,
                new Rect(0, 0, 250, _calculatedHeight));

            GUI.BeginGroup(new Rect(0, 0, 340, _calculatedQualityHeight));
            DrawGraphicsSelector();
            GUI.EndGroup();

            GUI.BeginGroup(new Rect(0, _calculatedQualityHeight, 340, _calculatedResolutionHeight));
            DrawResolutionSelector();
            GUI.EndGroup();

            GUI.BeginGroup(new Rect(0, _calculatedQualityHeight + _calculatedResolutionHeight, 340, _calculatedLanguageHeight));
            DrawLanguageSelector();
            GUI.EndGroup();

            GUI.EndScrollView();
        }

        protected override List<string> GetLmcLibrary()
        {
            return new List<string>
            {
                "Quality",
                "Resolution",
                "Language"
            };
        }
    }
}
