using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GUI_System.GUIObjects
{
    abstract public class Style
    {
        public Texture2D normalTexture { get;  set; }
        public Texture2D hooverTexture { get;  set; }
        public Texture2D TextureToDraw { get;  set; }
        public Vector2 Position { get;  set; }
        public Color Color { get;  set; }
        public Vector2 Origin { get;  set; }

        public Style(ContentManager Content)
        {
            Color = Color.White;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 origin)
        {
            Origin = origin;
            spriteBatch.Draw(TextureToDraw, Position, null, Color, 0, Origin, 1f, SpriteEffects.None, 0);
        }

        public virtual Rectangle HitBox()
        {
            return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, TextureToDraw.Width, TextureToDraw.Height);
        }

        public virtual void NormalTexture()
        {
            TextureToDraw = normalTexture;
        }

        public virtual void HooverTexture()
        {
            TextureToDraw = hooverTexture;
        }
    }
}