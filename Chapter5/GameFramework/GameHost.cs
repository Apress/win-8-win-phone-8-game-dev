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

        private GameObjectBase[] _objectArray;

        //-------------------------------------------------------------------------------------
        // Constructors

        public GameHost()
        {
            // Create new collections
            Textures = new Dictionary<string, Texture2D>();
            Fonts = new Dictionary<string, SpriteFont>();
            GameObjects = new List<GameObjectBase>();
            SoundEffects = new Dictionary<string, SoundEffect>();
            Songs = new Dictionary<string, Song>();
        }


        //-------------------------------------------------------------------------------------
        // Properties

        // A dictionary of loaded textures. Using a dictionary allows us to easily access the texture by name
        public Dictionary<string, Texture2D> Textures { get; set; }
        // A dictionary of loaded fonts.
        public Dictionary<string, SpriteFont> Fonts { get; set; }

        // A list of active game objects.
        public List<GameObjectBase> GameObjects { get; set; }

        // A dictionary of loaded sound effects.
        public Dictionary<string, SoundEffect> SoundEffects { get; set; }
        // A dictionary of loaded songs.
        public Dictionary<string, Song> Songs { get; set; }


        //-------------------------------------------------------------------------------------
        // Game functions

        /// <summary>
        /// Call the Update method on all objects in the GameObjects collection
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void UpdateAll(GameTime gameTime)
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



    }
}
