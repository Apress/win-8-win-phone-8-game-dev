using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace VertexAndIndexBuffers_Win8
{
    class VertexAndIndexBufferCubeObject : GameFramework.MatrixObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        // Declare a static array of vertices. As they will be the same
        // for every cube, this saves us having to use further vertex
        // memory for each CubeObject instance.
        private static VertexPositionColor[] _vertices;
        private static VertexBuffer _vertexBuffer;
        private static short[] _indices;
        private static IndexBuffer _indexBuffer;

        //-------------------------------------------------------------------------------------
        // Class constructors

        public VertexAndIndexBufferCubeObject(VertexAndIndexBuffersGame game, Vector3 position)
            : base(game)
        {
            // Set object properties
            Position = position;

        // Have we already built the cube vertex array in a previous instance?
        if (_vertices == null)
        {
            // No, so build them now
            BuildVertices();
            // Create a vertex buffer
            _vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionColor), _vertices.Length, BufferUsage.WriteOnly);
            _vertexBuffer.SetData(_vertices);

            // Create the index array
            BuildIndices();
            // Create an index buffer
            _indexBuffer = new IndexBuffer(game.GraphicsDevice, typeof(short), _indices.Length, BufferUsage.WriteOnly);
            _indexBuffer.SetData(_indices);
        }
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
            AngleY += MathHelper.ToRadians(2);
            AngleZ += MathHelper.ToRadians(0.4f);

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

            // Set the active vertex and index buffer
            effect.GraphicsDevice.SetVertexBuffer(_vertexBuffer);
            effect.GraphicsDevice.Indices = _indexBuffer;

            // Draw the object
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                // Apply the pass
                pass.Apply();
                // Draw the object using the active vertex buffer
                effect.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _vertices.Length, 0, 12);
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
            _vertices = new VertexPositionColor[24];

            // Set the vertex positions for a unit size cube.
            i = 0;
            // Front face...
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, 0.5f);
            // Back face...
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, -0.5f);
            // Left face...
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, 0.5f);
            // Right face...
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, 0.5f);
            // Top face...
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, 0.5f);
            // Bottom face...
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, -0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, 0.5f);

            // Set the vertex colors
            for (i = 0; i < _vertices.Length; i++)
            {
                // Switch to a new color for each face
                switch (i)
                {
                    case 0: thisColor = Color.Blue; break;
                    case 4: thisColor = Color.Yellow; break;
                    case 8: thisColor = Color.PaleGreen; break;
                    case 12: thisColor = Color.White; break;
                    case 16: thisColor = Color.Magenta; break;
                    case 20: thisColor = Color.Orange; break;
                }

                // Gradually fade the color towards black to make the shading
                // a little more interesting
                thisColor = thisColor * 0.9f;

                // Set the color into this vertex
                _vertices[i].Color = thisColor;
            }
        }

        /// <summary>
        /// Build the vertex array that stores the positions and colors of the cube vertices
        /// </summary>
        private void BuildIndices()
        {
            int i;

            // Create and initialize the indices
            _indices = new short[36];

            // Set the indices for the cube
            i = 0;
            // Front face...
            _indices[i++] = 0;
            _indices[i++] = 1;
            _indices[i++] = 2;
            _indices[i++] = 2;
            _indices[i++] = 1;
            _indices[i++] = 3;
            // Back face...
            _indices[i++] = 4;
            _indices[i++] = 5;
            _indices[i++] = 6;
            _indices[i++] = 5;
            _indices[i++] = 7;
            _indices[i++] = 6;
            // Left face...
            _indices[i++] = 8;
            _indices[i++] = 9;
            _indices[i++] = 10;
            _indices[i++] = 9;
            _indices[i++] = 11;
            _indices[i++] = 10;
            // Right face...
            _indices[i++] = 12;
            _indices[i++] = 13;
            _indices[i++] = 14;
            _indices[i++] = 14;
            _indices[i++] = 13;
            _indices[i++] = 15;
            // Top face...
            _indices[i++] = 16;
            _indices[i++] = 17;
            _indices[i++] = 18;
            _indices[i++] = 17;
            _indices[i++] = 19;
            _indices[i++] = 18;
            // Bottom face...
            _indices[i++] = 20;
            _indices[i++] = 21;
            _indices[i++] = 22;
            _indices[i++] = 21;
            _indices[i++] = 23;
            _indices[i++] = 22;
        }


    }
}
