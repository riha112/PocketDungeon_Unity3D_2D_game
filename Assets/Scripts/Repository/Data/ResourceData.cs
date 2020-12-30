using Assets.Scripts.Misc.SpriteLoader;
using Assets.Scripts.Misc.Translator;
using UnityEngine;

namespace Assets.Scripts.Repository.Data
{
    public class ResourceData : IIndexable
    {
        public int Id { get; set; }

        private string _title;

        public string Title
        {
            get => _title;
            set => _title = T.Translate(value);
        }

        public float Durability { get; set; }

        public Texture2D Icon { get; private set; }

        public string PathToIcon
        {
            set => Icon = Resources.Load<Texture2D>($"Icons/Resources/{value}");
        }

        public Sprite Grounded { get; private set; }

        public string PathToGroundedStyle
        {
            set => Grounded = SpriteLoader.GetSprite($"Textures/TileMaps/{value}");
        }

        public Sprite Submerged { get; private set; }

        public string PathToSubmergedStyle
        {
            set => Submerged = SpriteLoader.GetSprite($"Textures/TileMaps/{value}");
        }
    }
}