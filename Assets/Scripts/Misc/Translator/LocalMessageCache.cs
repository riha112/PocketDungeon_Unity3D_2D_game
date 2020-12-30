using System.Collections.Generic;

namespace Assets.Scripts.Misc.Translator
{
    /// <summary>
    /// Localizes T classes translations into self isolated object
    /// </summary>
    public class LocalMessageCache
    {
        public List<string> CachedMessages;
        private bool _isCached;

        /// <summary>
        /// Populates list with cached messages by replacing,
        /// "eng" messages with translated messages
        /// </summary>
        public void BuildCachedMessages()
        {
            if (_isCached || CachedMessages == null)
                return;

            for (var i = 0; i < CachedMessages.Count; i++)
            {
                CachedMessages[i] = T.Translate(CachedMessages[i]);
            }

            _isCached = true;
        }

        public string this[int i] => CachedMessages[i];
    }
}
