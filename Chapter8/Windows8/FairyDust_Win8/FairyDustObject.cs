using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace FairyDust_Win8
{
    class FairyDustObject : GameFramework.MatrixParticleObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        private FairyDustGame _game;

        // Declare a static array of vertices.
        private static VertexPositionNormalTexture[] _vertices;
        private static VertexBuffer _vertexBuffer;

        private Vector3 _velocity;

        //-------------------------------------------------------------------------------------
        // Class constructors

        public FairyDustObject(FairyDustGame game, Texture2D texture, Vector3 position)
            : base(game)
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

            // Reset the particle to an initial state
            ResetParticle(position);
        }

        /// <summary>
        /// Reset the particle to its initial state, allowing it to be re-used
        /// </summary>
        internal void ResetParticle(Vector3 position)
        {
            // Become active
            IsActive = true;

            // Reset to the smoke start position
            Position = position;
            // Start scaled very small, we will increase in size after creation
            Scale = new Vector3(0.1f);

            // Generate a random velocity
            _velocity = new Vector3(GameHelper.RandomNext(-0.02f, 0.02f), GameHelper.RandomNext(-0.02f, 0.0f), GameHelper.RandomNext(-0.02f, 0.02f));

            // Set the color and alpha
            ObjectColor = new Color(GameHelper.RandomNext(0, 255), GameHelper.RandomNext(0, 255), GameHelper.RandomNext(0, 255), 255);
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

            // Update the position
            Position += _velocity;
            // Apply gravity
            _velocity.Y -= 0.001f;
            // If we hit the ground, become inactive
            if (PositionY <= 0) IsActive = false;

            // Random scale
            Scale = new Vector3(GameHelper.RandomNext(0.08f, 0.12f));

            // Update the alpha
            SetAlpha(ObjectColor.A - 3);
            // Have we become invisible? If so, become inactive
            if (ObjectColor.A <= 0) IsActive = false;

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
            effect.GraphicsDevice.BlendState = BlendState.Additive;

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
