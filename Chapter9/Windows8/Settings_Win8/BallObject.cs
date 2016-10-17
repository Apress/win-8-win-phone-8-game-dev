using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using GameFramework;
using System;

namespace Settings_Win8
{
    internal class BallObject : SpriteObject
    {

        //-------------------------------------------------------------------------------------
        // Class-level variables

        // A strongly typed reference to the game
        private SettingsGame _game;

        // The ball's current x and y velocity
        private float _xadd;
        private float _yadd;
        // The level of "wobble" applied to the ball
        private float _wobble;

        //-------------------------------------------------------------------------------------
        // Class constructors

        internal BallObject(SettingsGame game, Texture2D texture, float speed)
            : base(game, Vector2.Zero, texture)
        {
            // Store a strongly-typed reference to the game
            _game = game;

            // Set a random position
            PositionX = GameHelper.RandomNext(0, _game.GraphicsDevice.Viewport.Width);
            PositionY = GameHelper.RandomNext(0, _game.GraphicsDevice.Viewport.Height);

            // Set the origin
            Origin = new Vector2(texture.Width, texture.Height) / 2;

            // Set a random color
            SpriteColor = new Color(GameHelper.RandomNext(0, 256), GameHelper.RandomNext(0, 256), GameHelper.RandomNext(0, 256));

            // Store the movement speed
            Speed = speed;

            // Set a horizontal movement speed for the box
            _xadd = GameHelper.RandomNext(-5.0f, 5.0f);
        }

        //-------------------------------------------------------------------------------------
        // Properties

        public float Speed { get; set; }

        //-------------------------------------------------------------------------------------
        // Game functions

        public override void Update(GameTime gameTime)
        {
            // Allow the base class to do any work it needs
            base.Update(gameTime);

            // Update the position of the ball
            PositionX += _xadd * Speed;
            PositionY += _yadd * Speed;

            // If we reach the side of the window, reverse the x velocity so that the ball bounces back
            if (PositionX < OriginX)
            {
                // Reset back to the left edge
                PositionX = OriginX;
                // Reverse the x velocity
                _xadd = -_xadd;
                // Add to the wobble
                _wobble += Math.Abs(_xadd);
            }
            if (PositionX > _game.GraphicsDevice.Viewport.Width - OriginX)
            {
                // Reset back to the right edge
                PositionX = _game.GraphicsDevice.Viewport.Width - OriginX;
                // Reverse the x velocity
                _xadd = -_xadd;
                // Add to the wobble
                _wobble += Math.Abs(_xadd);
            }

            // If we reach the bottom of the window, reverse the y velocity so that the ball bounces upwards
            if (PositionY >= _game.GraphicsDevice.Viewport.Height - OriginY)
            {
                // Reset back to the bottom of the window
                PositionY = _game.GraphicsDevice.Viewport.Height - OriginY;
                // Reverse the y-velocity
                _yadd = -_yadd; // +0.3f;
                // Add to the wobble
                _wobble += Math.Abs(_yadd);
            }
            else
            {
                // Increase the y velocity to simulate gravity
                _yadd += 0.3f;
            }

            // Is there any wobble?
            if (_wobble == 0)
            {
                // No, so reset the scale
                Scale = Vector2.One;
            }
            else
            {
                const float WobbleSpeed = 20.0f;
                const float WobbleIntensity = 0.015f;

                // Yes, so calculate the scaling on the x and y axes
                ScaleX = (float)Math.Sin(MathHelper.ToRadians(UpdateCount * WobbleSpeed)) * _wobble * WobbleIntensity + 1;
                ScaleY = (float)Math.Sin(MathHelper.ToRadians(UpdateCount * WobbleSpeed + 180.0f)) * _wobble * WobbleIntensity + 1;
                // Reduce the wobble level
                _wobble -= 0.2f;
                // Don't allow the wobble to fall below zero or to rise too high
                if (_wobble < 0) _wobble = 0;
                if (_wobble > 50) _wobble = 50;
            }
        }

    }
}
