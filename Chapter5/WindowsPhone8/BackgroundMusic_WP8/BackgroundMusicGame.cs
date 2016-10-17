using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameFramework;
using Microsoft.Xna.Framework.Media;
using System;
using Microsoft.Xna.Framework.Input;

namespace BackgroundMusic_WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BackgroundMusicGame : GameHost
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        public BackgroundMusicGame()
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

            // Load fonts
            Fonts.Add("Miramonte", Content.Load<SpriteFont>("Miramonte"));

            // Load songs.
            // Are we in control of the media player?
            if (MediaPlayer.GameHasControl)
            {
                // Load our song
                Songs.Add("2020", Content.Load<Song>("Breadcrumbs_2020"));

                // Play the song, repeating
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(Songs["2020"]);
            }

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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            DrawText(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }


        private void ResetGame()
        {
            if (Songs.Count == 0)
            {
                // Not currently playing
                GameObjects.Add(new TextObject(this, Fonts["Miramonte"], new Vector2(10, 50), "Game is not in control of MediaPlayer"));
            }
            else
            {
                // OK to play
                GameObjects.Add(new TextObject(this, Fonts["Miramonte"], new Vector2(10, 50), "Playing background music"));
            }
        }

    }
}
