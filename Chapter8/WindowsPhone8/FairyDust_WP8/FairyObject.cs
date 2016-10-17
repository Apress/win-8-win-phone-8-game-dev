using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace FairyDust_WP8
{
    class FairyObject : GameFramework.MatrixParticleObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        private FairyDustGame _game;

        // Declare a static array of vertices.
        private static VertexPositionNormalTexture[] _vertices;
        private static VertexBuffer _vertexBuffer;

        // Object values
        private float _zRotation;

        //-------------------------------------------------------------------------------------
        // Class constructors

        public FairyObject(FairyDustGame game, Texture2D texture, float zRotation)
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

            _zRotation = zRotation;
        }

        //-------------------------------------------------------------------------------------
        // Object Functions

        /// <summary>
        /// Update the ground position and calculate its transformation matrix
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Return immediately if we're not active
            if (!IsActive) return;

            PositionX = (float)Math.Sin(MathHelper.ToRadians(UpdateCount)) * 4;
            PositionZ = (float)Math.Cos(MathHelper.ToRadians(UpdateCount)) * 4;
            PositionY = (float)Math.Cos(MathHelper.ToRadians(UpdateCount * 0.5f)) + 2;

            AngleZ += _zRotation;

            // Update the alpha
            SetAlpha(GameHelper.RandomNext(230, 255));

            // Calculate the transformation matrix
            SetIdentity();
            // Apply the billboard transformation
            ApplyTransformation(CreateBillboard(Position, Game.Camera.Transformation.Translation, Game.Camera.Transformation.Up, Game.Camera.Transformation.Forward));
            // Rotate
            ApplyTransformation(Matrix.CreateRotationZ(AngleZ));

            // Add some fairy dust
            AddFairyDustParticle();
        }

        /// <summary>
        /// Add a FairyDust particle -- either a new object or a recycled existing object
        /// </summary>
        private void AddFairyDustParticle()
        {
            // First look for an inactive particle that we can re-use
            foreach (GameObjectBase obj in Game.GameObjects)
            {
                // Is this an inactive FairyDust particle?
                if (obj is FairyDustObject && ((FairyDustObject)obj).IsActive == false)
                {
                    // Yes, so reset it and return it
                    ((FairyDustObject)obj).ResetParticle(Position);
                    return;
                }
            }

            // Couldn't find an inactive particle so create a new one
            Game.GameObjects.Add(new FairyDustObject((FairyDustGame)Game, Game.Textures["FairyDust"], Position));
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
