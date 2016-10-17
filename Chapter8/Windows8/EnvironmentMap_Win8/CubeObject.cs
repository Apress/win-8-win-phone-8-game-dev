using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace EnvironmentMap_Win8
{
    class CubeObject : GameFramework.MatrixObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        // Declare a static array of vertices. As they will be the same
        // for every cube, this saves us having to use further vertex
        // memory for each CubeObject instance.
        private static VertexPositionNormalTexture[] _vertices;
        private static VertexBuffer _vertexBuffer;

        //-------------------------------------------------------------------------------------
        // Class constructors

        public CubeObject(EnvironmentMapGame game, Vector3 position, Texture2D texture, float envMapAmount, float fresnelFactor)
            : base(game)
        {
            // Have we already built the cube vertex array in a previous instance?
            if (_vertices == null)
            {
                // No, so build them now
                BuildVertices();
                // Create a vertex buffer
                _vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionNormalTexture), _vertices.Length, BufferUsage.WriteOnly);
                _vertexBuffer.SetData(_vertices);
            }

            // Set object properties
            Position = position;
            ObjectTexture = texture;
            EnvironmentMapAmount = envMapAmount;
            FresnelFactor = FresnelFactor;
        }

        //-------------------------------------------------------------------------------------
        // Object Functions

        /// <summary>
        /// Update the cube position and calculate its transformation matrix
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update the object rotation angles
            AngleY += MathHelper.ToRadians(1.0f);
            AngleZ += MathHelper.ToRadians(0.8f);

            // Calculate the transformation matrix
            SetIdentity();
            ApplyStandardTransformations();
        }

        /// <summary>
        /// Draw the cube
        /// </summary>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Effect effect)
        {
            // Prepare the effect for drawing
            PrepareEffect(effect);

            // Set the active vertex buffer
            effect.GraphicsDevice.SetVertexBuffer(_vertexBuffer);

            // Draw the object
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                // Apply the pass
                pass.Apply();
                // Draw the object using the active vertex buffer
                effect.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 12);
            }
        }


        /// <summary>
        /// Build the vertex array that stores the positions and colors of the cube vertices
        /// </summary>
        private void BuildVertices()
        {
            int i;
            Color thisColor = Color.Black;

            // Create and initialize the vertices
            _vertices = new VertexPositionNormalTexture[36];

            // Set the vertex positions for a unit size cube.
            i = 0;
            // Front face...
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, 0.5f);
            // Back face...
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, -0.5f);
            // Left face...
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, 0.5f);
            // Right face...
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, 0.5f);
            // Top face...
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, 0.5f);
            // Bottom face...
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, 0.5f);
            
            // Set the texture coordinates
            i = 0;
            // Front face...
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 1.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 1.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 1.0f);
            // Right face...
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 1.0f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 1.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 1.0f);
            // Back face...
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 1.0f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 1.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 1.0f);
            // Left face...
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 1.0f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 1.0f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 1.0f);
            // Top face...
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 1.0f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 1.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 1.0f);
            // Bottom face...
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 1.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(1.0f, 1.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 1.0f);

            // Set the vertex normals
            CalculateVertexNormals(_vertices);
        }


    }
}
