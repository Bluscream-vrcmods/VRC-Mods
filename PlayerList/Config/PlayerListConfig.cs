﻿using System;
using System.Collections.Generic;
using MelonLoader;
using PlayerList.Entries;
using UnityEngine;

namespace PlayerList.Config
{
    static class PlayerListConfig
    {
        public static event Action OnConfigChangedEvent;
        private static bool hasConfigChanged;

        // TODO: Make is so the vector 2 acutlaly uses the custom mapper when it gets fixed
        public static readonly string categoryIdentifier = "PlayerList Config";
        public static MelonPreferences_Category category = MelonPreferences.CreateCategory(categoryIdentifier);
        public static List<EntryWrapper> entries = new List<EntryWrapper>();

        public static EntryWrapper<bool> enabledOnStart;
        public static EntryWrapper<bool> condensedText;
        public static EntryWrapper<bool> numberedList;
        public static EntryWrapper<int> fontSize;
        public static EntryWrapper<int> snapToGridSize;

        public static EntryWrapper<bool> pingToggle;
        public static EntryWrapper<bool> fpsToggle;
        public static EntryWrapper<bool> platformToggle;
        public static EntryWrapper<bool> perfToggle;
        public static EntryWrapper<bool> distanceToggle;
        public static EntryWrapper<bool> photonIdToggle;
        public static EntryWrapper<bool> displayNameToggle;
        private static EntryWrapper<string> displayNameColorMode;

        public static PlayerEntry.DisplayNameColorMode DisplayNameColorMode
        {
            get { return (PlayerEntry.DisplayNameColorMode)Enum.Parse(typeof(PlayerEntry.DisplayNameColorMode), displayNameColorMode.Value); }
            set { displayNameColorMode.Value = value.ToString(); }
        }

        public static EntryWrapper<bool> excludeSelfFromSort;
        public static EntryWrapper<bool> sortFriendsFirst;
        private static EntryWrapper<string> currentSortType;

        private static EntryWrapper<string> menuButtonPosition;
        public static MenuManager.MenuButtonPositionEnum MenuButtonPosition
        {
            get { return (MenuManager.MenuButtonPositionEnum)Enum.Parse(typeof(MenuManager.MenuButtonPositionEnum), menuButtonPosition.Value); }
            set { menuButtonPosition.Value = value.ToString(); }
        }

        private static EntryWrapper<float> _playerListPositionX;
        private static EntryWrapper<float> _playerListPositionY;
        public static Vector2 PlayerListPosition
        {
            get { return Utilities.Converters.ConvertToUnityUnits(new Vector2(_playerListPositionX.Value, _playerListPositionY.Value)); }
            set 
            {
                Vector2 convertedVector = Utilities.Converters.ConvertToMenuUnits(value);
                _playerListPositionX.Value = convertedVector.x;
                _playerListPositionY.Value = convertedVector.y;
            }
        }

        public static void RegisterSettings()
        {
            enabledOnStart = CreateEntry(categoryIdentifier, nameof(enabledOnStart), true, is_hidden: true);
            condensedText = CreateEntry(categoryIdentifier, nameof(condensedText), false, is_hidden: true);
            numberedList = CreateEntry(categoryIdentifier, nameof(numberedList), true, is_hidden: true);
            fontSize = CreateEntry(categoryIdentifier, nameof(fontSize), 35, is_hidden: true);
            snapToGridSize = CreateEntry(categoryIdentifier, nameof(snapToGridSize), 420, is_hidden: true);

            pingToggle = CreateEntry(categoryIdentifier, nameof(pingToggle), true, is_hidden: true);
            fpsToggle = CreateEntry(categoryIdentifier, nameof(fpsToggle), true, is_hidden: true);
            platformToggle = CreateEntry(categoryIdentifier, nameof(platformToggle), true, is_hidden: true);
            perfToggle = CreateEntry(categoryIdentifier, nameof(perfToggle), true, is_hidden: true);
            distanceToggle = CreateEntry(categoryIdentifier, nameof(distanceToggle), true, is_hidden: true);
            photonIdToggle = CreateEntry(categoryIdentifier, nameof(photonIdToggle), false, is_hidden: true);
            displayNameToggle = CreateEntry(categoryIdentifier, nameof(displayNameToggle), true, is_hidden: true);
            displayNameColorMode = CreateEntry(categoryIdentifier, nameof(displayNameColorMode), "TrustAndFriends", is_hidden: true);

            menuButtonPosition = CreateEntry(categoryIdentifier, nameof(menuButtonPosition), "TopRight", is_hidden: true);

            _playerListPositionX = CreateEntry(categoryIdentifier, nameof(_playerListPositionX), 7.5f, is_hidden: true);
            _playerListPositionY = CreateEntry(categoryIdentifier, nameof(_playerListPositionY), 3.5f, is_hidden: true);

            foreach (EntryWrapper entry in entries)
                entry.OnValueChangedUntyped += OnConfigChanged;
        }

        public static EntryWrapper<T> CreateEntry<T>(string category_identifier, string entry_identifier, T default_value, string display_name = null, bool is_hidden = false)
        {
            MelonPreferences_Entry<T> melonPref = (MelonPreferences_Entry<T>)MelonPreferences.CreateEntry(category_identifier, entry_identifier, default_value, display_name, is_hidden);
            EntryWrapper<T> entry = new EntryWrapper<T>()
            {
                pref = melonPref
            };
            entries.Add(entry);

            return entry;
        }

        public static void OnConfigChanged()
        {
            OnConfigChangedEvent?.Invoke();
            hasConfigChanged = true;
        }
        public static void SaveEntries()
        {
            if (RoomManager.field_Internal_Static_ApiWorldInstance_0 == null) return;

            if (hasConfigChanged)
            {
                MelonPreferences.Save();
                hasConfigChanged = false;
            }

            ListPositionManager.shouldMove = false;
        }
    }
}