using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace AccelerometerDemo_Win8
{
    internal class BallObject : SpriteObject
    {

        private AccelerometerGame _game;

        //-------------------------------------------------------------------------------------
        // Class constructors

        internal BallObject(AccelerometerGame game, Vector2 position, Texture2D texture)
            : base(game, position, texture)
        {
            // Store a strongly typed reference to the game
            _game = game;

            // Set the origin to the middle of the sprite
            Origin = new Vector2(SpriteTexture.Width, SpriteTexture.Height) / 2;
        }


        //-------------------------------------------------------------------------------------
        // Class properties

        public Vector2 Velocity { get; set; }

        //-------------------------------------------------------------------------------------
        // Object functions

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Add the accelerometer vector to the velocity
            Velocity += new Vector2(_game.AccelerometerData.X, -_game.AccelerometerData.Y);

            // Add the velocity to the position
            Position += Velocity;

            // Bounce off the edges of the window
            if (BoundingBox.Right >= Game.GraphicsDevice.Viewport.Bounds.Right && Velocity.X > 0)
            {
                PositionX = Game.GraphicsDevice.Viewport.Bounds.Right - SpriteTexture.Width / 2;
                Velocity = new Vector2(Velocity.X * -0.5f, Velocity.Y);
            }
            if (BoundingBox.Left <= 0 && Velocity.X < 0)
            {
                PositionX = SpriteTexture.Width / 2;
                Velocity = new Vector2(Velocity.X * -0.5f, Velocity.Y);
            }
            if (BoundingBox.Bottom >= Game.GraphicsDevice.Viewport.Bounds.Bottom && Velocity.Y > 0)
            {
                PositionY = Game.GraphicsDevice.Viewport.Bounds.Bottom - SpriteTexture.Height / 2;
                Velocity = new Vector2(Velocity.X, Velocity.Y * -0.5f);
            }
            if (BoundingBox.Top <= 0 && Velocity.Y < 0)
            {
                PositionY = SpriteTexture.Height / 2;
                Velocity = new Vector2(Velocity.X, Velocity.Y * -0.5f);
            }
        }




    }
}
