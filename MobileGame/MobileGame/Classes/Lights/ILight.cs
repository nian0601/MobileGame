using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MobileGame.Lights
{
    abstract class ILight
    {
        protected Vector2 myPosition;
        public Vector2 Position { get { return myPosition; } }

        protected int myWidth;
        public int Width { get { return myWidth; } }

        protected int myHeight;
        public int Height { get { return myHeight; } }

        protected Texture2D myTexture;

        protected Color myColor;
        public Color Color { get { return myColor; } }

        abstract public void Draw(SpriteBatch spriteBatch);
    }
}
