using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GameFramework
{
    // Derive GameHost from the XNA Game class.
    // Our actual game classes can then derive from GameHost
    // in order to pick up all of the functionality added here.
    public class GameHost : Microsoft.Xna.Framework.Game
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        private GameObjectBase[] _objectArray = new GameObjectBase[0];

        // The following variables store the graphics device state values
        // that were present prior to sprite rendering so that they can be
        // restored afterwards
        private BlendState _preSpriteBlendState = BlendState.Opaque;
        private DepthStencilState _preSpriteDepthStencilState = DepthStencilState.Default;
        private RasterizerState _preSpriteRasterizerState = RasterizerState.CullCounterClockwise;
        private SamplerState _preSpriteSamplerState = SamplerState.LinearWrap;

        // The currently active game mode handler
        private GameModeBase _currentGameModeHandler;
        // A collection of all known game mode handler objects
        private Dictionary<Type, GameModeBase> _gameModeHandlers = new Dictionary<Type, GameModeBase>();

        //-------------------------------------------------------------------------------------
        // Constructors

        public GameHost()
        {
            // Create new collections
            Textures = new Dictionary<string, Texture2D>();
            Fonts = new Dictionary<string, SpriteFont>();
            Models = new Dictionary<string, Model>();
            GameObjects = new List<GameObjectBase>();
            SoundEffects = new Dictionary<string, SoundEffect>();
            Songs = new Dictionary<string, Song>();

            // Create other objects
            SettingsManager.Game = this;
            HighScores = new HighScores(this);
        }


        //-------------------------------------------------------------------------------------
        // Properties

        // A dictionary of loaded textures. Using a dictionary allows us to easily access the texture by name
        public Dictionary<string, Texture2D> Textures { get; set; }
        // A dictionary of loaded fonts.
        public Dictionary<string, SpriteFont> Fonts { get; set; }
        // A dictionary of loaded models.
        public Dictionary<string, Model> Models { get; set; }

        // A list of active game objects.
        public List<GameObjectBase> GameObjects { get; set; }

        // A dictionary of loaded sound effects.
        public Dictionary<string, SoundEffect> SoundEffects { get; set; }
        // A dictionary of loaded songs.
        public Dictionary<string, Song> Songs { get; set; }

        // A camera object, if one is required
        public MatrixCameraObject Camera;
        // A sky box, if one is required
        public MatrixSkyboxObject Skybox;
        
        // A pre-defined instance of the HighScores class
        public HighScores HighScores;

        // The current game mode handler object
        public GameModeBase CurrentGameModeHandler { get { return _currentGameModeHandler; } }

        //-------------------------------------------------------------------------------------
        // Game functions

        /// <summary>
        /// Call the Update method on all objects in the GameObjects collection
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void UpdateAll(GameTime gameTime)
        {
            int i;
            int objectCount;

            // First build our array of objects.
            // We will iterate across this rather than across the actual GameObjects
            // collection so that the collection can be modified by the game objects'
            // Update code.
            // First of all, do we have an array?
            if (_objectArray == null)
            {
                // No, so allocate it.
                // Allocate 20% more objects than we currently have, or 20 objects, whichever is more
                _objectArray = new GameObjectBase[(int)MathHelper.Max(20, GameObjects.Count * 1.2f)];
            }
            else if (GameObjects.Count > _objectArray.Length)
            {
                // The number of game objects has exceeded the array size.
                // Reallocate the array, adding 20% free space for further expansion.
                _objectArray = new GameObjectBase[(int)(GameObjects.Count * 1.2f)];
            }

            // Store the current object count for performance
            objectCount = GameObjects.Count;

            // Transfer the object references into the array
            for (i = 0; i < _objectArray.Length; i++)
            {
                // Is there an active object at this position in the GameObjects collection?
                if (i < objectCount)
                {
                    // Yes, so copy it to the array
                    _objectArray[i] = GameObjects[i];
                }
                else
                {
                    // No, so clear any reference stored at this index position
                    _objectArray[i] = null;
                }
            }

            // Loop for each element within the array
            for (i = 0; i < objectCount; i++)
            {
                // Update the object at this array position
                _objectArray[i].Update(gameTime);
            }

            // Finally, if we have a camera or sky box, update them too
            if (Camera != null) Camera.Update(gameTime);
            if (Skybox != null) Skybox.Update(gameTime);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Update the current game mode handler if there is one
            if (_currentGameModeHandler != null) _currentGameModeHandler.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Draw the current game mode handler if there is one
            if (_currentGameModeHandler != null) _currentGameModeHandler.Draw(gameTime);

            base.Draw(gameTime);
        }


        //-------------------------------------------------------------------------------------
        // Game Mode functions

        /// <summary>
        /// Add a known GameMode handler to the class
        /// </summary>
        /// <param name="gameMode"></param>
        public void AddGameModeHandler(GameModeBase gameMode)
        {
            Type modeType = gameMode.GetType();

            // Does this mode already exist in the dictionary?
            if (_gameModeHandlers.ContainsKey(modeType))
            {
                // Yes, so update the dictionary with the newly-provided instance
                _gameModeHandlers[modeType] = gameMode;
            }
            else
            {
                // No, so add to the dictionary with the game mode type name as the key
                _gameModeHandlers.Add(modeType, gameMode);
            }
        }

        public T SetGameMode<T>() where T : GameModeBase
        {
            Type modeType = typeof(T);

            // If this is the current mode, do nothing
            if (_currentGameModeHandler != null && modeType == _currentGameModeHandler.GetType()) return _currentGameModeHandler as T;

            // Leave the current mode
            if (_currentGameModeHandler != null) _currentGameModeHandler.Deactivate();

            // Select the new mode
            if (_gameModeHandlers.ContainsKey(modeType))
            {
                _currentGameModeHandler = _gameModeHandlers[modeType];
            }
            else
            {
                // Don't know of any game mode handler with this name so deactivate the current mode handler
                _currentGameModeHandler = null;
            }

            // Enter the new mode
            if (_currentGameModeHandler != null)
            {
                // Set this handler's list of game objects into the game itself
                GameObjects = _currentGameModeHandler.GameObjects;
                _currentGameModeHandler.Activate();
            }

            return _currentGameModeHandler as T;
        }

        /// <summary>
        /// Retrieve the game mode handler for a specifed game mode
        /// </summary>
        /// <param name="modeType"></param>
        /// <returns></returns>
        public T GetGameModeHandler<T>() where T : GameModeBase
        {
            Type modeType = typeof(T);

            // Does the handler collection contain a handler for this mode?
            if (_gameModeHandlers.ContainsKey(modeType))
            {
                // Yes, so return it
                return _gameModeHandlers[modeType] as T;
            }
            // No, so return null
            return null;
        }


        //-------------------------------------------------------------------------------------
        // Sprite functions

        /// <summary>
        /// Call the Draw method on all SpriteObject-based objects in the game host.
        /// </summary>
        /// <param name="gameTime"></param>
        public void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Call the other DrawSprites overload, passing null for restrictToTexture
            DrawSprites(gameTime, spriteBatch, null);
        }

        /// <summary>
        /// Call the Draw method on all SpriteObject-based objects in the game host
        /// whose texture matches the one provided.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch, Texture2D restrictToTexture)
        {
            GameObjectBase obj;
            int objectCount;

            // Loop for each sprite object
            objectCount = _objectArray.Length;
            for (int i = 0; i < objectCount; i++)
            {
                obj = _objectArray[i];
                // Is this a sprite object, and not a text object (which is handled separately using DrawText)?
                if (obj is SpriteObject && !(obj is TextObject))
                {
                    // If we are restricting to a texture, does the texture match?
                    if (restrictToTexture == null || ((SpriteObject)obj).SpriteTexture == restrictToTexture)
                    {
                        // Yes, so call its Draw method
                        ((SpriteObject)obj).Draw(gameTime, spriteBatch);
                    }
                }
            }
        }

        /// <summary>
        /// Call the Draw method on all TextObject-based objects in the game host
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public virtual void DrawText(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GameObjectBase obj;
            int objectCount;

            // Draw each sprite object
            objectCount = _objectArray.Length;
            for (int i = 0; i < objectCount; i++)
            {
                obj = _objectArray[i];
                // Is this a text object?
                if (obj is TextObject)
                {
                    // Yes, so call its Draw method
                    ((TextObject)obj).Draw(gameTime, spriteBatch);
                }
            }
        }


        /// <summary>
        /// Scan the sprites to find those that contain the specified position.
        /// Returns an array of all matching sprites.
        /// </summary>
        /// <param name="testPosition"></param>
        public SpriteObject[] GetSpritesAtPoint(Vector2 testPosition)
        {
            SpriteObject spriteObj;
            SpriteObject[] hits = new SpriteObject[GameObjects.Count];
            int hitCount = 0;

            // Loop for all of the SelectableSpriteObjects
            foreach (GameObjectBase obj in GameObjects)
            {
                // Is this a SpriteObject?
                if (obj is SpriteObject)
                {
                    // Yes... Cast it to a SelectableSpriteObject
                    spriteObj = (SpriteObject)obj;
                    // Is the point in the object?
                    if (spriteObj.IsPointInObject(testPosition))
                    {
                        // Add to the array
                        hits[hitCount] = spriteObj;
                        hitCount += 1;
                    }
                }
            }

            // Trim the empty space from the end of the array
            Array.Resize(ref hits, hitCount);

            return hits;
        }

        /// <summary>
        /// Scan the sprites to find those that contain the specified position.
        /// Returns an array of all matching sprites.
        /// </summary>
        /// <param name="testPosition"></param>
        public SpriteObject GetSpriteAtPoint(Vector2 testPosition)
        {
            SpriteObject spriteObj;
            SpriteObject ret = null;
            float lowestLayerDepth = float.MaxValue;

            // Loop for all of the SelectableSpriteObjects
            foreach (GameObjectBase obj in GameObjects)
            {
                // Is this a SpriteObject?
                if (obj is SpriteObject)
                {
                    // Yes... Cast it to a SelectableSpriteObject
                    spriteObj = (SpriteObject)obj;
                    // Is its layerdepth the same or lower than the lowest we have seen so far?
                    // If not, previously encountered objects are in front of this one
                    // and so we have no need to check it.
                    if (spriteObj.LayerDepth <= lowestLayerDepth)
                    {
                        // Is the point in the object?
                        if (spriteObj.IsPointInObject(testPosition))
                        {
                            // Mark this as the current frontmost object
                            // and remember its layerdepth for future checks
                            ret = spriteObj;
                            lowestLayerDepth = spriteObj.LayerDepth;
                        }
                    }
                }
            }
            
            return ret;
        }


        /// <summary>
        /// Store all of the graphics device state values that are modified by
        /// the sprite batch. These can be restored by later calling the
        /// RestoreStateAfterSprites function.
        /// </summary>
        protected void StoreStateBeforeSprites()
        {
            _preSpriteBlendState = GraphicsDevice.BlendState;
            _preSpriteDepthStencilState = GraphicsDevice.DepthStencilState;
            _preSpriteRasterizerState = GraphicsDevice.RasterizerState;
            _preSpriteSamplerState = GraphicsDevice.SamplerStates[0];
        }

        /// <summary>
        /// Restore all of the graphics device state values that are modified by
        /// the sprite batch to their previous values, as saved by an earlier call to
        /// StoreStateBeforeSprites function.
        /// </summary>
        protected void RestoreStateAfterSprites()
        {
            GraphicsDevice.BlendState = _preSpriteBlendState;
            GraphicsDevice.DepthStencilState = _preSpriteDepthStencilState;
            GraphicsDevice.RasterizerState = _preSpriteRasterizerState;
            GraphicsDevice.SamplerStates[0] = _preSpriteSamplerState;
        }
        
        //-------------------------------------------------------------------------------------
        // Matrix-based object functions

        /// <summary>
        /// Call the Draw method on all matrix objects in the game
        /// </summary>
        public virtual void DrawObjects(GameTime gameTime, Effect effect)
        {
            DrawMatrixObjects(gameTime, effect, false, false, null);
        }

        /// <summary>
        /// Call the Draw method on all matrix objects in the game that use
        /// the specified texture. Pass as null to draw only objects that do
        /// not have a texture specified at all.
        /// </summary>
        public virtual void DrawObjects(GameTime gameTime, Effect effect, Texture2D restrictToTexture)
        {
            DrawMatrixObjects(gameTime, effect, true, false, restrictToTexture);
        }

        /// <summary>
        /// Call the Draw method on all matrix particle objects in the game
        /// </summary>
        public virtual void DrawParticles(GameTime gameTime, Effect effect)
        {
            DrawMatrixObjects(gameTime, effect, false, true, null);
        }

        /// <summary>
        /// Call the Draw method on all matrix particle objects in the game that use
        /// the specified texture. Pass as null to draw only objects that do
        /// not have a texture specified at all.
        /// </summary>
        public virtual void DrawParticles(GameTime gameTime, Effect effect, Texture2D restrictToTexture)
        {
            DrawMatrixObjects(gameTime, effect, true, true, restrictToTexture);
        }

        /// <summary>
        /// Draw the specified objects
        /// </summary>
        private void DrawMatrixObjects(GameTime gameTime, Effect effect, bool specifiedTextureOnly, bool renderParticles, Texture2D restrictToTexture)
        {
            GameObjectBase obj;
            int objectCount;

            // If we have a camera, draw it first
            if (Camera != null) Camera.Draw(gameTime, effect);

            // Now draw the sky box if there is one
            if (Skybox != null && Skybox._renderedThisFrame == false) Skybox.Draw(gameTime, effect);

            // Draw each matrix-based object
            objectCount = _objectArray.Length;
            for (int i = 0; i < objectCount; i++)
            {
                obj = _objectArray[i];
                // Is this a matrix object?
                if (obj is MatrixObjectBase)
                {
                    // Check whether this is a particle, and whether we are supposed to render it
                    if ((renderParticles && obj is MatrixParticleObjectBase) || (!renderParticles && !(obj is MatrixParticleObjectBase)))
                    {
                        // Does this object use the required texture?
                        if (specifiedTextureOnly == false || ((MatrixObjectBase)obj).ObjectTexture == restrictToTexture)
                        {
                            ((MatrixObjectBase)obj).Draw(gameTime, effect);
                        }
                    }
                }
            }
        }


        //-------------------------------------------------------------------------------------
        // Miscellaneous functions

        /// <summary>
        /// Scan the GameObjects list looking for an object with the specified tag
        /// </summary>
        /// <param name="tag">The tag to search for</param>
        /// <returns>The tagged object if one is found, or null if there was no match</returns>
        public GameObjectBase GetObjectByTag(string tag)
        {
            // Loop through the objects
            foreach (GameObjectBase obj in GameObjects)
            {
                // Does this tag match?
                if (tag.Equals(obj.Tag))
                {
                    // Yes, so return the object
                    return obj;
                }
            }
            // No matching tag
            return null;
        }

    }
}
