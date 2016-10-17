using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using GameFramework;

namespace Settings_Win8
{
    class Mode_Settings : GameModeBase
    {

        private SettingsGame _game;

        public Mode_Settings(SettingsGame game)
            : base(game)
        {
            _game = game;
        }

        /// <summary>
        /// Add all of the game objects required for this mode
        /// </summary>
        public override void Reset()
        {
            SpriteObject spriteObject;

            base.Activate();

            // Add the title
            GameObjects.Add(new TextObject(_game, _game.Fonts["WascoSans"], new Vector2(20, 10), "Game Settings"));

            // Add some settings items...
            // Game speed
            spriteObject = new SettingsItemObject(_game, _game.Fonts["WascoSans"], new Vector2(60, 100), 0.9f,
                    "Speed", "Speed", "1", new string[] { "1", "2", "3" });
            spriteObject.SpriteColor = Color.Yellow;
            GameObjects.Add(spriteObject);
            // Difficulty
            spriteObject = new SettingsItemObject(_game, _game.Fonts["WascoSans"], new Vector2(60, 170), 0.9f,
                    "Difficulty", "Difficulty", "Medium", new string[] { "Easy", "Medium", "Hard" });
            spriteObject.SpriteColor = Color.Yellow;
            GameObjects.Add(spriteObject);
            // Music volume
            spriteObject = new SettingsItemObject(_game, _game.Fonts["WascoSans"], new Vector2(60, 240), 0.9f,
                    "MusicVolume", "Music volume", "Medium", new string[] { "Off", "Quiet", "Medium", "Loud" });
            spriteObject.SpriteColor = Color.Yellow;
            GameObjects.Add(spriteObject);
            // Sound effects volume
            spriteObject = new SettingsItemObject(_game, _game.Fonts["WascoSans"], new Vector2(60, 310), 0.9f,
                    "SoundEffectsVolume", "Sound effects volume", "Loud", new string[] { "Off", "Quiet", "Medium", "Loud" });
            spriteObject.SpriteColor = Color.Yellow;
            GameObjects.Add(spriteObject);

            // Add the "continue" text
            spriteObject = new TextObject(_game, _game.Fonts["WascoSans"], new Vector2(20, 400), "Continue...");
            spriteObject.Tag = "Continue";
            GameObjects.Add(spriteObject);
        }

        /// <summary>
        /// This mode is deactivating, store and apply any settings that the user has selected
        /// </summary>
        public override void Deactivate()
        {
            int newSpeed;

            base.Deactivate();

            // Store all of the values that the user has configured
            SettingsManager.StoreSettingsItemValues();

            // Update the speed of each ball to match the modified settings
            newSpeed = SettingsManager.GetValue("Speed", 1);
            // Loop through the "Game" mode's game objects
            foreach (GameObjectBase obj in _game.GetGameModeHandler<Mode_Game>().GameObjects)
            {
                if (obj is BallObject)
                {
                    (obj as BallObject).Speed = newSpeed;
                }
            }
        }


        public override void Update(GameTime gameTime)
        {
            GameObjectBase obj;
            TouchCollection tc;

            base.Update(gameTime);

            // Update all the game objects
            _game.UpdateAll(gameTime);

            // Has the user touched the screen?
            tc = TouchPanel.GetState();
            if (tc.Count == 1 && tc[0].State == TouchLocationState.Pressed)
            {
                // Find the object at the touch point
                obj = _game.GetSpriteAtPoint(tc[0].Position);
                // Did we get a settings option object?
                if (obj is GameFramework.SettingsItemObject)
                {
                    // Yes, so toggle it to the next value
                    (obj as SettingsItemObject).SelectNextValue();
                }
                else if (obj is TextObject)
                {
                    // No, but see if this is a tagged object that we recognise
                    switch (obj.Tag)
                    {
                        case "Continue":
                            // Return to the game
                            _game.SetGameMode<Mode_Game>();
                            break;
                    }
                }
            }
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _game.GraphicsDevice.Clear(Color.Coral);

            _game._spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            _game.DrawText(gameTime, _game._spriteBatch);
            _game._spriteBatch.End();
        }


    }
}
