using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Translator;
using Assets.Scripts.Repository;
using Assets.Scripts.Repository.Data;
using Assets.Scripts.UI;
using Assets.Scripts.User.Magic;
using Assets.Scripts.User.Messages;
using Assets.Scripts.World.Generation;
using Assets.Scripts.World.Generation.Data;
using UnityEngine;

namespace Assets.Scripts.User.Resource
{
    public class ResourceController : Popup
    {
        public GameObject Tile;
        public Color TileColor;

        private bool _buildMode;
        private ResourceData _activeResource;

        public Transform Placeholder;
        private SpriteRenderer _placeholdersSpriteRenderer;

        private DungeonSectionData _dungeonData;
        private DungeonSectionData DungeonData
        {
            get
            {
                if (_dungeonData == null)
                    _dungeonData = DI.Fetch<DungeonSectionData>();
                return _dungeonData;
            }
        }

        private CharacterEntity _characterEntity;
        private CharacterEntity CharacterEntity
        {
            get
            {
                if (_characterEntity == null)
                    _characterEntity = DI.Fetch<CharacterEntity>();
                return _characterEntity;
            }
        }

        private bool BuildMode
        {
            get => _buildMode;
            set
            {
                _buildMode = value;
                RectConfig.ShowBackground = !_buildMode;
                DI.Fetch<InGameUiController>().enabled = !_buildMode;
                DI.Fetch<MessageController>().Offset = _buildMode ? 220 : 365;
                Placeholder.gameObject.SetActive(_buildMode);
            }
        }

        protected override void Init()
        {
            RectConfig.Title = T.Translate("RESOURCES");
            RectConfig.TitleRect.x += 105 / 2;
            RectConfig.Popup.width = 550;
            RectConfig.Popup.height = 300;
            RectConfig.Popup.x -= 105 / 2;
            RectConfig.Popup.y = ScreenSize.y / 2 - 200;
            RectConfig.Body.y = 100;
            RectConfig.Body.width = 445;
            RectConfig.Body.height = 200;

            _placeholdersSpriteRenderer = Placeholder.GetComponent<SpriteRenderer>();
        }

        public override void Toggle(bool state)
        {
            BuildMode = false;
            base.Toggle(state);
        }

        protected override void Design()
        {
            if (!BuildMode)
            {
                DrawBackground();
                DrawPopup();
            }
            else
            {
                BuildingScreen();
            }
        }

        protected override void DrawBody()
        {
            for (var i = 0; i < ResourceRepository.Repository.Count; i++)
            {
                if (GUI.Button(new Rect(25 + i * 105, 50, 80, 80), ResourceRepository.Repository[i].Icon, "inv_item"))
                {
                    BuildMode = true;
                    _activeResource = ResourceRepository.Repository[i];
                }

                GUI.Box(new Rect(20 + i * 105, 140, 90, 20), ResourceRepository.Repository[i].Title, "att_points");
                GUI.Label(new Rect(80 + i * 105, 115, 25, 15), GetResourceCount(ResourceRepository.Repository[i].Id).ToString());
            }
        }

        private int GetResourceCount(int id) => CharacterEntity.Resources[id];

        private void BuildingScreen()
        {
            if (GUI.Button(new Rect(ScreenSize.x - 100, ScreenSize.y - 100, 80, 80), _activeResource.Icon,
                "magic_slot"))
            {
                BuildMode = false;
            }

            GUI.Label(new Rect(ScreenSize.x - 45, ScreenSize.y - 35, 25, 15), GetResourceCount(_activeResource.Id).ToString());

            var point = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
            var tile = DungeonData.GetTileByCoords(point);

            if (tile != null && tile.Instance != null)
            {
                Placeholder.transform.position = tile.Instance.transform.position;
                if((tile.Type != TileType.Floor && tile.Type != TileType.Water) || tile.Instance.transform.childCount > 0)
                    _placeholdersSpriteRenderer.color = Color.red;
                else
                    _placeholdersSpriteRenderer.color = Color.white;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (GetResourceCount(_activeResource.Id) <= 0)
                {
                    DI.Fetch<MessageController>()?.AddMessage(T.Translate("Not enough resources"));
                    return;
                }

                if (tile == null || tile.Instance == null || (tile.Type != TileType.Floor && tile.Type != TileType.Water))
                    return;

                if (tile.Instance.transform.childCount > 0)
                    return;

                var block = Instantiate(Tile);
                block.transform.position = new Vector3(
                    tile.Instance.transform.position.x,
                    tile.Instance.transform.position.y,
                    tile.Instance.transform.position.y
                );
                block.transform.parent = tile.Instance.transform;

                var sr = block.GetComponent<SpriteRenderer>();
                sr.color = TileColor;

                if (tile.Type == TileType.Water)
                {
                    tile.Instance.GetComponent<BoxCollider2D>().enabled = false;
                    block.GetComponent<BoxCollider2D>().enabled = false;
                    sr.sortingOrder = 0;
                    block.transform.localPosition = new Vector3(0, 0, -0.1f);
                    sr.sprite = _activeResource.Submerged;
                }
                else
                {
                    block.AddComponent<ResourceObject>();
                    block.GetComponent<ResourceObject>().Durability = (int)_activeResource.Durability;
                    block.tag = "Resource";
                    sr.sortingOrder = 2;
                    sr.sprite = _activeResource.Grounded;
                }

                CharacterEntity.Resources[_activeResource.Id]--;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Toggle(false);
            }
        }
    }
}
