using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameFramework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;

namespace GesturesDemo_WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GesturesDemoGame : GameHost
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        public GesturesDemoGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load content
            Textures.Add("Tap", Content.Load<Texture2D>("Tap"));
            Textures.Add("DoubleTap", Content.Load<Texture2D>("DoubleTap"));
            Textures.Add("Hold", Content.Load<Texture2D>("Hold"));
            Textures.Add("VerticalDrag", Content.Load<Texture2D>("VerticalDrag"));
            Textures.Add("HorizontalDrag", Content.Load<Texture2D>("HorizontalDrag"));
            Textures.Add("FreeDrag", Content.Load<Texture2D>("FreeDrag"));
            Textures.Add("Flick", Content.Load<Texture2D>("Flick"));
            Textures.Add("Pinch", Content.Load<Texture2D>("Pinch"));
            Textures.Add("Pinch2", Content.Load<Texture2D>("Pinch2"));

            ResetGame();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            SpriteObject gestureSprite;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) this.Exit();

            // Update all game objects
            UpdateAll(gameTime);

            while (TouchPanel.IsGestureAvailable)
            {
                // Read the next gesture
                GestureSample gesture = TouchPanel.ReadGesture();

                // Get a typed reference to the first game object
                gestureSprite = (SpriteObject)GameObjects[0];
                // Set the sprite position (except for Flick, as this has no position)
                if (gesture.GestureType != GestureType.Flick)
                {
                    gestureSprite.Position = gesture.Position - new Vector2(50, 50);
                }
                // Scale to full size
                gestureSprite.Scale = Vector2.One;
                // Set the texture to match the gesture type
                if (Textures.ContainsKey(gesture.GestureType.ToString()))
                {
                    gestureSprite.SpriteTexture = Textures[gesture.GestureType.ToString()];
                }

                // Is this a pinch?
                if (gesture.GestureType == GestureType.Pinch)
                {
                    // Yes, so set the size, texture and position of the second sprite too.
                    // Get a typed reference to the second game object
                    gestureSprite = (SpriteObject)GameObjects[1];
                    gestureSprite.Position = gesture.Position2 - new Vector2(50, 50);
                    // Scale to full size
                    gestureSprite.Scale = Vector2.One;
                    // Set the texture
                    gestureSprite.SpriteTexture = Textures["Pinch2"];
                }
            }

            // Reduce the scale of all objects so that they fade away
            for (int i = 0; i < GameObjects.Count; i++)
            {
                // Is the object at this position a sprite (and not text)?
                if (GameObjects[i] is SpriteObject && !(GameObjects[i] is TextObject))
                {
                    // Yes, so scale it down a little
                    ((SpriteObject)GameObjects[i]).Scale *= 0.95f;
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            DrawSprites(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Reset and initialize the game
        /// </summary>
        private void ResetGame()
        {
            SpriteObject gestureObj;

            // Clear any existing game objects
            GameObjects.Clear();

            // Add two game objects to show the current active gesture
            // (including the second point for pinch gestures)
            for (int i = 0; i < 2; i++)
            {
                // Create a sprite object
                gestureObj = new SpriteObject(this, Vector2.Zero, Textures["Tap"]);
                // Origin at the middle of the sprite
                gestureObj.Origin = new Vector2(gestureObj.SpriteTexture.Width / 2, gestureObj.SpriteTexture.Height / 2);
                // Set zero scale so that the object is infinitely small
                gestureObj.Scale = Vector2.Zero;
                // Add to the game
                GameObjects.Add(gestureObj);
            }

            // Enable the gestures that we want to be able to respond to
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.DoubleTap | GestureType.FreeDrag | GestureType.Flick | GestureType.Hold | GestureType.Pinch;
        }

    }
}
