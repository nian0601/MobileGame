using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MobileGame.Managers;

namespace MobileGame.Lights
{
    class BasicLight : ILight
    {
        public BasicLight(int X, int Y, int Width, int Height, Color Color)
        {
            this.Position = new Vector2(X, Y);
            this.Width = Width;
            this.Height = Height;
            this.Texture = TextureManager.LightSource;
            this.Color = Color;
        }

        public override void Draw(SpriteBatch SpriteBatch)
        {
            Rectangle Rect = new Rectangle((int)Position.X - Width / 2, (int)Position.Y - Height / 2, Width, Height);
            SpriteBatch.Draw(Texture, Rect, Color);
        }
    }
}
