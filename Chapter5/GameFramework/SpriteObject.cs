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
        // Enumerations

        public enum AutoHitTestModes
        {
            Rectangle,
            Ellipse
        };


        //-------------------------------------------------------------------------------------
        // Class constructors

        public SpriteObject(GameHost game)
            : base(game)
        {
            // Set the default scale and color
            ScaleX = 1;
            ScaleY = 1;
            SpriteColor = Color.White;
            AutoHitTestMode = AutoHitTestModes.Rectangle;
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

        /// <summary>
        /// The mode to use when performing hit tests within the SpriteObject.
        /// </summary>
        /// <remarks>The IsPointInObject function can be overridden if custom
        /// hit test calculations are required.</remarks>
        public AutoHitTestModes AutoHitTestMode { get; set; }


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

        /// <summary>
        /// Determine whether the specified point is contained within the sprite
        /// </summary>
        /// <param name="point"></param>
        /// <returns>True if the point is within the shape, false if not</returns>
        public override bool IsPointInObject(Vector2 point)
        {
            switch (AutoHitTestMode)
            {
                case AutoHitTestModes.Rectangle:
                    return IsPointInObject_RectangleTest(point);
                case AutoHitTestModes.Ellipse:
                    return IsPointInObject_EllipseTest(point);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Treat the sprite as a box shape and see if the specified point is contained within.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>True if the point is within the shape, false if not</returns>
        protected bool IsPointInObject_RectangleTest(Vector2 point)
        {
            Rectangle bbox;
            float width;
            float height;
            Vector2 rotatedPoint = Vector2.Zero;

            // Retrieve the bounding box
            bbox = BoundingBox;

            // If no rotation is applied, we can simply check against the bounding box
            if (Angle == 0) return bbox.Contains((int)point.X, (int)point.Y);

            // Get the sprite width and height
            width = bbox.Width;
            height = bbox.Height;

            // Subtract the sprite position to retrieve the test point in
            // object space rather than in screen space
            point -= Position;

            // Rotate the point by the negative angle of the sprite to cancel out the sprite rotation
            rotatedPoint.X = (float)(Math.Cos(-Angle) * point.X - Math.Sin(-Angle) * point.Y);
            rotatedPoint.Y = (float)(Math.Sin(-Angle) * point.X + Math.Cos(-Angle) * point.Y);

            //System.Diagnostics.Debug.WriteLine(rotatedPoint.ToString());

            // Move the bounding box to object space too
            bbox.Offset((int)-PositionX, (int)-PositionY);

            // Does the bounding box contain the rotated sprite?
            return bbox.Contains((int)rotatedPoint.X, (int)rotatedPoint.Y);
        }

        /// <summary>
        /// Treat the sprite as a circular shape and see if the specified point is contained within.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>True if the point is within the shape, false if not</returns>
        protected bool IsPointInObject_EllipseTest(Microsoft.Xna.Framework.Vector2 point)
        {
            Rectangle bbox;
            Vector2 rotatedPoint = Vector2.Zero;

            // Retrieve the basic sprite bounding box
            bbox = BoundingBox;

            // Subtract the ellipse's top-left position from the test point so that the test
            // point is relative to the origin position rather than relative to the screen
            point -= Position;

            // Rotate the point by the negative angle of the sprite to cancel out the sprite rotation
            rotatedPoint.X = (float)(Math.Cos(-Angle) * point.X - Math.Sin(-Angle) * point.Y);
            rotatedPoint.Y = (float)(Math.Sin(-Angle) * point.X + Math.Cos(-Angle) * point.Y);

            // Add back the origin point multiplied by the scale.
            // This will put us in the top-left corner of the bounding box.
            rotatedPoint += Origin * Scale;
            // Subtract the bounding box midpoint from each axis.
            // This will put us in the center of the ellipse.
            rotatedPoint -= new Vector2(bbox.Width / 2, bbox.Height / 2);

            // Divide the point by the width and height of the bounding box.
            // This will result in values between -0.5 and +0.5 on each axis for
            // positions within the bounding box. As both axes are then on the same
            // scale we can check the distance from the center point as a circle,
            // without having to worry about elliptical shapes.
            rotatedPoint /= new Vector2(bbox.Width, bbox.Height);

            //System.Diagnostics.Debug.WriteLine(rotatedPoint.Length());

            // See if the distance from the origin to the point is <= 0.5
            // (the radius of a unit-size circle). If so, we are within the ellipse.
            return (rotatedPoint.Length() <= 0.5f);
        }

    }
}
