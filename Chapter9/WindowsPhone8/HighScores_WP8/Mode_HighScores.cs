using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using GameFramework;

namespace HighScores_WP8
{
    class Mode_HighScores : GameModeBase
    {
        // A typed reference to the main game class
        private HighScoresGame _game;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="game">The main game object</param>
        public Mode_HighScores(HighScoresGame game)
            : base(game)
        {
            _game = game;
        }


        /// <summary>
        /// Read the current score from the Game mode
        /// </summary>
        /// <returns></returns>
        private int GetLatestScore()
        {
            return _game.GetGameModeHandler<Mode_Game>().Score;
        }


        public override void Activate()
        {
            base.Activate();

            // Clear any existing game objects
            GameObjects.Clear();

            // Did the player's score qualify?
            if (_game.HighScores.GetTable("Normal").ScoreQualifies(GetLatestScore()))
            {
                // Yes, so display the input dialog
                GameHelper.BeginShowKeyboardInput(_game._graphics, KeyboardInputCallback, "High score achieved", "Please enter your name", SettingsManager.GetValue("PlayerName", ""));
            }
            else
            {
                // Show the highscores now. No score added so nothing to highlight
                ResetHighscoreTableDisplay(null);
            }
        }


        private void KeyboardInputCallback(bool result, string text)
        {
            HighScoreEntry newEntry = null;

            // Did we get a name from the player?
            if (result && !string.IsNullOrEmpty(text))
            {
                // Add the name to the highscore
                newEntry = _game.HighScores.GetTable("Normal").AddEntry(text, GetLatestScore());
                // Save the scores
                _game.HighScores.SaveScores();
                // Store the name so that we can recall it the next time a high score is achieved
                SettingsManager.SetValue("PlayerName", text);
            }

            // Show the highscores now and highlight the new entry if we have one
            ResetHighscoreTableDisplay(newEntry);
        }

        /// <summary>
        /// Set the game objects collection to display the high score table
        /// </summary>
        /// <param name="highlightEntry">If a new score has been added, pass its entry here and it will be highlighted</param>
        private void ResetHighscoreTableDisplay(HighScoreEntry highlightEntry)
        {
            // Add the title
            GameObjects.Add(new TextObject(_game, _game.Fonts["WascoSans"], new Vector2(10, 10), "High Scores"));

            // Add the score objects
            _game.HighScores.CreateTextObjectsForTable("Normal", _game.Fonts["WascoSans"], 0.7f, 80, 30, Color.White, Color.Blue, highlightEntry, Color.Yellow);
        }


        public override void Update(GameTime gameTime)
        {
            TouchCollection tc;

            base.Update(gameTime);

            _game.UpdateAll(gameTime);

            // Assuming we're not still entering the player's name...
            if (!GameHelper.KeyboardInputIsVisible)
            {
                // Has the user touched the screen?
                tc = TouchPanel.GetState();
                if (tc.Count == 1 && tc[0].State == TouchLocationState.Pressed)
                {
                    // Return back to Game mode
                    _game.SetGameMode<Mode_Game>();
                    // Reset the game
                    _game.CurrentGameModeHandler.Reset();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _game.GraphicsDevice.Clear(Color.OrangeRed);

            _game._spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            _game.DrawText(gameTime, _game._spriteBatch);
            _game._spriteBatch.End();
        }

    }
}
