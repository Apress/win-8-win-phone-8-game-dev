using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlphaBlending_Win8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class AlphaBlendingGame : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        // Variables required for the scene to be rendered
        private BasicEffect _effect;
        private VertexPositionTexture[] _vertices = new VertexPositionTexture[4];
        private Texture2D _texture;

        private float _angle;

        public AlphaBlendingGame()
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

            // Use one of the following two approaches to set a blend state:
            // Either pick one of the built-in blend states (e.g., BlendState.Opaque
            // or BlendState.AlphaBlend), or create a BlendState object and set
            // its properties as required.

            // Assign a built-in BlendState object
            GraphicsDevice.BlendState = BlendState.Opaque;

            // Create a new BlendState object
            //BlendState blendState = new BlendState();
            //// Set the color blend properties
            //blendState.ColorBlendFunction = BlendFunction.Add;
            //blendState.ColorSourceBlend = Blend.SourceAlpha;
            //blendState.ColorDestinationBlend = Blend.InverseSourceAlpha;
            //// Copy the color blend properties to the alpha blend properties
            //blendState.AlphaBlendFunction = blendState.ColorBlendFunction;
            //blendState.AlphaSourceBlend = blendState.ColorSourceBlend;
            //blendState.AlphaDestinationBlend = blendState.ColorDestinationBlend;
            //// Set the object into the GraphicsDevice
            //GraphicsDevice.BlendState = blendState;

            // Set the alpha level for the overall effect
            //_effect.Alpha = 0.5f;

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
            _texture = Content.Load<Texture2D>("Grapes");
            // Set it as the active texture within our effect
            _effect.Texture = _texture;
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

            // Draw three objects so that we can see how they blend together
            for (int i = 0; i < 3; i++)
            {
                // Translate a small way up or down the screen
                _effect.World = Matrix.CreateTranslation(0, i - 1, 0);
                // Set the world matrix so that the square rotates
                _effect.World = Matrix.CreateRotationZ(_angle) * _effect.World;

                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    // Apply the pass
                    pass.Apply();
                    // Draw the square
                    GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, _vertices, 0, 2);
                }
            }

            base.Draw(gameTime);
        }
    }
}
