using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MobileGame
{
    class SimpleEnemy : IEnemy
    {
        protected Texture2D enemyTex;
        protected Vector2 position;
        protected Vector2 velocity;
        protected float gravity;

        protected bool isOnGround;

        protected Color[] colorArray;

        protected Random rand;

        public SimpleEnemy(int x, int y)
        {
            enemyTex = TextureManager.SmallerEnemyTex;
            position = new Vector2(x * TextureManager.AirTile.Width, y * TextureManager.AirTile.Height);
            gravity = 0.4f;

            isOnGround = false;

            colorArray = new Color[enemyTex.Width * enemyTex.Height];
            enemyTex.GetData(colorArray);

            rand = new Random();

            int value = rand.Next(1, 3);
            if (value == 1)
                velocity = new Vector2(1, 0);
            else if (value == 2)
                velocity = new Vector2(-1, 0);
        }

        public virtual void Update(float elapsedTime)
        {
            position.X += velocity.X;
        }

        public virtual void Update(float elapsedTime, Player player, List<SimpleTile> collisionList)
        {
            CheckIfOnGround(collisionList);

            position.X += velocity.X;
            HorizontalCollision(collisionList);

            velocity.Y += gravity;

            position.Y += velocity.Y;
            VerticalCollision(collisionList);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(enemyTex, position, Color.White);
        }

        public virtual void CollideWithPlayer(Player player)
        {
            if (PixelCol(HitBox(), colorArray, player.HitBox(), player.ColorArray))
                player.ResetPosition();
        }

        public virtual void SpecialAbility(Player player)
        {

        }

        public virtual Rectangle HitBox()
        {
            Rectangle temp = new Rectangle((int)position.X, (int)position.Y, enemyTex.Width, enemyTex.Height);

            return new Rectangle((int)position.X, (int)position.Y, enemyTex.Width, enemyTex.Height);
        }

        public virtual Vector2 Position
        {
            get
            {
                return position;
            }
        }

        private void CheckIfOnGround(List<SimpleTile> collisionList)
        {
            Point collisionPointLeft = new Point(HitBox().Left + 1, HitBox().Top + HitBox().Height);
            Point collisionPointRight = new Point(HitBox().Right - 1, HitBox().Top + HitBox().Height);

            isOnGround = false;

            for (int i = 0; i < collisionList.Count; i++)
            {
                if (collisionList[i].HitBox().Contains(collisionPointLeft) || collisionList[i].HitBox().Contains(collisionPointRight))
                {
                    isOnGround = true;
                    break;
                }
            }
        }

        //Collision in the X-Axis
        private void HorizontalCollision(List<SimpleTile> collisionList)
        {
            Point collisionPointLeft = new Point(HitBox().Left - 2, HitBox().Top + HitBox().Height / 2);
            Point collisionPointRight = new Point(HitBox().Right + 2, HitBox().Top + HitBox().Height / 2);

            for (int i = 0; i < collisionList.Count; i++)
            {
                if (collisionList[i].HitBox().Intersects(HitBox()))
                {
                    if (PixelCol(HitBox(), colorArray, collisionList[i].HitBox(), collisionList[i].ColorArray))
                    {
                        if (velocity.X >= 0)
                            position.X = collisionList[i].HitBox().Left - HitBox().Width;
                        else if (velocity.X < 0)
                            position.X = collisionList[i].HitBox().Left + collisionList[i].HitBox().Width;

                        velocity.X *= -1;
                        break;
                    }                    
                }
            }
        }

        //Collision in the Y-Axis
        private void VerticalCollision(List<SimpleTile> collisionList)
        {
            Point collisionPointTop = new Point(HitBox().Left + HitBox().Width / 2, HitBox().Top);
            Point collisionPointBot = new Point(HitBox().Left + HitBox().Width / 2, HitBox().Bottom);

            for (int i = 0; i < collisionList.Count; i++)
            {
                if (collisionList[i].HitBox().Intersects(HitBox()))
                {
                    if (PixelCol(HitBox(), colorArray, collisionList[i].HitBox(), collisionList[i].ColorArray))
                    {
                        if (velocity.Y >= 0)
                            position.Y = collisionList[i].HitBox().Top - HitBox().Height;
                        else if (velocity.Y < 0)
                            position.Y = collisionList[i].HitBox().Top + collisionList[i].HitBox().Height;

                        velocity.Y = 0;
                        break;
                    } 
                }
            }
        }

        private Boolean PixelCol(Rectangle rectA, Color[] arrayA, Rectangle rectB, Color[] arrayB)
        {
            int top = Math.Max(rectA.Top, rectB.Top);
            int bottom = Math.Min(rectA.Bottom, rectB.Bottom);
            int left = Math.Max(rectA.Left, rectB.Left);
            int right = Math.Min(rectA.Right, rectB.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color colorA = arrayA[(x - rectA.Left) + (y - rectA.Top) * rectA.Width];
                    Color colorB = arrayB[(x - rectB.Left) + (y - rectB.Top) * rectB.Width];

                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
