using GameFramework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace CosmicRocksPartII_Win8
{
    class BulletObject : GameFramework.SpriteObject
    {

        // A strongly typed reference to the game
        private CosmicRocksPartIIGame _game;

        private Vector2 _velocity;
        private int _updates;

        //-------------------------------------------------------------------------------------
        // Class constructors and initialization functions

        internal BulletObject(CosmicRocksPartIIGame game, Texture2D texture)
            : base(game, Vector2.Zero, texture)
        {
            // Store a strongly-typed reference to the game
            _game = game;
            // Set the origin to the top-center of the sprite
            Origin = new Vector2(texture.Width / 2, 0);
        }

        /// <summary>
        /// Reset the properties of the bullet ready to move from the specified
        /// position in the specified direction.
        /// </summary>
        internal void InitializeBullet(Vector2 Position, float Angle)
        {
            // Initialize the bullet properties
            this.Position = Position;
            this.Angle = Angle;

            // Calculate the velocity vector for the bullet
            _velocity = new Vector2((float)Math.Sin(Angle), -(float)Math.Cos(Angle));

            // Mark the bullet as active
            IsActive = true;
            // Reset its update count
            _updates = 0;
        }

        //-------------------------------------------------------------------------------------
        // Object properties

        public bool IsActive { get; set; }

        
        //-------------------------------------------------------------------------------------
        // Game functions

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Only update if we are active
            if (!IsActive) return;

            // Add the velocity to the position
            Position += _velocity * 10;

            // Have we hit a rock?
            CheckForCollision();

            // If we pass the edge of the window, reset to the opposite side
            if (BoundingBox.Bottom < _game.GraphicsDevice.Viewport.Bounds.Top && _velocity.Y < 0)
            {
                PositionY = _game.GraphicsDevice.Viewport.Bounds.Height + SpriteTexture.Height;
            }
            if (BoundingBox.Top > _game.GraphicsDevice.Viewport.Bounds.Bottom && _velocity.Y > 0)
            {
                PositionY = -SpriteTexture.Height;
            }
            if (BoundingBox.Right < _game.GraphicsDevice.Viewport.Bounds.Left && _velocity.X < 0)
            {
                PositionX = _game.GraphicsDevice.Viewport.Bounds.Width + SpriteTexture.Width;
            }
            if (BoundingBox.Left > _game.GraphicsDevice.Viewport.Bounds.Right && _velocity.X > 0)
            {
                PositionX = -SpriteTexture.Width;
            }

            // See if we have updated enough times to run out of energy and disappear
            _updates += 1;
            if (_updates > 40) IsActive = false;
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Only draw it we are active
            if (!IsActive) return;

            // Draw the sprite
            base.Draw(gameTime, spriteBatch);
        }

        //-------------------------------------------------------------------------------------
        // Bullet functions

        private void CheckForCollision()
        {
            int objectCount;
            GameObjectBase gameObj;
            RockObject rockObj;
            float rockSize;
            float rockDistance;

            // Loop backwards through the rocks as we may modify the collection when a rock is destroyed
            objectCount = _game.GameObjects.Count;
            for (int i = objectCount - 1; i >= 0;  i--)
            {
                // Get a reference to the object at this position
                gameObj = _game.GameObjects[i];
                // Is this a space rock?
                if (gameObj is RockObject)
                {
                    // It is... Does its bounding rectangle contain the bullet position?
                    rockObj = (RockObject)gameObj;
                    if (rockObj.BoundingBox.Contains((int)PositionX, (int)PositionY))
                    {
                        // It does.. See if the distance is small enough for them to collide.
                        // First calculate the size of the object
                        rockSize = rockObj.SpriteTexture.Width / 2.0f * rockObj.ScaleX;
                        // Find the distance between the two points
                        rockDistance = Vector2.Distance(Position, rockObj.Position);
                        // Is the distance less than the rock size?
                        if (rockDistance < rockSize)
                        {
                            // Yes, so we have hit the rock
                            rockObj.DamageRock();
                            // Destroy the bullet
                            IsActive = false;
                        }
                    }
                }
            }
        }



    }
}
