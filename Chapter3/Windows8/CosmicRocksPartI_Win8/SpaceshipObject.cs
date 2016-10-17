using GameFramework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace CosmicRocksPartI_Win8
{
    internal class SpaceshipObject : GameFramework.SpriteObject
    {

        // A strongly typed reference to the game
        private CosmicRocksPartIGame _game;


        //-------------------------------------------------------------------------------------
        // Class constructors

        internal SpaceshipObject(CosmicRocksPartIGame game, Texture2D texture, Vector2 position)
            : base(game, position, texture)
        {
            // Store a strongly-typed reference to the game
            _game = game;

            // Set the origin
            Origin = new Vector2(texture.Width, texture.Height) / 2;

            // Set the scale
            Scale = new Vector2(0.2f, 0.2f);

            // We are not (yet) exploding
            ExplosionUpdateCount = 0;
        }

        //-------------------------------------------------------------------------------------
        // Properties

        /// <summary>
        /// Is the spaceship currently exploding?
        /// </summary>
        internal bool IsExploding
        {
            get { return (ExplosionUpdateCount > 0); }
        }

        /// <summary>
        /// How many updates are left before the current explosion finishes?
        /// </summary>
        private int ExplosionUpdateCount { get; set; }



        //-------------------------------------------------------------------------------------
        // Game functions


        public override void Update(GameTime gameTime)
        {
            SpriteObject collisionObj;

            // Are we currently alive?
            if (IsExploding)
            {
                // We are currently exploding. Reduce the update count
                ExplosionUpdateCount -= 1;
            }
            else
            {
                // We are alive. See if we have hit anything...
                collisionObj = HasCollided();

                // Did we hit a rock?
                if (collisionObj is RockObject)
                {
                    // We have collided with a rock.
                    ((RockObject)collisionObj).DamageRock();
                }

                // Did we hit anything at all?
                if (collisionObj != null)
                {
                    // We are now exploding
                    Explode();
                }
            }
            
            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsExploding) base.Draw(gameTime, spriteBatch);
        }




        //-------------------------------------------------------------------------------------
        // Spaceship functions

        /// <summary>
        /// Determine if the ship has collided with anything hostile
        /// </summary>
        /// <returns></returns>
        internal SpriteObject HasCollided()
        {
            SpriteObject spriteObj;
            Rectangle shipBox;
            float shipSize;
            float objectSize;
            float objectDistance;

            // Retrieve the ship's bounding rectangle.
            // Getting this just once reduces the workload as we will
            // be using it multiple times.
            shipBox = BoundingBox;

            // Calculate the distance from the center of the ship to the
            // edge of its bounding circle. This is the texture size
            // (we'll use the width) divided by two (as the origin is in
            // the middle) multiplied by the sprite scale.
            shipSize = SpriteTexture.Width / 2.0f * ScaleX;

            foreach (GameObjectBase gameObj in _game.GameObjects)
            {
                // Is this a space rock?
                if (gameObj is RockObject)
                {
                    // It is... Does its bounding rectangle intersect with the spaceship?
                    spriteObj = (SpriteObject)gameObj;
                    if (spriteObj.BoundingBox.Intersects(shipBox))
                    {
                        // It does.. See if the distance is small enough for them to collide.
                        // First calculate the size of the object
                        objectSize = spriteObj.SpriteTexture.Width / 2.0f * spriteObj.ScaleX;
                        // Find the distance between the two points
                        objectDistance = Vector2.Distance(Position, spriteObj.Position);
                        // Is this less than the combined object sizes?
                        if (objectDistance < shipSize + objectSize)
                        {
                            // Yes, so we have collided
                            return spriteObj;
                        }
                    }
                }
            }

            // The ship hasn't hit anything
            return null;
        }

        /// <summary>
        /// Set the ship to explode. Call when something collides with the ship.
        /// </summary>
        private void Explode()
        {
            ParticleObject[] particleObjects;
            ParticleObject particleObj;

            // Set a delay of 120 updates before the ship returns to play
            ExplosionUpdateCount = 120;

            // Add some particles for the explosion.
            // First retrieve the particles from the game.
            particleObjects = _game.GetParticleObjects(150);
            // Loop for each object
            for (int i = 0; i < particleObjects.Length; i++)
            {
                particleObj = particleObjects[i];
                particleObj.ResetProperties(Position, _game.Textures["SmokeParticle"]);
                switch (GameHelper.RandomNext(4))
                {
                    case 0: particleObj.SpriteColor = Color.Yellow; break;
                    case 1: particleObj.SpriteColor = Color.Orange; break;
                    case 2: particleObj.SpriteColor = Color.Red; break;
                    default: particleObj.SpriteColor = Color.White; break;
                }
                particleObj.Scale = new Vector2(0.5f, 0.5f);
                particleObj.IsActive = true;
                particleObj.Intensity = 255;
                particleObj.IntensityFadeAmount = 2 + GameHelper.RandomNext(1.5f);
                particleObj.Speed = GameHelper.RandomNext(5.0f);
                particleObj.Inertia = 0.9f + GameHelper.RandomNext(0.015f);
            }
        }


    }
}
