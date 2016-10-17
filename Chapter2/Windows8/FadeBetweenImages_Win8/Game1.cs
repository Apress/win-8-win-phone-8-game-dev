using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FadeBetweenImages_Win8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        private int _faderAlpha;
        private const int IMAGE_COUNT = 3;
        private Texture2D[] _backgroundTextures = new Texture2D[IMAGE_COUNT];
        private int _backgroundImageIndex = 0;
        private int _faderImageIndex = 1;

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

            // Load the fader images
            _backgroundTextures[0] = Content.Load<Texture2D>("Beach_Landscape");
            _backgroundTextures[1] = Content.Load<Texture2D>("Sunset_Landscape");
            _backgroundTextures[2] = Content.Load<Texture2D>("Trees_Landscape");
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
            // Increment the opacity of the fader
            _faderAlpha += 1;
            // Has it reached full opacity?
            if (_faderAlpha > 255)
            {
                // Yes, so reset to zero and move to the next pair of images
                _faderAlpha = 0;
                _backgroundImageIndex = (_backgroundImageIndex + 1) % IMAGE_COUNT;
                _faderImageIndex = (_faderImageIndex + 1) % IMAGE_COUNT;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // No need to clear as we are drawing a full-screen image
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            // Begin the spriteBatch
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            // Draw the background image
            _spriteBatch.Draw(_backgroundTextures[_backgroundImageIndex], GraphicsDevice.Viewport.Bounds, Color.White);
            // Draw the fader
            _spriteBatch.Draw(_backgroundTextures[_faderImageIndex], GraphicsDevice.Viewport.Bounds, new Color(Color.White, _faderAlpha));
            // End the spriteBatch
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
