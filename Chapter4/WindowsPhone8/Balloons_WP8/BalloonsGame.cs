using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameFramework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;

namespace Balloons_WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BalloonsGame : GameHost
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        public BalloonsGame()
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

            Textures.Add("Balloon", Content.Load<Texture2D>("Balloon"));

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
            TouchCollection touchPoints;
            SpriteObject touchSprite;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) this.Exit();

            // Update the game objects
            UpdateAll(gameTime);

            // Has a new touch point been pressed?
            touchPoints = TouchPanel.GetState();
            if (touchPoints.Count > 0 && touchPoints[0].State == TouchLocationState.Pressed)
            {
                touchSprite = GetSpriteAtPoint(touchPoints[0].Position);
                if (touchSprite is BalloonObject)
                {
                    // Randomize the sprite to effectively remove it and create another balloon
                    ((BalloonObject)touchSprite).Randomize();
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw all of the balloons, sorted by their LayerDepth from back to front
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            DrawSprites(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Reset the game to its default state
        /// </summary>
        private void ResetGame()
        {
            GameObjects.Clear();

            // Add 50 new balloons
            for (int i = 0; i < 50; i++)
            {
                GameObjects.Add(new BalloonObject(this, Textures["Balloon"]));
            }
        }


    }
}
