using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using GameFramework;
using Microsoft.Xna.Framework.Input;

namespace MultipleModes_WP8
{
    class Mode_Credits : GameModeBase
    {

        private MultipleModesGame _game;

        public Mode_Credits(MultipleModesGame game)
            : base(game)
        {
            _game = game;
        }


        public override void Reset()
        {
            TextObject gameText;

            base.Reset();

            // Clear existing objects
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

            // Add the rest of the content for this mode
            gameText = new TextObject(_game, _game.Fonts["Miramonte"],
                            new Vector2(_game.GraphicsDevice.Viewport.Width * 0.5f, _game.GraphicsDevice.Viewport.Height * 0.5f),
                            "Credits go here", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near);
            gameText.Scale = new Vector2(0.6f);
            GameObjects.Add(gameText);

            // Add the rest of the content for this mode
            gameText = new TextObject(_game, _game.Fonts["Miramonte"],
                            new Vector2(_game.GraphicsDevice.Viewport.Width * 0.5f, _game.GraphicsDevice.Viewport.Height * 0.6f),
                            "(Tap or click to return to the menu)", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near);
            gameText.Scale = new Vector2(0.3f);
            GameObjects.Add(gameText);
        }


        public override void Activate()
        {
            base.Activate();

            // Reset the settings
            Reset();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            TouchCollection touches;

            // Update all game objects
            _game.UpdateAll(gameTime);

            // Has the player touched the screen or pressed the back button?
            touches = TouchPanel.GetState();
            if (touches.Count == 1 && touches[0].State == TouchLocationState.Pressed
                ||
                GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                // Return back to the menu
                Game.SetGameMode<Mode_Menu>();
            }

            base.Update(gameTime);
        }


        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.DarkCyan);

            _game._spriteBatch.Begin();
            _game.DrawText(gameTime, _game._spriteBatch);
            _game._spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
