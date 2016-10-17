using GameFramework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace MultipleModes_WP8
{
    internal class SquareObject : SpriteObject
    {

        //-------------------------------------------------------------------------------------
        // Class-level variables

        // A strongly typed reference to the game
        private MultipleModesGame _game;

        // The speed at which the box is falling
        private float _moveSpeed;
        // The rate at which the box is rotating
        private float _rotateSpeed;
        // A speed multiplier used to ramp the speed up gradually to the set values
        private float _moveSpeedMultiplier;

        //-------------------------------------------------------------------------------------
        // Class constructors

        internal SquareObject(MultipleModesGame game, Texture2D texture)
            : base(game, Vector2.Zero, texture)
        {
            // Store a strongly-typed reference to the game
            _game = game;

            // Set a random position
            PositionX = GameHelper.RandomNext(0, _game.GraphicsDevice.Viewport.Bounds.Width);
            PositionY = GameHelper.RandomNext(0, _game.GraphicsDevice.Viewport.Bounds.Height);

            // Set the origin
            Origin = new Vector2(texture.Width, texture.Height) / 2;

            // Set a random movement speed
            _moveSpeed = GameHelper.RandomNext(2.0f) + 2;

            // Set a random rotation speed
            _rotateSpeed = GameHelper.RandomNext(-5.0f, 5.0f);

            // Set the speed multiplier to zero so that the objects gradually accelerate to their set speed
            _moveSpeedMultiplier = 0;

            Scale = new Vector2(0.3f);
        }


        //-------------------------------------------------------------------------------------
        // Game functions

        public override void Update(GameTime gameTime)
        {
            // Allow the base class to do any work it needs
            base.Update(gameTime);

            // Update the position of the box
            PositionY += (_moveSpeed * _moveSpeedMultiplier);
            // If we pass the bottom of the window, reset back to the top
            if (BoundingBox.Top > _game.GraphicsDevice.Viewport.Bounds.Bottom)
            {
                PositionY = -SpriteTexture.Height;
            }

            // Rotate the box
            Angle += MathHelper.ToRadians(_rotateSpeed * _moveSpeedMultiplier);

            // Ramp the speed multiplier up to full speed
            _moveSpeedMultiplier += 0.005f;
            if (_moveSpeedMultiplier > 1) _moveSpeedMultiplier = 1;

        }

    }
}
