using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.Items.Type.Info;

namespace Assets.Scripts.Items.Type.Controller
{
    /// <summary>
    /// Holds real items data for items with which user can attack
    /// </summary>
    public class WeaponItem : EquipableItem
    {
        /// <inheritdoc cref="SimpleItem"/>
        public override ItemType[] Resolves()
        {
            return new[] {ItemType.Weapon};
        }

        /// <summary>
        /// Converts current ItemData object into WeaponItemData object
        /// </summary>
        public WeaponItemData WeaponInfo
        {
            get
            {
                if (Info is WeaponItemData item)
                    return item;
                return null;
            }
        }

        /// <summary>
        /// Used to check whether or not user can use command Attack
        /// </summary>
        protected bool CanAttack = true;

        /// <summary>
        /// Sets timeout after each attack
        /// </summary>
        protected virtual void SetCollDown()
        {
            CanAttack = false;

            // Little Unity hack to replace MonoBehaviour.Invoke for non attached classes
            // Resets attack state after X seconds, so that user can attack again
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep((int) (WeaponInfo.AttackCoolDown * 1000));
                CanAttack = true;
            });
        }

        /// <summary>
        /// Called when user is requesting "Primary" & "Secondary" weapon usage via <seealso cref="FightingController"/>
        /// </summary>
        public virtual void Attack()
        {
            Durability -= EquipableData.DurabilityReducer;
        }
    }
}