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
        protected Texture2D tileTexture;
        protected Vector2 index;
        protected Vector2 pixelPos;
        protected int tileSize;

        public Tile(int x, int y)
        {   
            index = new Vector2(x, y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tileTexture, pixelPos, Color.White);
        }

        public Rectangle HitBox()
        {
            return new Rectangle((int)pixelPos.X, (int)pixelPos.Y, tileSize, tileSize);
        }
    }
}
