using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Perspective_Win8
{
    class TexturedSquareObject : GameFramework.MatrixObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        private VertexPositionTexture[] _vertices;

        //-------------------------------------------------------------------------------------
        // Class constructors

        public TexturedSquareObject(PerspectiveGame game, Texture2D texture)
            : base(game)
        {
            // Store parameter values
            Position = new Vector3(GameHelper.RandomNext(-5.0f, 5.0f), GameHelper.RandomNext(-5.0f, 5.0f), GameHelper.RandomNext(-100, 0));
            ObjectTexture = texture;
            Scale = new Vector3(0.5f);

            // Create and initialize the vertices
            _vertices = new VertexPositionTexture[4];

            // Set the vertex positions for a unit size square
            _vertices[0].Position = new Vector3(-0.5f, -0.5f, 0);
            _vertices[1].Position = new Vector3(-0.5f, 0.5f, 0);
            _vertices[2].Position = new Vector3(0.5f, -0.5f, 0);
            _vertices[3].Position = new Vector3(0.5f, 0.5f, 0);
            // Set the texture coordinates to display the entire texture
            _vertices[0].TextureCoordinate = new Vector2(0, 1);
            _vertices[1].TextureCoordinate = new Vector2(0, 0);
            _vertices[2].TextureCoordinate = new Vector2(1, 1);
            _vertices[3].TextureCoordinate = new Vector2(1, 0);
        }

        //-------------------------------------------------------------------------------------
        // Object Functions

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Move the object towards the camera
            PositionZ += 0.25f;
            // If we reach the camera, reset back into the distance
            if (PositionZ > 0) PositionZ -= 100;

            // Calculate the transformation matrix
            SetIdentity();
            ApplyStandardTransformations();
        }

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
                effect.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, _vertices, 0, 2);
            }
        }

    }
}
