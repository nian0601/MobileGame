using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MobileGame.Lights
{
    class BasicLight : ILight
    {
        public BasicLight(int X, int Y, int Width, int Height, Texture2D Texture, Color Color)
        {
            this.Rect = new Rectangle(X - Width/2, Y - Height/2, Width, Height);
            this.Texture = Texture;
            this.Color = Color;
        }

        public override void Draw(SpriteBatch SpriteBatch)
        {
            SpriteBatch.Draw(Texture, Rect, Color);
        }
    }
}
