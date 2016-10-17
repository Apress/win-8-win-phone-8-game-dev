using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace GameFramework
{
    public class SettingsItemObject : TextObject
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        private int _valueIndex;

        //-------------------------------------------------------------------------------------
        // Constructors

        public SettingsItemObject(GameHost game, SpriteFont font, Vector2 position, float scale, string name, string title, string defaultValue, string[] values)
            : base(game)
        {
            // Set object properties
            Position = position;
            Scale = new Vector2(scale);
            Font = font;
            Name = name;
            Title = title;
            Values = values;

            _valueIndex = GetValueIndex(SettingsManager.GetValue(name, defaultValue));

            // Set the text for the option item
            SetText();
        }

        //-------------------------------------------------------------------------------------
        // Properties

        /// <summary>
        /// Sets or retrieves the currently selected value
        /// </summary>
        public string SelectedValue
        {
            get
            {
                return Values[_valueIndex];
            }
            set
            {
                _valueIndex = GetValueIndex(value);
            }
        }

        public string Name { get; set; }

        public string Title { get; set; }

        public string[] Values { get; set; }

        //-------------------------------------------------------------------------------------
        // Object functions

        /// <summary>
        /// Find the value index of the specified string
        /// </summary>
        /// <returns>Returns the index, or 0 if no match was found</returns>
        private int GetValueIndex(string value)
        {
            // Loop for each value
            for (int i = 0; i < Values.Length; i++)
            {
                // Does this value match the one we are looking for?
                if (Values[i] == value)
                {
                    // This is the matching value so return this index
                    return i;
                }
            }

            // No match found so return 0 to select the first item
            return 0;
        }

        /// <summary>
        /// Set the object's text string
        /// </summary>
        private void SetText()
        {
            Text = Title + ": " + SelectedValue;
        }

        /// <summary>
        /// Cycles to the next value from the values array
        /// </summary>
        public void SelectNextValue()
        {
            _valueIndex += 1;
            if (_valueIndex >= Values.Length) _valueIndex = 0;
            SetText();
        }

    }
}
