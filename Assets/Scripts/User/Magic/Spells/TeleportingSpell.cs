using Assets.Scripts.Misc;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.User.Messages;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;

namespace Assets.Scripts.User.Magic.Spells
{
    /// <summary>
    /// Implementation of teleportation
    /// - Moves character to specific tile, that cursor is pointing
    ///   on to, only if tile pointed tile is floor.
    /// </summary>
    public class TeleportingSpell : MonoBehaviour
    {
        private void Start()
        {
            var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var tile = DI.Fetch<DungeonSectionData>()?.GetTileByCoords(cursorPosition);
            if (tile == null || tile.Type != TileType.Floor)
                DI.Fetch<MessageController>()?.AddMessage("Unable to teleport to specific point");
            else
                Util.GetCharacterTransform().position = tile.Instance.transform.position;
            Destroy(gameObject);
        }
    }
}