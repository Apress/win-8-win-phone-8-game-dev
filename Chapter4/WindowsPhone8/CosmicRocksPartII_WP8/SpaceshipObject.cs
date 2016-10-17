using GameFramework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;

namespace CosmicRocksPartII_WP8
{
    internal class SpaceshipObject : GameFramework.SpriteObject
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        // A strongly typed reference to the game
        private CosmicRocksPartIIGame _game;

        // The angle towards which we are rotating (as set by the last user touch)
        private float _targetAngle;

        // The current ship velocity
        private Vector2 _velocity;

        // The time at which the last touch point was established.
        // When this is held for long enough the ship will start to thrust.
        private DateTime _holdTime;

        // Track whether the fire button was pressed during the last update
        private KeyboardState _lastKeyboardState;

        // Variables controlling hyperspace
        private int _hyperspaceZoom;
        private int _hyperspaceZoomAdd;

        // Shield time -- while this is counting down to zero, the player is invulnerable
        private int _shieldTime;
        private const int MaxShieldTime = 100;

        // How many bullets can the player have on screen at once?
        private const int MaxBullets = 4;

        // How fast does the ship rotate with the keyboard/gamepad?
        private const float RotationSpeed = 7;

        //-------------------------------------------------------------------------------------
        // Class constructors

        internal SpaceshipObject(CosmicRocksPartIIGame game, Texture2D texture, Vector2 position)
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

            // Activate the shield
            _shieldTime = MaxShieldTime;
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


        // If the player is hyperspacing, zoom the spaceship to indicate this
        public override float ScaleX
        {
            get { return base.ScaleX + (_hyperspaceZoom * 0.02f); }
        }
        // If the player is hyperspacing, zoom the spaceship to indicate this
        public override float ScaleY
        {
            get { return base.ScaleX + (_hyperspaceZoom * 0.02f); }
        }

        // If the player is hyperspacing, fade out the spaceship to indicate this
        public override Color SpriteColor
        {
            get
            {
                Color ret = base.SpriteColor;
                ret.A = (byte)MathHelper.Clamp(255 - _hyperspaceZoom * 2.5f, 0, 255);
                return ret;
            }
        }

        //-------------------------------------------------------------------------------------
        // Game functions

        /// <summary>
        /// Update the ship and process player inputs
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            SpriteObject collisionObj;

            base.Update(gameTime);

            // Are we currently alive?
            if (IsExploding)
            {
                // We are currently exploding. Reduce the update count
                ExplosionUpdateCount -= 1;
                // If the explosion has finished, reset the ship
                if (ExplosionUpdateCount == 0)
                {
                    // Reset the ship
                    Position = new Vector2(_game.GraphicsDevice.Viewport.Bounds.Width, _game.GraphicsDevice.Viewport.Bounds.Height) / 2;
                    Angle = 0;
                    _targetAngle = 0;
                    _shieldTime = MaxShieldTime;
                    _velocity = Vector2.Zero;
                    _hyperspaceZoom = 0;
                    _hyperspaceZoomAdd = 0;
                }
                // Discard any queued gestures and then return
                while (TouchPanel.IsGestureAvailable) { TouchPanel.ReadGesture(); }
                // No more to do until the explosion is finished.
                return;
            }

            // Are we hyperspacing?
            if (_hyperspaceZoomAdd != 0)
            {
                // Add to the zoom
                _hyperspaceZoom += _hyperspaceZoomAdd;

                // Have we reached maximum zoom?
                if (_hyperspaceZoom >= 150)
                {
                    // Yes, so move to the new location
                    // Start to zoom back out
                    _hyperspaceZoomAdd = -_hyperspaceZoomAdd;
                    // Set a random new position
                    PositionX = GameHelper.RandomNext(0, _game.GraphicsDevice.Viewport.Bounds.Width - SpriteTexture.Width) + SpriteTexture.Width / 2;
                    PositionY = GameHelper.RandomNext(0, _game.GraphicsDevice.Viewport.Bounds.Height - SpriteTexture.Height) + SpriteTexture.Height / 2;
                }
                // Have we finished hyperspacing?
                if (_hyperspaceZoom <= 0)
                {
                    // Yes, so cancel the hyperspace variables
                    _hyperspaceZoom = 0;
                    _hyperspaceZoomAdd = 0;
                    // Stop movement
                    _velocity = Vector2.Zero;
                }
                // Discard any queued gestures and then return
                while (TouchPanel.IsGestureAvailable) { TouchPanel.ReadGesture(); }
                // Don't allow any other updates while hyperspacing
                return;
            }

            // Move the spaceship
            Position += _velocity;

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

            // Rotate towards the target angle
            if (Angle != _targetAngle)
            {
                Angle += (_targetAngle - Angle) * 0.2f;
            }

            // See if we have hit anything...
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

            // Reduce the shield time
            if (_shieldTime > 0) _shieldTime -= 1;

            // Process touch input
            Update_ProcessTouchInput();
        }


        private void Update_ProcessTouchInput()
        {
            // Is the player tapping the screen?
            TouchCollection tc = TouchPanel.GetState();
            if (tc.Count == 1)
            {
                // Has the first touch point just been touched?
                if (tc[0].State == TouchLocationState.Pressed)
                {
                    // Yes, so rotate to this position and fire
                    RotateToFacePoint(tc[0].Position);
                    // Shoot a bullet in the current direction
                    FireBullet();
                    // Note the time so we can detect held contact
                    _holdTime = DateTime.Now;
                }
                if (tc[0].State == TouchLocationState.Moved)
                {
                    // Has sufficient time passed to start thrusting?
                    if (DateTime.Now.Subtract(_holdTime).TotalMilliseconds > 300)
                    {
                        // Yes, so thrust towards this position
                        RotateToFacePoint(tc[0].Position);
                        Thrust();
                    }
                }
            }

            // Is the player pinching?
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();
                switch (gesture.GestureType)
                {
                    case GestureType.Pinch:
                        Hyperspace();
                        break;
                }
            }

        }

        private void Update_ProcessKeyboardInput()
        {
            // Read the gamepad state
            KeyboardState keyboardState = Keyboard.GetState();

            // If the left/right cursor key has been pressed, rotate the ship at a fixed speed.
            // The value here can be varied to change how quickly the ship rotates.
            if (keyboardState.IsKeyDown(Keys.Left)) _targetAngle -= MathHelper.ToRadians(RotationSpeed);
            if (keyboardState.IsKeyDown(Keys.Right)) _targetAngle -= MathHelper.ToRadians(-RotationSpeed);

            // If the up cursor key has been pressed, thrust the ship
            if (keyboardState.IsKeyDown(Keys.Up)) Thrust();

            // If the player is pressing Space, fire a bullet
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                // Was the first button pressed during the last update too? If so, ignore this button press
                if (!_lastKeyboardState.IsKeyDown(Keys.Space))
                {
                    // Fire a bullet
                    FireBullet();
                }
            }

            // If the player is pressing the Ctrl key, hyperspace
            if (keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.RightControl)) Hyperspace();

            // Remember the keyboard state for next time so that we can detect key down/up events
            _lastKeyboardState = keyboardState;
        }


        /// <summary>
        /// Draw the ship
        /// </summary>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // No ship to draw if it is exploding
            if (IsExploding) return;
            // If the shield is active, draw only alternate updates
            if (_shieldTime > 0 && _shieldTime % 2 == 0) return;
            
            // Draw the ship as normal
            base.Draw(gameTime, spriteBatch);
        }




        //-------------------------------------------------------------------------------------
        // Spaceship functions

        /// <summary>
        /// Calculate the target angle towards which the ship needs to rotate
        /// in order to face the specified point.
        /// </summary>
        /// <param name="point"></param>
        private void RotateToFacePoint(Vector2 point)
        {
            // Find the angle between the spaceship and the specified point.
            // First find the position of the point relative to the position of the spaceship.
            point -= Position;

            // Is the point is exactly on the spaceship, ignore the touch
            if (point == Vector2.Zero) return;

            // Ensure that the current angle is between 0 and 2 PI
            while (Angle < 0) { Angle += MathHelper.TwoPi; }
            while (Angle > MathHelper.TwoPi) { Angle -= MathHelper.TwoPi; }
            // Get the current angle in degrees
            float angleDegrees;
            angleDegrees = MathHelper.ToDegrees(Angle);

            // Calculate the angle between the ship and the touch point, convert to degrees
            float targetAngleDegrees;
            targetAngleDegrees = MathHelper.ToDegrees((float)Math.Atan2(point.Y, point.X));
            // MonoGame puts 0 degrees upwards, whereas Atan2 returns it facing left, so add 90 degrees
            // to rotate the Atan2 value into alignment with MonoGame
            targetAngleDegrees += 90;
            // Atan2 returns values between -180 and +180, so having added 90 degrees we now
            // have a value in the range -90 to +270. In case we are less than zero, add
            // 360 to get an angle in the range 0 to 360.
            if (targetAngleDegrees < 0) targetAngleDegrees += 360;

            // Is the target angle over 180 degrees less than the current angle?
            if (targetAngleDegrees < angleDegrees - 180)
            {
                // Yes, so instead of rotating the whole way around to the left,
                // rotate the smaller distance to the right instead.
                targetAngleDegrees += 360;
            }
            // Is the target angle over 180 degrees more than the current angle?
            if (targetAngleDegrees > angleDegrees + 180)
            {
                // Yes, so instead of rotating the whole way around to the right,
                // rotate the smaller distance to the left instead.
                targetAngleDegrees -= 360;
            }

            // Store the calculated angle, converted back to radians
            _targetAngle = MathHelper.ToRadians(targetAngleDegrees);
        }


        /// <summary>
        /// Fire a bullet in the direction the ship is currently facing
        /// </summary>
        private void FireBullet()
        {
            BulletObject bulletObj;

            // Try to obtain a bullet object to shoot
            bulletObj = GetBulletObject();
            // Did we find one?
            if (bulletObj == null)
            {
                // No, so we can't shoot at the moment
                return;
            }

            // Initialize the bullet with our own position and angle
            bulletObj.InitializeBullet(Position, Angle);
        }

        /// <summary>
        /// Find (or create) a bullet object to fire.
        /// </summary>
        /// <returns>Returns an object if one is available, of null
        /// if there are no more bullets available at the current time.</returns>
        private BulletObject GetBulletObject()
        {
            int objectCount;
            int bulletCount = 0;
            GameObjectBase gameObj;
            BulletObject bulletObj = null;

            // Look for an inactive bullet
            objectCount = _game.GameObjects.Count;
            for (int i = 0; i < objectCount; i++)
            {
                // Get a reference to the object at this position
                gameObj = _game.GameObjects[i];
                // Is this object a bullet?
                if (gameObj is BulletObject)
                {
                    // Count the number of bullets found
                    bulletCount += 1;
                    // Is it inactive?
                    if (((BulletObject)gameObj).IsActive == false)
                    {
                        // Yes, so re-use this bullet
                        return (BulletObject)gameObj;
                    }
                }
            }

            // Did we find a bullet?
            if (bulletObj == null)
            {
                // No, do we have capacity to add a new bullet?
                if (bulletCount < MaxBullets)
                {
                    // Yes, so create a new bullet
                    bulletObj = new BulletObject(_game, _game.Textures["Bullet"]);
                    _game.GameObjects.Add(bulletObj);
                    return bulletObj;
                }
            }

            // No more bullets available
            return null;
        }

        /// <summary>
        /// Applies thrust to the ship in its current direction
        /// </summary>
        /// <param name="targetPosition"></param>
        private void Thrust()
        {
            Vector2 shipFacing;
            ParticleObject[] particleObjects;
            ParticleObject particleObj;

            // Calculate the vector towards which the ship is facing
            shipFacing = new Vector2((float)Math.Sin(Angle), (float)-Math.Cos(Angle));
            // Scale down and add to the velocity
            _velocity += shipFacing / 10;
            // Stop the ship from traveling too fast
            if (_velocity.Length() > 25)
            {
                _velocity.Normalize();
                _velocity *= 25;
            }

            // Add some particles for the thrust exhaust.
            // First retrieve the particles from the game.
            particleObjects = _game.GetParticleObjects(10);
            // Loop for each object
            for (int i = 0; i < particleObjects.Length; i++)
            {
                particleObj = particleObjects[i];
                particleObj.ResetProperties(Position, _game.Textures["SmokeParticle"]);
                // Set to the rear of the spaceship
                particleObj.Position -= shipFacing * SpriteTexture.Width / 2 * ScaleX;
                switch (GameHelper.RandomNext(3))
                {
                    case 0: particleObj.SpriteColor = Color.White; break;
                    case 1: particleObj.SpriteColor = Color.Cyan; break;
                    case 2: particleObj.SpriteColor = Color.Blue; break;
                }
                particleObj.Scale = new Vector2(0.25f, 0.25f);
                particleObj.IsActive = true;
                particleObj.Intensity = 50;
                particleObj.IntensityFadeAmount = 3 + GameHelper.RandomNext(1.5f);
                particleObj.Speed = GameHelper.RandomNext(3.0f);
                particleObj.Inertia = 0.9f + GameHelper.RandomNext(0.015f);
                particleObj.Direction += -shipFacing * 5;
            }
        }

        /// <summary>
        /// Initiates hyperspace
        /// </summary>
        private void Hyperspace()
        {
            // Cancel any shield that may be active
            _shieldTime = 0;
            // Initiate the hyperspace by setting the zoom add
            _hyperspaceZoomAdd = 5;
        }

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

            // If the shield is active, we haven't collided with anything
            if (_shieldTime > 0) return null;

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
                particleObj.Direction += _velocity / 3;
            }
        }


    }
}
