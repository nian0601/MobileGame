using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GUI_System.GUIObjects
{
    abstract public class ColorPickerStyle : IStyle
    {
        public Texture2D TextureToPickFrom { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public Color Color { get; set; }

        public ColorPickerStyle(ContentManager Content)
        {
            Color = Color.White;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Origin = new Vector2(ClickableArea().Width / 2, ClickableArea().Height / 2);
            spriteBatch.Draw(TextureToPickFrom, Position, null, Color, 0, Origin, 1f, SpriteEffects.None, 0f);
        }

        public virtual Rectangle ClickableArea()
        {
            return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, TextureToPickFrom.Width, TextureToPickFrom.Height);
        }
    }
}
