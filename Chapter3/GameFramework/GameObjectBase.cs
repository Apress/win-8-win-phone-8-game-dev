using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;

namespace GameFramework
{
    public abstract class GameObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class constructors

        /// <summary>
        /// Constructor for the object
        /// </summary>
        /// <param name="game">A reference to the MonoGame Game class inside which the object resides</param>
        public GameObjectBase(GameHost game)
        {
            // Store a reference to the game
            Game = game;
        }

        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// A reference back to the game that owns the object
        /// </summary>
        protected GameHost Game { get; set; }

        /// <summary>
        /// The number of calls that have been made to the Update method
        /// </summary>
        public int UpdateCount { get; set; }


        //-------------------------------------------------------------------------------------
        // Game functions


        /// <summary>
        /// Update the object state
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            // Increment the UpdateCount
            UpdateCount += 1;
        }

    }
}
