using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace FireAndSmoke_Win8
{
    class FireParticleObject : GameFramework.MatrixParticleObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        private FireAndSmokeGame _game;

        // Declare a static array of vertices.
        private static VertexPositionNormalTexture[] _vertices;
        private static VertexBuffer _vertexBuffer;

        // The vertical speed of this particle
        private float _yVelocity;
        // The maximum permitted height of this particle
        private float _yMax;
        // The particle's z-axis rotation speed
        private float _rotateSpeed;

        //-------------------------------------------------------------------------------------
        // Class constructors

        public FireParticleObject(FireAndSmokeGame game, Texture2D texture, Vector3 position, Vector3 scale)
            : base(game, texture, position, scale)
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

            // Maximum height
            _yMax = GameHelper.RandomNext(0.4f, 0.8f);

            // Offset the position to randomize around the center of the fire
            PositionX += GameHelper.RandomNext(-0.1f, 0.1f);
            PositionY += GameHelper.RandomNext(0.0f, _yMax);
            PositionZ += GameHelper.RandomNext(-0.1f, 0.1f);

            // Random start speed
            _yVelocity = GameHelper.RandomNext(0.0f, 0.0002f);

            // Random angle
            AngleZ = GameHelper.RandomNext(0, MathHelper.TwoPi);
            _rotateSpeed = MathHelper.ToRadians(GameHelper.RandomNext(-5.0f, 5.0f));

            // Set a random color between yellow and red or red and white
            if (GameHelper.RandomNext(2) == 0)
            {
                ObjectColor = new Color(255, GameHelper.RandomNext(128, 256), 0);
            }
            else
            {
                ObjectColor = new Color(255, 255, GameHelper.RandomNext(256));
            }
        }

        //-------------------------------------------------------------------------------------
        // Object Functions

        /// <summary>
        /// Update the object position and calculate its transformation matrix
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update the object position and velocity
            PositionY += _yVelocity;
            _yVelocity += 0.0008f;

            // Fade away
            SetAlpha(ObjectColor.A - 4);

            // Have we reached our maximum height, or become invisible?
            if (PositionY > _yMax || ObjectColor.A == 0)
            {
                // Yes, so reset to the base of the fire
                PositionY -= _yMax;
                _yVelocity = GameHelper.RandomNext(0.0f, 0.0002f);
                SetAlpha(255);
            }

            // Rotate the object
            AngleZ += _rotateSpeed;

            // Calculate the transformation matrix
            SetIdentity();
            // Apply the billboard transformation
            ApplyTransformation(CreateBillboard(Position, Game.Camera.Transformation.Translation, Game.Camera.Transformation.Up, Game.Camera.Transformation.Forward));
            // Rotate and scale
            ApplyTransformation(Matrix.CreateRotationZ(AngleZ));
            ApplyTransformation(Matrix.CreateScale(Scale));

            // Create a smoke particle? We'll add this for 1% of the fire particle updates.
            // More fire therefore results in more smoke.
            if (GameHelper.RandomNext(100) == 0)
            {
                AddSmokeParticle();
            }
        }


        /// <summary>
        /// Add a smoke particle -- either a new object or a recycled existing object
        /// </summary>
        private void AddSmokeParticle()
        {
            // First look for an inactive particle that we can re-use
            foreach (GameObjectBase obj in _game.GameObjects)
            {
                // Is this an inactive smoke particle?
                if (obj is SmokeParticleObject && ((SmokeParticleObject)obj).IsActive == false)
                {
                    // Yes, so reset it and return it
                    ((SmokeParticleObject)obj).ResetParticle();
                    return;
                }
            }

            // Couldn't find an inactive particle so create a new one
            _game.GameObjects.Add(new SmokeParticleObject(_game, Game.Textures["Smoke"], Vector3.Zero, new Vector3(0.08f)));
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
