using UnityEngine;

namespace Assets.Scripts.User.Party
{
    /// <summary>
    /// Information about all party members
    /// </summary>
    public interface IPartyMember
    {
        /// <summary>
        /// Percentage of current health
        /// </summary>
        float HealthMlt { get; }

        /// <summary>
        /// Percentage of current magic
        /// </summary>
        float MagicMlt { get; }

        /// <summary>
        /// Name of entity
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Icon for entity
        /// </summary>
        Texture2D Design { get; }
    }
}