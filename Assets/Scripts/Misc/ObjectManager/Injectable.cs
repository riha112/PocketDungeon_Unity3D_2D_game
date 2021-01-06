using UnityEngine;

namespace Assets.Scripts.Misc.ObjectManager
{
    public class Injectable : MonoBehaviour
    {
        protected virtual void Awake()
        {
            DI.Register(this);
            Init();
        }

        protected virtual void Init()
        {

        }
    }
}
