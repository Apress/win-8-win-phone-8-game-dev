using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace GameFramework
{
    public abstract class MatrixObjectBase : GameObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class constructors

        public MatrixObjectBase(GameHost game)
            : base(game)
        {
            // Set the default object properties
            Transformation = Matrix.Identity;
            Scale = Vector3.One;
            ObjectColor = Color.White;
        }

        public MatrixObjectBase(GameHost game, Vector3 position)
            : this(game)
        {
            // Store the provided position
            Position = position;
        }

        public MatrixObjectBase(GameHost game, Vector3 position, Texture2D texture)
            : this(game, position)
        {
            // Store the provided texture
            ObjectTexture = texture;
        }

        //-------------------------------------------------------------------------------------
        // Properties

        /// <summary>
        /// A reference to the default texture used by this object
        /// </summary>
        public virtual Texture2D ObjectTexture { get; set; }

        /// <summary>
        /// A composite transformation that is being used by this object
        /// </summary>
        public virtual Matrix Transformation { get; set; }

        /// <summary>
        /// The object's X coordinate
        /// </summary>
        public virtual float PositionX { get; set; }
        /// <summary>
        /// The object's Y coordinate
        /// </summary>
        public virtual float PositionY { get; set; }
        /// <summary>
        /// The object's Z coordinate
        /// </summary>
        public virtual float PositionZ { get; set; }

        /// <summary>
        /// The object's rotation angle around the X axis (in radians)
        /// </summary>
        public virtual float AngleX { get; set; }
        /// <summary>
        /// The object's rotation angle around the Y axis (in radians)
        /// </summary>
        public virtual float AngleY { get; set; }
        /// <summary>
        /// The object's rotation angle around the Z axis (in radians)
        /// </summary>
        public virtual float AngleZ { get; set; }

        /// <summary>
        /// The object's X scale
        /// </summary>
        public virtual float ScaleX { get; set; }
        /// <summary>
        /// The object's Y scale
        /// </summary>
        public virtual float ScaleY { get; set; }
        /// <summary>
        /// The object's Z scale
        /// </summary>
        public virtual float ScaleZ { get; set; }

        /// <summary>
        /// The object's color
        /// </summary>
        public virtual Color ObjectColor { get; set; }


        /// <summary>
        /// The object's position represented as a Vector3 structure
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return new Vector3(PositionX, PositionY, PositionZ);
            }
            set
            {
                PositionX = value.X;
                PositionY = value.Y;
                PositionZ = value.Z;
            }
        }

        /// <summary>
        /// The object's rotation angles represented as a Vector3 structure
        /// </summary>
        public Vector3 Angle
        {
            get
            {
                return new Vector3(AngleX, AngleY, AngleZ);
            }
            set
            {
                AngleX = value.X;
                AngleY = value.Y;
                AngleZ = value.Z;
            }
        }

        /// <summary>
        /// The object's scale represented as a Vector3 structure
        /// </summary>
        public Vector3 Scale
        {
            get
            {
                return new Vector3(ScaleX, ScaleY, ScaleZ);
            }
            set
            {
                ScaleX = value.X;
                ScaleY = value.Y;
                ScaleZ = value.Z;
            }
        }


        //-------------------------------------------------------------------------------------
        // Game Functions

        /// <summary>
        /// Ensure that all derived objects implement a Draw method for a BasicEffect
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="effect"></param>
        public abstract void Draw(GameTime gameTime, Effect effect);


        //-------------------------------------------------------------------------------------
        // Object Functions

        /// <summary>
        /// Applies the standard translation, rotation (around the x, y and z axes)
        /// and scaling transformations (in that order) to the object's Transformation
        /// matrix.
        /// </summary>
        protected void ApplyStandardTransformations()
        {
            Matrix result;

            // First obtain the object's underlying transformation
            result = Transformation;

            // Apply the object position if any of the coordinates are non-zero
            if (PositionX != 0 || PositionY != 0 || PositionZ != 0)
            {
                // Yes, so apply the position to the current transformation
                result = Matrix.CreateTranslation(Position) * result;
            }

            // Rotate the object if any of the angles are non-zero
            if (AngleX != 0) result = Matrix.CreateRotationX(AngleX) * result;
            if (AngleY != 0) result = Matrix.CreateRotationY(AngleY) * result;
            if (AngleZ != 0) result = Matrix.CreateRotationZ(AngleZ) * result;

            // Scale the object if any of the scale values are set to a value other than 1
            if (ScaleX != 1 || ScaleY != 1 || ScaleZ != 1)
            {
                // Yes, so apply the Scale to the current transformation
                result = Matrix.CreateScale(Scale) * result;
            }

            // Store the final calculated matrix
            Transformation = result;
        }

        /// <summary>
        /// Reset the Transformation property to the identity matrix
        /// </summary>
        protected void SetIdentity()
        {
            Transformation = Matrix.Identity;
        }

        /// <summary>
        /// Apply a transformation to the matrix in the Transformation property
        /// </summary>
        /// <param name="newTransformation"></param>
        protected void ApplyTransformation(Matrix newTransformation)
        {
            Transformation = newTransformation * Transformation;
        }

        /// <summary>
        /// Prepare an effect so that it is ready for rendering
        /// </summary>
        /// <param name="effect"></param>
        protected void PrepareEffect(Effect effect)
        {
            // We have been passed a base effect object.
            // Determine its type and call the appropriate PrepareEffect overload.
            if (effect is BasicEffect)
            {
                PrepareEffect((BasicEffect)effect);
                return;
            }

            // Not a supported effect
            throw new NotSupportedException("Cannot prepare effects of type '" + effect.GetType().Name + "', not currently implemented.");
        }

        /// <summary>
        /// Prepare a BasicEffect for rendering
        /// </summary>
        /// <param name="effect"></param>
        protected void PrepareEffect(BasicEffect effect)
        {
            // Do we have a texture? Set the effect as required
            if (ObjectTexture == null)
            {
                // No testure so disable texturing
                effect.TextureEnabled = false;
            }
            else
            {
                // Enable texturing and set the texture into the effect
                effect.TextureEnabled = true;
                if (ObjectTexture != effect.Texture) effect.Texture = ObjectTexture;
            }

            // Set the color and alpha
            effect.DiffuseColor = ObjectColor.ToVector3();
            effect.Alpha = (float)ObjectColor.A / 255.0f;

            // Apply the transformation matrix
            effect.World = Transformation;

            // Now the effect is ready for the derived class to actually draw the object
        }

        public override bool IsPointInObject(Vector2 point)
        {
            // Not currently implemented for 3D objects
            return false;
        }

    }
}
