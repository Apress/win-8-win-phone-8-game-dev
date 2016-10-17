using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace OrbitingSprite_Win8
{
    class MoonObject : SpriteObject
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        // The sprite around which the moon is orbiting
        private SpriteObject _targetObject;
        private float _speed;
        private float _distance;

        //-------------------------------------------------------------------------------------
        // Class constructors

        public MoonObject(OrbitingSpriteGame game, Texture2D texture, SpriteObject targetObject, float speed, float distance, float size, Color color)
            : base(game, Vector2.Zero, texture)
        {
            // Apply the constructor parameters
            _targetObject = targetObject;
            _speed = speed;
            _distance = distance;
            Scale = new Vector2(size, size);
            SpriteColor = color;
            // Set the origin to the center of the moon
            Origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        /// <summary>
        /// Calculate the x position of the moon
        /// </summary>
        public override float PositionX
        {
            get
            {
                return _targetObject.PositionX + (float)Math.Sin(UpdateCount * _speed) * _distance;
            }
        }

        /// <summary>
        /// Calculate the y position of the moon
        /// </summary>
        public override float PositionY
        {
            get
            {
                return _targetObject.PositionY + (float)Math.Cos(UpdateCount * _speed) * _distance;
            }
        }

        /// <summary>
        /// Calculate the angle of the moon
        /// </summary>
        public override float Angle
        {
            get
            {
                return -UpdateCount * _speed * 0.25f;
            }
        }

    }
}
