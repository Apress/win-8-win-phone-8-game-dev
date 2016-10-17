using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BackgroundImage_WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        Texture2D _backgroundTexture;
        Texture2D _spriteTexture;

        private Vector2 _spritePosition;

        public Game1()
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
            // Reset the game
            ResetGame();

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

            // Load required game content here
            _backgroundTexture = Content.Load<Texture2D>("Background");
            _spriteTexture = Content.Load<Texture2D>("Mouse");
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

            // Update the game state
            _spritePosition.Y += 5;
            if (_spritePosition.Y >= GraphicsDevice.Viewport.Height) _spritePosition.Y = -_spriteTexture.Height;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // No need to clear as we are redrawing the entire screen with our background image
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            // Begin the spriteBatch
            _spriteBatch.Begin();
            // Draw the background image
            _spriteBatch.Draw(_backgroundTexture, GraphicsDevice.Viewport.Bounds, Color.White);
            // Draw the smiley face
            _spriteBatch.Draw(_spriteTexture, _spritePosition, Color.White);
            // End the spriteBatch
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Reset the game to its default state
        /// </summary>
        private void ResetGame()
        {
            // Set the initial smiley position
            _spritePosition = new Vector2(100, 100);
        }
    }
}
