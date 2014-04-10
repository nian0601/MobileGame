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
        public SimpleTile(int x, int y, int tileType) : base(x, y)
        {
            switch (tileType)
            {
                case 0:
                    tileTexture = TextureManager.AirTile;
                    shouldDraw = false;
                    break;
                case 1:
                    tileTexture = TextureManager.TopLeftTile;
                    break;
                case 2:
                    tileTexture = TextureManager.TopMiddleTile;
                    break;
                case 3:
                    tileTexture = TextureManager.TopRightTile;
                    break;
                case 4:
                    tileTexture = TextureManager.MiddleLeftTile;
                    break;
                case 5:
                    tileTexture = TextureManager.MiddleTile;
                    break;
                case 6:
                    tileTexture = TextureManager.MiddleRightTile;
                    break;
                case 7:
                    tileTexture = TextureManager.BottomLeftTile;
                    break;
                case 8:
                    tileTexture = TextureManager.BottomMiddleTile;
                    break;
                case 9:
                    tileTexture = TextureManager.BottomRightTile;
                    break;
                case 10:
                    tileTexture = TextureManager.TopLeftCorner;
                    break;
                case 11:
                    tileTexture = TextureManager.TopRightCorner;
                    break;
                case 12:
                    tileTexture = TextureManager.BottomLeftCorner;
                    break;
                case 13:
                    tileTexture = TextureManager.BottomRightCorner;
                    break;
            }

            tileSize = tileTexture.Height;
            pixelPos = new Vector2(index.X * tileSize, index.Y * tileSize);
        }
    }
}
