using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace GameFramework
{
    public class BenchmarkObject : TextObject
    {

        //-------------------------------------------------------------------------------------
        // Class variables
        private double _lastUpdateMilliseconds;
        private int _drawCount;
        private int _lastDrawCount;
        private int _lastUpdateCount;

        // Create a single StringBuilder instance to avoid creating objects each update
        private StringBuilder _strBuilder = new StringBuilder();

        //-------------------------------------------------------------------------------------
        // Class constructors

        public BenchmarkObject(GameHost game, SpriteFont font, Vector2 position, Color textColor)
            : base(game, font, position)
        {
            SpriteColor = textColor;
        }


        public override void Update(GameTime gameTime)
        {
            int newDrawCount;
            int newUpdateCount;
            double newElapsedTime;

            // Allow the base class to do its stuff
            base.Update(gameTime);

            // Has 1 second passed since we last updated the text?
            if (gameTime.TotalGameTime.TotalMilliseconds > _lastUpdateMilliseconds + 1000)
            {
                // Find how many frames have been drawn within the last second
                newDrawCount = _drawCount - _lastDrawCount;
                // Find how many updates have taken place within the last second
                newUpdateCount = UpdateCount - _lastUpdateCount;
                // Find out exactly how much time has passed
                newElapsedTime = gameTime.TotalGameTime.TotalMilliseconds - _lastUpdateMilliseconds;

                // Build a message to display the details and set it into the Text property
                _strBuilder.Length = 0;
                _strBuilder.AppendLine("Object count: " + Game.GameObjects.Count.ToString());
                _strBuilder.AppendLine("Frames per second: " + ((float)newDrawCount / newElapsedTime * 1000).ToString("0.0"));
                _strBuilder.AppendLine("Updates per second: " + ((float)newUpdateCount / newElapsedTime * 1000).ToString("0.0"));
                Text = _strBuilder.ToString();

                // Update the counters for use the next time we calculate
                _lastUpdateMilliseconds = gameTime.TotalGameTime.TotalMilliseconds;
                _lastDrawCount = _drawCount;
                _lastUpdateCount = UpdateCount;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Update the number of times draw has been called
            _drawCount += 1;

            // Draw the text
            base.Draw(gameTime, spriteBatch);
        }


    }
}
