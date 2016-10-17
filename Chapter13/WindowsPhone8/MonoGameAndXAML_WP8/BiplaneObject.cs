using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace MonoGameAndXAML_WP8
{
    class BiplaneObject : GameFramework.SpriteObject
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        //-------------------------------------------------------------------------------------
        // Class constructors

        public BiplaneObject(GameHost game, Texture2D texture)
            : base(game, Vector2.Zero, texture)
        {
            // Set the sprite origin to the center of the sprite
            Origin = new Vector2(SpriteTexture.Width / 2, SpriteTexture.Height / 4);
        }



        //-------------------------------------------------------------------------------------
        // Game functions

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Move up the screen (speed based on the balloon scale)
            PositionY += (float)Math.Sin((float)UpdateCount * 0.025f);
            PositionX += (float)Math.Cos((float)UpdateCount * 0.02f);

            if (UpdateCount % 4 >= 2)
            {
                SourceRect = new Rectangle(0, 0, SpriteTexture.Width, SpriteTexture.Height / 2);
            }
            else
            {
                SourceRect = new Rectangle(0, SpriteTexture.Height / 2, SpriteTexture.Width, SpriteTexture.Height / 2);
            }

        }

    


    }
}
