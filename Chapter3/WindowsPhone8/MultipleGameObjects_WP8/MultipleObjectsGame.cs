using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameFramework;
using Microsoft.Xna.Framework.Input;

namespace MultipleGameObjects_WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MultipleObjectsGame : GameFramework.GameHost
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        public MultipleObjectsGame()
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
            Textures.Add("Box", this.Content.Load<Texture2D>("Box"));

            // Load fonts
            Fonts.Add("Kootenay", this.Content.Load<SpriteFont>("Kootenay"));

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
            GraphicsDevice.Clear(Color.Wheat);

            // Begin the spritebatch
            _spriteBatch.Begin();
            // Draw the sprites
            DrawSprites(gameTime, _spriteBatch);
            // Draw the text
            DrawText(gameTime, _spriteBatch);
            // End the spritebatch
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Reset the game to its initial state
        /// </summary>
        private void ResetGame()
        {
            TextObject message;

            // Remove any existing objects
            GameObjects.Clear();

            // Add 10 boxes and 10 balls
            for (int i = 0; i < 10; i++)
            {
                GameObjects.Add(new BoxObject(this, Textures["Box"]));
            }
            for (int i = 0; i < 10; i++)
            {
                GameObjects.Add(new BallObject(this, Textures["Ball"]));
            }

            // Add some text
            message = new TextObject(this, Fonts["Kootenay"], new Vector2(GraphicsDevice.Viewport.Bounds.Width / 2, GraphicsDevice.Viewport.Height / 2), "MonoGame Game Development", TextObject.TextAlignment.Center, TextObject.TextAlignment.Center);
            message.SpriteColor = Color.DarkBlue;
            message.Scale = new Vector2(1.0f, 1.5f);
            //GameObjects.Add(message);
        }

    }
}
