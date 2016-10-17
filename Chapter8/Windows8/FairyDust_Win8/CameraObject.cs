using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace FairyDust_Win8
{
    class CameraObject : GameFramework.MatrixCameraObject
    {

        //-------------------------------------------------------------------------------------
        // Class constructors

        public CameraObject(FairyDustGame game)
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
            // Translate the camera away from the origin
            ApplyTransformation(Matrix.CreateTranslation(0, 4, -10));

            // Look at the origin
            LookAtTarget = Vector3.Zero;
        }

    }
}
