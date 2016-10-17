using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameFramework;

namespace Lighting_Win8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class LightingGame : GameHost
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        // Variables required for the scene to be rendered
        private BasicEffect _effect;

        public LightingGame()
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
            Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 6), Vector3.Zero, Vector3.Up);

            // Create and initialize the effect
            _effect = new BasicEffect(GraphicsDevice);
            _effect.VertexColorEnabled = false;
            _effect.TextureEnabled = false;
            _effect.Projection = projection;
            _effect.View = view;
            _effect.World = Matrix.Identity;
            _effect.LightingEnabled = true;
            //_effect.PreferPerPixelLighting = true;

            //_effect.EnableDefaultLighting();

            _effect.DirectionalLight0.Enabled = true;
            _effect.DirectionalLight0.Direction = new Vector3(0, 0, -1);
            _effect.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
            _effect.DirectionalLight0.SpecularColor = Color.White.ToVector3();

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

            // Reset the game
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
            // Update all the game objects
            UpdateAll(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw all objects
            DrawObjects(gameTime, _effect);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Reset the game
        /// </summary>
        private void ResetGame()
        {
            CubeObject cube;
            CylinderObject cylinder;

            // Clear any existing objects
            GameObjects.Clear();

            // Add a cube
            cube = new CubeObject(this);
            cube.Position = new Vector3(0, 1, 0);
            cube.ObjectColor = Color.Blue;
            //cube.SpecularColor = Color.White;
            //cube.SpecularPower = 10;
            GameObjects.Add(cube);

            // Add a cylinder
            cylinder = new CylinderObject(this);
            cylinder.Position = new Vector3(0, -1, 0);
            cylinder.ObjectColor = Color.MistyRose;
            //cylinder.SpecularColor = Color.White;
            //cylinder.SpecularPower = 10;
            GameObjects.Add(cylinder);

        }

    }
}
