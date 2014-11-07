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
        public Vector2 Position;
        public int Width;
        public int Height;

        protected Texture2D Texture;
        protected Color Color;
        abstract public void Draw(SpriteBatch spriteBatch);
    }
}
