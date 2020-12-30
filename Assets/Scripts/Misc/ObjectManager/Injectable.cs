using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Misc.Translator;
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
