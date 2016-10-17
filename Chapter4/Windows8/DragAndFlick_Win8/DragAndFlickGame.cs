using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using GameFramework;

namespace DragAndFlick_Win8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class DragAndFlickGame : GameFramework.GameHost
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public DragAndFlickGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Set the TouchPanel to support mouse input
            TouchPanel.EnableMouseGestures = true;
            TouchPanel.EnableMouseTouchPoint = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Textures.Add("Circle", Content.Load<Texture2D>("Circle"));
            Textures.Add("Box", Content.Load<Texture2D>("Box"));

            ResetGame();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Get the raw touch input
            TouchCollection touches = TouchPanel.GetState();
            // Is there a touch?
            if (touches.Count > 0)
            {
                // Is the first touch point just pressed?
                if (touches[0].State == TouchLocationState.Pressed)
                {
                    // Yes, so select the objects at this position.
                    // First clear all existing selections
                    DeselectAllObjects();
                    // The select all touched sprites
                    SelectAllMatches(touches[0].Position);
                }
            }

            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();
                switch (gesture.GestureType)
                {
                    case GestureType.FreeDrag:
                    case GestureType.HorizontalDrag:
                    case GestureType.VerticalDrag:
                        // The object is being dragged
                        ProcessDrag(gesture.Delta);
                        break;
                    case GestureType.Flick:
                        // The object has been flicked
                        ProcessFlick(gesture.Delta * (float)TargetElapsedTime.TotalSeconds);
                        break;
                    case GestureType.Hold:
                        // A hold gesture is used to reset the sprite positions
                        ResetGame();
                        break;
                }
            }

            // Update everything
            UpdateAll(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw all the sprites
            _spriteBatch.Begin();
            DrawSprites(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Select all of the sprites at the specified position.
        /// </summary>
        /// <param name="testPosition"></param>
        private void SelectAllMatches(Vector2 testPosition)
        {
            SelectableSpriteObject selectableObj;
            SpriteObject[] hits;

            // Find all the sprites at the test position
            hits = GetSpritesAtPoint(testPosition);

            // Loop for all of the SelectableSpriteObjects
            for (int i = 0; i < hits.Length; i++)
            {
                // Is this a SelectableSpriteObject?
                if (hits[i] is SelectableSpriteObject)
                {
                    // Yes... Cast it to a SelectableSpriteObject
                    selectableObj = (SelectableSpriteObject)hits[i];
                    // Select this object
                    selectableObj.Selected = true;
                }
            }
        }

        /// <summary>
        /// Move the selected sprites by the specified delta
        /// </summary>
        /// <param name="delta"></param>
        private void ProcessDrag(Vector2 delta)
        {
            int objectCount = GameObjects.Count;

            for (int i = 0; i < objectCount; i++)
            {
                // Is this a selectable sprite that is also selected?
                if (GameObjects[i] is SelectableSpriteObject && ((SelectableSpriteObject)GameObjects[i]).Selected)
                {
                    // Yes, so add the delta to the sprite position
                    ((SelectableSpriteObject)GameObjects[i]).Position += delta;
                }
            }
        }

        /// <summary>
        /// Flick the selected sprites with an initial delta as specified
        /// </summary>
        /// <param name="delta"></param>
        private void ProcessFlick(Vector2 delta)
        {
            int objectCount = GameObjects.Count;

            for (int i = 0; i < objectCount; i++)
            {
                // Is this a selectable sprite that is also selected?
                if (GameObjects[i] is SelectableSpriteObject && ((SelectableSpriteObject)GameObjects[i]).Selected)
                {
                    // Yes, so add the delta to the sprite position
                    ((SelectableSpriteObject)GameObjects[i]).KineticVelocity = delta;
                }
            }
        }


        /// <summary>
        /// Set all SelectableSpriteObjects to be unselected
        /// </summary>
        private void DeselectAllObjects()
        {
            int objectCount = GameObjects.Count;

            // Loop for each object
            for (int i = 0; i < objectCount; i++)
            {
                // Is this a selectable object?
                if (GameObjects[i] is SelectableSpriteObject)
                {
                    // Yes, so deselect it
                    ((SelectableSpriteObject)GameObjects[i]).Selected = false;
                }
            }
        }

        /// <summary>
        /// Set the initial game state
        /// </summary>
        private void ResetGame()
        {
            Vector2 position;
            SelectableSpriteObject selectableObj;
            GameObjects.Clear();

            // Add some randomly positioned box objects
            for (int i = 0; i < 20; i++)
            {
                // Randomize the position
                position = new Vector2(GameHelper.RandomNext(Window.ClientBounds.Width), GameHelper.RandomNext(Window.ClientBounds.Height));
                // Create a new object
                selectableObj = new SelectableSpriteObject(this, position, Textures["Box"]);
                // Set the origin
                selectableObj.Origin = new Vector2(selectableObj.SpriteTexture.Width / 2, selectableObj.SpriteTexture.Height / 2);
                // Randomize the angle and scale
                selectableObj.Angle = MathHelper.ToRadians(GameHelper.RandomNext(360));
                selectableObj.Scale = new Vector2(GameHelper.RandomNext(0.5f, 2.0f), GameHelper.RandomNext(0.5f, 2.0f));
                // Set the hit test mode
                selectableObj.AutoHitTestMode = SpriteObject.AutoHitTestModes.Rectangle;
                // Add to the game
                GameObjects.Add(selectableObj);
            }

            // Add some randomly positioned circle objects
            for (int i = 0; i < 20; i++)
            {
                // Randomize the position
                position = new Vector2(GameHelper.RandomNext(Window.ClientBounds.Width), GameHelper.RandomNext(Window.ClientBounds.Height));
                // Create a new object
                selectableObj = new SelectableSpriteObject(this, position, Textures["Circle"]);
                // Set the origin
                selectableObj.Origin = new Vector2(selectableObj.SpriteTexture.Width / 2, selectableObj.SpriteTexture.Height / 2);
                // Randomize the angle and scale
                selectableObj.Angle = MathHelper.ToRadians(GameHelper.RandomNext(360));
                selectableObj.Scale = new Vector2(GameHelper.RandomNext(0.5f, 2.0f), GameHelper.RandomNext(0.5f, 2.0f));
                // Set the hit test mode
                selectableObj.AutoHitTestMode = SpriteObject.AutoHitTestModes.Ellipse;
                // Add to the game
                GameObjects.Add(selectableObj);
            }

            // Look for Tap and Hold gestures
            TouchPanel.EnabledGestures = GestureType.FreeDrag | GestureType.Flick | GestureType.Hold;
        }

    }
}
