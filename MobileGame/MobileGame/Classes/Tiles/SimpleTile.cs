using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MobileGame.FileManagement;

namespace MobileGame
{
    class SimpleTile : Tile
    {
        public SimpleTile(int x, int y, int tileType, bool collidable)
            : base(x, y, collidable)
        {
            if (tileType == 0)
                shouldDraw = false;

            //tileSize = tileTexture.Height;
            tileSize = FileLoader.LoadedLevelTileSize;
            pixelPos = new Vector2(index.X * tileSize, index.Y * tileSize);
        }   
    }
}
