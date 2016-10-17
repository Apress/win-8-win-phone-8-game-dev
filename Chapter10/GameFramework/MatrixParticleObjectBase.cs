using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace GameFramework
{
    public abstract class MatrixParticleObjectBase : MatrixObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class constructors

        public MatrixParticleObjectBase(GameHost game)
            : base(game)
        {
            // Default to active
            IsActive = true;
        }

        public MatrixParticleObjectBase(GameHost game, Texture2D texture, Vector3 position, Vector3 scale)
            : this(game)
        {
            ObjectTexture = texture;
            Position = position;
            Scale = scale;
        }

        //-------------------------------------------------------------------------------------
        // Properties

        /// <summary>
        /// Is this object active or dormant?
        /// </summary>
        public bool IsActive { get; set; }


        //-------------------------------------------------------------------------------------
        // Matrix functions


        /// <summary>
        /// Create a billboard matrix for the supplied parameters.
        /// </summary>
        /// <remarks>This is a wrapper around the MonoGame CreateBillboard overload that returns
        /// its billboard matrix using an "out" parameter rather than as its return value, as the
        /// overload with the return value is currently broken. Once the return-value overload is
        /// fixed in MonoGame, it can be called directly instead of requiring this function to be
        /// used.</remarks>
        protected Matrix CreateBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 cameraUpVector, Vector3 cameraForwardVector)
        {
            Matrix billboard;
            Matrix.CreateBillboard(ref objectPosition, ref cameraPosition, ref cameraUpVector, cameraForwardVector, out billboard);
            return billboard;
        }

    }
}
