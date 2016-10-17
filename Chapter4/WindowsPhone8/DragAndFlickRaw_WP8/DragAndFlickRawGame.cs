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

namespace DragAndFlickRaw_WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class DragAndFlickRawGame : GameFramework.GameHost
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Most recent movement positions, used to calculate the flick vector
        private Vector2[] _movementQueue = new Vector2[5];

        public DragAndFlickRawGame()
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) this.Exit();

            // Get the raw touch input
            TouchCollection touches = TouchPanel.GetState();
            // Is there a touch?
            if (touches.Count > 0)
            {
                // What is the state of the first touch point?
                switch (touches[0].State)
                {
                    case TouchLocationState.Pressed:
                        // New touch so select the objects at this position.
                        // First clear all existing selections
                        DeselectAllObjects();
                        // The select all touched sprites
                        SelectAllMatches(touches[0].Position);
                        // Clear the movement queue
                        ClearMovementQueue();
                        break;
                    case TouchLocationState.Moved:
                        // Drag the objects. Make sure we have a previous position
                        TouchLocation previousPosition;
                        if (touches[0].TryGetPreviousLocation(out previousPosition))
                        {
                            // Calculate the movement delta
                            Vector2 delta = touches[0].Position - previousPosition.Position;
                            ProcessDrag(delta);
                            // Add the delta to the movement queue
                            AddDeltaToMovementQueue(delta);
                        }
                        break;
                    case TouchLocationState.Released:
                        // Flick the objects by the average queue delta
                        ProcessFlick(GetAverageMovementDelta());
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
        /// Reset all elements of the movement queue back to a "blank" state
        /// </summary>
        private void ClearMovementQueue()
        {
            for (int i = 0; i < _movementQueue.Length; i++)
            {
                _movementQueue[i] = new Vector2(float.MinValue, float.MinValue);
            }
        }

        /// <summary>
        /// Add the provided delta to the end of the movement queue, discarding
        /// the item at the start of the queue.
        /// </summary>
        /// <param name="delta"></param>
        private void AddDeltaToMovementQueue(Vector2 delta)
        {
            // Move everything one place up the queue
            for (int i = 0; i < _movementQueue.Length - 1; i++)
            {
                _movementQueue[i] = _movementQueue[i + 1];
            }
            // Add the new delta value to the end
            _movementQueue[_movementQueue.Length - 1] = delta;
        }

        /// <summary>
        /// Find the average movement delta from the values in the movement queue
        /// </summary>
        /// <returns></returns>
        private Vector2 GetAverageMovementDelta()
        {
            Vector2 totalDelta = Vector2.Zero;
            int totalDeltaPoints = 0;

            for (int i = 0; i < _movementQueue.Length; i++)
            {
                // Is there something in the queue at this index?
                if (_movementQueue[i].X > float.MinValue)
                {
                    // Add to the totalMovement
                    totalDelta += _movementQueue[i];
                    // Increment to the number of points added
                    totalDeltaPoints += 1;
                }
            }
            // Divide the accumulated vector by the number of elements
            // to retrieve the average
            return (totalDelta / totalDeltaPoints);
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
                position = new Vector2(GameHelper.RandomNext(GraphicsDevice.Viewport.Bounds.Width), GameHelper.RandomNext(GraphicsDevice.Viewport.Bounds.Height));
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
                position = new Vector2(GameHelper.RandomNext(GraphicsDevice.Viewport.Bounds.Width), GameHelper.RandomNext(GraphicsDevice.Viewport.Bounds.Height));
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
        }

    }
}
