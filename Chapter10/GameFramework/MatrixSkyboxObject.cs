using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace GameFramework
{
    public class MatrixSkyboxObject : MatrixObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        // Declare a static array of vertices. As they will be the same
        // for every cube, this saves us having to use further vertex
        // memory for each SkyboxObject instance.
        private static VertexPositionColorTexture[] _vertices;
        private static VertexBuffer _vertexBuffer;

        internal bool _renderedThisFrame = false;

        //-------------------------------------------------------------------------------------
        // Class constructors

        public MatrixSkyboxObject(GameHost game, Texture2D texture, Vector3 position, Vector3 scale)
            : base(game)
        {
            // Have we already built the cube vertex array in a previous instance?
            if (_vertices == null)
            {
                // No, so build them now
                BuildVertices();
                // Create a vertex buffer
                _vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionColorTexture), _vertices.Length, BufferUsage.WriteOnly);
                _vertexBuffer.SetData(_vertices);
            }

            // Set other object properties
            ObjectTexture = texture;
            Position = position;
            Scale = scale;
        }

        //-------------------------------------------------------------------------------------
        // Object Functions

        /// <summary>
        /// Update the ground position and calculate its transformation matrix
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Calculate the transformation matrix
            SetIdentity();

            // Observe the camera's position if one is active
            if (Game.Camera != null)
            {
                // Read the camera's calculated position
                ApplyTransformation(Matrix.CreateTranslation(Game.Camera.Transformation.Translation));
            }

            // Now apply the standard transformations
            ApplyStandardTransformations();

            // Indicate that the skybox has not been rendered this frame
            _renderedThisFrame = false;
        }

        /// <summary>
        /// Draw the cube
        /// </summary>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Effect effect)
        {
            // Prepare the effect for drawing
            PrepareEffect(effect);

            // Disable lighting but remember whether it was switched on...
            bool lightingEnabled = ((BasicEffect)effect).LightingEnabled;
            ((BasicEffect)effect).LightingEnabled = false;
            // Disable the depth buffer
            DepthStencilState depthState = effect.GraphicsDevice.DepthStencilState;
            effect.GraphicsDevice.DepthStencilState = DepthStencilState.None;

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

            // Re-enable lighting and the depth buffer if required
            if (lightingEnabled) ((BasicEffect)effect).LightingEnabled = true;
            effect.GraphicsDevice.DepthStencilState = depthState;

            // Indicate that the skybox has been rendered this frame
            _renderedThisFrame = true;
        }


        /// <summary>
        /// Build the vertex array that stores the positions and colors of the cube vertices
        /// </summary>
        private void BuildVertices()
        {
            int i;
            Color thisColor = Color.Black;

            // Create and initialize the vertices
            _vertices = new VertexPositionColorTexture[24];

            // Set the vertex positions for a unit size cube.
            i = 0;
            // Front face...
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, 0.5f);
            // Right face...
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, 0.5f);
            // Back face...
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, -0.5f);
            // Left face...
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, 0.5f);

            // Set the texture coordinates
            i = 0;
            // Front face...
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 0.001f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 0.999f);
            _vertices[i++].TextureCoordinate = new Vector2(0.25f, 0.999f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 0.001f);
            _vertices[i++].TextureCoordinate = new Vector2(0.25f, 0.999f);
            _vertices[i++].TextureCoordinate = new Vector2(0.25f, 0.001f);
            // Right face...
            _vertices[i++].TextureCoordinate = new Vector2(0.25f, 0.999f);
            _vertices[i++].TextureCoordinate = new Vector2(0.5f, 0.999f);
            _vertices[i++].TextureCoordinate = new Vector2(0.5f, 0.001f);
            _vertices[i++].TextureCoordinate = new Vector2(0.25f, 0.999f);
            _vertices[i++].TextureCoordinate = new Vector2(0.5f, 0.001f);
            _vertices[i++].TextureCoordinate = new Vector2(0.25f, 0.001f);
            // Back face...
            _vertices[i++].TextureCoordinate = new Vector2(0.5f, 0.999f);
            _vertices[i++].TextureCoordinate = new Vector2(0.75f, 0.999f);
            _vertices[i++].TextureCoordinate = new Vector2(0.75f, 0.001f);
            _vertices[i++].TextureCoordinate = new Vector2(0.5f, 0.001f);
            _vertices[i++].TextureCoordinate = new Vector2(0.5f, 0.999f);
            _vertices[i++].TextureCoordinate = new Vector2(0.75f, 0.001f);
            // Left face...
            _vertices[i++].TextureCoordinate = new Vector2(0.75f, 0.001f);
            _vertices[i++].TextureCoordinate = new Vector2(0.75f, 0.999f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 0.999f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 0.001f);
            _vertices[i++].TextureCoordinate = new Vector2(0.75f, 0.001f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 0.999f);

            // Set the vertex colors -- all white
            for (i = 0; i < _vertices.Length; i++)
            {
                _vertices[i].Color = Color.White;
            }
        }


    }
}
