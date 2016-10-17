using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameFramework;
using Microsoft.Xna.Framework.Input;

namespace GamePadDemo_Win8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GamePadDemoGame : GameHost
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        System.Text.StringBuilder _padStateDetails;

        TextObject _padStateDetailsObject;

        public GamePadDemoGame()
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

            // Load resources
            Fonts.Add("Miramonte", Content.Load<SpriteFont>("Miramonte"));

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
            GamePadState gamepadState;

            UpdateAll(gameTime);

            // Set the text to contain the details of the gamepad
            gamepadState = GamePad.GetState(PlayerIndex.One);
            _padStateDetails.Clear();
            // DPad
            _padStateDetails.Append("DPad:");
            _padStateDetails.Append(" Left = " + gamepadState.DPad.Left.ToString());
            _padStateDetails.Append(", Up = " + gamepadState.DPad.Up.ToString());
            _padStateDetails.Append(", Right = " + gamepadState.DPad.Right.ToString());
            _padStateDetails.AppendLine(", Down = " + gamepadState.DPad.Down.ToString());
            // Thumbsticks
            _padStateDetails.AppendLine("Thumbstick left: " + gamepadState.ThumbSticks.Left.ToString());
            _padStateDetails.AppendLine("Thumbstick right: " + gamepadState.ThumbSticks.Right.ToString());
            // Triggers
            _padStateDetails.AppendLine("Trigger left: " + gamepadState.Triggers.Left.ToString());
            _padStateDetails.AppendLine("Trigger right: " + gamepadState.Triggers.Right.ToString());
            // Buttons
            _padStateDetails.Append("Buttons: ");
            if (gamepadState.Buttons.A == ButtonState.Pressed) _padStateDetails.Append("A ");
            if (gamepadState.Buttons.B == ButtonState.Pressed) _padStateDetails.Append("B ");
            if (gamepadState.Buttons.Back == ButtonState.Pressed) _padStateDetails.Append("Back ");
            if (gamepadState.Buttons.BigButton == ButtonState.Pressed) _padStateDetails.Append("BigButton ");
            if (gamepadState.Buttons.LeftShoulder == ButtonState.Pressed) _padStateDetails.Append("LeftShoulder ");
            if (gamepadState.Buttons.LeftStick == ButtonState.Pressed) _padStateDetails.Append("LeftStick ");
            if (gamepadState.Buttons.RightShoulder == ButtonState.Pressed) _padStateDetails.Append("RightShoulder ");
            if (gamepadState.Buttons.RightStick == ButtonState.Pressed) _padStateDetails.Append("RightStick ");
            if (gamepadState.Buttons.Start == ButtonState.Pressed) _padStateDetails.Append("Start ");
            if (gamepadState.Buttons.X == ButtonState.Pressed) _padStateDetails.Append("X ");
            if (gamepadState.Buttons.Y == ButtonState.Pressed) _padStateDetails.Append("Y ");
            _padStateDetails.AppendLine("");

            // Set the text into the game object
            _padStateDetailsObject.Text = _padStateDetails.ToString();

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
            // Create a stringbuilder to write out gamepad details to
            _padStateDetails = new System.Text.StringBuilder();

            // Create a text object to display the details on the screen
            _padStateDetailsObject = new TextObject(this, Fonts["Miramonte"], new Vector2(10, 10));
            GameObjects.Add(_padStateDetailsObject);
        }


    }
}
