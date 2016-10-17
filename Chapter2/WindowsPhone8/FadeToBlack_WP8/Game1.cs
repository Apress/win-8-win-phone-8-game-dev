using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FadeToBlack_WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        private Random _rand = new Random();

        private Texture2D _spriteTexture;
        private Texture2D _faderTexture;

        private Vector2[] _spritePositions;

        private int _faderAlpha = 0;
        private int _faderAlphaAdd = 2;

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
            _spriteTexture = Content.Load<Texture2D>("Mouse");

            // Create the texture for our fader sprite with a size of 1 x 1 pixel
            _faderTexture = new Texture2D(GraphicsDevice, 1, 1);
            // Create an array of colors for the texture -- just one color
            // as the texture consists of only one pixel
            Color[] faderColors = new Color[] { Color.White };
            // Set the color data into the texture
            _faderTexture.SetData<Color>(faderColors);

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

            // Update the fader alpha level
            _faderAlpha += _faderAlphaAdd;
            // Have we reached the extent of the fade?
            // If so, reverse the fade direction
            if (_faderAlpha <= 0)
            {
                // The fader is at its invisible point
                // Make sure we don't go below zero...
                _faderAlpha = 0;
                // Reverse direction
                _faderAlphaAdd = -_faderAlphaAdd;
            }
            if (_faderAlpha >= 255)
            {
                // The fader is at its opaque point
                // Make sure we don't go above 255
                _faderAlpha = 255;
                // Reverse direction
                _faderAlphaAdd = -_faderAlphaAdd;
                // Get some new sprite positions too while the sprites are hidden
                RandomizeSprites();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Begin the spriteBatch
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            // Draw the sprite face sprites
            for (int i = 0; i < _spritePositions.Length; i++)
            {
                _spriteBatch.Draw(_spriteTexture, _spritePositions[i], Color.White);
            }

            // Draw the fader
            _spriteBatch.Draw(_faderTexture, GraphicsDevice.Viewport.Bounds, new Color(Color.Black, _faderAlpha));

            // End the spriteBatch
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResetGame()
        {
            // Set a random position for each sprite
            _spritePositions = new Vector2[5];
            RandomizeSprites();
        }

        /// <summary>
        /// Generate random positions for each of the sprites
        /// </summary>
        private void RandomizeSprites()
        {
            for (int i = 0; i < _spritePositions.Length; i++)
            {
                _spritePositions[i].X = _rand.Next(0, GraphicsDevice.Viewport.Bounds.Width - _spriteTexture.Width);
                _spritePositions[i].Y = _rand.Next(0, GraphicsDevice.Viewport.Bounds.Height - _spriteTexture.Height);
            }
        }

    }
}
