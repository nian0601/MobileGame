using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GUI_System.GUIObjects
{
    public class ListStyle : IStyle
    {
        public Vector2 ItemOffset { get; set; }
        public int ItemHeight { get; set; }

        public Texture2D Texture { get; set; }

        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public Vector2 Origin { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public ListStyle(ContentManager Content)
        {
            Color = Color.White;
            Origin = new Vector2(ClickableArea().Width / 2, ClickableArea().Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(Texture, Position, null, Color, 0, Origin, 1f, SpriteEffects.None, 0);
        }

        public virtual Rectangle ClickableArea()
        {
            return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, Width, Height);
        }
    }
}
