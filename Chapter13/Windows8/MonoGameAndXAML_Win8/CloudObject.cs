using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace MonoGameAndXAML_Win8
{
    class CloudObject : GameFramework.SpriteObject
    {

        public CloudObject(GameHost game, Texture2D texture)
            : base(game, Vector2.Zero, texture)
        {
            // Set the sprite origin to the center of the sprite
            Origin = new Vector2(SpriteTexture.Width / 2, SpriteTexture.Height / 2);
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Move to the left
            PositionX -= 12 * ScaleX;

            if (PositionX < -SpriteTexture.Width * ScaleX * 0.5f)
            {
                // Reset to the right edge
                PositionX = Game.GraphicsDevice.Viewport.Width + (float)(SpriteTexture.Width * ScaleX * 0.5);
                // Randomize the vertical position
                PositionY = GameHelper.RandomNext(Game.GraphicsDevice.Viewport.Height);
            }
        }

    }
}
