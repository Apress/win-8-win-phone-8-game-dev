using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LayerDepth_Win8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        Texture2D _spriteTexture;

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

            // Load the sprite texture
            _spriteTexture = this.Content.Load<Texture2D>("Mouse");
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
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Begin a sprite batch with BackToFront sorting enabled
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            // Draw some sprites with different layerDepth values
            _spriteBatch.Draw(_spriteTexture, new Vector2(100, 100), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            _spriteBatch.Draw(_spriteTexture, new Vector2(200, 100), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.Draw(_spriteTexture, new Vector2(300, 100), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            // End the sprite batch
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
