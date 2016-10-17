using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameFramework;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace HitTesting_WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class HitTestingGame : GameHost
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        public HitTestingGame()
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
            // TODO: Add your initialization logic here

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

            // Update everything
            UpdateAll(gameTime);

            TouchCollection touches = TouchPanel.GetState();
            if (touches.Count == 1)
            {
                // Is this a new touch in the top-left corner of the screen?
                if (touches[0].Position.X < 50 && touches[0].Position.Y < 50 && touches[0].State == TouchLocationState.Pressed)
                {
                    // Yes, so reset to provide another set of random shapes
                    ResetGame();
                }

                // Clear all existing selections
                DeselectAllObjects();
                // See if the user tapped a sprite
                SelectAllMatches(touches[0].Position);
                //SelectFrontmost(touches[0].Position);
            }

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
        /// Select the front-most sprite at the specified position.
        /// </summary>
        /// <param name="testPosition"></param>
        private void SelectFrontmost(Vector2 testPosition)
        {
            SpriteObject hit;

            hit = GetSpriteAtPoint(testPosition);

            if (hit is SelectableSpriteObject) ((SelectableSpriteObject)hit).Selected = true;
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

            // Look for Tap and Hold gestures
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.Hold;
        }

    }
}
