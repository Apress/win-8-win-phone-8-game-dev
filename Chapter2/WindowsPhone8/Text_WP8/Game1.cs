using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Text_WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        private SpriteFont _fontMiramonte;

        private float _angle;

        public Game1()
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

            // Load the spritefont
            _fontMiramonte = Content.Load<SpriteFont>("Miramonte");
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

            // Rotate the text
            _angle += 1;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Vector2 textsize;
            Color textcolor;
            string textString;

            GraphicsDevice.Clear(Color.Blue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            // Display some static text with a shadow
            _spriteBatch.DrawString(_fontMiramonte, "Hello world", new Vector2(102, 102), Color.Black);
            _spriteBatch.DrawString(_fontMiramonte, "Hello world", new Vector2(100, 100), Color.White);

            // Draw some rotated, scaled text with alpha blending...
            // Calculate the size of the text
            textString = "Text in MonoGame!";
            textsize = _fontMiramonte.MeasureString(textString);
            // Draw it lots of times
            for (int i = 25; i >= 0; i--)
            {
                // For the final iteration, use black text;
                // otherwise use white text with gradually increasing alpha levels
                if (i > 0)
                {
                    textcolor = new Color(Color.White, 255 - i * 10);
                }
                else
                {
                    textcolor = Color.Black;
                }
                // Draw our text with its origin at the middle of the screen and
                // in the center of the text, rotated and scaled based on the
                // iteration number.
                _spriteBatch.DrawString(_fontMiramonte, textString, new Vector2(240, 400), textcolor, MathHelper.ToRadians(_angle * ((i + 5) * 0.1f)), textsize / 2, 1 + (i / 5.0f), SpriteEffects.None, 0);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
