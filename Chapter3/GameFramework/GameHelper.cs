using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace GameFramework
{
    public static class GameHelper
    {

        //-------------------------------------------------------------------------------------
        // Class-level variables

        // A static Random object that will be used by all calls to RandomNext throughout
        // the lifetime of the game.
        private static Random _rand;

        
        //-------------------------------------------------------------------------------------
        // Static class functions

        /// <summary>
        /// Returns an nonnegative random value less than the specified maximum
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the number to be generated. Must be greater than zero.</param>
        /// <returns></returns>
        public static int RandomNext(int maxValue)
        {
            return RandomNext(0, maxValue);
        }

        /// <summary>
        /// Returns an random value within a specified range
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the number to be generated.</param>
        /// <param name="maxValue">The exclusive upper bound of the number to be generated.</param>
        /// <returns></returns>
        public static int RandomNext(int minValue, int maxValue)
        {
            // Create the Random object is not already available
            if (_rand == null) _rand = new Random();
            return _rand.Next(minValue, maxValue);
        }

        /// <summary>
        /// Returns a random float between zero and the provided value.
        /// </summary>
        /// <param name="maxValue">The maximum permitted value</param>
        /// <returns></returns>
        public static float RandomNext(float maxValue)
        {
            return RandomNext(0.0f, maxValue);
        }

        /// <summary>
        /// Returns a random float between the two provided values.
        /// </summary>
        /// <param name="minValue">The minimum permitted value.</param>
        /// <param name="maxValue">The maximum permitted value.</param>
        /// <returns></returns>
        public static float RandomNext(float minValue, float maxValue)
        {
            // Create the Random object is not already available
            if (_rand == null) _rand = new Random();
            return (float)_rand.NextDouble() * (maxValue - minValue) + minValue;
        }

    }
}
