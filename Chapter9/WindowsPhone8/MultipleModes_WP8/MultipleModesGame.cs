using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using GameFramework;

namespace MultipleModes_WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MultipleModesGame : GameHost
    {
        internal GraphicsDeviceManager _graphics;
        internal SpriteBatch _spriteBatch;

        public MultipleModesGame()
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
            // Add the game mode handlers
            AddGameModeHandler(new Mode_Menu(this));
            AddGameModeHandler(new Mode_Game(this));
            AddGameModeHandler(new Mode_Settings(this));
            AddGameModeHandler(new Mode_HighScores(this));
            AddGameModeHandler(new Mode_Credits(this));

            TouchPanel.EnableMouseTouchPoint = true;

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

            Textures.Add("Heart", Content.Load<Texture2D>("Heart"));
            Textures.Add("Diamond", Content.Load<Texture2D>("Diamond"));
            Textures.Add("Spade", Content.Load<Texture2D>("Spade"));
            Textures.Add("Club", Content.Load<Texture2D>("Club"));

            Fonts.Add("Miramonte", Content.Load<SpriteFont>("Miramonte"));

            // Activate the initial handler
            SetGameMode<Mode_Menu>();
            // Reset the handler
            CurrentGameModeHandler.Reset();
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
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
