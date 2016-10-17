using GameFramework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace CosmicRocksPartI_Win8
{
    internal class ParticleObject : GameFramework.SpriteObject
    {

        //-------------------------------------------------------------------------------------
        // Constructors

        public ParticleObject(GameHost game)
            : base(game)
        {
        }

        //-------------------------------------------------------------------------------------
        // Properties

        /// <summary>
        /// Is this particle currently active? If not it will ignore calls to
        /// Update and Draw until reactivated.
        /// </summary>
        internal bool IsActive { get; set; }

        /// <summary>
        /// The direction that this particle is moving in
        /// </summary>
        internal Vector2 Direction { get; set; }
        /// <summary>
        /// The speed that this particle is moving
        /// </summary>
        internal float Speed { get; set; }
        /// <summary>
        /// The inertia to apply to the particle. The speed will be multiplied
        /// by this value each update. Set to 1 for no itertia, 0.95 for medium,
        /// 0.8 for high, and so on.
        /// </summary>
        internal float Inertia { get; set; }

        /// <summary>
        /// The current sprite intensity this will be used as the SpriteColor alpha value
        /// </summary>
        internal float Intensity { get; set; }
        /// <summary>
        /// The rate at which the intensity fades. This will be subtracted from Intensity
        /// each update. When Intensity reaches zero, the particle will become inactive.
        /// </summary>
        internal float IntensityFadeAmount { get; set; }

        //-------------------------------------------------------------------------------------
        // Property overrides


        /// <summary>
        /// Return the color of the sprite, taking the intensity into account
        /// </summary>
        public override Color SpriteColor
        {
            get
            {
                Color col;
                byte alpha = 0;

                // Get the sprite color from the base class
                col = base.SpriteColor;

                // Get the intensity as a byte
                if (Intensity >= 0 && Intensity <= 255) alpha = (byte)Intensity;
                if (Intensity > 255) alpha = 255;

                // Set the intensity into the color
                col.A = alpha;

                return col;
            }
        }

        //-------------------------------------------------------------------------------------
        // Game functions

        public override void Update(GameTime gameTime)
        {
            // Exit if not currently active
            if (!IsActive) return;
            
            // Move the particle
            Position += Direction * Speed;
            // Apply inertia
            Speed *= Inertia;

            // Fade the intensity
            Intensity -= IntensityFadeAmount;
            // Have we faded away?
            if (Intensity <= 0)
            {
                // Yes, so become inactive
                IsActive = false;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Exit if not currently active
            if (!IsActive) return;

            base.Draw(gameTime, spriteBatch);
        }

        
        //-------------------------------------------------------------------------------------
        // Object functions

        internal void ResetProperties(Vector2 position, Texture2D texture)
        {
            // Store provided parameter values
            Position = position;
            SpriteTexture = texture;

            // The Origin is the center of the texture
            Origin = new Vector2(SpriteTexture.Width / 2, SpriteTexture.Height / 2);

            // Random angle
            Angle = MathHelper.ToRadians(GameHelper.RandomNext(360.0f));

            // Set the particle as initially active
            IsActive = true;
            // Have maximum intensity and fade slowly
            Intensity = 255;
            IntensityFadeAmount = 3;

            // Default to random movement
            do
            {
                Direction = new Vector2(GameHelper.RandomNext(-1.0f, 1.0f), GameHelper.RandomNext(-1.0f, 1.0f));
            } while (Direction == Vector2.Zero);
            Direction.Normalize();

            // No inertia by default
            Inertia = 1.0f;
        }

    }
}
