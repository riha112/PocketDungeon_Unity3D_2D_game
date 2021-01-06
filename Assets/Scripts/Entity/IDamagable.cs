namespace Assets.Scripts.Entity
{
    /// <summary>
    /// Used for objects that can be damaged,
    /// meaning their HP or durability may be reduced
    /// </summary>
    public interface IDamagable
    {
        /// <summary>
        /// Called when object sustains an damage
        /// </summary>
        /// <param name="hitPoints">Raw damage taken</param>
        /// <returns>Actual damage taken</returns>
        int TakeDamage(int hitPoints);
    }
}
