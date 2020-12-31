using UnityEngine;

namespace Assets.Scripts.Misc.GUI
{
    /// <summary>
    /// Extends UI class to add search functionality
    /// <seealso cref="UI"/>
    /// </summary>
    public class Searchable : UI
    {
        private string _searchMessage;
        protected string SearchMessage
        {
            get => _searchMessage;
            set
            {
                if (value != _searchMessage) Search(value);
                _searchMessage = value;
            }
        }

        protected virtual void Search(string msg)
        {
            Debug.Log(msg);
        }
    }
}
