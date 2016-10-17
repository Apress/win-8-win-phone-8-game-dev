using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Windows.Devices.Sensors;
using GameFramework;

namespace AccelerometerDemo_Win8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class AccelerometerGame : GameHost
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        private Accelerometer _accelerometer;
        private TextObject _accText;
        private BallObject _accBall;

        internal Vector3 AccelerometerData { get; set; }

        public AccelerometerGame()
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

            // Instantiate the accelerometer
            _accelerometer = Accelerometer.GetDefault();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load game content
            Textures.Add("ShinyBall", Content.Load<Texture2D>("ShinyBall"));
            Fonts.Add("Kootenay", Content.Load<SpriteFont>("Kootenay"));

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
            // Read the accelerometer data
            GetAccelerometerReading();

            // Update the game objects
            UpdateAll(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(240, 240, 240));

            _spriteBatch.Begin();
            DrawSprites(gameTime, _spriteBatch);
            DrawText(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResetGame()
        {
            // Create a single text object
            GameObjects.Clear();

            // Add a text object to display the accelerometer vector
            _accText = new TextObject(this, Fonts["Kootenay"], new Vector2(20, 100), "Accelerometer data:");
            _accText.SpriteColor = Color.DarkBlue;
            GameObjects.Add(_accText);

            // Add a ball to roll around the screen
            _accBall = new BallObject(this, new Vector2(240, 400), Textures["ShinyBall"]);
            _accBall.Origin = new Vector2(_accBall.SpriteTexture.Width, _accBall.SpriteTexture.Height) / 2;
            GameObjects.Add(_accBall);
        }

        void GetAccelerometerReading()
        {
            // Check we have an accelerometer to read...
            if (_accelerometer == null)
            {
                AccelerometerData = Vector3.Zero;
                _accText.Text = "No accelerometer available.";
            }
            else
            {
                // Get the current accelerometer reading
                AccelerometerReading accData = _accelerometer.GetCurrentReading();
                // Translate it into a Vector3 structure
                AccelerometerData = new Vector3((float)accData.AccelerationX, (float)accData.AccelerationY, (float)accData.AccelerationZ);

                // Update the content of the text object
                _accText.Text = "Accelerometer data:\n" + AccelerometerData.X.ToString("0.000")
                                                    + ", " + AccelerometerData.Y.ToString("0.000")
                                                    + ", " + AccelerometerData.Z.ToString("0.000");
            }
        }

    }
}
