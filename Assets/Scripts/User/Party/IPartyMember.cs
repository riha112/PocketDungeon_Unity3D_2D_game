using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.User.Party
{
    public interface IPartyMember
    {
        float HealthMlt { get; }
        float MagicMlt { get; }
        string Name { get; }
        Texture2D Design { get; }
    }
}
