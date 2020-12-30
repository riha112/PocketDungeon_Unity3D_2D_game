namespace Assets.Scripts.Weapons.Projectile
{
    public class EnemyProjectile : MovingDamagerOnCollision
    {
        protected override string GetTargetTag()
        {
            return "Player";
        }
    }
}