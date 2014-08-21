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
using MobileGame.FileManagement;
using MobileGame.Managers;

namespace MobileGame.Tiles
{
    class Tile : IFocusable
    {
        public bool shouldDraw;
        public bool collidable;
        public bool canJumpThrough;
        public Color Color;

        protected Texture2D tileTexture;
        protected Vector2 index;
        protected Vector2 pixelPos;
        protected int tileSize;
        
        protected Color[] colorArray;

        protected int interestRadius;
        protected int controlRadius;

        #region Properties

        public Color[] ColorArray
        {
            get { return colorArray; }
        }

        public Vector2 PixelPosition
        {
            get { return pixelPos; }
        }

        public Vector2 IndexPos
        {
            get { return index; }
        }

        public Vector2 Position
        {
            get { return pixelPos; }
        }

        public int InterestRadius
        {
            get { return interestRadius; }
            set { interestRadius = value; }
        }

        public int ControlRadius
        {
            get { return controlRadius; }
            set { controlRadius = value; }
        }

        private Texture2D interestCircle;
        public Texture2D InterestCircle
        {
            get { return interestCircle; }
            set { interestCircle = value; }
        }

        private Texture2D controlCircle;
        public Texture2D ControlCircle
        {
            get { return controlCircle; }
            set { controlCircle = value; }
        }

        #endregion

        public Tile(int x, int y)
        {   
            index = new Vector2(x, y);
            tileSize = FileLoader.LoadedLevelTileSize;
            shouldDraw = true;
            collidable = false;
            canJumpThrough = false;

            interestRadius = 200;
            controlRadius = 50;

            Color = Color.White;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (shouldDraw)
            {
                spriteBatch.Draw(tileTexture, pixelPos, null, Color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f); 
                Color = Color.White;
            }     
        }


        public virtual Rectangle HitBox()
        {
            return new Rectangle((int)pixelPos.X+1, (int)pixelPos.Y+1, tileSize-1, tileSize-1);
        }
    }
}
