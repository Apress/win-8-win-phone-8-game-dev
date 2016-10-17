using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace DragAndFlick_WP8
{
    class SelectableSpriteObject : GameFramework.SpriteObject
    {

        //-------------------------------------------------------------------------------------
        // Class constructors

        public SelectableSpriteObject(GameHost game, Vector2 position, Texture2D texture)
            : base(game, position, texture)
        {
            // Set a default friction
            KineticFriction = 0.9f;
        }


        //-------------------------------------------------------------------------------------
        // Properties

        /// <summary>
        /// Is this object currently selected?
        /// </summary>
        public bool Selected { get; set; }

        public Vector2 KineticVelocity { get; set; }

        /// <summary>
        /// The friction to apply to kinetic object movement in the range 0 to 1
        /// (0 == no friction at all, 1 == infinite friction).
        /// </summary>
        public float KineticFriction { get; set; }

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

        //-------------------------------------------------------------------------------------
        // Object functions

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Is the movement vector non-zero?
            if (KineticVelocity != Vector2.Zero)
            {
                // Yes, so add the vector to the position
                Position += KineticVelocity;
                // Ensure that the friction value is within range
                KineticFriction = MathHelper.Clamp(KineticFriction, 0, 1);
                // Apply 'friction' to the vector so that movement slows and stops
                KineticVelocity *= KineticFriction;
            }
        }


    }
}
