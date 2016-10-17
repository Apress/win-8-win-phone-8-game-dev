using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Windows.Storage;

#if WINDOWS_PHONE
using System.IO.IsolatedStorage;
#endif

namespace GameFramework
{
    public static class SettingsManager
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        // A reference to the current game class
        internal static GameHost Game;

#if NETFX_CORE
        // A data container into which our settings will be written.
        // This is declared at class level to avoid the need to repeatedly
        // instantiate it.
        private static ApplicationDataContainer _settings;
#endif
        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor. Scope is internal so external code cannot create instances.
        /// </summary>
        static SettingsManager()
        {
#if !WINDOWS_PHONE
            // Initialise the _settings object
            _settings = ApplicationData.Current.LocalSettings;
            _settings.CreateContainer("main", ApplicationDataCreateDisposition.Always);
#endif
        }


        //-------------------------------------------------------------------------------------
        // Class functions

        /// <summary>
        /// Add a new setting or update a setting value
        /// </summary>
        /// <param name="settingName">The name of the setting to add or update</param>
        /// <param name="value">The new value for the setting</param>
        public static void SetValue(string settingName, string value)
        {
            // Convert the setting name to lower case so that names are case-insensitive
            settingName = settingName.ToLower();

#if WINDOWS_PHONE
            // Does a setting with this name already exist?
            if (IsolatedStorageSettings.ApplicationSettings.Contains(settingName))
            {
                // Yes, so update its value
                IsolatedStorageSettings.ApplicationSettings[settingName] = value;
            }
            else
            {
                // No, so add it
                IsolatedStorageSettings.ApplicationSettings.Add(settingName, value);
            }
#else
            _settings.Containers["main"].Values[settingName] = value;
#endif
        }
        /// <summary>
        /// Add or update a setting as an integer value
        /// </summary>
        public static void SetValue(string settingName, int value)
        {
            SetValue(settingName, value.ToString());
        }

        /// <summary>
        /// Add or update a setting as a float value
        /// </summary>
        public static void SetValue(string settingName, float value)
        {
            SetValue(settingName, value.ToString());
        }

        /// <summary>
        /// Add or update a setting as a bool value
        /// </summary>
        public static void SetValue(string settingName, bool value)
        {
            SetValue(settingName, value.ToString());
        }

        /// <summary>
        /// Add or update a setting as a date value
        /// </summary>
        public static void SetValue(string settingName, DateTime value)
        {
            SetValue(settingName, value.ToString("yyyy-MM-ddTHH:mm:ss"));
        }


        /// <summary>
        /// Retrieve a setting value from the object
        /// </summary>
        /// <param name="settingName">The name of the setting to be retrieved</param>
        /// <param name="defaultValue">A value to return if the requested setting does not exist</param>
        public static string GetValue(string settingName, string defaultValue)
        {
            // Convert the setting name to lower case so that names are case-insensitive
            settingName = settingName.ToLower();

#if WINDOWS_PHONE
            // Does a setting with this name exist?
            if (IsolatedStorageSettings.ApplicationSettings.Contains(settingName))
            {
                // Yes, so return it
                return IsolatedStorageSettings.ApplicationSettings[settingName].ToString();
            }
            else
            {
                // No, so return the default value
                return defaultValue;
            }
#else
            if (_settings.Containers["main"].Values.ContainsKey(settingName))
            {
                return _settings.Containers["main"].Values[settingName].ToString();
            }
            else
            {
                return defaultValue;
            }
#endif
        }

        /// <summary>
        /// Retrieve a setting as an int value
        /// </summary>
        public static int GetValue(string settingName, int defaultValue)
        {
            return int.Parse(GetValue(settingName, defaultValue.ToString()));
        }

        /// <summary>
        /// Retrieve a setting as a float value
        /// </summary>
        public static float GetValue(string settingName, float defaultValue)
        {
            return float.Parse(GetValue(settingName, defaultValue.ToString()));
        }

        /// <summary>
        /// Retrieve a setting as a bool value
        /// </summary>
        public static bool GetValue(string settingName, bool defaultValue)
        {
            return bool.Parse(GetValue(settingName, defaultValue.ToString()));
        }

        /// <summary>
        /// Retrieve a setting as a date value
        /// </summary>
        public static DateTime GetValue(string settingName, DateTime defaultValue)
        {
            return DateTime.Parse(GetValue(settingName, defaultValue.ToString("yyyy-MM-ddTHH:mm:ss")));
        }

        /// <summary>
        /// Clear all current setting values
        /// </summary>
        public static void ClearValues()
        {
#if WINDOWS_PHONE
            IsolatedStorageSettings.ApplicationSettings.Clear();
#else
            _settings.Containers["main"].Values.Clear();
#endif
        }

        /// <summary>
        /// Delete a setting value
        /// </summary>
        /// <param name="settingName">The name of the setting to be deleted</param>
        public static void DeleteValue(string settingName)
        {
            // Convert the setting name to lower case so that names are case-insensitive
            settingName = settingName.ToLower();
#if WINDOWS_PHONE
            // Do we have this setting in the dictionary?
            if (IsolatedStorageSettings.ApplicationSettings.Contains(settingName))
            {
                // Yes, so remove it
                IsolatedStorageSettings.ApplicationSettings.Remove(settingName);
            }
#else
            _settings.Containers["main"].Values.Remove(settingName);
#endif
        }

        /// <summary>
        /// Read the values from all SettingsItemObjects contained within the
        /// game's GameObjects collection and store them all for later retrieval.
        /// </summary>
        public static void StoreSettingsItemValues()
        {
            // Loop through the GameObjects list looking for settings objects
            foreach (GameObjectBase obj in Game.GameObjects)
            {
                // Is this a setting object?
                if (obj is SettingsItemObject)
                {
                    // It is, so apply its value to the dictionary
                    SetValue(((SettingsItemObject)obj).Name, ((SettingsItemObject)obj).SelectedValue);
                }
            }
        }

    }
}
