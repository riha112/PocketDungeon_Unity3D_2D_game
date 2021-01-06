using UnityEngine;

namespace Assets.Scripts.User.Magic
{
    /// <summary>
    /// Used for items that user can pin onto game-screen action slots
    /// </summary>
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