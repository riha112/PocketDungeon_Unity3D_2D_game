using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Misc.ObjectManager;
using UnityEngine;

namespace Assets.Scripts.Misc.GUI
{
    /// <summary>
    /// Holds all instances of UI elements and
    /// draws them via Unity method OnGUI
    /// </summary>
    public class Gui : Injectable
    {
        private List<UI> _designs;
        public GUISkin Theme;

        /// <summary>
        /// Adds new UI component to the renderer lists
        /// + Sorts by depth (smallest depth is rendered first)
        /// </summary>
        /// <param name="component">UI component to render</param>
        public void Subscribe(UI component)
        {
            if(_designs == null)
                _designs = new List<UI>();

            _designs.Add(component);
            _designs = _designs.OrderByDescending(c => c.Depth).ToList();
        }


        /// <summary>
        /// Removes UI component from renderer
        /// </summary>
        /// <param name="component">UI component to remove</param>
        public void UnSubscribe(UI component)
        {
            _designs?.Remove(component);
        }

        /// <summary>
        /// Outputs Unity3D GUI
        /// </summary>
        public void OnGUI()
        {
            UnityEngine.GUI.skin = Theme;
            useGUILayout = false;

            foreach (var design in _designs)
            {
                if(!design.enabled) continue;
                design.GuiDraw();
            }
        }
    }
}
