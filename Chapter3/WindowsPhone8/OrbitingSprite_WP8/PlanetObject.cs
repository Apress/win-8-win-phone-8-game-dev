using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace OrbitingSprite_WP8
{
    class PlanetObject : SpriteObject
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        // The horizontal movement speed of the object
        private float _xspeed = GameHelper.RandomNext(1.0f, 3.0f);

        //-------------------------------------------------------------------------------------
        // Class constructors

        public PlanetObject(OrbitingSpriteGame game, Vector2 position, Texture2D texture, float size)
            : base(game, position, texture)
        {
            // Set the origin to the center of the planet
            Origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Scale = new Vector2(size, size);
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Add the movement speed to the position
            PositionX += _xspeed;

            // Bounce off the sides of the window
            if (BoundingBox.Right >= Game.GraphicsDevice.Viewport.Bounds.Width && _xspeed > 0)
            {
                _xspeed = -_xspeed;
            }
            if (BoundingBox.Left <= 0 && _xspeed < 0)
            {
                _xspeed = -_xspeed;
            }
        }

    }
}
