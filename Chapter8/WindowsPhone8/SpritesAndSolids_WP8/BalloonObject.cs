using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace SpritesAndSolids_WP8
{
    class BalloonObject : GameFramework.SpriteObject
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        private float _swayOffset;

        internal static int _balloonCount = 0;

        //-------------------------------------------------------------------------------------
        // Class constructors

        public BalloonObject(GameHost game, Texture2D texture)
            : base(game, Vector2.Zero, texture)
        {

            // Set the sprite origin to the center of the sprite
            Origin = new Vector2(SpriteTexture.Width / 2, SpriteTexture.Height / 2);

            // Use elliptical hit testing
            AutoHitTestMode = AutoHitTestModes.Ellipse;

            // Generate a random "sway offset" so that the balloons don't all sway
            // at the same angle
            _swayOffset = GameHelper.RandomNext(MathHelper.Pi * 2);

            // Randomize the appearance and position of the balloon
            Randomize();
        }



        //-------------------------------------------------------------------------------------
        // Game functions

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Move up the screen (speed based on the balloon scale)
            PositionY -= ScaleY * 4;

            // Have we passed the top of the screen?
            if (BoundingBox.Bottom < 0)
            {
                // Yes, so randomize the balloon to effectively create
                // it into a new balloon
                Randomize();
            }
        }

        /// <summary>
        /// Override the angle so that the balloon sways as it moves along
        /// </summary>
        public override float Angle
        {
            get
            {
                return (float)Math.Sin(UpdateCount / 15.0f + _swayOffset) / 5;
            }
        }

        //-------------------------------------------------------------------------------------
        // Object functions

        internal void Randomize()
        {
            // Set a layer depth between 0 (frontmost) and 1
            LayerDepth = GameHelper.RandomNext(1.0f);

            // Set the scale according to the layerdepth, so objects at the front are scaled larger
            // than those at the back
            Scale = new Vector2((1 - LayerDepth) * 0.4f + 0.2f);

            // Randomize the position so that it is below the bottom of the visible screen area
            Position = new Vector2(GameHelper.RandomNext(Game.GraphicsDevice.Viewport.Width),
                                   GameHelper.RandomNext(Game.GraphicsDevice.Viewport.Height)
                                            + Game.GraphicsDevice.Viewport.Height
                                            + BoundingBox.Height);

            // Randomize the color
            switch (GameHelper.RandomNext(5))
            {
                case 0: SpriteColor = Color.Blue; break;
                case 1: SpriteColor = Color.Red; break;
                case 2: SpriteColor = Color.Green; break;
                case 3: SpriteColor = Color.Yellow; break;
                case 4: SpriteColor = Color.Purple; break;
            }

            // Increment the balloon count
            _balloonCount += 1;
        }


    }
}
