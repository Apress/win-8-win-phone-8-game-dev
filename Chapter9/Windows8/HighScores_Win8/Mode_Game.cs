using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using GameFramework;

namespace HighScores_Win8
{
    class Mode_Game : GameModeBase
    {

        // A typed reference to the main game class
        private HighScoresGame _game;

        // The player's score
        public int Score { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="game">The main game object</param>
        public Mode_Game(HighScoresGame game)
            : base(game)
        {
            _game = game;
        }


        public override void Reset()
        {
            base.Reset();

            GameObjects.Clear();

            // Generate a random score
            Score = GameHelper.RandomNext(100, 200) * 10;

            // Show the player some "game over" text
            GameObjects.Add(new TextObject(_game, _game.Fonts["WascoSans"], new Vector2(_game.GraphicsDevice.Viewport.Width / 2, 30), "*** Game over ***", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near));
            GameObjects.Add(new TextObject(_game, _game.Fonts["WascoSans"], new Vector2(_game.GraphicsDevice.Viewport.Width / 2, 160), "Your score was " + Score.ToString(), TextObject.TextAlignment.Center, TextObject.TextAlignment.Near));
            // Is this good enough for a high score?
            if (_game.HighScores.GetTable("Normal").ScoreQualifies(Score))
            {
                GameObjects.Add(new TextObject(_game, _game.Fonts["WascoSans"], new Vector2(_game.GraphicsDevice.Viewport.Width / 2, 250), "You got a high score!", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near));
            }
            else
            {
                GameObjects.Add(new TextObject(_game, _game.Fonts["WascoSans"], new Vector2(_game.GraphicsDevice.Viewport.Width / 2, 250), "No high score this time...", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near));
            }
            GameObjects.Add(new TextObject(_game, _game.Fonts["WascoSans"], new Vector2(_game.GraphicsDevice.Viewport.Width / 2, 400), "Click or tap to continue...", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near));

        }


        public override void Update(GameTime gameTime)
        {
            TouchCollection tc;

            base.Update(gameTime);

            _game.UpdateAll(gameTime);

            // Has the user touched the screen?
            tc = TouchPanel.GetState();
            if (tc.Count == 1 && tc[0].State == TouchLocationState.Pressed)
            {
                // Enter HighScores mode
                _game.SetGameMode<Mode_HighScores>();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _game.GraphicsDevice.Clear(Color.DarkBlue);

            _game._spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            _game.DrawText(gameTime, _game._spriteBatch);
            _game._spriteBatch.End();
        }
    
    }
}
