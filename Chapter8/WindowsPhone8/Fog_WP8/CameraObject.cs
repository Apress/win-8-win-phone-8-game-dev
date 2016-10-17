using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace Fog_WP8
{
    class CameraObject : GameFramework.MatrixCameraObject
    {

        //-------------------------------------------------------------------------------------
        // Class constructors

        public CameraObject(FogGame game)
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

            // Reset the position using the identity matrix
            SetIdentity();
            // Rotate the camera
            ApplyTransformation(Matrix.CreateRotationY(AngleY));
            // Translate the camera away from the origin
            ApplyTransformation(Matrix.CreateTranslation(0, 0, -10));
            // Translate the camera a little way upwards to give it some height.
            // Use the rotation angle to calculate the height so that the camera
            // moves up and down.
            ApplyTransformation(Matrix.CreateTranslation(0, 1, 0));

            LookAtTarget = Vector3.Zero;
        }

    }
}
