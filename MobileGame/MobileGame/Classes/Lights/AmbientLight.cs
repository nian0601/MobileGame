using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MobileGame.Managers;

namespace MobileGame.Lights
{
    class AmbientLight : ILight
    {
        public AmbientLight(int X, int Y, int Width, int Height, Color Color)
        {
            this.Position = new Vector2(X, Y);
            this.Width = Width;
            this.Height = Height;
            this.Color = Color;
            this.Texture = TextureManager.FilledSquare;
        }

        public AmbientLight(Color Color)
        {
            this.Position = new Vector2(0, 0);
            this.Width = MapManager.MapWidth * MapManager.TileSize;
            this.Height = MapManager.MapHeight * MapManager.TileSize;
            this.Color = Color;
            this.Texture = TextureManager.FilledSquare;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle Rect = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            spriteBatch.Draw(Texture, Rect, Color);
        }
    }
}
