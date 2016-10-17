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

namespace Benchmark_WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BenchmarkGame : GameFramework.GameHost
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // How many planets are we drawing?
        private const int PlanetCount = 10;

        public BenchmarkGame()
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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Textures.Add("Planet", Content.Load<Texture2D>("Planet"));
            Textures.Add("Moon", Content.Load<Texture2D>("Moon"));

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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) this.Exit();

            UpdateAll(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(0, 0, 40));

            // Version 1: draw all sprites without paying attention to textures
            _spriteBatch.Begin();
            DrawSprites(gameTime, _spriteBatch);
            DrawText(gameTime, _spriteBatch);
            _spriteBatch.End();

            //// Version 2: draw all sprites one texture at a time
            //_spriteBatch.Begin();
            //DrawSprites(gameTime, _spriteBatch, Textures["Planet"]);
            //DrawSprites(gameTime, _spriteBatch, Textures["Moon"]);
            //DrawText(gameTime, _spriteBatch);
            //_spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Reset the game ready to play
        /// </summary>
        private void ResetGame()
        {
            AddPlanets_Interleaved();
            //AddPlanets_InBlocks();

            // Add the benchmark object
            GameObjects.Add(new BenchmarkObject(this, Fonts["Kootenay"], new Vector2(50, 50), Color.White));
        }

        /// <summary>
        /// Add the planets to the game interleaved. The objects will be added so that the planets
        /// and moons alternate within the object list.
        /// </summary>
        private void AddPlanets_Interleaved()
        {
            Vector2 planetPosition;
            PlanetObject planet;

            // Add all of the planets...
            for (int i = 0; i < PlanetCount; i++)
            {
                // Add a planet...
                planetPosition = new Vector2(GameHelper.RandomNext(GraphicsDevice.Viewport.Bounds.Width), GameHelper.RandomNext(GraphicsDevice.Viewport.Bounds.Bottom));
                planet = new PlanetObject(this, planetPosition, Textures["Planet"], 0.5f);
                GameObjects.Add(planet);

                // Add a moon for the planet
                GameObjects.Add(new MoonObject(this, Textures["Moon"], planet, 0.1f, 30.0f, 0.2f, Color.White));
            }
        }


        /// <summary>
        /// Add the planets to the game in blocks. The objects will be added so that all the planets appear
        /// at the start of the list and all the moons afterwards at the end of the list.
        /// </summary>
        private void AddPlanets_InBlocks()
        {
            Vector2 planetPosition;

            // Add all of the planets...
            for (int i = 0; i < PlanetCount; i++)
            {
                // Add a planet...
                planetPosition = new Vector2(GameHelper.RandomNext(GraphicsDevice.Viewport.Bounds.Width), GameHelper.RandomNext(GraphicsDevice.Viewport.Bounds.Bottom));
                GameObjects.Add(new PlanetObject(this, planetPosition, Textures["Planet"], 0.5f));
            }
            // ...and then add all of the moons
            for (int i = 0; i < PlanetCount; i++)
            {
                // Add a moon...
                GameObjects.Add(new MoonObject(this, Textures["Moon"], (SpriteObject)GameObjects[i], 0.1f, 30.0f, 0.2f, Color.White));
            }
        }
    }
}
