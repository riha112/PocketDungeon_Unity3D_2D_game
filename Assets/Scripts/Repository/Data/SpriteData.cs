using Assets.Scripts.Misc.SpriteLoader;
using UnityEngine;

namespace Assets.Scripts.Repository.Data
{
    public class SpriteData
    {
        public Sprite Sprite { get; private set; }
        public int Percentage { get; set; }

        public string PathToSprite
        {
            set => Sprite = SpriteLoader.GetSprite($"Textures/TileMaps/{value}");
        }
    }
}