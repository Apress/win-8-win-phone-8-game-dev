using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace Lighting_WP8
{
    class CylinderObject : GameFramework.MatrixObjectBase
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

        public CylinderObject(LightingGame game)
            : base(game)
        {
            Scale = new Vector3(0.5f);

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

            // Update the object rotation angles
            AngleY += MathHelper.ToRadians(1.5f);
            AngleX += MathHelper.ToRadians(0.7f);

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
                effect.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _vertices.Length / 3);
            }
        }

        /// <summary>
        /// Build the vertex array that stores the positions and colors of the cube vertices
        /// </summary>
        private void BuildVertices()
        {
            int i;
            Color thisColor = Color.Black;
            float thisX, thisY, nextX, nextY;

            const int Segments = 20;

            // Create and initialize the vertices.
            // We need one triangle for the top of each segment,
            // one for the bottom, and two for the sides.
            // This is a total of (4 x 3 x Segments) vertices.
            _vertices = new VertexPositionNormalTexture[Segments * 4 * 3];

            // Set the vertex positions for the Cylinder
            i = 0;
            for (int s = 0; s < Segments; s++)
            {
                // Calculate the x and y coordinate for the edge of the cylinder at
                // the beginning and end of this segment
                thisX = (float)Math.Sin((float)s * MathHelper.TwoPi / Segments);
                thisY = (float)Math.Cos((float)s * MathHelper.TwoPi / Segments);
                nextX = (float)Math.Sin((float)(s + 1) * MathHelper.TwoPi / Segments);
                nextY = (float)Math.Cos((float)(s + 1) * MathHelper.TwoPi / Segments);

                // First create the top of the Cylinder.
                // The first point is always at the Cylinder top center point
                _vertices[i].Position = new Vector3(0, 0, 1.0f);
                // The next two points are at the top edge of the Cylinder, around its perimiter
                _vertices[i + 1].Position = new Vector3(thisX, thisY, 1.0f);
                _vertices[i + 2].Position = new Vector3(nextX, nextY, 1.0f);

                // Now we need to triangles to create a rectangular strip for the edge of the cone.
                _vertices[i + 3].Position = new Vector3(thisX, thisY, 1.0f);
                _vertices[i + 4].Position = new Vector3(thisX, thisY, -1.0f);
                _vertices[i + 5].Position = new Vector3(nextX, nextY, 1.0f);
                //
                _vertices[i + 6].Position = new Vector3(nextX, nextY, 1.0f);
                _vertices[i + 7].Position = new Vector3(thisX, thisY, -1.0f);
                _vertices[i + 8].Position = new Vector3(nextX, nextY, -1.0f);

                // Finally the bottom triangle
                _vertices[i + 9].Position = new Vector3(nextX, nextY, -1.0f);
                _vertices[i + 10].Position = new Vector3(thisX, thisY, -1.0f);
                _vertices[i + 11].Position = new Vector3(0, 0, -1.0f);

                // Set the vertex normals
                // First the top
                _vertices[i].Normal = new Vector3(0, 0, 1);
                _vertices[i + 1].Normal = new Vector3(0, 0, 1);
                _vertices[i + 2].Normal = new Vector3(0, 0, 1);
                // Now the two side triangles.
                // Use the position relative to the origin as the normal, too.
                // This will create smooth interpolated normals across each face of the cylinder.
                _vertices[i + 3].Normal = new Vector3(thisX, thisY, 0);
                _vertices[i + 4].Normal = new Vector3(thisX, thisY, 0);
                _vertices[i + 5].Normal = new Vector3(nextX, nextY, 0);
                _vertices[i + 6].Normal = new Vector3(nextX, nextY, 0);
                _vertices[i + 7].Normal = new Vector3(thisX, thisY, 0);
                _vertices[i + 8].Normal = new Vector3(nextX, nextY, 0);
                // Finally the bottom
                _vertices[i + 9].Normal = new Vector3(0, 0, -1);
                _vertices[i + 10].Normal = new Vector3(0, 0, -1);
                _vertices[i + 11].Normal = new Vector3(0, 0, -1);

                // Move to the next set of triangles
                i += 12;
            }
        }


    }
}
