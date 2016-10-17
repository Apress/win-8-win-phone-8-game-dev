using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace GameFrameworkExample_Win8
{
    class TexturedSquareObject : GameFramework.MatrixObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        private VertexPositionTexture[] _vertices;

        //-------------------------------------------------------------------------------------
        // Class constructors

        public TexturedSquareObject(GameFrameworkExampleGame game, Vector3 position, Texture2D texture, float scale)
            : base(game)
        {
            // Store parameter values
            Position = position;
            ObjectTexture = texture;
            Scale = new Vector3(scale);

            // Create and initialize the vertices
            _vertices = new VertexPositionTexture[4];

            // Set the vertex positions for a unit size square
            _vertices[0].Position = new Vector3(-0.5f, -0.5f, 0);
            _vertices[1].Position = new Vector3(-0.5f,  0.5f, 0);
            _vertices[2].Position = new Vector3( 0.5f, -0.5f, 0);
            _vertices[3].Position = new Vector3( 0.5f,  0.5f, 0);
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

            // Rotate around the Z axis
            AngleZ += MathHelper.ToRadians(2);

            // Calculate the transformation matrix
            SetIdentity();
            ApplyTransformation(Matrix.CreateRotationZ(AngleZ));
            ApplyTransformation(Matrix.CreateScale(Scale));
            ApplyTransformation(Matrix.CreateTranslation(Position));
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
