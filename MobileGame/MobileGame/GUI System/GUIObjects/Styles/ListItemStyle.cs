using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GUI_System.GUIObjects
{
    public class ListItemStyle : IStyle
    {
        public SpriteFont Font { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public Color DefaultColor { get; set; }
        public Vector2 Origin { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public string Text { get; set; }

        public ListItemStyle(ContentManager content)
        {
            Color = DefaultColor;
            Origin = new Vector2(ClickableArea().Width / 2, ClickableArea().Height / 2);
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, Position, Color, 0f, Origin, 1f, SpriteEffects.None, 1f);
        }

        public virtual Rectangle ClickableArea()
        {
            return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, Width, Height);
        }
    }
}
