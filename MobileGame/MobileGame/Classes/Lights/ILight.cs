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
        protected Rectangle Rect;
        protected Texture2D Texture;
        protected Color Color;
        abstract public void Draw(SpriteBatch spriteBatch);
    }
}
