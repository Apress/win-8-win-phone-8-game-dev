using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameFramework;

namespace WindowResize_Win8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class WindowResizeGame : GameHost
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        // Variables required for the scene to be rendered
        private BasicEffect _effect;

        public WindowResizeGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Support multiple orientations
            _graphics.SupportedOrientations = DisplayOrientation.Portrait | DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Calculate the screen aspect ratio
            float aspectRatio = (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;
            // Create a projection matrix
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), aspectRatio, 0.1f, 1000.0f);

            // Calculate a view matrix (where we are looking from and to)
            Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 1), Vector3.Zero, Vector3.Up);

            // Create and initialize the effect
            _effect = new BasicEffect(GraphicsDevice);
            _effect.LightingEnabled = false;
            _effect.VertexColorEnabled = true;
            _effect.TextureEnabled = false;
            _effect.Projection = projection;
            _effect.View = view;
            _effect.World = Matrix.Identity;

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

            // Load game content
            Fonts.Add("WascoSans", Content.Load<SpriteFont>("WascoSans"));

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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw all objects
            DrawObjects(gameTime, _effect);

            // Draw text
            _spriteBatch.Begin();
            DrawText(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Resize(GameHost.WindowStates windowState, Vector2 newSize, Vector2 oldSize)
        {
            base.Resize(windowState, newSize, oldSize);

            // Calculate the screen aspect ratio
            float aspectRatio = newSize.X / newSize.Y;
            // Create a projection matrix
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), aspectRatio, 0.1f, 1000.0f);
            // Set the projection matrix into the effect
            _effect.Projection = projection;

            // Update the text object to show the new window size
            SetWindowSizeText();
            // Update text object positions to match the new window size
            AutoUpdateTextPositions(newSize.X, oldSize.X);
        }

        /// <summary>
        /// Reset the game
        /// </summary>
        private void ResetGame()
        {
            TextObject textObj;

            // Clear any existing objects
            GameObjects.Clear();

            // Add a cube
            GameObjects.Add(new CubeObject(this));

            // Add some text
            textObj = new TextObject(this, Fonts["WascoSans"], Vector2.Zero, "Window sizing");
            GameObjects.Add(textObj);

            textObj = new TextObject(this, Fonts["WascoSans"], new Vector2(GraphicsDevice.Viewport.Bounds.Width, 0), "Window size: ", TextObject.TextAlignment.Far, TextObject.TextAlignment.Near);
            textObj.Tag = "WindowSizeText";
            GameObjects.Add(textObj);
            // Update the text object to show the current window size
            SetWindowSizeText();
        }

        /// <summary>
        /// Update the WindowSizeText text object to show the current window size
        /// </summary>
        private void SetWindowSizeText()
        {
            // Retrieve the "WindowSizeText" object
            TextObject textObj = GetObjectByTag("WindowSizeText") as TextObject;

            // Update the text to show the current window size
            textObj.Text = string.Format("Size: {0} x {1}", GraphicsDevice.Viewport.Bounds.Width, GraphicsDevice.Viewport.Bounds.Height);

            // If in snapped mode, move the text down a little so it doesn't overlap the "Window sizing" title text
            if (CurrentWindowState != WindowStates.Snapped)
            {
                textObj.PositionY = 0;
            }
            else
            {
                textObj.PositionY = 40;
            }
        }

    }
}
