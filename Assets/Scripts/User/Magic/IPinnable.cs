using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.User.Magic
{
    public interface IPinnable
    {
        int Id { get; }
        bool CanUse { get; }
        bool IsSingleUse { get; }
        float CoolDownTimer { get; }
        Texture2D Icon { get; }
        string Title { get; }

        void OnPinned();
        void OnUnPinned();
        bool OnUsed();
    }
}
