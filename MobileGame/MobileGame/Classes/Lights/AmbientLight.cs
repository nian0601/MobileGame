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
            this.Rect = new Rectangle(X, Y, Width, Height);
            this.Color = Color;
            this.Texture = TextureManager.FilledSquare;
        }

        public AmbientLight(Color Color)
        {
            this.Rect = new Rectangle(0, 0, MapManager.MapWidth * MapManager.TileSize, MapManager.MapHeight * MapManager.TileSize);
            this.Color = Color;
            this.Texture = TextureManager.FilledSquare;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rect, Color);
        }
    }
}
