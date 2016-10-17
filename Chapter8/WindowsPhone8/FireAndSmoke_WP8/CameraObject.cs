using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace FireAndSmoke_WP8
{
    class CameraObject : GameFramework.MatrixCameraObject
    {

        //-------------------------------------------------------------------------------------
        // Class constructors

        public CameraObject(FireAndSmokeGame game)
            : base(game)
        {
        }


        //-------------------------------------------------------------------------------------
        // Object Functions

        /// <summary>
        /// Update the camera position and calculate its transformation matrix
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Increase the y axis rotation angle for our camera transformation
            AngleY += MathHelper.ToRadians(0.2f);

            // Reset the position using the identity matrix
            SetIdentity();
            // Rotate the camera
            ApplyTransformation(Matrix.CreateRotationY(AngleY));
            // Set the camera position
            ApplyTransformation(Matrix.CreateTranslation(0, 0.7f, 2));

            // Look at the world origin
            LookAtTarget = new Vector3(0, 0.6f, 0);
        }

    }
}
