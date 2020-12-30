using System.Collections.Generic;

namespace Assets.Scripts.Misc.Translator
{
    /// <summary>
    /// Translator class
    /// - Responsible for managing multi-language support within game.
    /// </summary>
    public static class T
    {
        private static Dictionary<string, string> Library { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Translates sentence by looking it up in library
        /// </summary>
        /// <param name="sentence">Sentence to translate</param>
        /// <returns>Translated or original sentence</returns>
        public static string Translate(string sentence)
        {
            if (Library.Count == 0)
            {
                BuildLibrary("jp_JP");
            }
            return Library.ContainsKey(sentence) ? Library[sentence] : sentence;
        }

        /// <summary>
        /// Adds new word to translation library
        /// </summary>
        /// <param name="from">Sentence in base (eng) language</param>
        /// <param name="to">Sentence in system language</param>
        public static void AddTranslation(string from, string to)
        {
            Library[from] = to;
        }

        /// <summary>
        /// Populates translation Library from json type file
        /// </summary>
        /// <param name="language">Location of json file</param>
        public static void BuildLibrary(string language)
        {
            Library = Util.LoadJsonFromFile<Dictionary<string,string>>($"Languages/{language}");
        }
    }
}
