using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Items;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.UI;
using Assets.Scripts.User;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class GameUi : MonoBehaviour
{
    public Sprite[] HealthSprites;
    public Sprite[] MagicSprites;
    public Texture2D CursorTexture2D;
    private static CharacterEntity _characterEntity;
    private Image _healthImage;
    private Image _magicImage;

    private void Awake()
    {
        Cursor.SetCursor(CursorTexture2D, Vector2.zero, CursorMode.Auto);
       // return;
        

        //_healthImage = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();
        //_magicImage = GameObject.FindGameObjectWithTag("MagicBar").GetComponent<Image>();

        //if (_characterEntity == null)
        //{
        //    _characterEntity = DI.Fetch<CharacterEntity>();
        //}

        //if (_characterEntity == null) return;
        //_characterEntity.HealthUpdated += OnHealthChanges;
        //_characterEntity.MagicUpdated += OnMagicChanges;
        //_characterEntity.Attributes.AttributeUpdate += OnAttributeChange;
    }

    //private void OnAttributeChange([CanBeNull] object sender, (int, short) data)
    //{
    //    switch (data.Item1)
    //    {
    //        case 1:
    //            OnHealthChanges(null, _characterEntity.Health);
    //            break;
    //        case 3:
    //            OnMagicChanges(null, _characterEntity.Magic);
    //            break;
    //    }
    //}

    //private void OnHealthChanges([CanBeNull] object sender, float health)
    //{
    //    _healthImage.sprite = HealthSprites[GetCalculatedSpriteId(health, _characterEntity.Stats.MaxHealth)];
    //}

    //private void OnMagicChanges([CanBeNull] object sender, float magic)
    //{
    //    _magicImage.sprite = MagicSprites[GetCalculatedSpriteId(magic, _characterEntity.Stats.MaxMagic)];
    //}

    //private int GetCalculatedSpriteId(float current, float max) 
    //{
    //    return (HealthSprites.Length - 1) - (int) ((HealthSprites.Length - 1) * (current / max));
    //}
}
