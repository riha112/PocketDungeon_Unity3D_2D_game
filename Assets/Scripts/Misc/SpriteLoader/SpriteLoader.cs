using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Misc.SpriteLoader
{
    /// <summary>
    /// Utility for loading sprites from Unity Sprite texture
    /// by isolating Texture name & sprite name via "::"
    /// </summary>
    public static class SpriteLoader
    {
        private static readonly Dictionary<string, Sprite[]> CachedSprites = new Dictionary<string, Sprite[]>();

        public static Sprite GetSprite(string path)
        {
            var segments = path.Split(':');
            return GetSprite(segments[0], segments[1]);
        }

        public static Sprite GetSprite(string tileMapPath, string tileName)
        {
            if (!CachedSprites.ContainsKey(tileMapPath))
                CachedSprites[tileMapPath] = Resources.LoadAll<Sprite>(tileMapPath);

            return CachedSprites[tileMapPath].FirstOrDefault(sprite => sprite.name == tileName);
        }

    }
}
