using GameFramework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace CosmicRocksPartII_Win8
{
    internal class StarObject : GameFramework.SpriteObject
    {

        // A strongly typed reference to the game
        private CosmicRocksPartIIGame _game;

        private float _scaleMin;
        private float _scaleMax;
        private float _scale;

        //-------------------------------------------------------------------------------------
        // Class constructors

        internal StarObject(CosmicRocksPartIIGame game, Texture2D texture)
            : base(game, Vector2.Zero, texture)
        {
            // Store a strongly-typed reference to the game
            _game = game;

            // Set a random position
            PositionX = GameHelper.RandomNext(0, _game.GraphicsDevice.Viewport.Bounds.Width);
            PositionY = GameHelper.RandomNext(0, _game.GraphicsDevice.Viewport.Bounds.Height);

            // Set the origin
            Origin = new Vector2(texture.Width, texture.Height) / 2;

            // Set the scale range
            _scaleMin = GameHelper.RandomNext(0.1f, 0.6f);
            _scaleMax = _scaleMin + GameHelper.RandomNext(0.1f, 0.4f);
        }


        //-------------------------------------------------------------------------------------
        // Game functions


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _scale = GameHelper.RandomNext(_scaleMin, _scaleMax);

        }

        public override float ScaleX
        {
            get { return _scale; }
        }
        public override float ScaleY
        {
            get { return _scale; }
        }



    }
}
