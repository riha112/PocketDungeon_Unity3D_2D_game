namespace Assets.Scripts.Weapons.Projectile
{
    /// <summary>
    /// <inheritdoc cref="MovingDamagerOnCollision"/>
    /// Targets Player as target
    /// </summary>
    public class EnemyProjectile : MovingDamagerOnCollision
    {
        protected override string GetTargetTag() => "Player";
    }
}