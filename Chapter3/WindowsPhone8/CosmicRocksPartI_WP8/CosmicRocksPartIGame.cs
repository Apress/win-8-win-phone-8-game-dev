using GameFramework;
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

namespace CosmicRocksPartI_WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class CosmicRocksPartIGame : GameFramework.GameHost
    {

        //-------------------------------------------------------------------------------------
        // Private class variables

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpaceshipObject _playerShip;

        //-------------------------------------------------------------------------------------
        // Constructor

        public CosmicRocksPartIGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        //-------------------------------------------------------------------------------------
        // XNA functions

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

            // Load the object textures into the textures dictionary
            Textures.Add("Spaceship", this.Content.Load<Texture2D>("Spaceship"));
            Textures.Add("Rock1", this.Content.Load<Texture2D>("Rock1"));
            Textures.Add("Rock2", this.Content.Load<Texture2D>("Rock2"));
            Textures.Add("Rock3", this.Content.Load<Texture2D>("Rock3"));
            Textures.Add("Star", this.Content.Load<Texture2D>("Star"));
            Textures.Add("SmokeParticle", this.Content.Load<Texture2D>("SmokeParticle"));

            // Load fonts
            Fonts.Add("Miramonte", this.Content.Load<SpriteFont>("Miramonte"));
            
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
            GraphicsDevice.Clear(Color.Black);

            // Begin the main spritebatch
            _spriteBatch.Begin();
            // Draw the sprites...
            DrawSprites(gameTime, _spriteBatch, Textures["Star"]);
            DrawSprites(gameTime, _spriteBatch, Textures["Rock1"]);
            DrawSprites(gameTime, _spriteBatch, Textures["Rock2"]);
            DrawSprites(gameTime, _spriteBatch, Textures["Rock3"]);
            DrawSprites(gameTime, _spriteBatch, Textures["Spaceship"]);
            // Draw text
            DrawText(gameTime, _spriteBatch);
            // End the spritebatch
            _spriteBatch.End();

            // Begin a spritebatch with NonPremultiplied blending for the particles.
            // This will allow them to smoothly fade away as their intensity fades.
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            // Draw the sprites...
            DrawSprites(gameTime, _spriteBatch, Textures["SmokeParticle"]);
            // End the spritebatch
            _spriteBatch.End();

            base.Draw(gameTime);
        }


        //-------------------------------------------------------------------------------------
        // Game functions


        /// <summary>
        /// Reset the game to its initial state
        /// </summary>
        private void ResetGame()
        {
            string rockTextureName;

            // Remove any existing objects
            GameObjects.Clear();

            // Add some stars
            for (int i = 0; i < 50; i++)
            {
                GameObjects.Add(new StarObject(this, Textures["Star"]));
            }

            // Add some space rocks
            for (int i = 0; i < 5; i++)
            {
                rockTextureName = "Rock" + GameHelper.RandomNext(1, 4).ToString();
                GameObjects.Add(new RockObject(this, Textures[rockTextureName], 2, 0.5f, 2.0f));
            }

            // Add the player ship
            _playerShip = new SpaceshipObject(this, Textures["Spaceship"], new Vector2(GraphicsDevice.Viewport.Bounds.Width / 2, GraphicsDevice.Viewport.Bounds.Height / 2));
            GameObjects.Add(_playerShip);

            // Add a benchmark object
            //GameObjects.Add(new BenchmarkObject(this, Fonts["Miramonte"], new Vector2(0, 40), Color.White));
        }

        /// <summary>
        /// Recycle or create the required number of ParticleObject instances.
        /// </summary>
        /// <param name="particleCount"></param>
        /// <returns></returns>
        internal ParticleObject[] GetParticleObjects(int particleCount)
        {
            // Create an empty array to hold the particle objects.
            // Note that this doesn't instantiate any particles, all array elements
            // are initially null.
            ParticleObject[] particles = new ParticleObject[particleCount];
            GameObjectBase obj;
            int particleIndex = 0;
            int objectCount = GameObjects.Count;

            // Scan the GameObjects collection looking for inactive particles
            for (int i = 0; i < objectCount; i++)
            {
                // Get a reference to the object
                obj = GameObjects[i];
                // Is this a particle object?
                if (obj is ParticleObject)
                {
                    // Is it inactive?
                    if (((ParticleObject)obj).IsActive == false)
                    {
                        // Yes, so add it to the array
                        particles[particleIndex] = (ParticleObject)obj;
                        // Move to the next array index
                        particleIndex += 1;
                        // Have we filled the array now? If so, exit the loop
                        if (particleIndex == particleCount) break;
                    }
                }
            }

            // Do we need more particle objects than we have found?
            // If so, loop for the remaining array indexes creating an
            // object for each.
            for (; particleIndex < particleCount; particleIndex++)
            {
                // Add a new object to the array
                particles[particleIndex] = new ParticleObject(this);
                // Add to the GameObjects list
                GameObjects.Add(particles[particleIndex]);
            }

            return particles;
        }

    }
}
