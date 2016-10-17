using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace CameraMovement_Win8
{
    class CameraObject : GameFramework.MatrixCameraObject
    {

        //-------------------------------------------------------------------------------------
        // Class constructors

        public CameraObject(CameraMovementGame game)
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
            AngleY += MathHelper.ToRadians(1.0f);

            // Method 1: set the camera position as a Vector3
            // Reset the position using the identity matrix
            SetIdentity();
            // Calculate the camera position
            Position = new Vector3((float)Math.Sin(AngleY) * 14, 5, (float)Math.Cos(AngleY) * 14);
            // Apply the standard transformations to the object
            ApplyStandardTransformations();
            // Calculate where the camera is looking
            LookAtTarget = new Vector3((float)Math.Sin(AngleY + MathHelper.PiOver2) * 4, 1, (float)Math.Cos(AngleY + MathHelper.PiOver2) * 4);

            //// Method 2: transform the camera using matrix calculations
            //// Reset the position using the identity matrix
            //SetIdentity();
            //// Rotate the camera
            //ApplyTransformation(Matrix.CreateRotationY(AngleY));
            //// Translate the camera away from the origin
            //ApplyTransformation(Matrix.CreateTranslation(0, 0, -14));
            //// Translate the camera a little way upwards to give it some height.
            //// Use the rotation angle to calculate the height so that the camera
            //// moves up and down.
            //ApplyTransformation(Matrix.CreateTranslation(0, (float)Math.Sin(AngleY) * 5 + 5.1f, 0));
        }

    }
}
