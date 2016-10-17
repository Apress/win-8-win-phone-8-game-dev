using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using GameFramework;

namespace SpritesAndSolids_Win8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SpritesAndSolidsGame : GameFramework.GameHost
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Variables required for the scene to be rendered
        private BasicEffect _effect;

        private TextObject _balloonText;

        public SpritesAndSolidsGame()
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
            // Calculate the screen aspect ratio
            float aspectRatio = (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;
            // Create a projection matrix
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), aspectRatio, 0.1f, 1000.0f);

            // Calculate a view matrix (where we are looking from and to)
            Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 4), Vector3.Zero, Vector3.Up);

            // Create and initialize the effect
            _effect = new BasicEffect(GraphicsDevice);
            _effect.Projection = projection;
            _effect.View = view;
            _effect.World = Matrix.Identity;
            _effect.EnableDefaultLighting();

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

            // Load textures
            Textures.Add("Marble", Content.Load<Texture2D>("Marble"));
            Textures.Add("Balloon", Content.Load<Texture2D>("Balloon"));
            // Loat fonts
            Fonts.Add("Miramonte", Content.Load<SpriteFont>("Miramonte"));

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

            // Update the balloon count text
            _balloonText.Text = "Number of balloons: " + BalloonObject._balloonCount.ToString();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw all 3D objects
            DrawObjects(gameTime, _effect);

            // Draw all sprites and text...
            // First store the graphics device state
            StoreStateBeforeSprites();
            // Draw the sprites
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            DrawSprites(gameTime, _spriteBatch);
            DrawText(gameTime, _spriteBatch);
            _spriteBatch.End();
            // Now restore the graphics device state
            RestoreStateAfterSprites();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Reset the game
        /// </summary>
        private void ResetGame()
        {
            // Clear any existing objects
            GameObjects.Clear();

            // Add some cubes
            GameObjects.Add(new CubeObject(this, new Vector3(0, 0.5f, 0), Textures["Marble"], new Vector3(0, 1, 0.6f)));
            GameObjects.Add(new CubeObject(this, new Vector3(0, -0.5f, 0), Textures["Marble"], new Vector3(0, -0.8f, 0.4f)));

            // Add some balloons
            for (int i = 0; i < 200; i++)
            {
                GameObjects.Add(new BalloonObject(this, Textures["Balloon"]));
            }

            // Add some text
            _balloonText = new TextObject(this, Fonts["Miramonte"], new Vector2(40, 40));
            GameObjects.Add(_balloonText);
        }

    }
}
