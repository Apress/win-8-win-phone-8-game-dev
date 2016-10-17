using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace FireAndSmoke_WP8
{
    class SmokeParticleObject : GameFramework.MatrixParticleObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        private FireAndSmokeGame _game;

        // Declare a static array of vertices.
        private static VertexPositionNormalTexture[] _vertices;
        private static VertexBuffer _vertexBuffer;


        // The initial position of the particle
        private Vector3 _startPosition;
        private Vector3 _startScale;

        // The velocity of this particle
        private Vector3 _velocity;
        // The particle's z-axis rotation speed
        private float _rotateSpeed;

        //-------------------------------------------------------------------------------------
        // Class constructors

        public SmokeParticleObject(FireAndSmokeGame game, Texture2D texture, Vector3 position, Vector3 scale)
            : base(game, texture, position, scale)
        {
            _game = game;

            // Have we already built the ground vertex array in a previous instance?
            if (_vertices == null)
            {
                // No, so build it now
                BuildVertices();
                // Create a vertex buffer
                _vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionNormalTexture), _vertices.Length, BufferUsage.WriteOnly);
                _vertexBuffer.SetData(_vertices);
            }

            // Set object properties
            ObjectTexture = texture;

            // Store the supplied start position
            _startPosition = position;
            _startScale = scale;

            // Reset the particle to an initial state
            ResetParticle();

        }

        /// <summary>
        /// Reset the particle to its initial state, allowing it to be re-used
        /// </summary>
        internal void ResetParticle()
        {
            int grayLevel;

            // Become active
            IsActive = true;

            // Reset to the smoke start position
            Position = _startPosition;
            Scale = _startScale;

            // Offset the position to randomize around the center of the fire
            PositionX += GameHelper.RandomNext(-0.03f, 0.03f);
            PositionY += GameHelper.RandomNext(0.0f, 0.02f);
            PositionZ += GameHelper.RandomNext(-0.03f, 0.03f);

            // Start speed
            _velocity = new Vector3(0.006f, 0.012f, 0.000f);

            // Random angle
            AngleZ = GameHelper.RandomNext(0, MathHelper.TwoPi);
            _rotateSpeed = MathHelper.ToRadians(GameHelper.RandomNext(-5.0f, 5.0f));

            // Set a random color and mid-level alpha
            grayLevel = GameHelper.RandomNext(200, 255);
            ObjectColor = new Color(grayLevel, grayLevel, grayLevel, 150);
        }


        //-------------------------------------------------------------------------------------
        // Object Functions

        /// <summary>
        /// Update the object position and calculate its transformation matrix
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Return immediately if we're not active
            if (!IsActive) return;

            // Update the alpha
            SetAlpha(ObjectColor.A - 2);
            // Have we become invisible? If so, become inactive
            if (ObjectColor.A <= 0) IsActive = false;

            // Update the object position
            Position += _velocity;

            // Scale up
            Scale *= 1.01f;

            // Rotate the object
            AngleZ += _rotateSpeed;

            // Calculate the transformation matrix
            SetIdentity();
            // Apply the billboard transformation
            ApplyTransformation(CreateBillboard(Position, Game.Camera.Transformation.Translation, Game.Camera.Transformation.Up, Game.Camera.Transformation.Forward));
            // Rotate and scale
            ApplyTransformation(Matrix.CreateRotationZ(AngleZ));
            ApplyTransformation(Matrix.CreateScale(Scale));

        }

        /// <summary>
        /// Sets the SpriteColor alpha level to the specified value
        /// </summary>
        /// <param name="alpha"></param>
        private void SetAlpha(int alpha)
        {
            // Keep in the range 0 to 255
            if (alpha < 0) alpha = 0;
            if (alpha > 255) alpha = 255;

            // Update the color
            Color c = ObjectColor;
            c.A = (byte)alpha;
            ObjectColor = c;
        }

        /// <summary>
        /// Draw the object
        /// </summary>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Effect effect)
        {
            // Return immediately if we're not active
            if (!IsActive) return;

            // Prepare the effect for drawing
            PrepareEffect(effect);

            // Disable lighting but remember whether it was switched on...
            bool lightingEnabled = ((BasicEffect)effect).LightingEnabled;
            ((BasicEffect)effect).LightingEnabled = false;

            // Disable writing to the depth buffer
            DepthStencilState depthState = effect.GraphicsDevice.DepthStencilState;
            effect.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            // Enable transparency
            BlendState blendState = effect.GraphicsDevice.BlendState;
            effect.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            // Set the active vertex buffer
            effect.GraphicsDevice.SetVertexBuffer(_vertexBuffer);

            // Draw the object
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                // Apply the pass
                pass.Apply();
                // Draw the sky box
                effect.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _vertices.Length / 3);
            }

            // Restore the lighting, depth buffer and blandstate to their original values
            if (lightingEnabled) ((BasicEffect)effect).LightingEnabled = true;
            effect.GraphicsDevice.DepthStencilState = depthState;
            effect.GraphicsDevice.BlendState = blendState;
        }


        /// <summary>
        /// Build the vertex array that stores the positions and colors of the ground vertices
        /// </summary>
        private void BuildVertices()
        {
            int i;
            Color thisColor = Color.Black;

            // Create and initialize the vertices
            _vertices = new VertexPositionNormalTexture[6];

            // Set the vertex positions for the ground
            i = 0;
            _vertices[i++].Position = new Vector3(-1.0f, -1.0f, 0.0f);
            _vertices[i++].Position = new Vector3(1.0f, -1.0f, 0.0f);
            _vertices[i++].Position = new Vector3(-1.0f, 1.0f, 0.0f);
            _vertices[i++].Position = new Vector3(1.0f, -1.0f, 0.0f);
            _vertices[i++].Position = new Vector3(1.0f, 1.0f, 0.0f);
            _vertices[i++].Position = new Vector3(-1.0f, 1.0f, 0.0f);
            // Set the texture coordinates for the ground
            i = 0;
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 1.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 1.0f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 1.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 0.0f);
            // Set the normals
            for (i = 0; i < _vertices.Length; i++)
            {
                _vertices[i].Normal = new Vector3(0, 1, 0);
            }
        }


    }
}
