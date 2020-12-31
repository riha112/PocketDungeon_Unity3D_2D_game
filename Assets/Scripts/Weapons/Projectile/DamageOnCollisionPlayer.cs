namespace Assets.Scripts.Weapons.Projectile
{
    public class DamageOnCollisionPlayer : DamageOnCollision
    {
        protected override string GetTargetTag() => "Player";
    }
}
