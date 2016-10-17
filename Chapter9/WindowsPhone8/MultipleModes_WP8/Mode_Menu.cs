using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using GameFramework;
using Microsoft.Xna.Framework.Input;

namespace MultipleModes_WP8
{
    class Mode_Menu : GameModeBase
    {

        private MultipleModesGame _game;

        public Mode_Menu(MultipleModesGame game)
            : base(game)
        {
            _game = game;
        }


        public override void Reset()
        {
            base.Reset();

            TextObject gameText;

            // Clear any existing objects
            GameObjects.Clear();

            // Add the title
            gameText = new TextObject(_game, _game.Fonts["Miramonte"],
                            new Vector2(_game.GraphicsDevice.Viewport.Width * 0.5f, _game.GraphicsDevice.Viewport.Height * 0.1f),
                            "Multiple Modes Game", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near);
            gameText.Scale = new Vector2(0.75f);
            GameObjects.Add(gameText);
            // Add the title shadow
            gameText = new TextObject(_game, _game.Fonts["Miramonte"],
                            new Vector2(_game.GraphicsDevice.Viewport.Width * 0.5f + 3, _game.GraphicsDevice.Viewport.Height * 0.1f + 3),
                            "Multiple Modes Game", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near);
            gameText.Scale = new Vector2(0.75f);
            gameText.SpriteColor = new Color(0, 0, 0, 50);
            GameObjects.Add(gameText);

            // Add the "new game" option
            gameText = new TextObject(_game, _game.Fonts["Miramonte"],
                            new Vector2(_game.GraphicsDevice.Viewport.Width * 0.5f, _game.GraphicsDevice.Viewport.Height * 0.3f),
                            "Start new game", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near);
            gameText.Scale = new Vector2(0.6f);
            gameText.Tag = "NewGame";
            GameObjects.Add(gameText);

            // Add the "resume game" option
            gameText = new TextObject(_game, _game.Fonts["Miramonte"],
                            new Vector2(_game.GraphicsDevice.Viewport.Width * 0.5f, _game.GraphicsDevice.Viewport.Height * 0.4f),
                            "Resume the current game", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near);
            gameText.Scale = new Vector2(0.6f);
            gameText.Tag = "ResumeGame";
            // Set to use a faded alpha to begin with to indicate that this item is disabled, there is no
            // game to resume at first.
            gameText.SpriteColor = new Color(Color.Black, 40);
            GameObjects.Add(gameText);

            // Add the "High Scores" option
            gameText = new TextObject(_game, _game.Fonts["Miramonte"],
                            new Vector2(_game.GraphicsDevice.Viewport.Width * 0.5f, _game.GraphicsDevice.Viewport.Height * 0.5f),
                            "View the high scores", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near);
            gameText.Scale = new Vector2(0.6f);
            gameText.Tag = "HighScores";
            GameObjects.Add(gameText);

            // Add the "Settings" option
            gameText = new TextObject(_game, _game.Fonts["Miramonte"],
                            new Vector2(_game.GraphicsDevice.Viewport.Width * 0.5f, _game.GraphicsDevice.Viewport.Height * 0.6f),
                            "Settings", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near);
            gameText.Scale = new Vector2(0.6f);
            gameText.Tag = "Settings";
            GameObjects.Add(gameText);

            // Add the "Credits" option
            gameText = new TextObject(_game, _game.Fonts["Miramonte"],
                new Vector2(_game.GraphicsDevice.Viewport.Width * 0.5f, _game.GraphicsDevice.Viewport.Height * 0.7f),
                "About this game", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near);
            gameText.Scale = new Vector2(0.6f);
            gameText.Tag = "Credits";
            GameObjects.Add(gameText);
        }


        public override void Activate()
        {
            base.Activate();

            // Have the game objects been loaded yet?
            if (GameObjects.Count == 0)
            {
                // No, so exit
                return;
            }

            // See if there is a game active at present
            if (Game.GetGameModeHandler<Mode_Game>().GameIsActive)
            {
                // Yes, so "enable" the ResumeGame option
                (Game.GetObjectByTag("ResumeGame") as TextObject).SpriteColor = Color.White;
            }
            else
            {
                // No, so "disable" the ResumeGame option
                (Game.GetObjectByTag("ResumeGame") as TextObject).SpriteColor = new Color(Color.Black, 40);
            }
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) _game.Exit();

            TouchCollection touches;
            SpriteObject touchedText;

            // Update all game objects
            _game.UpdateAll(gameTime);

            // Has the player touched the screen?
            touches = TouchPanel.GetState();
            if (touches.Count == 1 && touches[0].State == TouchLocationState.Pressed)
            {
                // Find which text object (if any) has been touched
                touchedText = Game.GetSpriteAtPoint(touches[0].Position);
                // Did we get something?
                if (touchedText != null)
                {
                    // See what it was
                    switch (touchedText.Tag)
                    {
                        case "NewGame":
                            // Switch to gameplay mode
                            Game.SetGameMode<Mode_Game>();
                            // Reset for a new game
                            Game.CurrentGameModeHandler.Reset();
                            break;

                        case "ResumeGame":
                            // Is the game already active?
                            if (Game.GetGameModeHandler<Mode_Game>().GameIsActive)
                            {
                                // Yes, so switch back to the existing game
                                Game.SetGameMode<Mode_Game>();
                            }
                            break;

                        case "HighScores":
                            // Switch to High Scores mode
                            Game.SetGameMode<Mode_HighScores>();
                            break;

                        case "Settings":
                            // Switch to Settings mode
                            Game.SetGameMode<Mode_Settings>();
                            break;

                        case "Credits":
                            // Switch to Credits mode
                            Game.SetGameMode<Mode_Credits>();
                            break;

                    }

                }
            }

            base.Update(gameTime);
        }


        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.SteelBlue);

            _game._spriteBatch.Begin();
            _game.DrawText(gameTime, _game._spriteBatch);
            _game._spriteBatch.End();

            base.Draw(gameTime);
        }



    }
}
