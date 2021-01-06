namespace Assets.Scripts.Weapons.Projectile
{
    /// <summary>
    /// <inheritdoc cref="DamageOnCollision"/>
    /// Used to target Player
    /// </summary>
    public class DamageOnCollisionPlayer : DamageOnCollision
    {
        protected override string GetTargetTag() => "Player";
    }
}
