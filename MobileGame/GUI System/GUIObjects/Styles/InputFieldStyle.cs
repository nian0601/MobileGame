using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GUI_System.GUIObjects
{
    abstract public class InputFieldStyle : IStyle
    {
        public Texture2D LabelTexture { get; set; }
        public Texture2D InputBGTexture { get; set; }
        public Texture2D InputBGHooverTexture { get; set; }
        public Texture2D InputTextureToDraw { get; set; }
        public SpriteFont Font { get; set; }
        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public Vector2 Origin { get; set; }

        public InputFieldStyle(ContentManager Content)
        {
            Color = Color.White;
            Text = "";
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 LabelPos = Position;
            Vector2 InputBGPos = new Vector2(Position.X + LabelTexture.Width, Position.Y);

            spriteBatch.Draw(LabelTexture, Position, Color);
            spriteBatch.Draw(InputTextureToDraw, InputBGPos, Color);

            float TextXPos = InputBGPos.X + 10;
            float TextYPos = InputBGPos.Y + InputTextureToDraw.Height / 2 - Font.MeasureString(Text).Y / 2;

            Vector2 TextPos = new Vector2(TextXPos, TextYPos);

            spriteBatch.DrawString(Font, Text, TextPos, Color.Black);
        }

        public Rectangle ClickableArea()
        {
            Vector2 ClickablePos = new Vector2(Position.X + LabelTexture.Width, Position.Y);

            return new Rectangle((int)ClickablePos.X, (int)ClickablePos.Y, InputTextureToDraw.Width, InputTextureToDraw.Height);
        }

        public Rectangle Size()
        {
            int totalWidth = LabelTexture.Width + InputTextureToDraw.Width;
            int totalHeight = LabelTexture.Height + InputTextureToDraw.Height;

            return new Rectangle((int)Position.X, (int)Position.Y, totalWidth, totalHeight);
        }

        public virtual void NormalTexture()
        {
            InputTextureToDraw = InputBGTexture;
        }

        public virtual void HooverTexture()
        {
            InputTextureToDraw = InputBGHooverTexture;
        }
    }
}
