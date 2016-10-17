using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace FireAndSmoke_Win8
{
    class GroundObject : GameFramework.MatrixObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        // Declare a static array of vertices.
        private static VertexPositionNormalTexture[] _vertices;

        //-------------------------------------------------------------------------------------
        // Class constructors

        public GroundObject(FireAndSmokeGame game, Texture2D texture)
            : base(game)
        {
            // Have we already built the ground vertex array in a previous instance?
            if (_vertices == null)
            {
                // No, so build it now
                BuildVertices();
            }

            // Set other object properties
            ObjectTexture = texture;
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
            // Now apply the standard transformations
            ApplyStandardTransformations();
        }

        /// <summary>
        /// Draw the ground
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
                // Draw the square
                effect.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length / 3);
            }
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
            _vertices[i++].Position = new Vector3(-15.0f, 0.0f, -15.0f);
            _vertices[i++].Position = new Vector3(15.0f, 0.0f, -15.0f);
            _vertices[i++].Position = new Vector3(-15.0f, 0.0f, 15.0f);
            _vertices[i++].Position = new Vector3(15.0f, 0.0f, -15.0f);
            _vertices[i++].Position = new Vector3(15.0f, 0.0f, 15.0f);
            _vertices[i++].Position = new Vector3(-15.0f, 0.0f, 15.0f);
            // Set the texture coordinates for the ground
            i = 0;
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(8.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 8.0f);
            _vertices[i++].TextureCoordinate = new Vector2(8.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(8.0f, 8.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 8.0f);
            // Set the normals
            for (i = 0; i < _vertices.Length; i++)
            {
                _vertices[i].Normal = new Vector3(0, 1, 0);
            }
        }


    }
}
