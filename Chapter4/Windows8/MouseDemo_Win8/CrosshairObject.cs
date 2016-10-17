using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MouseDemo_Win8
{
    public class CrosshairObject : SpriteObject
    {

        StringBuilder _buttonText;
        TextObject _buttonTextObject;

        public CrosshairObject(GameHost game, Vector2 position, Texture2D texture)
            : base(game, position, texture)
        {
            // Set the origin to be the center of the sprite
            Origin = new Vector2(texture.Width / 2, texture.Height / 2);

            // Create a StringBuilder into which the button state can be written
            _buttonText = new StringBuilder();

            // Add a new text object to the game so that we can report the button states
            _buttonTextObject = new TextObject(game, game.Fonts["Miramonte"], Vector2.Zero, "", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near);
            game.GameObjects.Add(_buttonTextObject);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseData;
            float scale;

            // Get the mouse state
            mouseData = Mouse.GetState();

            // Set the sprite position to match the mouse position
            this.Position = new Vector2(mouseData.X, mouseData.Y);
            // Scale the sprite if the mousewheel position changes
            scale = (mouseData.ScrollWheelValue / 1000.0f) + 1;
            this.Scale = new Vector2(scale);

            // Set the button text position to match the sprite position too
            _buttonTextObject.Position = new Vector2(PositionX, PositionY + 50);
            // Prepare the text
            _buttonText.Clear();
            if (mouseData.LeftButton == ButtonState.Pressed) _buttonText.Append("Left ");
            if (mouseData.MiddleButton == ButtonState.Pressed) _buttonText.Append("Middle ");
            if (mouseData.RightButton == ButtonState.Pressed) _buttonText.Append("Right ");
            if (mouseData.XButton1 == ButtonState.Pressed) _buttonText.Append("X1 ");
            if (mouseData.XButton2 == ButtonState.Pressed) _buttonText.Append("X2 ");
            // Set the text into the object
            _buttonTextObject.Text = _buttonText.ToString();

            base.Update(gameTime);
        }


    }
}
