using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GUI_System.GUIObjects
{
    abstract public class ButtonStyle : IStyle
    {
        private Texture2D normalTexture;
        public Texture2D NormalTexture 
        {
            get { return normalTexture; }
            set
            {
                normalTexture = value;

                SourceRect = new Rectangle(0, 0, normalTexture.Width, normalTexture.Height);
            }
        }
        public Texture2D hooverTexture { get;  set; }
        public Texture2D TextureToDraw { get;  set; }
        public Vector2 Position { get;  set; }
        public Color Color { get;  set; }
        public Vector2 Origin { get;  set; }
        public Rectangle SourceRect { get; set; }

        public ButtonStyle(ContentManager Content)
        {
            Color = Color.White;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Origin = new Vector2(SourceRect.Width / 2, SourceRect.Height/2);
            spriteBatch.Draw(TextureToDraw, Position, SourceRect, Color, 0, Origin, 1f, SpriteEffects.None, 0);
        }

        public virtual Rectangle ClickableArea()
        {
            return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, SourceRect.Width, SourceRect.Height);
        }

        public virtual void ActivateNormalTexture()
        {
            TextureToDraw = normalTexture;
        }

        public virtual void ActivateHooverTexture()
        {
            TextureToDraw = hooverTexture;
        }
    }
}