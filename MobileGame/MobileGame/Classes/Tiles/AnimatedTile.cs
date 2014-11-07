using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MobileGame.Animations;
using MobileGame.Managers;

namespace MobileGame.Tiles
{
    class AnimatedTile
    {
        Animator myAnimator;
        int myStartCol;
        int myStartRow;
        int myEndCol;
        int myEndRow;
        int myTimePerFrame;

        Vector2 myPosition;
        Texture2D mySpriteSheet;

        public AnimatedTile(int aXIndex, int aYIndex, Texture2D aSpriteSheet, int aEndCol, int aEndRow) : this(aXIndex, aYIndex, aSpriteSheet, 0, 0, aEndCol, aEndRow, 100) { }

        public AnimatedTile(int aXIndex, int aYIndex, Texture2D aSpriteSheet, int aStartCol, int aStartRow, int aEndCol, int aEndRow, int aTimePerFrame = 100)
        {
            myPosition = new Vector2(aXIndex * MapManager.TileSize, aYIndex * MapManager.TileSize);
            mySpriteSheet = aSpriteSheet;

            myStartCol = aStartCol;
            myStartRow = aStartRow;
            myEndCol = aEndCol;
            myEndRow = aEndRow;

            myTimePerFrame = aTimePerFrame;

            myAnimator = new Animator(20, 20);
            myAnimator.AddAnimation(new Animation("animation", myTimePerFrame, myStartCol, myStartRow, myEndCol, myEndRow, true));
            myAnimator.StartAnimation("animation");
        }

        public void Update(float aElaspedTime)
        {
            myAnimator.Update(aElaspedTime);
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(mySpriteSheet, myPosition, myAnimator.SourceRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }
    }
}
