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

namespace OrbitingSprite_Win8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class OrbitingSpriteGame : GameFramework.GameHost
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public OrbitingSpriteGame()
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
            GraphicsDevice.Clear(new Color(0, 0, 40));

            _spriteBatch.Begin();
            DrawSprites(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Reset the game ready to play
        /// </summary>
        private void ResetGame()
        {
            PlanetObject planet;
            MoonObject moon;

            // Add a planet...
            planet = new PlanetObject(this, new Vector2(150, 200), Textures["Planet"], 0.7f);
            GameObjects.Add(planet);
            // ...and give it a moon
            GameObjects.Add(new MoonObject(this, Textures["Moon"], planet, 0.02f, 60, 0.3f, Color.White));

            // Add another planet...
            planet = new PlanetObject(this, new Vector2(300, 500), Textures["Planet"], 1.0f);
            GameObjects.Add(planet);
            // ...and give it some moons
            GameObjects.Add(new MoonObject(this, Textures["Moon"], planet, 0.04f, 90, 0.2f, Color.OrangeRed));
            GameObjects.Add(new MoonObject(this, Textures["Moon"], planet, 0.025f, 130, 0.4f, Color.PaleGreen));
            moon = new MoonObject(this, Textures["Moon"], planet, 0.01f, 180, 0.25f, Color.Silver);
            GameObjects.Add(moon);
            // Add a moon to the moon
            GameObjects.Add(new MoonObject(this, Textures["Moon"], moon, 0.1f, 25, 0.15f, Color.White));
        }
    }
}
