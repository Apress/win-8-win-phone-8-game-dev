using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using GameFramework;

namespace DeviceCapabilities_Win8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class DeviceCapabilitiesGame : GameHost
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        public DeviceCapabilitiesGame()
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

            // Load content
            Fonts.Add("Kootenay", Content.Load<SpriteFont>("Kootenay"));

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

            _spriteBatch.Begin();
            DrawText(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResetGame()
        {
            System.Text.StringBuilder capstext = new System.Text.StringBuilder();
            GameHelper.InputCapabilities inputCaps = GameHelper.GetInputCapabilities();

            // Do we have a mouse?
            if (inputCaps.IsMousePresent)
            {
                capstext.AppendLine("There is a mouse present.");
            }
            else
            {
                capstext.AppendLine("There is no mouse present.");
            }
            capstext.AppendLine("");

            // Do we have a keyboard?
            if (inputCaps.IsKeyboardPresent)
            {
                capstext.AppendLine("There is a keyboard present.");
            }
            else
            {
                capstext.AppendLine("There is no keyboard present.");
            }
            capstext.AppendLine("");

            // Do we have touch capabilities?
            if (inputCaps.IsTouchPresent)
            {
                capstext.AppendLine("There is a touch screen present.");
            }
            else
            {
                capstext.AppendLine("There is no touch screen present.");
            }
            capstext.AppendLine("");

            // Do we have a GamePad?
            if (inputCaps.IsGamePadPresent)
            {
                capstext.AppendLine("There is a GamePad present.");
            }
            else
            {
                capstext.AppendLine("There is no GamePad present.");
            }
            capstext.AppendLine("");

            // Do we have accelerometer capabilities?
            if (inputCaps.IsAccelerometerPresent)
            {
                capstext.AppendLine("There is an accelerometer present.");
            }
            else
            {
                capstext.AppendLine("There is no accelerometer present.");
            }
            capstext.AppendLine("");

            // Add a text object to display the results
            GameObjects.Add(new TextObject(this, Fonts["Kootenay"], new Vector2(10, 10), capstext.ToString()));
        }

    }
}
