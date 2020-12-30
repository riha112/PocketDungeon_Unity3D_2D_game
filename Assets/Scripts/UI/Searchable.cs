using UnityEngine;

namespace Assets.Scripts.UI
{
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
