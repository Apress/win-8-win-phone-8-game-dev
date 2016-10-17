using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace SkyBox_Win8
{
    class CameraObject : GameFramework.MatrixCameraObject
    {

        //-------------------------------------------------------------------------------------
        // Class constructors

        public CameraObject(SkyBoxGame game)
            : base(game)
        {
        }


        //-------------------------------------------------------------------------------------
        // Object Functions

        /// <summary>
        /// Update the ground position and calculate its transformation matrix
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
            ApplyTransformation(Matrix.CreateTranslation(0, 1.25f, (float)Math.Sin(MathHelper.ToRadians(UpdateCount * 2)) * 3 - 10));

            // Look at the world origin
            LookAtTarget = Vector3.Zero;
        }

    }
}
