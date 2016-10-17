using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace DualTexture_WP8
{
    class SquareObject : GameFramework.MatrixObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        // Declare a static array of vertices. As they will be the same
        // for every Square, this saves us having to use further vertex
        // memory for each SquareObject instance.
        private VertexPositionDualTexture[] _vertices;

        //-------------------------------------------------------------------------------------
        // Class constructors

        public SquareObject(DualTextureGame game, Vector3 position, Texture2D texture1, Texture2D texture2)
            : base(game)
        {
            // Have we already built the Square vertex array in a previous instance?
            if (_vertices == null)
            {
                // No, so build them now
                BuildVertices();
            }

            // Set object properties
            Position = position;
            ObjectTexture = texture1;
            ObjectTexture2 = texture2;
        }

        //-------------------------------------------------------------------------------------
        // Object Functions

        /// <summary>
        /// Update the Square position and calculate its transformation matrix
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update the object rotation angles
            AngleZ += MathHelper.ToRadians(0.4f);

            // Calculate the transformation matrix
            SetIdentity();
            ApplyStandardTransformations();


            // Update the texture coordinates
            for (int i = 0; i < _vertices.Length; i++)
            {
                _vertices[i].TexCoord0.X -= 0.009f;
                _vertices[i].TexCoord0.Y += 0.000f;

                _vertices[i].TexCoord1.X += 0.006f;
                _vertices[i].TexCoord1.Y += 0.003f;
            }

        }

        /// <summary>
        /// Draw the Square
        /// </summary>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Effect effect)
        {
            // Prepare the effect for drawing
            PrepareEffect(effect);

            // Draw the object
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                // Apply the pass
                pass.Apply();
                // Draw the object using the active vertex buffer
                effect.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _vertices, 0, 2);
            }
        }


        /// <summary>
        /// Build the vertex array that stores the positions and colors of the Square vertices
        /// </summary>
        private void BuildVertices()
        {
            int i;

            // Create and initialize the vertices
            _vertices = new VertexPositionDualTexture[6];

            // Set the vertex positions for a unit size Square.
            i = 0;
            // Front face...
            _vertices[i++].Position = new Vector3(-0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, -0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(-0.5f, 0.5f, 0.5f);
            _vertices[i++].Position = new Vector3(0.5f, 0.5f, 0.5f);

            // Set the texture coordinates
            i = 0;
            // Front face...
            _vertices[i++].TexCoord0 = new Vector2(1.0f, 0.0f);
            _vertices[i++].TexCoord0 = new Vector2(1.0f, 1.0f);
            _vertices[i++].TexCoord0 = new Vector2(0.0f, 0.0f);
            _vertices[i++].TexCoord0 = new Vector2(0.0f, 0.0f);
            _vertices[i++].TexCoord0 = new Vector2(1.0f, 1.0f);
            _vertices[i++].TexCoord0 = new Vector2(0.0f, 1.0f);

            // Set the texture coordinates
            i = 0;
            // Front face...
            _vertices[i++].TexCoord1 = new Vector2(0.9f, 0.0f);
            _vertices[i++].TexCoord1 = new Vector2(0.9f, 0.9f);
            _vertices[i++].TexCoord1 = new Vector2(0.0f, 0.0f);
            _vertices[i++].TexCoord1 = new Vector2(0.0f, 0.0f);
            _vertices[i++].TexCoord1 = new Vector2(0.9f, 0.9f);
            _vertices[i++].TexCoord1 = new Vector2(0.0f, 0.9f);
        }


    }
}
