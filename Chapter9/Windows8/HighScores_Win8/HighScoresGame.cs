using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameFramework;
using Microsoft.Xna.Framework.Input.Touch;

namespace HighScores_Win8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class HighScoresGame : GameHost
    {
        internal GraphicsDeviceManager _graphics;
        internal SpriteBatch _spriteBatch;

        public HighScoresGame()
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
        protected async override void Initialize()
        {
            // Add the game mode handlers
            AddGameModeHandler(new Mode_Game(this));
            AddGameModeHandler(new Mode_HighScores(this));

            // Initialize and load the high scores
            HighScores.InitializeTable("Normal", 20);
            HighScores.InitializeTable("Difficult", 20);
            await HighScores.LoadScoresAsync();

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

            // Load content
            Fonts.Add("WascoSans", Content.Load<SpriteFont>("WascoSans"));

            // Reset the game modes
            GetGameModeHandler<Mode_Game>().Reset();
            GetGameModeHandler<Mode_HighScores>().Reset();

            // Set and initialize the game mode
            SetGameMode<Mode_Game>();
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
            // Update functionality is being handled by the game mode classes
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Draw functionality is being handled by the game mode classes
            base.Draw(gameTime);
        }
    }
}
