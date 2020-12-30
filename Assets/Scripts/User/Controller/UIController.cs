using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Misc;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.User.Attributes;
using Assets.Scripts.User.Equipment;
using Assets.Scripts.User.Inventory;
using Assets.Scripts.User.Map;
using Assets.Scripts.User.Resource;
using UnityEngine;

namespace Assets.Scripts.User.Controller
{
    public class UISection
    {
        public List<UI.UI> UiControllers { get; set; }
        public KeyCode KeyCode { get; set; }
        public bool IsToggled { get; set; } = false;
    }

    public class UIController : MonoBehaviour
    {
        protected List<UISection> UiSections { get; set; }

        public bool IsFrozen { get; set; } = false;

        public static event EventHandler<Vector2> ActionKeyPress;

        public void Awake()
        {
            DI.Register(this);
        }

        public void Start()
        {
            PopulateSections();
        }

        public void SetToggleState<T>(bool state)
        {
            foreach (var uiSection in UiSections)
                foreach (var controller in uiSection.UiControllers)
                    if (controller is T)
                        uiSection.IsToggled = state;
        }

        protected void PopulateSections()
        {
            UiSections = new List<UISection>()
            {
                new UISection()
                {
                    UiControllers = new List<UI.UI>()
                    {
                        DI.Fetch<InventoryController>(),
                        DI.Fetch<EquipmentController>()
                    },
                    KeyCode = KeyCode.Q
                },
                new UISection()
                {
                    UiControllers = new List<UI.UI>()
                    {
                        DI.Fetch<AttributeController>()
                    },
                    KeyCode = KeyCode.E
                },
                new UISection()
                {
                    UiControllers = new List<UI.UI>()
                    {
                        DI.Fetch<MapController>()
                    },
                    KeyCode = KeyCode.Tab
                },
                new UISection()
                {
                    UiControllers = new List<UI.UI>()
                    {
                        DI.Fetch<ResourceController>()
                    },
                    KeyCode = KeyCode.Space
                }
            };
        }

        public void HideAllSections()
        {
            foreach (var section in UiSections.Where(section => section.IsToggled))
            {
                section.UiControllers.ForEach(controller => controller.Toggle(false));
                section.IsToggled = false;
            }
        }

        protected void HideAllExcept(UISection section)
        {
            var cacheState = section.IsToggled;
            HideAllSections();
            if (cacheState)
            {
                ToggledSection(section);
            }
        }

        protected void ToggledSection(UISection section)
        {
            section.IsToggled = !section.IsToggled;
            section.UiControllers.ForEach(controller => controller.Toggle(section.IsToggled));
        }


        private void Update()
        {
            if (IsFrozen)
            {
                return;
            }

            if (Input.GetKeyUp(KeyCode.F))
            {
                ActionKeyPress?.Invoke(this, Util.GetCharacter.transform.position);
            }

            foreach (var section in UiSections.Where(section => Input.GetKeyDown(section.KeyCode)))
            {
                HideAllExcept(section);
                ToggledSection(section);
            }
        }
    }
}
