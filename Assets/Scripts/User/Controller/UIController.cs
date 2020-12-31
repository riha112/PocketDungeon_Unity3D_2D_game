using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Misc;
using Assets.Scripts.Misc.GUI;
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
        public List<UI> UiControllers { get; set; }
        public KeyCode KeyCode { get; set; }
        public bool IsToggled { get; set; }
    }

    /// <summary>
    /// Controls UI linkage with keyboards input,
    /// meaning that when pressing specific key specific
    /// windows is shown
    /// </summary>
    public class UIController : Injectable
    {
        /// <summary>
        /// List of Key-Ui linkage configurations
        /// </summary>
        protected List<UISection> UiSections { get; set; }

        /// <summary>
        /// When frozen controller is not listening for any
        /// key inputs
        /// </summary>
        public bool IsFrozen { get; set; } = false;

        /// <summary>
        /// Event for dedicated key firing - "F" key AKA action key
        /// </summary>
        public static event EventHandler<Vector2> ActionKeyPress;

        public void Start()
        {
            PopulateSections();
        }

        /// <summary>
        /// Configurations for UI
        /// </summary>
        protected void PopulateSections()
        {
            UiSections = new List<UISection>
            {
                new UISection
                {
                    UiControllers = new List<UI>
                    {
                        DI.Fetch<InventoryPopupUi>(),
                        DI.Fetch<EquipmentController>()
                    },
                    KeyCode = KeyCode.Q
                },
                new UISection
                {
                    UiControllers = new List<UI>
                    {
                        DI.Fetch<AttributePopupUi>()
                    },
                    KeyCode = KeyCode.E
                },
                new UISection
                {
                    UiControllers = new List<UI>
                    {
                        DI.Fetch<MapController>()
                    },
                    KeyCode = KeyCode.Tab
                },
                new UISection
                {
                    UiControllers = new List<UI>
                    {
                        DI.Fetch<ResourceController>()
                    },
                    KeyCode = KeyCode.Space
                }
            };
        }

        /// <summary>
        /// Changes state for specific UI elements, with type of T
        /// </summary>
        /// <typeparam name="T">Which UI elements to target</typeparam>
        /// <param name="state">New state</param>
        public void SetToggleState<T>(bool state)
        {
            foreach (var uiSection in UiSections)
            {
                foreach (var controller in uiSection.UiControllers)
                {
                    if (controller is T)
                    {
                        uiSection.IsToggled = state;
                    }
                }
            }
        }

        /// <summary>
        /// Hides all UI elements
        /// </summary>
        public void HideAllSections()
        {
            foreach (var section in UiSections.Where(section => section.IsToggled))
            {
                section.UiControllers.ForEach(controller => controller.Toggle(false));
                section.IsToggled = false;
            }
        }

        /// <summary>
        /// Hides all except one
        /// </summary>
        /// <param name="section">One to keep open</param>
        protected void HideAllExcept(UISection section)
        {
            var cacheState = section.IsToggled;
            HideAllSections();
            if (cacheState) ToggledSection(section);
        }

        /// <summary>
        /// Hide/Un-hide specific section
        /// </summary>
        /// <param name="section"></param>
        protected void ToggledSection(UISection section)
        {
            section.IsToggled = !section.IsToggled;
            section.UiControllers.ForEach(controller => controller.Toggle(section.IsToggled));
        }


        private void Update()
        {
            if (IsFrozen) return;

            // Action key reading
            if (Input.GetKeyUp(KeyCode.F)) ActionKeyPress?.Invoke(this, Util.GetCharacter.transform.position);

            // Binded key reading 
            foreach (var section in UiSections.Where(section => Input.GetKeyDown(section.KeyCode)))
            {
                HideAllExcept(section);
                ToggledSection(section);
            }
        }
    }
}
