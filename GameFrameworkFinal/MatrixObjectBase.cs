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
            SpecularColor = Color.Black;
            SpecularPower = 1;
            EmissiveColor = Color.Black;
            EnvironmentMapAmount = 1;
            EnvironmentMapSpecular = Color.Black;
            FresnelFactor = 0;
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
        /// A reference to a second texture used by this object (for DualTextureEffect rendering)
        /// </summary>
        public virtual Texture2D ObjectTexture2 { get; set; }

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
        /// The object's specular color
        /// </summary>
        public virtual Color SpecularColor { get; set; }

        /// <summary>
        /// The object's specular power
        /// </summary>
        public virtual float SpecularPower { get; set; }

        /// <summary>
        /// The object's emissive light color
        /// </summary>
        public virtual Color EmissiveColor { get; set; }

        /// <summary>
        /// The object's environment map amount for when working with the EnvironmentMapEffect
        /// </summary>
        public virtual float EnvironmentMapAmount { get; set; }

        /// <summary>
        /// The object's environment map specular level for when working with the EnvironmentMapEffect
        /// </summary>
        public virtual Color EnvironmentMapSpecular { get; set; }

        /// <summary>
        /// The object's fresnel factor for when working with the EnvironmentMapEffect
        /// </summary>
        public virtual float FresnelFactor { get; set; }



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
            if (effect is AlphaTestEffect)
            {
                PrepareEffect((AlphaTestEffect)effect);
                return;
            }
            if (effect is DualTextureEffect)
            {
                PrepareEffect((DualTextureEffect)effect);
                return;
            }
            if (effect is EnvironmentMapEffect)
            {
                PrepareEffect((EnvironmentMapEffect)effect);
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
            SetEffectTexture(effect, ObjectTexture);

            // Set the color and alpha
            effect.DiffuseColor = ObjectColor.ToVector3();
            effect.SpecularColor = SpecularColor.ToVector3();
            effect.SpecularPower = SpecularPower;
            effect.EmissiveColor = EmissiveColor.ToVector3();
            effect.Alpha = (float)ObjectColor.A / 255.0f;

            // Apply the transformation matrix
            effect.World = Transformation;

            // Now the effect is ready for the derived class to actually draw the object
        }

        /// <summary>
        /// Prepare a BasicEffect for rendering
        /// </summary>
        /// <param name="effect"></param>
        protected void PrepareEffect(AlphaTestEffect effect)
        {
            if (effect.Texture != ObjectTexture) effect.Texture = ObjectTexture;

            // Set the color and alpha
            effect.DiffuseColor = ObjectColor.ToVector3();
            effect.Alpha = (float)ObjectColor.A / 255.0f;

            // Apply the transformation matrix
            effect.World = Transformation;

            // Now the effect is ready for the derived class to actually draw the object
        }

        /// <summary>
        /// Prepare a BasicEffect for rendering
        /// </summary>
        /// <param name="effect"></param>
        protected void PrepareEffect(DualTextureEffect effect)
        {
            if (effect.Texture != ObjectTexture) effect.Texture = ObjectTexture;
            if (effect.Texture2 != ObjectTexture2) effect.Texture2 = ObjectTexture2;

            // Set the color and alpha
            effect.DiffuseColor = ObjectColor.ToVector3();
            effect.Alpha = (float)ObjectColor.A / 255.0f;

            // Apply the transformation matrix
            effect.World = Transformation;

            // Now the effect is ready for the derived class to actually draw the object
        }

        /// <summary>
        /// Prepare a BasicEffect for rendering
        /// </summary>
        /// <param name="effect"></param>
        protected void PrepareEffect(EnvironmentMapEffect effect)
        {
            // Apply the texture if we have one and it differs from the active texture
            if (effect.Texture != ObjectTexture && ObjectTexture != null) effect.Texture = ObjectTexture;

            // Set the color and alpha
            effect.DiffuseColor = ObjectColor.ToVector3();
            effect.EmissiveColor = EmissiveColor.ToVector3();
            effect.Alpha = (float)ObjectColor.A / 255.0f;

            // Set other effect properties
            // Do we have a valid texture?
            if (effect.Texture != null)
            {
                // Yes, so apply the other environment map properties as normal
                effect.EnvironmentMapAmount = EnvironmentMapAmount;
                effect.EnvironmentMapSpecular = EnvironmentMapSpecular.ToVector3();
                effect.FresnelFactor = FresnelFactor;
            }
            else
            {
                // No, so make the object completely reflective.
                // Any texture set by other objects will remain but will be entirely hidden by the environment map
                effect.EnvironmentMapAmount = 1.0f;
                effect.EnvironmentMapSpecular = Vector3.Zero;
                effect.FresnelFactor = 0.0f;
            }

            // Apply the transformation matrix
            effect.World = Transformation;

            // Now the effect is ready for the derived class to actually draw the object
        }

        /// <summary>
        /// Set the texture for an effect
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="texture"></param>
        protected void SetEffectTexture(BasicEffect effect, Texture2D texture)
        {
            // Do we have a texture? Set the effect as required
            if (texture == null)
            {
                // No testure so disable texturing
                effect.TextureEnabled = false;
            }
            else
            {
                // Enable texturing and set the texture into the effect
                effect.TextureEnabled = true;
                if (texture != effect.Texture) effect.Texture = texture;
            }
        }


        public override bool IsPointInObject(Vector2 point)
        {
            // Not currently implemented for 3D objects
            return false;
        }


        /// <summary>
        /// Calculate the normals for the supplied TriangleList vertices
        /// </summary>
        public void CalculateVertexNormals(VertexPositionNormalTexture[] vertices)
        {
            short[] indices;
            short i;

            // Build an array that allows us to treat the vertices as if they were indexed.
            // As the triangles are drawn sequentially, the indexes are actually just
            // an increasing sequence of numbers: the first triangle is formed from
            // vertices 0, 1 and 2, the second triangle from vertices 3, 4 and 5, etc.

            // First create the array with an element for each vertex
            indices = new short[vertices.Length];

            // Then set the elements within the array so that each contains
            // the next sequential vertex index
            for (i = 0; i < indices.Length; i++)
            {
                indices[i] = i;
            }

            // Finally delegate to the other overload to do the work
            CalculateVertexNormals(vertices, indices);
        }

        /// <summary>
        /// Calculate the normals for the supplied indexed TriangleList vertices
        /// </summary>
        public void CalculateVertexNormals(VertexPositionNormalTexture[] vertices, short[] indices)
        {
            // Vectors to describe the relationships between the vertices of the triangle
            // being processed
            Vector3 vectora;
            Vector3 vectorb;
            // The resulting normal vector
            Vector3 normal;

            // Loop for each triangle (each triangle uses three indices)
            for (int index = 0; index < indices.Length; index += 3)
            {
                // Create the a and b vectors from the vertex positions
                // First the a vector from vertices 2 and 1
                vectora = vertices[index + 2].Position - vertices[index + 1].Position;
                // Next the b vector from vertices 1 and 0
                vectorb = vertices[index + 1].Position - vertices[index + 0].Position;

                // Calculate the normal as the cross product of the two vectors
                normal = Vector3.Cross(vectora, vectorb);

                // Normalize the normal
                normal.Normalize();

                // Write the normal back into all three of the triangle vertices
                vertices[index].Normal = normal;
                vertices[index+1].Normal = normal;
                vertices[index+2].Normal = normal;
            }
        }

    }
}
