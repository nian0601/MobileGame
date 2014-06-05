using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MobileGame
{
    class SimpleTile : Tile
    {
        public SimpleTile(int x, int y, int tileType, bool collidable)
            : base(x, y, collidable)
        {
            tileTexture = TextureManager.AirTile;

            if (tileType == 0)
                shouldDraw = false;

            tileSize = tileTexture.Height;
            pixelPos = new Vector2(index.X * tileSize, index.Y * tileSize);
        }   
    }
}
