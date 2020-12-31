using System;
using System.Collections.Generic;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Translator;
using UnityEngine;

namespace Assets.Scripts.Misc.GUI
{
    /// <summary>
    /// UI Element contains GUI design
    /// </summary>
    public class UI : Injectable
    {
        /// <summary>
        /// Cached size of current screen resolution
        /// </summary>
        protected static Vector2 ScreenSize;

        /// <summary>
        /// Cached reacts for GUI
        /// </summary>
        protected Rect[] ReactCache { get; set; }

        /// <summary>
        /// Render priority, lesser the value higher the priority,
        /// <example>Element with depth of 5 will be rendered on top of element
        /// with depth of 10</example>
        /// </summary>
        public virtual int Depth { get; set; } = 50;

        /// <summary>
        /// Localized translated message cache
        /// <seealso cref="LocalMessageCache"/>
        /// </summary>
        protected LocalMessageCache LMC { get; set; } = new LocalMessageCache();

        /// <summary>
        /// Event that is fired when UI component is disabled/enabled
        /// </summary>
        public static event EventHandler<bool> ToggleEvent;


        /// <summary>
        /// Called on moment of components creation
        /// </summary>
        protected override void Awake()
        {
            // Translates messages
            if (LMC.Messages == null || LMC.Messages.Count == 0)
            {
                LMC.Messages = GetLmcLibrary();
                LMC.BuildCachedMessages();
            }

            // Caches screen size
            ScreenSize = new Vector2(Screen.width, Screen.height);

            base.Awake();
        }

        protected virtual void Start()
        {
            // Registers UI component to global GUI component
            // so that its visible within Unity.
            DI.Fetch<Gui>()?.Subscribe(this);
        }

        /// <summary>
        /// Replacement method for Unity3D method OnGUI,
        /// used by Gui class to draw UI onto Unity3D game environment
        /// </summary>
        public virtual void GuiDraw()
        {
            Design();
        }

        /// <summary>
        /// Changes state of component - enabled/disabled
        /// </summary>
        /// <param name="state">new state - enabled/disabled</param>
        public virtual void Toggle(bool state)
        {
            enabled = state;
            ToggleEvent?.Invoke(this, state);
        }

        /// <summary>
        /// Main body of UI element
        /// </summary>
        protected virtual void Design()
        {
        }

        /// <summary>
        /// Messages to be translated
        /// </summary>
        protected virtual List<string> GetLmcLibrary()
        {
            return null;
        }

        protected string LT(string text)
        {
            return LMC.Messages.Contains(text) ? LMC[text] : T.Translate(text);
        }
    }
}
