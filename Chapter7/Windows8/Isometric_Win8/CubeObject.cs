using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace Isometric_Win8
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

        // The sin position to allow the scale to be manipulated
        private float _scalePos;

        //-------------------------------------------------------------------------------------
        // Class constructors

        public CubeObject(IsometricGame game, Vector3 position, Color objectColor)
            : base(game)
        {
            // Set properties
            Position = position;
            ObjectColor = objectColor;

            _scalePos = GameHelper.RandomNext(0, MathHelper.TwoPi);

            // Have we already built the cube vertex array in a previous instance?
            if (_vertices == null)
            {
                // No, so build them now
                BuildVertices();
                // Create a vertex buffer
                _vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionNormalTexture), _vertices.Length, BufferUsage.WriteOnly);
                _vertexBuffer.SetData(_vertices);
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

            // Update the object scale
            _scalePos += MathHelper.ToRadians(5);
            ScaleY = (float)Math.Sin(_scalePos) + 1.1f;

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

            // Set the vertex normals
            i = 0;
            // Front face...
            _vertices[i++].Normal = new Vector3(0, 0, 1);
            _vertices[i++].Normal = new Vector3(0, 0, 1);
            _vertices[i++].Normal = new Vector3(0, 0, 1);
            _vertices[i++].Normal = new Vector3(0, 0, 1);
            _vertices[i++].Normal = new Vector3(0, 0, 1);
            _vertices[i++].Normal = new Vector3(0, 0, 1);
            // Back face...
            _vertices[i++].Normal = new Vector3(0, 0, -1);
            _vertices[i++].Normal = new Vector3(0, 0, -1);
            _vertices[i++].Normal = new Vector3(0, 0, -1);
            _vertices[i++].Normal = new Vector3(0, 0, -1);
            _vertices[i++].Normal = new Vector3(0, 0, -1);
            _vertices[i++].Normal = new Vector3(0, 0, -1);
            // Left face...
            _vertices[i++].Normal = new Vector3(-1, 0, 0);
            _vertices[i++].Normal = new Vector3(-1, 0, 0);
            _vertices[i++].Normal = new Vector3(-1, 0, 0);
            _vertices[i++].Normal = new Vector3(-1, 0, 0);
            _vertices[i++].Normal = new Vector3(-1, 0, 0);
            _vertices[i++].Normal = new Vector3(-1, 0, 0);
            // Right face...
            _vertices[i++].Normal = new Vector3(1, 0, 0);
            _vertices[i++].Normal = new Vector3(1, 0, 0);
            _vertices[i++].Normal = new Vector3(1, 0, 0);
            _vertices[i++].Normal = new Vector3(1, 0, 0);
            _vertices[i++].Normal = new Vector3(1, 0, 0);
            _vertices[i++].Normal = new Vector3(1, 0, 0);
            // Top face...
            _vertices[i++].Normal = new Vector3(0, 1, 0);
            _vertices[i++].Normal = new Vector3(0, 1, 0);
            _vertices[i++].Normal = new Vector3(0, 1, 0);
            _vertices[i++].Normal = new Vector3(0, 1, 0);
            _vertices[i++].Normal = new Vector3(0, 1, 0);
            _vertices[i++].Normal = new Vector3(0, 1, 0);
            // Bottom face...
            _vertices[i++].Normal = new Vector3(0, -1, 0);
            _vertices[i++].Normal = new Vector3(0, -1, 0);
            _vertices[i++].Normal = new Vector3(0, -1, 0);
            _vertices[i++].Normal = new Vector3(0, -1, 0);
            _vertices[i++].Normal = new Vector3(0, -1, 0);
            _vertices[i++].Normal = new Vector3(0, -1, 0);

            // Programmatically calculate the vertex normals
            //CalculateVertexNormals(_vertices);

        }


    }
}
