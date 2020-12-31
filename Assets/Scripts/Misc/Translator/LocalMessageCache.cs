using System.Collections.Generic;
using JetBrains.Annotations;

namespace Assets.Scripts.Misc.Translator
{
    /// <summary>
    /// Localizes T classes translations into self isolated object
    /// </summary>
    public class LocalMessageCache
    {
        public List<string> Messages { get; set; }
        private Dictionary<string, string> _translatedDictionary;
        private bool _isCached;

        public LocalMessageCache()
        {
            T.LanguageChange += OnLanguageChanged;
        }

        /// <summary>
        /// Rebuilds library when language is changed
        /// </summary>
        /// <param name="sender">always null, as T class is static</param>
        /// <param name="language">new language</param>
        private void OnLanguageChanged([CanBeNull] object sender, string language)
        {
            _isCached = false;
            BuildCachedMessages();
        }

        /// <summary>
        /// Populates list with cached messages by replacing,
        /// "eng" messages with translated messages
        /// </summary>
        public void BuildCachedMessages()
        {
            if (_isCached || Messages == null)
                return;

            _translatedDictionary = new Dictionary<string, string>();

            foreach (var msg in Messages)
            {
                if(!_translatedDictionary.ContainsKey(msg))
                    _translatedDictionary.Add(msg, T.Translate(msg));
            }

            _isCached = true;
        }

        public string this[string i] => _translatedDictionary[i];
        public string this[int i] => _translatedDictionary[Messages[i]];
    }
}
