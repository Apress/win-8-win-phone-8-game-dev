using GameFramework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace CosmicRocksPartII_Win8
{
    internal class RockObject : GameFramework.SpriteObject
    {


        // A strongly typed reference to the game
        private CosmicRocksPartIIGame _game;

        // The speed value passed to the constructor
        private float _constructorSpeed;
        // The actual speed at which the rock is falling
        private float _moveSpeed;
        // The direction of movement
        private Vector2 _direction;
        // The rate at which the rock is rotating
        private float _rotateSpeed;
        // The rock generation
        // This will be decremented by 1 each time the rock is hit and divides into two.
        // When it reaches zero, the rock will be destroyed.
        private int _generation;

        //-------------------------------------------------------------------------------------
        // Class constructors

        internal RockObject(CosmicRocksPartIIGame game, Texture2D texture, int generation, float size, float speed)
            : base(game, Vector2.Zero, texture)
        {
            // Store a strongly-typed reference to the game
            _game = game;

            // Set a random position
            PositionX = GameHelper.RandomNext(0, _game.GraphicsDevice.Viewport.Bounds.Width);
            PositionY = GameHelper.RandomNext(0, _game.GraphicsDevice.Viewport.Bounds.Height);

            // Set the origin
            Origin = new Vector2(texture.Width, texture.Height) / 2;

            // Store the other constructor parameters
            _constructorSpeed = speed;
            _generation = generation;

            // Initialize the remainder of the rock properties
            InitializeRock(size);
        }


        //-------------------------------------------------------------------------------------
        // Game functions

        public override void Update(GameTime gameTime)
        {
            // Allow the base class to do any work it needs
            base.Update(gameTime);

            // Update the position of the rock
            Position += _direction * _moveSpeed;

            // If we pass the edge of the window, reset to the opposite side
            if (BoundingBox.Bottom < _game.GraphicsDevice.Viewport.Bounds.Top && _direction.Y < 0)
            {
                PositionY = _game.GraphicsDevice.Viewport.Bounds.Height + SpriteTexture.Height;
            }
            if (BoundingBox.Top > _game.GraphicsDevice.Viewport.Bounds.Bottom && _direction.Y > 0)
            {
                PositionY = -SpriteTexture.Height;
            }
            if (BoundingBox.Right < _game.GraphicsDevice.Viewport.Bounds.Left && _direction.X < 0)
            {
                PositionX = _game.GraphicsDevice.Viewport.Bounds.Width + SpriteTexture.Width;
            }
            if (BoundingBox.Left > _game.GraphicsDevice.Viewport.Bounds.Right && _direction.X > 0)
            {
                PositionX = -SpriteTexture.Width;
            }

            // Rotate the rock
            Angle += MathHelper.ToRadians(_rotateSpeed);
        }

        //-------------------------------------------------------------------------------------
        // Rock functions

        /// <summary>
        /// Initialize and randomize the rock
        /// </summary>
        /// <param name="size"></param>
        /// <param name="speed"></param>
        private void InitializeRock(float size)
        {
            // Set the scale
            Scale = new Vector2(size, size);

            // Set a random movement speed for the rock
            _moveSpeed = GameHelper.RandomNext(_constructorSpeed) + _constructorSpeed;

            // Set a random rotation speed for the rock
            _rotateSpeed = GameHelper.RandomNext(-5.0f, 5.0f);

            // Create a random direction for the rock. Ensure that it doesn't have zero
            // as the direction on both the x and y axes
            do
            {
                _direction = new Vector2(GameHelper.RandomNext(-1.0f, 1.0f), GameHelper.RandomNext(-1.0f, 1.0f));
            } while (_direction == Vector2.Zero);
            // Normalize the movement vector so that it is exactly 1 unit in length
            _direction.Normalize();
        }

        /// <summary>
        /// The rock has been damaged so divide it or remove it from the game
        /// </summary>
        internal void DamageRock()
        {
            RockObject newRock;
            ParticleObject[] particleObjects;
            ParticleObject particleObj;

            // Add some particles for the rock dust.
            // First retrieve the particles from the game.
            particleObjects = _game.GetParticleObjects(5);
            // Loop for each object
            for (int i = 0; i < particleObjects.Length; i++)
            {
                particleObj = particleObjects[i];
                particleObj.ResetProperties(Position, _game.Textures["SmokeParticle"]);
                switch (GameHelper.RandomNext(3))
                {
                    case 0: particleObj.SpriteColor = Color.DarkGray; break;
                    case 1: particleObj.SpriteColor = Color.LightGray; break;
                    default: particleObj.SpriteColor = Color.White; break;
                }
                particleObj.Scale = new Vector2(0.4f, 0.4f);
                particleObj.IsActive = true;
                particleObj.Intensity = 255;
                particleObj.IntensityFadeAmount = 3 + GameHelper.RandomNext(1.5f);
                particleObj.Speed = GameHelper.RandomNext(3.0f);
                particleObj.Inertia = 0.9f + GameHelper.RandomNext(0.015f);
            }


            // Is this a generation-zero rock?
            // This is the smallest we get, so in this case the rock has actually been destroyed.
            if (_generation == 0)
            {
                // Remove the rock from the game
                _game.GameObjects.Remove(this);
            }
            else
            {
                // Re-initialize the rock at half its current size
                InitializeRock(ScaleX * 0.7f);
                // Decrease the generation value
                _generation -= 1;

                // Create another rock alongside this one
                newRock = new RockObject(_game, SpriteTexture, _generation, ScaleX, _constructorSpeed);
                // Position the new rock exactly on top of this rock
                newRock.Position = Position;
                // Add the new rock to the game
                _game.GameObjects.Add(newRock);
            }
        }


    }
}
