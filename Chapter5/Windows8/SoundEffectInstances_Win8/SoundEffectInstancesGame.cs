using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameFramework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Audio;

namespace SoundEffectInstances_Win8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SoundEffectInstancesGame : GameHost
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        private SoundEffectInstance[] _soundInstances = new SoundEffectInstance[4];

        public SoundEffectInstancesGame()
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
            TouchPanel.EnableMouseGestures = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures and fonts
            Textures.Add("Box", Content.Load<Texture2D>("Box"));
            Fonts.Add("Miramonte", Content.Load<SpriteFont>("Miramonte"));

            // Load the sound effects
            SoundEffects.Add("EnergySound", Content.Load<SoundEffect>("EnergySound"));
            SoundEffects.Add("MagicSpell", Content.Load<SoundEffect>("MagicSpell"));
            SoundEffects.Add("Motorbike", Content.Load<SoundEffect>("Motorbike"));
            SoundEffects.Add("Piano", Content.Load<SoundEffect>("Piano"));

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
            int screenRegion;

            UpdateAll(gameTime);

            TouchCollection tc = TouchPanel.GetState();
            if (tc.Count > 0)
            {
                // Find the region of the screen that has been touched
                screenRegion = (int)(tc[0].Position.Y * 4 / Window.ClientBounds.Height);
                // Ensure we have a region between 0 and 3
                if (screenRegion >= 0 && screenRegion <= 3)
                {
                    // What type of touch event do we have?
                    switch (tc[0].State)
                    {
                        case TouchLocationState.Pressed:
                            // Set the pitch based on the horizontal touch position
                            _soundInstances[screenRegion].Pitch = (tc[0].Position.X / this.Window.ClientBounds.Width) * 2 - 1;
                            // Start the sound for this region
                            _soundInstances[screenRegion].Play();
                            break;
                        case TouchLocationState.Moved:
                            // Is the sound for this region currently playing?
                            if (_soundInstances[screenRegion].State == SoundState.Playing)
                            {
                                // Yes, so set the pitch based on the horizontal touch position
                                _soundInstances[screenRegion].Pitch = (tc[0].Position.X / this.Window.ClientBounds.Width) * 2 - 1;
                            }
                            break;
                        case TouchLocationState.Released:
                            // Pause all of the sounds
                            for (int i = 0; i < _soundInstances.Length; i++)
                            {
                                _soundInstances[i].Pause();
                            }
                            break;
                    }
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

            _spriteBatch.Begin();
            DrawSprites(gameTime, _spriteBatch);
            DrawText(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResetGame()
        {
            SpriteObject sprite;
            TextObject text;
            for (int y = 0; y < 4; y++)
            {
                // Create a sprite for the background area
                sprite = new SpriteObject(this, new Vector2(0, Window.ClientBounds.Height / 4 * y), Textures["Box"]);
                sprite.ScaleX = (float)Window.ClientBounds.Width / sprite.SpriteTexture.Width;
                sprite.ScaleY = (float)Window.ClientBounds.Height / 4 / sprite.SpriteTexture.Width;
                switch (y)
                {
                    case 0: sprite.SpriteColor = Color.LightBlue; break;
                    case 1: sprite.SpriteColor = Color.LightGoldenrodYellow; break;
                    case 2: sprite.SpriteColor = Color.LightSeaGreen; break;
                    case 3: sprite.SpriteColor = Color.PaleVioletRed; break;
                }
                GameObjects.Add(sprite);

                // Create a text object for the sound name
                text = new TextObject(this, Fonts["Miramonte"], new Vector2(Window.ClientBounds.Width / 2, sprite.PositionY + Window.ClientBounds.Height / 8));
                text.HorizontalAlignment = TextObject.TextAlignment.Center;
                text.VerticalAlignment = TextObject.TextAlignment.Center;
                switch (y)
                {
                    case 0: text.Text = "EnergySound"; break;
                    case 1: text.Text = "Piano"; break;
                    case 2: text.Text = "MagicSpell"; break;
                    case 3: text.Text = "Motorbike"; break;
                }
                GameObjects.Add(text);

                // Create a sound effect instance for this sound
                _soundInstances[y] = SoundEffects[text.Text].CreateInstance();
                _soundInstances[y].IsLooped = true;
            }

        }


    }
}
