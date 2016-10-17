using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace GameFramework
{
    public class TextObject : SpriteObject
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        // A reference to the font that we are using
        private SpriteFont _font;
        // The text to display
        private string _text;
        // Text alignment
        private TextAlignment _horizontalAlignment = TextAlignment.Manual;
        private TextAlignment _verticalAlignment = TextAlignment.Manual;

        //-------------------------------------------------------------------------------------
        // Enumerations

        /// <summary>
        /// Controls the alignment of text.
        /// </summary>
        public enum TextAlignment
        {
            /// <summary>
            /// Manual perform alignment using the Origin property.
            /// </summary>
            Manual,
            /// <summary>
            /// Align to the Top or Left
            /// </summary>
            Near,
            /// <summary>
            /// Align to the Center
            /// </summary>
            Center,
            /// <summary>
            /// Align to the Bottom or Right
            /// </summary>
            Far
        };

        //-------------------------------------------------------------------------------------
        // Class constructors

        public TextObject(GameHost game)
            : base(game)
        {
            ScaleX = 1;
            ScaleY = 1;
            SpriteColor = Color.White;
        }

        public TextObject(GameHost game, SpriteFont font)
            : this(game)
        {
            Font = font;
        }

        public TextObject(GameHost game, SpriteFont font, Vector2 position)
            : this(game, font)
        {
            PositionX = position.X;
            PositionY = position.Y;
        }

        public TextObject(GameHost game, SpriteFont font, Vector2 position, String text)
            : this(game, font, position)
        {
            Text = text;
        }

        public TextObject(GameHost game, SpriteFont font, Vector2 position, String text, TextAlignment horizontalAlignment, TextAlignment verticalAlignment)
            : this(game, font, position, text)
        {
            HorizontalAlignment = horizontalAlignment;
            VerticalAlignment = verticalAlignment;
        }

        //-------------------------------------------------------------------------------------
        // Properties

        /// <summary>
        /// A reference to the font used by this spritefont object
        /// </summary>
        public SpriteFont Font
        {
            get { return _font; }
            set
            {
                // Has the font changed from whatever we already have stored?
                if (_font != value)
                {
                    // Yes, so store the new font and recalculate the origin if needed
                    _font = value;
                    CalculateAlignmentOrigin();
                }
            }
        }
        
        /// <summary>
        /// The text to display
        /// </summary>
        public String Text
        {
            get { return _text; }
            set
            {
                // Has the text changed from whatever we already have stored?
                if (_text != value)
                {
                    // Yes, so store the new text and recalculate the origin if needed
                    _text = value;
                    CalculateAlignmentOrigin();
                }
            }
        }

        /// <summary>
        /// Allows the horizontal alignment of the text to be automatically calculated
        /// </summary>
        public TextAlignment HorizontalAlignment
        {
            get { return _horizontalAlignment; }
            set
            {
                if (_horizontalAlignment != value)
                {
                    _horizontalAlignment = value;
                    CalculateAlignmentOrigin();
                }
            }
        }
        /// <summary>
        /// Allows the vertical alignment of the text to be automatically calculated
        /// </summary>
        public TextAlignment VerticalAlignment
        {
            get { return _verticalAlignment; }
            set
            {
                if (_verticalAlignment != value)
                {
                    _verticalAlignment = value;
                    CalculateAlignmentOrigin();
                }
            }
        }


      
        //-------------------------------------------------------------------------------------
        // Game functions

        /// <summary>
        /// Draw the text using its default settings
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Do we have a font? And some text? If not then there is nothing to draw...
            if (Font != null && !string.IsNullOrEmpty(Text))
            {
                // Draw the text
                spriteBatch.DrawString(Font, Text, Position, SpriteColor, Angle, Origin, Scale, SpriteEffects.None, LayerDepth);
            }
        }

        /// <summary>
        /// Calculate a simple bounding box for the text
        /// </summary>
        /// <remarks>Note that this doesn't currently take rotation into account so that
        /// the box size remains constant when rotating.</remarks>
        public override Rectangle BoundingBox
        {
            get
            {
                Rectangle result;
                Vector2 size;

                // Measure the string
                size = Font.MeasureString(Text);

                // Build a rectangle whose position and size matches that of the sprite
                // (taking scaling into account for the size)
                result = new Rectangle((int)PositionX, (int)PositionY, (int)(size.X * ScaleX), (int)(size.Y * ScaleY));

                // Offset the sprite by the origin
                result.Offset((int)(-OriginX * ScaleX), (int)(-OriginY * ScaleY));

                // Return the finished rectangle
                return result;
            }
        }


        /// <summary>
        /// Set the alignment of the text (automatically sets the origin)
        /// </summary>
        private void CalculateAlignmentOrigin()
        {
            Vector2 size;

            // Is the alignment being performed manually?
            if (HorizontalAlignment == TextAlignment.Manual && VerticalAlignment == TextAlignment.Manual)
            {
                // Yes, so there's nothing to do
                return;
            }

            // Make sure we have a font and some text
            if (Font == null || Text == null || Text.Length == 0)
            {
                // Nothing to render
                return;
            }

            // Measure the string
            size = Font.MeasureString(Text);

            // Set the origin as appropriate
            switch (HorizontalAlignment)
            {
                case TextAlignment.Near: OriginX = 0; break;
                case TextAlignment.Center: OriginX = size.X / 2; break;
                case TextAlignment.Far: OriginX = size.X; break;
            }
            switch (VerticalAlignment)
            {
                case TextAlignment.Near: OriginY = 0; break;
                case TextAlignment.Center: OriginY = size.Y / 2; break;
                case TextAlignment.Far: OriginY = size.Y; break;
            }
        }



    }
}
