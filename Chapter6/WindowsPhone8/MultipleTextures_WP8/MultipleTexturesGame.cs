using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MultipleTextures_WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MultipleTexturesGame : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        // Variables required for the scene to be rendered
        private BasicEffect _effect;
        private VertexPositionTexture[] _vertices = new VertexPositionTexture[4];
        private Texture2D _texture1;
        private Texture2D _texture2;

        private float _angle;

        public MultipleTexturesGame()
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
            // Calculate the screen aspect ratio
            float aspectRatio = (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;
            // Create a projection matrix
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), aspectRatio, 0.1f, 1000.0f);

            // Calculate a view matrix (where we are looking from and to)
            Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 10), Vector3.Zero, Vector3.Up);

            _effect = new BasicEffect(GraphicsDevice);
            _effect.LightingEnabled = false;
            _effect.VertexColorEnabled = false;
            _effect.TextureEnabled = true;
            _effect.Projection = projection;
            _effect.View = view;
            _effect.World = Matrix.Identity;

            _vertices[0].Position = new Vector3(-1, -1, 0);
            _vertices[1].Position = new Vector3(-1, 1, 0);
            _vertices[2].Position = new Vector3(1, -1, 0);
            _vertices[3].Position = new Vector3(1, 1, 0);

            _vertices[0].TextureCoordinate = new Vector2(0, 1);
            _vertices[1].TextureCoordinate = new Vector2(0, 0);
            _vertices[2].TextureCoordinate = new Vector2(1, 1);
            _vertices[3].TextureCoordinate = new Vector2(1, 0);

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

            // Load our texture
            _texture1 = Content.Load<Texture2D>("Grapes");
            _texture2 = Content.Load<Texture2D>("Strawberry");
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

            _angle += MathHelper.ToRadians(1);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Activate the first texture
            _effect.Texture = _texture1;
            // Apply a transformation to move and rotate the object
            _effect.World = Matrix.CreateRotationZ(_angle);
            _effect.World = Matrix.CreateTranslation(0, 1.2f, 0) * _effect.World;
            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                // Apply the pass
                pass.Apply();
                // Draw the square
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, _vertices, 0, 2);
            }

            // Activate the second texture
            _effect.Texture = _texture2;
            // Apply a transformation to move and rotate the object
            _effect.World = Matrix.CreateRotationZ(_angle);
            _effect.World = Matrix.CreateTranslation(0, -1.2f, 0) * _effect.World;
            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                // Apply the pass
                pass.Apply();
                // Draw the square
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, _vertices, 0, 2);
            }

            base.Draw(gameTime);
        }
    }
}
