﻿using System.Collections.Generic;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Misc.Translator;
using UnityEngine;

namespace Assets.Scripts.User.Messages
{
    /// <summary>
    /// This class controls output of user information messages
    /// into in-game UI
    /// </summary>
    public class MessageController : MonoBehaviour
    {
        /// <summary>
        /// Maximum amount of messages to be visible
        /// </summary>
        private const int MAX_QUE_SIZE = 10;

        public List<string> Messages;

        /// <summary>
        /// Color of nth message, color count must be the same as MAX_QUE_SIZE
        /// </summary>
        public List<Color> MessageColors;


        public GUISkin Theme;

        /// <summary>
        /// UI position from bottom, used to change toggle location
        /// when hiding InGameUi component
        /// </summary>
        public int OffsetY = 365;

        /// <summary>
        /// UI position from left
        /// </summary>
        public int OffsetX = 25;

        /// <summary>
        /// Called on instance initiation
        /// </summary>
        private void Awake()
        {
            // Registers itself  into dependency injection class for later access
            DI.Register(this);

            // Fixes colors if not set or less than QUE size
            if (MessageColors == null || MessageColors.Count < MAX_QUE_SIZE)
                BuildDefaultMessageColors();

            // Invokes message clearing... removes one message per 3 seconds
            InvokeRepeating(nameof(ShiftMessages), 2, 3);
        }

        /// <summary>
        /// Sets default color values for messages.
        /// </summary>
        private void BuildDefaultMessageColors()
        {
            MessageColors = new List<Color>();
            const float delta = 1.0f / MAX_QUE_SIZE;
            for(var i = 0; i < MAX_QUE_SIZE; i++)
                MessageColors.Add(new Color(1,1,1, delta * i));
        }

        /// <summary>
        /// Adds message into que, if count
        /// is larger then MAX_QUE_SIZE then shifts messages
        /// </summary>
        /// <param name="msg">Message to be added</param>
        public void AddMessage(string msg)
        {
            if (Messages.Count >= MAX_QUE_SIZE)
                ShiftMessages();
            Messages.Add(T.Translate(msg));
        }

        /// <summary>
        /// Moves all messages to front of stack by one unit,
        /// thus removing the oldest message
        /// </summary>
        private void ShiftMessages()
        {
            if (Messages.Count == 0) return;

            for (var i = 0; i < Messages.Count - 1; i++)
                Messages[i] = Messages[i + 1];

            Messages.RemoveAt(Messages.Count - 1);
        }

        /// <summary>
        /// Draws GUI design
        /// </summary>
        private void OnGUI()
        {
            if(Messages.Count == 0) return;

            GUI.skin = Theme;
            GUI.depth = 200;

            GUI.BeginGroup(new Rect(OffsetX, Screen.height - OffsetY, 350, 200));

            for (var i = 0; i < Messages.Count; i++)
            {
                var y = 200 - (Messages.Count - i) * 20;
                GUI.color = MessageColors[MAX_QUE_SIZE - Messages.Count + i];
                GUI.Label(new Rect(0, y, 350, 15), Messages[i]);
            }

            GUI.EndGroup();
        }
    }
}
