using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Translator;
using Assets.Scripts.User.Inventory;
using Assets.Scripts.World.Items;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UI : Injectable
    {
        protected static Vector2 ScreenSize;
        protected Rect[] ReactCache { get; set; }

        public GUISkin Theme;
        protected virtual int Depth { get; set; } = 50;
        protected LocalMessageCache LMC { get; set; } = new LocalMessageCache();

        public static event EventHandler<bool> ToggleEvent;

        protected override void Awake()
        {
            if (LMC.CachedMessages == null || LMC.CachedMessages.Count == 0)
            {
                LMC.CachedMessages = GetLmcLibrary();
                LMC.BuildCachedMessages();
            }

            ScreenSize = new Vector2(Screen.width, Screen.height);
            base.Awake();
        }

        public virtual void OnGUI()
        {
            Config();
            Design();
        }

        public virtual void Toggle(bool state)
        {
            enabled = state;
            ToggleEvent?.Invoke(this, state);
        }

        protected virtual void Config()
        {
            GUI.skin = Theme;
            GUI.depth = Depth;
            useGUILayout = false;
        }

        protected virtual void Design()
        {
        }

        protected virtual List<string> GetLmcLibrary()
        {
            return null;
        }
    }
}
