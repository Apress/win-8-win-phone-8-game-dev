using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace ChaseCam_Win8
{
    class PaperPlaneObject : GameFramework.MatrixModelObject
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        // The spline calculation variables
        internal int _splineIndex = 0;
        internal float _splineWeight = 0;
        internal float _splineSpeed = 0.02f;

        // Points on the spline movement path
        static Vector3[] _movementPath = 
        {
            new Vector3(-1, 1.5f, -2),
            new Vector3(1.5f, 2.5f, 2),
            new Vector3(0, 1, 6),
            new Vector3(3, 0.5f, 6),
            new Vector3(4, 1, 2),
            new Vector3(0, 0.4f, 2),
            new Vector3(-4, 0.8f, 1),
            new Vector3(-5, 1.5f, 1),
            new Vector3(-4, 2.5f, -2),
            new Vector3(2, 2.0f, -4),
            new Vector3(4, 1.5f, -7),
            new Vector3(1, 1.0f, -7.2f),
            new Vector3(0, 0.5f, -6),
        };
        internal static int _movementPathLength = _movementPath.Length;



        //-------------------------------------------------------------------------------------
        // Class constructors

        public PaperPlaneObject(ChaseCamGame game, Vector3 position, Model model)
            : base(game, position, model)
        {
        }

        //-------------------------------------------------------------------------------------
        // Object Functions

        /// <summary>
        /// Update the ground position and calculate its transformation matrix
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            Vector3 nextPosition;
            Vector3 delta;

            // Allow the base class to do its work.
            // Do this first so that we can customize the transformation afterward.
            base.Update(gameTime);

            // Move along the spline path
            _splineWeight += _splineSpeed;
            // Have we reached the end of this path segment?
            if (_splineWeight >= 1)
            {
                // Yes, so reset and move to the next segment
                _splineWeight -= 1;
                _splineIndex += 1;
                // Have we reached the end of the whole path?
                if (_splineIndex >= _movementPath.Length)
                {
                    // Yes, so reset to the beginning
                    _splineIndex = 0;
                }
            }

            // Calculate the current position and store in the Position property
            Position = GetPlanePosition(_splineIndex, _splineWeight);

            // Calculate the next position too so we know which way we are moving
            nextPosition = GetPlanePosition(_splineIndex, _splineWeight + 0.1f);

            // Find the movement direction
            delta = nextPosition - Position;

            // Create the world matrix for the plane
            Transformation = Matrix.CreateWorld(Position, delta, Vector3.Up);
            // The plane needs to be rotated 90 degrees so that it points
            // forward, so apply a rotation
            ApplyTransformation(Matrix.CreateRotationY(MathHelper.ToRadians(-90)));
        }

        /// <summary>
        /// Draw the object
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="effect"></param>
        public override void Draw(GameTime gameTime, Effect effect)
        {
            if (Game.Camera.ChaseObject == this && Game.Camera.ChaseDistance == 0)
            {
                // First-person view, don't draw the plane
            }
            else
            {
                base.Draw(gameTime, effect);
            }
        }

        /// <summary>
        /// Calculate the position of the plane through its movement paths
        /// </summary>
        /// <param name="splineIndex">The first index of the four-point spline segment</param>
        /// <param name="splineWeight">The weight within the spline segment (0 = start, 1 = end)</param>
        /// <returns></returns>
        private Vector3 GetPlanePosition(int splineIndex, float splineWeight)
        {
            Vector3 ret;

            // If the weight exceeds 1, reduce by 1 and move to the next index
            if (splineWeight > 1)
            {
                splineWeight -= 1;
                splineIndex += 1;
            }
            // Keep the spline index within the array bounds
            splineIndex = splineIndex % _movementPath.Length;

            // Calculate the spline position
            ret = Vector3.CatmullRom(_movementPath[splineIndex],
                                _movementPath[(splineIndex + 1) % _movementPathLength],
                                _movementPath[(splineIndex + 2) % _movementPathLength],
                                _movementPath[(splineIndex + 3) % _movementPathLength],
                                splineWeight);

            return ret;
        }

    }
}
