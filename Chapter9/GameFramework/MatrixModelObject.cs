using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace GameFramework
{
    public class MatrixModelObject : MatrixObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class constructors

        public MatrixModelObject(GameHost game)
            : base(game)
        {
        }

        public MatrixModelObject(GameHost game, Vector3 position, Model model)
            : this(game)
        {
            // Store the provided parameter values
            Position = position;
            ObjectModel = model;
        }


        //-------------------------------------------------------------------------------------
        // Properties

        /// <summary>
        /// The model being used by this object
        /// </summary>
        public virtual Model ObjectModel { get; set; }

        /// <summary>
        /// Override ObjectTexture to return the first object from the assigned
        /// model if one is present. If a texture is explicitly set, this will
        /// take precedence and be returned instead.
        /// </summary>
        public override Texture2D ObjectTexture
        {
            get
            {
                // Do we have an explicitly set texture?
                if (base.ObjectTexture != null) return base.ObjectTexture;

                if (ObjectModel == null || ObjectModel.Meshes.Count == 0 || ObjectModel.Meshes[0].MeshParts.Count == 0)
                {
                    // No valid model available
                    return null;
                }

                // Return the first-used texture from the model
                return ((BasicEffect)ObjectModel.Meshes[0].MeshParts[0].Effect).Texture;
            }
        }


        //-------------------------------------------------------------------------------------
        // Object functions

        /// <summary>
        /// Update the object position and calculate its transformation matrix
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Apply an identity transformation matrix
            SetIdentity();
            ApplyStandardTransformations();
        }

        /// <summary>
        /// Draw the object using its default configuration
        /// </summary>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Effect effect)
        {
            // Prepare the effect for drawing
            PrepareEffect(effect);

            // Draw the model
            DrawModel((BasicEffect)effect);
        }


        /// <summary>
        /// Draw the loaded model using the provided effect
        /// </summary>
        /// <param name="effect"></param>
        protected virtual void DrawModel(BasicEffect effect)
        {
            Matrix initialWorld;
            Matrix[] boneTransforms;

            // Ensure we have a model to draw
            if (ObjectModel == null) return;

            // Store the initial world matrix
            initialWorld = effect.World;

            // Build an array of the absolute bone transformation matrices
            boneTransforms = new Matrix[ObjectModel.Bones.Count];
            ObjectModel.CopyAbsoluteBoneTransformsTo(boneTransforms);

            // Loop for each mesh
            foreach (ModelMesh mesh in ObjectModel.Meshes)
            {
                // Update the world matrix to account for the position of this bone
                effect.World = boneTransforms[mesh.ParentBone.Index] * initialWorld;

                // Loop for each mesh part
                foreach (ModelMeshPart meshpart in mesh.MeshParts)
                {
                    // Set the texture for this meshpart unless we have been explicitly
                    // given a texture to use
                    if (base.ObjectTexture != null)
                    {
                        SetEffectTexture(effect, base.ObjectTexture);
                    }
                    else
                    {
                        SetEffectTexture(effect, ((BasicEffect)meshpart.Effect).Texture);
                    }
                    // Set the vertex and index buffers
                    effect.GraphicsDevice.SetVertexBuffer(meshpart.VertexBuffer);
                    effect.GraphicsDevice.Indices = meshpart.IndexBuffer;

                    // Draw the mesh part
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        // Apply the pass
                        pass.Apply();
                        // Draw this meshpart
                        effect.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, meshpart.VertexOffset, 0, meshpart.NumVertices, meshpart.StartIndex, meshpart.PrimitiveCount);
                    }
                }
            }

            // Restore the initial world matrix
            effect.World = initialWorld;
        }

    }
}
