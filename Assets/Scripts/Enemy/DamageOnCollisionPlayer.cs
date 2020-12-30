using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Helper;
using Assets.Scripts.Weapons.Projectile;

namespace Assets.Scripts.Enemy
{
    public class DamageOnCollisionPlayer : DamageOnCollision
    {
        protected override string GetTargetTag() => "Player";
    }
}
