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

namespace Isometric_WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class IsometricGame : GameFramework.GameHost
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Variables required for the scene to be rendered
        private BasicEffect _effect;

        public IsometricGame()
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
            Matrix projection = Matrix.CreateOrthographic(10 * aspectRatio, 10, -100, 100.0f);

            // Calculate a view matrix (where we are looking from and to) for isometric projection
            Matrix view = Matrix.CreateLookAt(new Vector3(1, 1, 1), Vector3.Zero, Vector3.Up);

            // Create and initialize the effect
            _effect = new BasicEffect(GraphicsDevice);
            _effect.VertexColorEnabled = false;
            _effect.TextureEnabled = false;
            _effect.Projection = projection;
            _effect.View = view;
            _effect.World = Matrix.Identity;
            _effect.LightingEnabled = true;

            _effect.DirectionalLight0.Enabled = true;
            _effect.DirectionalLight0.Direction = new Vector3(-0.3f, -1, -0.6f);
            _effect.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
            _effect.DirectionalLight0.SpecularColor = Color.White.ToVector3();

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
            GraphicsDevice.Clear(Color.LightGray);// Color.CornflowerBlue);

            // Draw all objects
            DrawObjects(gameTime, _effect);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Reset the game
        /// </summary>
        private void ResetGame()
        {
            CubeObject cube;
            Color cubeColor;

            // Clear any existing objects
            GameObjects.Clear();

            // Add a cube
            for (int x = -3; x <= 3; x++)
            {
                for (int z = -3; z <= 3; z++)
                {
                    cubeColor = new Color(GameHelper.RandomNext(256), GameHelper.RandomNext(256), GameHelper.RandomNext(256));
                    cube = new CubeObject(this, new Vector3(x, 0, z), cubeColor);
                    GameObjects.Add(cube);
                }
            }
        }

    }
}
