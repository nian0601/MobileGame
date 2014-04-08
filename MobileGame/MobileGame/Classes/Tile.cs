using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace MobileGame
{
    class Tile
    {
        private Texture2D tileTexture;
        private Vector2 index;
        private int tileType;
        private int tileSize;

        public Tile(int x, int y, int TileType)
        {
            index = new Vector2(x, y);

            tileType = TileType;

            if (tileType == 0)
                tileTexture = TextureManager.AirTile;
            else if (tileType == 1)
                tileTexture = TextureManager.PlatformTile;
            else if (tileType == 2)
                tileTexture = TextureManager.GoalTile;
                
            tileSize = tileTexture.Height; 
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 PixelPos)
        {
            spriteBatch.Draw(tileTexture, PixelPos, Color.White);
        }

        public Rectangle HitBox()
        {
            Vector2 pixelPos = new Vector2(index.X * tileSize, index.Y * tileSize);

            return new Rectangle((int)pixelPos.X, (int)pixelPos.Y, tileSize, tileSize);
        }

        //returns the tileType
        public int TileType
        {
            get
            {
                return tileType;
            }
        }
    }
}
