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
        private float myPower;
        public float Power { get { return myPower; } }

        public AmbientLight(int aX, int aY, int aWidth, int aHeight, Color aColor, float aPower)
        {
            myPosition = new Vector2(aX, aY);
            myWidth = aWidth;
            myHeight = aHeight;
            myColor = aColor;
            myTexture = TextureManager.FilledSquare;
            myPower = aPower;
        }

        public AmbientLight(Color aColor, float aPower)
        {
            myPosition = new Vector2(0, 0);
            myWidth = MapManager.MapWidth * MapManager.TileSize;
            myHeight = MapManager.MapHeight * MapManager.TileSize;
            myColor = aColor;
            myTexture = TextureManager.FilledSquare;
            myPower = aPower;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle Rect = new Rectangle((int)myPosition.X, (int)myPosition.Y, myWidth, myHeight);
            spriteBatch.Draw(myTexture, Rect, myColor * myPower);
        }
    }
}
