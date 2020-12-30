using Assets.Scripts.Misc.Random;
using UnityEngine;

namespace Assets.Scripts.Repository.Data
{
    public class TileData
    {
        public SpriteData[] Sprites { get; set; }

        public Sprite GetRandomSprite()
        {
            var percentage = R.RandomRange(0, 100);
            var curr = 0;
            for (var i = 0; i < Sprites.Length - 1; i++)
            {
                curr += Sprites[i].Percentage;
                if (curr > percentage)
                    return Sprites[i].Sprite;
            }

            return Sprites[Sprites.Length - 1].Sprite;
        }
    }
}