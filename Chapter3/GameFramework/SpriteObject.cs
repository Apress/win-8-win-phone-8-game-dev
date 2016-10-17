using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace GameFramework
{
    public class SpriteObject : GameObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class constructors

        public SpriteObject(GameHost game)
            : base(game)
        {
            // Set the default scale and color
            ScaleX = 1;
            ScaleY = 1;
            SpriteColor = Color.White;
        }

        public SpriteObject(GameHost game, Vector2 position)
            : this(game)
        {
            // Store the provided position
            Position = position;
        }

        public SpriteObject(GameHost game, Vector2 position, Texture2D texture)
            : this(game, position)
        {
            // Store the provided texture
            SpriteTexture = texture;
        }

        //-------------------------------------------------------------------------------------
        // Properties

        /// <summary>
        /// A reference to the default texture used by this sprite
        /// </summary>
        public virtual Texture2D SpriteTexture { get; set; }

        /// <summary>
        /// The sprite's X coordinate
        /// </summary>
        public virtual float PositionX { get; set; }
        /// <summary>
        /// The sprite's Y coordinate
        /// </summary>
        public virtual float PositionY { get; set; }
        
        /// <summary>
        /// The sprite's origin X coordinate
        /// </summary>
        public virtual float OriginX { get; set; }
        /// <summary>
        /// The sprite's origin Y coordinate
        /// </summary>
        public virtual float OriginY { get; set; }
        
        /// <summary>
        /// The sprite's rotation angle (in radians)
        /// </summary>
        public virtual float Angle { get; set; }

        /// <summary>
        /// The sprite's X scale
        /// </summary>
        public virtual float ScaleX { get; set; }
        /// <summary>
        /// The sprite's Y scale
        /// </summary>
        public virtual float ScaleY { get; set; }

        /// <summary>
        /// An optional source rectangle to read from the sprite texture
        /// </summary>
        public virtual Rectangle SourceRect { get; set; }

        /// <summary>
        /// The sprite's color
        /// </summary>
        public virtual Color SpriteColor { get; set; }

        /// <summary>
        /// The sprite's layer depth
        /// </summary>
        public virtual float LayerDepth { get; set; }

        /// <summary>
        /// The sprite's position represented as a Vector2 structure
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return new Vector2(PositionX, PositionY);
            }
            set
            {
                PositionX = value.X;
                PositionY = value.Y;
            }
        }

        /// <summary>
        /// The sprite's origin represented as a Vector2 structure
        /// </summary>
        public Vector2 Origin
        {
            get
            {
                return new Vector2(OriginX, OriginY);
            }
            set
            {
                OriginX = value.X;
                OriginY = value.Y;
            }
        }

        /// <summary>
        /// The sprite's scale represented as a Vector2 structure
        /// </summary>
        public Vector2 Scale
        {
            get
            {
                return new Vector2(ScaleX, ScaleY);
            }
            set
            {
                ScaleX = value.X;
                ScaleY = value.Y;
            }
        }

        /// <summary>
        /// Calculate a simple bounding box for the sprite
        /// </summary>
        /// <remarks>Note that this doesn't currently take rotation into account so that
        /// the box size remains constant when rotating.</remarks>
        public virtual Rectangle BoundingBox
        {
            get
            {
                Rectangle result;
                Vector2 spritesize;

                if (SourceRect.IsEmpty)
                {
                    // The size is that of the whole texture
                    spritesize = new Vector2(SpriteTexture.Width, SpriteTexture.Height);
                }
                else
                {
                    // The size is that of the rectangle
                    spritesize = new Vector2(SourceRect.Width, SourceRect.Height);
                }

                // Build a rectangle whose position and size matches that of the sprite
                // (taking scaling into account for the size)
                result = new Rectangle((int)PositionX, (int)PositionY, (int)(spritesize.X * ScaleX), (int)(spritesize.Y * ScaleY));

                // Offset the sprite by the origin
                result.Offset((int)(-OriginX * ScaleX), (int)(-OriginY * ScaleY));

                // Return the finished rectangle
                return result;
            }
        }


        //-------------------------------------------------------------------------------------
        // Game functions

        /// <summary>
        /// Draw the sprite using its default settings
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Do we have a texture? If not then there is nothing to draw...
            if (SpriteTexture != null)
            {
                // Has a source rectangle been set?
                if (SourceRect.IsEmpty)
                {
                    // No, so draw the entire sprite texture
                    spriteBatch.Draw(SpriteTexture, Position, null, SpriteColor, Angle, Origin, Scale, SpriteEffects.None, LayerDepth);
                }
                else
                {
                    // Yes, so just draw the specified SourceRect
                    spriteBatch.Draw(SpriteTexture, Position, SourceRect, SpriteColor, Angle, Origin, Scale, SpriteEffects.None, LayerDepth);
                }
            }
        }

    }
}
