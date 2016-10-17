using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameFramework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameAndXAML_WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MonoGameAndXAMLGame : GameHost
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        private int Score { get; set; }
        private int Lives { get; set; }

        public MonoGameAndXAMLGame()
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

            Textures.Add("Biplane", Content.Load<Texture2D>("Biplane"));
            Textures.Add("Cloud", Content.Load<Texture2D>("Cloud"));

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

            UpdateAll(gameTime);

            // Did the player click the Reset button?
            if (GamePage.Current.ResetButtonClicked)
            {
                // Reset the score
                Score = 0;
                // Clear the 'clicked' property
                GamePage.Current.ResetButtonClicked = false;
            }

            // Increase the player's score
            Score += 10;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            DrawSprites(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);

            GamePage.Current.SetLives(Lives);
            GamePage.Current.SetScore(Score);
        }

        /// <summary>
        /// Reset the game to its default state
        /// </summary>
        private void ResetGame()
        {
            BiplaneObject biplane;
            CloudObject cloud;

            // Add the biplane
            biplane = new BiplaneObject(this, Textures["Biplane"]);
            biplane.Position = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height/2);
            biplane.LayerDepth = 0.5f;
            biplane.Scale = new Vector2(0.5f);
            GameObjects.Add(biplane);

            // Add some clouds
            for (int i = 1; i < 15; i++)
            {
                cloud = new CloudObject(this, Textures["Cloud"]);
                cloud.Position = new Vector2(GameHelper.RandomNext(GraphicsDevice.Viewport.Width), GameHelper.RandomNext(GraphicsDevice.Viewport.Height));
                cloud.LayerDepth = (GameHelper.RandomNext(1.0f));
                cloud.Scale = new Vector2(2, 1) * (1.2f - cloud.LayerDepth);
                GameObjects.Add(cloud);
            }

            // Reset the player data
            Lives = 3;
            Score = 0;
        }


    }
}
