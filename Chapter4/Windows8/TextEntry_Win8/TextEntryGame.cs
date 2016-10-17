using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameFramework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.GamerServices;
using System;

namespace TextEntry_Win8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TextEntryGame : GameHost
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        public TextEntryGame()
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

            TouchPanel.EnableMouseTouchPoint = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

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
            UpdateAll(gameTime);

            // Has the user touched the screen?
            TouchCollection touches = TouchPanel.GetState();
            if (touches.Count == 1 && touches[0].State == TouchLocationState.Pressed)
            {
                // Make sure the input dialog is not already visible
                if (!(GameHelper.KeyboardInputIsVisible))
                {
                    // Show the input dialog to get text from the user
                    GameHelper.BeginShowKeyboardInput(_graphics, KeyboardInputComplete, "High score achieved", "Please enter your name!", "My name");
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Callback function that will be called when the keyboard input panel is closed
        /// </summary>
        /// <param name="result">true if the OK button was clicked, false if Cancel</param>
        /// <param name="text">If OK was clicked, contains the entered text</param>
        private void KeyboardInputComplete(bool result, string text)
        {
            if (result)
            {
                // Store it in the text object
                ((TextObject)GameObjects[0]).Text = "Your name is " + text;
            }
            else
            {
                // The input panel was cancelled
                ((TextObject)GameObjects[0]).Text = "Name entry was canceled.";
            }
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
            // Create a single text object
            GameObjects.Clear();

            GameObjects.Add(new TextObject(this, Fonts["Kootenay"], new Vector2(20, 100), "No name entered, click or tap the screen..."));
        }

    }
}
