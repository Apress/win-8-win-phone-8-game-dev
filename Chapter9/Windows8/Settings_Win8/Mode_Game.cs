using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using GameFramework;

namespace Settings_Win8
{
    class Mode_Game : GameModeBase
    {
        
        // A typed reference to the main game class
        private SettingsGame _game;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="game">The main game object</param>
        public Mode_Game(SettingsGame game)
            : base(game)
        {
            _game = game;
        }

        /// <summary>
        /// Reset this game mode
        /// </summary>
        public override void Reset()
        {
            int ballSpeed;

            base.Reset();

            // Get the ball speed
            ballSpeed = SettingsManager.GetValue("Speed", 1);

            // Add some balls, observing the speed
            for (int i = 0; i < 10; i++)
            {
                GameObjects.Add(new BallObject(_game, _game.Textures["Ball"], ballSpeed));
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            TouchCollection tc;

            // Has the user touched the screen?
            tc = TouchPanel.GetState();
            if (tc.Count == 1 && tc[0].State == TouchLocationState.Pressed)
            {
                // Enter settings mode
                _game.SetGameMode<Mode_Settings>();
            }

            // Update all the game objects
            _game.UpdateAll(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _game.GraphicsDevice.Clear(Color.PaleGoldenrod);

            _game._spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            _game.DrawSprites(gameTime, _game._spriteBatch);
            _game._spriteBatch.End();
        }


    }
}
