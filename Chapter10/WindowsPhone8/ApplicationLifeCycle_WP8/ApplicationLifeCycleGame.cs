using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameFramework;
using Windows.ApplicationModel.Activation;
using Windows.UI.Popups;
using Microsoft.Xna.Framework.Input;

namespace ApplicationLifeCycle_WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ApplicationLifeCycleGame : GameHost
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        public ApplicationLifeCycleGame()
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

            // Load the object textures into the textures dictionary
            Textures.Add("Ball", this.Content.Load<Texture2D>("Ball"));

            // Reset the game
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

            // Update all the game objects
            UpdateAll(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Begin the spritebatch
            _spriteBatch.Begin();
            // Draw the sprites
            DrawSprites(gameTime, _spriteBatch);
            // End the spritebatch
            _spriteBatch.End();

            base.Draw(gameTime);
        }


        /// <summary>
        /// Reset the game to its initial state
        /// </summary>
        private void ResetGame()
        {
            // Remove any existing objects
            GameObjects.Clear();

            // Add 10 balls
            for (int i = 0; i < 10; i++)
            {
                GameObjects.Add(new BallObject(this, Textures["Ball"]));
            }
        }

    }
}
