using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace ImportingGeometry_Win8
{
    class ImportedObject : GameFramework.MatrixModelObject
    {

        //-------------------------------------------------------------------------------------
        // Class constructors

        public ImportedObject(ImportingGeometryGame game, Vector3 position, Model model)
            : base(game, position, model)
        {
        }

        //-------------------------------------------------------------------------------------
        // Object Functions

        /// <summary>
        /// Update the object position and calculate its transformation matrix
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Update the object rotation angles
            AngleY += MathHelper.ToRadians(2);
            AngleZ += MathHelper.ToRadians(0.4f);

            // The base class can take care of the rest of the processing
            base.Update(gameTime);
        }

    }
}
