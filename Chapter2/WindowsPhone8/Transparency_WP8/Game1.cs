using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Transparency_WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        private Random _random = new Random();

        private Texture2D _colorKeyTexture;
        private Texture2D _alphaChannelTexture;

        private Color _backColor = Color.LightBlue;
        private TimeSpan _lastColorChange;

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

            // Load textures
            _colorKeyTexture = this.Content.Load<Texture2D>("ColorKey");
            _alphaChannelTexture = this.Content.Load<Texture2D>("AlphaChannel");
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

            // Have 3 seconds passed since we last changed color?
            if (gameTime.TotalGameTime.Subtract(_lastColorChange).TotalSeconds > 3)
            {
                _lastColorChange = gameTime.TotalGameTime;
                _backColor = new Color(_random.Next(0, 256), _random.Next(0, 256), _random.Next(0, 256));
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Clear to our random background color
            GraphicsDevice.Clear(_backColor);

            _spriteBatch.Begin();

            // Draw the texture using the color key
            _spriteBatch.Draw(_colorKeyTexture, new Vector2(100, 100), Color.White);
            _spriteBatch.Draw(_colorKeyTexture, new Vector2(150, 150), Color.White);

            // Draw the texture using the alpha channel
            _spriteBatch.Draw(_alphaChannelTexture, new Vector2(100, 300), Color.White);
            _spriteBatch.Draw(_alphaChannelTexture, new Vector2(150, 350), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
