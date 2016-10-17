using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameFramework;

namespace KeyboardDemo_WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class KeyboardGame : GameHost
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        // Store the keyboard state from the previous update.
        private KeyboardState _lastKeyState;

        public KeyboardGame()
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

            Textures.Add("Circle", Content.Load<Texture2D>("Circle"));

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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) this.Exit();

            KeyboardState currentKeyState;

            UpdateAll(gameTime);

            // Read the current keyboard state
            currentKeyState = Keyboard.GetState();

            // Get a strongly-typed reference to our sprite
            SpriteObject sprite = (SpriteObject)GameObjects[0];

            // Move the sprite?
            if (currentKeyState.IsKeyDown(Keys.Up)) sprite.PositionY -= 5;
            if (currentKeyState.IsKeyDown(Keys.Down)) sprite.PositionY += 5;
            if (currentKeyState.IsKeyDown(Keys.Left)) sprite.PositionX -= 5;
            if (currentKeyState.IsKeyDown(Keys.Right)) sprite.PositionX += 5;

            // Check for pressed/released keys.
            // Loop for each possible pressed key (those that are pressed this update)
            Keys[] keys = currentKeyState.GetPressedKeys();
            for (int i = 0; i < keys.Length; i++)
            {
                // Was this key up during the last update?
                if (_lastKeyState.IsKeyUp(keys[i]))
                {
                    // Yes, so this key has been pressed
                    System.Diagnostics.Debug.WriteLine("Pressed: " + keys[i].ToString());
                }
            }
            // Loop for each possible released key (those that were pressed last update)
            keys = _lastKeyState.GetPressedKeys();
            for (int i = 0; i < keys.Length; i++)
            {
                // Is this key now up?
                if (currentKeyState.IsKeyUp(keys[i]))
                {
                    // Yes, so this key has been released
                    System.Diagnostics.Debug.WriteLine("Released: " + keys[i].ToString());
                }
            }

            // Store the state for the next loop
            _lastKeyState = currentKeyState;

            base.Update(gameTime);
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


        private void ResetGame()
        {
            // Set up a single sprite for the user to control
            GameObjects.Clear();
            GameObjects.Add(new SpriteObject(this, new Vector2(200, 200), Textures["Circle"]));
        }

    }
}
