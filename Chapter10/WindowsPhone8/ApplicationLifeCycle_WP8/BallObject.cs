using GameFramework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace ApplicationLifeCycle_WP8
{
    internal class BallObject : SpriteObject
    {

        //-------------------------------------------------------------------------------------
        // Class-level variables

        // A strongly typed reference to the game
        private ApplicationLifeCycleGame _game;

        // The ball's current x and y velocity
        private float _xadd;
        private float _yadd;

        //-------------------------------------------------------------------------------------
        // Class constructors

        internal BallObject(ApplicationLifeCycleGame game, Texture2D texture)
            : base(game, Vector2.Zero, texture)
        {
            // Store a strongly-typed reference to the game
            _game = game;

            // Set a random position
            PositionX = GameHelper.RandomNext(0, _game.GraphicsDevice.Viewport.Bounds.Width);
            PositionY = GameHelper.RandomNext(0, _game.GraphicsDevice.Viewport.Bounds.Height);

            // Set the origin
            Origin = new Vector2(texture.Width, texture.Height) / 2;

            // Set a random color
            SpriteColor = new Color(GameHelper.RandomNext(0, 256), GameHelper.RandomNext(0, 256), GameHelper.RandomNext(0, 256));

            // Set a horizontal movement speed for the box
            _xadd = GameHelper.RandomNext(-5.0f, 5.0f);
        }


        //-------------------------------------------------------------------------------------
        // Game functions

        public override void Update(GameTime gameTime)
        {
            // Allow the base class to do any work it needs
            base.Update(gameTime);

            // Update the position of the ball
            PositionX += _xadd;
            PositionY += _yadd;

            // If we reach the side of the window, reverse the x velocity so that the ball bounces back
            if (PositionX < OriginX)
            {
                // Reset back to the left edge
                PositionX = OriginX;
                // Reverse the x velocity
                _xadd = -_xadd;
            }
            if (PositionX > _game.GraphicsDevice.Viewport.Bounds.Width - OriginX)
            {
                // Reset back to the right edge
                PositionX = _game.GraphicsDevice.Viewport.Bounds.Width - OriginX;
                // Reverse the x velocity
                _xadd = -_xadd;
            }

            // If we reach the bottom of the window, reverse the y velocity so that the ball bounces upwards
            if (PositionY >= _game.GraphicsDevice.Viewport.Bounds.Bottom - OriginY)
            {
                // Reset back to the bottom of the window
                PositionY = _game.GraphicsDevice.Viewport.Bounds.Bottom - OriginY;
                // Reverse the y-velocity
                _yadd = -_yadd; // +0.3f;
            }
            else
            {
                // Increase the y velocity to simulate gravity
                _yadd += 0.3f;
            }
        }

    }
}
