using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MobileGame
{
    class AnimatedObject
    {
        private Texture2D SpriteSheet;
        private Vector2 Position;
        private int FrameWidth, FrameHeight;
        private int Rows, Cols;
        private int CurrentRow, CurrentCol;
        private float TimePerFrame, CurrentTime;
        private Rectangle SourceRect;

        public AnimatedObject()
        {
            SpriteSheet = TextureManager.PlayerSheet;
            Position = new Vector2(200, 200);
            FrameWidth = 30;
            FrameHeight = 35;
            Rows = SpriteSheet.Height / FrameHeight;
            Cols = SpriteSheet.Width / FrameWidth;
            CurrentCol = 0;
            CurrentRow = 0;
            CurrentTime = 0;
            TimePerFrame = 150;

            UpdateSourceRect();
        }

        public virtual void Update(float ElapsedTime)
        {
            DoAnimation(ElapsedTime);
        }

        public virtual void Draw(SpriteBatch SpriteBatch)
        {
            SpriteBatch.Draw(SpriteSheet, Position, SourceRect, Color.White); 
        }

        protected virtual void DoAnimation(float ElapsedTime)
        {
            CurrentTime += ElapsedTime;

            UpdateSourceRect();

            if (CurrentTime >= TimePerFrame)
            {
                CurrentCol++;

                if (CurrentCol >= Cols)
                {
                    CurrentCol = 0;
                    CurrentRow++;

                    if (CurrentRow >= Rows)
                    {
                        CurrentRow = 0;
                    }
                }

                CurrentTime -= TimePerFrame;  
            }
        }

        protected virtual void UpdateSourceRect()
        {
            SourceRect = new Rectangle(CurrentCol * FrameWidth, CurrentRow * FrameHeight, FrameWidth, FrameHeight);
        }
    }
}
