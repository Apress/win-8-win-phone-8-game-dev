using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Orientation_WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        private SpriteFont _miramonteFont;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Add a handler to the OrientationChanged event
            Window.OrientationChanged += Window_OrientationChanged;

            // Set the supported orientations
            _graphics.SupportedOrientations = DisplayOrientation.Portrait | DisplayOrientation.LandscapeLeft |
                                DisplayOrientation.LandscapeRight;
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

            _miramonteFont = Content.Load<SpriteFont>("Miramonte");
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

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Display some information about the orientation
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_miramonteFont, "Current orientation: " + Window.CurrentOrientation.ToString(), new Vector2(10, 100), Color.White);
            _spriteBatch.DrawString(_miramonteFont, "Window size: " + Window.ClientBounds.Width + ", " + Window.ClientBounds.Height, new Vector2(10, 130), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Respond to the orientation changing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Window_OrientationChanged(object sender, System.EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Orientation changed to " + Window.CurrentOrientation.ToString());
        }

    }
}
