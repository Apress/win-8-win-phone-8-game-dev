using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using GameFramework;

namespace MultipleModes_Win8
{
    class Mode_Game : GameModeBase
    {

        private MultipleModesGame _game;

        public bool GameIsActive { get; set; }

        public Mode_Game(MultipleModesGame game)
            : base(game)
        {
            _game = game;

            // Indicate that the game is not yet active
            GameIsActive = false;
        }


        /// <summary>
        /// Reset the game to its initial state
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            // Clear any existing objects
            GameObjects.Clear();

            // Add some new objects to the game
            for (int i = 0; i < 5; i++)
            {
                GameObjects.Add(new SquareObject(_game, Game.Textures["Club"]));
                GameObjects.Add(new SquareObject(_game, Game.Textures["Spade"]));
                GameObjects.Add(new SquareObject(_game, Game.Textures["Heart"]));
                GameObjects.Add(new SquareObject(_game, Game.Textures["Diamond"]));
            }

            // Remember that the game is now playing
            GameIsActive = true;
        }



        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            TouchCollection touches = TouchPanel.GetState();

            // Has the player touched the screen?
            if (touches.Count == 1 && touches[0].State == TouchLocationState.Pressed)
            {
                // Yes, so switch to gameplay mode
                Game.SetGameMode<Mode_Menu>();
            }

            Game.UpdateAll(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.PaleGreen);

            _game._spriteBatch.Begin();
            _game.DrawSprites(gameTime, _game._spriteBatch);
            _game._spriteBatch.End();

            base.Draw(gameTime);
        }



    }
}
