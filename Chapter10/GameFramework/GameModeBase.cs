using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace GameFramework
{
    public abstract class GameModeBase
    {

        /// <summary>
        /// A reference back to the game that owns the object
        /// </summary>
        protected GameHost Game { get; set; }

        /// <summary>
        /// This game mode handler's private list of game objects
        /// </summary>
        public List<GameObjectBase> GameObjects { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="game">The GameHost object that owns this game mode</param>
        public GameModeBase(GameHost game)
        {
            // Store a reference to the game
            Game = game;

            // Create the list of game objects for this class
            GameObjects = new List<GameObjectBase>();
        }

        /// <summary>
        /// Reset this game mode handler to its default state
        /// </summary>
        public virtual void Reset()
        {
        }

        /// <summary>
        /// Called when this game mode is activated
        /// </summary>
        public virtual void Activate()
        {
        }

        /// <summary>
        /// Called when this game mode is deactivated
        /// </summary>
        public virtual void Deactivate()
        {
        }

        /// <summary>
        /// When this game mode is active, called for each game update
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// When this game mode is active, called for each game draw
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Draw(GameTime gameTime)
        {
        }

        /// <summary>
        /// Virtual method which the game may override if it needs to respond to the game window resizing
        /// </summary>
        /// <param name="windowState">The new window state</param>
        /// <param name="newSize">A Vector2 structure containing the new window size</param>
        /// <param name="oldSize">A Vector2 structure containing the previous window size</param>
        protected internal virtual void Resize(GameHost.WindowStates windowState, Vector2 newSize, Vector2 oldSize)
        {
            // Nothing to do by default
        }

    }
}
