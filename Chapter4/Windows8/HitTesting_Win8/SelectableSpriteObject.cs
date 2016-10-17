using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace HitTesting_Win8
{
    class SelectableSpriteObject : GameFramework.SpriteObject
    {

        //-------------------------------------------------------------------------------------
        // Class constructors

        public SelectableSpriteObject(GameHost game, Vector2 position, Texture2D texture)
            : base(game, position, texture)
        {
        }


        //-------------------------------------------------------------------------------------
        // Properties

        /// <summary>
        /// Is this object currently selected?
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Override SpriteColor to indicate the selected objects
        /// </summary>
        public override Color SpriteColor
        {
            get
            {
                // If not selected then return the sprite base color
                if (!Selected) return base.SpriteColor;

                // Otherwise return a shade of red to indicate the selection
                return Color.PaleVioletRed;
            }
        }

    }
}
