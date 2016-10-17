using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace GameFramework
{
    public class MatrixCameraObject : GameFramework.MatrixObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        private Vector3 _lastChaseCamDelta = new Vector3(0, 0, 1);

        //-------------------------------------------------------------------------------------
        // Class constructors

        public MatrixCameraObject(GameHost game)
            : base(game)
        {
            // Set default chase parameters
            ChaseDistance = 1;
            ChaseElevation = 0.1f;
        }


        //-------------------------------------------------------------------------------------
        // Properties

        /// <summary>
        /// A vector indicating the position toward which the camera is looking
        /// </summary>
        public Vector3 LookAtTarget { get; set; }

        /// <summary>
        /// An object that the camera will chase (null to disable the chase cam)
        /// </summary>
        public MatrixModelObject ChaseObject { get; set; }
        /// <summary>
        /// The distance that the camera should stay from the ChaseObject (-ve to look from in front)
        /// </summary>
        public float ChaseDistance { get; set; }
        /// <summary>
        /// The vertical from which the chase camera will look at the ChaseObject
        /// </summary>
        public float ChaseElevation { get; set; }

        //-------------------------------------------------------------------------------------
        // Object Functions

        /// <summary>
        /// Update the camera position and calculate its transformation matrix
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            Vector3 delta;

            base.Update(gameTime);

            // Do we have a chase object?
            if (ChaseObject == null)
            {
                // No, so simply apply the identity matrix
                // Calculate and apply the standard camera transformations
                SetIdentity();
                ApplyStandardTransformations();
                return;
            }

            // Find the vector between the current position and the chase object position
            delta = Position - ChaseObject.Position;
            // Normalize the delta vector
            delta.Normalize();
            // If the delta is zero (the camera position is already directly on the chase
            // object, which will happen if the object stops moving) retain the last used delta
            if (delta == Vector3.Zero)
            {
                delta = _lastChaseCamDelta;
            }
            else
            {
                // Store the delta for later use
                _lastChaseCamDelta = delta;
            }

            // Transform the camera position to position it relative to the chase object
            SetIdentity();
            // Translate to the chase object's position
            ApplyTransformation(Matrix.CreateTranslation(ChaseObject.Position));
            // Apply the chase distance. Are we in first- or third-person view?
            if (ChaseDistance != 0)
            {
                // Third person view
                // Translate towards or away from the object based on the ChaseDistance
                ApplyTransformation(Matrix.CreateTranslation(delta * ChaseDistance));
                // Apply the vertical offset
                ApplyTransformation(Matrix.CreateTranslation(0, ChaseElevation, 0));
            }
            else
            {
                // First person view
                // Translate a tiny distance back from the view point
                ApplyTransformation(Matrix.CreateTranslation(delta * 0.01f));
            }

            // Ensure that we are looking at the chase object
            LookAtTarget = ChaseObject.Position;

            // Set the camera position to exactly match the chase object position
            // so that we can continue to follow it in the next update
            Position = ChaseObject.Position;
        }

        /// <summary>
        /// Position the camera
        /// </summary>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Effect effect)
        {
            // This is a camera, so no drawing is required.
            // Instead we just set the effect's View matrix.
            ((BasicEffect)effect).View = Matrix.CreateLookAt(Transformation.Translation, LookAtTarget, Transformation.Up);
        }
    
    }
}
