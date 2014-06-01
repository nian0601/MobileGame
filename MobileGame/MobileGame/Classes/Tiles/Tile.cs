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

using MobileGame.CameraManagement;

namespace MobileGame
{
    class Tile : IFocusable
    {
        protected Texture2D tileTexture;
        protected Vector2 index;
        protected Vector2 pixelPos;
        protected int tileSize;
        protected bool shouldDraw;
        protected Color[] colorArray;

        public Tile(int x, int y)
        {   
            index = new Vector2(x, y);
            tileSize = TextureManager.TileSize;
            shouldDraw = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(shouldDraw)
                spriteBatch.Draw(tileTexture, pixelPos, Color.White);
        }

        public void SetTileType(TextureManager.TileTypes tileType)
        {
            tileTexture = TextureManager.TileTextures[(int)tileType];
            colorArray = new Color[tileTexture.Width * tileTexture.Height];
            tileTexture.GetData(colorArray);
        }

        public Color[] ColorArray
        {
            get
            {
                return colorArray;
            }
        }

        public Rectangle HitBox()
        {
            return new Rectangle((int)pixelPos.X, (int)pixelPos.Y, tileSize, tileSize);
        }

        public Vector2 PixelPosition
        {
            get
            {
                return pixelPos;
            }
        }

        public Vector2 IndexPos
        {
            get
            {
                return index;
            }
        }

        public Vector2 Position
        {
            get
            {
                return pixelPos;
            }
        }
    }
}
