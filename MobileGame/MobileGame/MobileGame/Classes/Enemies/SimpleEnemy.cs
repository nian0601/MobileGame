using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MobileGame.FileManagement;
using MobileGame.Tiles;
using MobileGame.Managers;
using MobileGame.Units;

namespace MobileGame.Enemies
{
    class SimpleEnemy : IEnemy
    {
        protected Texture2D enemyTex;
        protected Vector2 position;
        protected Vector2 velocity;
        protected float gravity;
        protected int colRange;

        protected bool isOnGround;
        protected bool canFall;

        protected Color[] colorArray;

        protected Random rand;

        public SimpleEnemy(int x, int y, bool CanFall)
        {
            enemyTex = TextureManager.SmallerEnemyTex;
            position = new Vector2(x * FileLoader.LoadedLevelTileSize, y * FileLoader.LoadedLevelTileSize);
            gravity = 0.4f;

            isOnGround = false;
            canFall = CanFall;

            colorArray = new Color[enemyTex.Width * enemyTex.Height];
            enemyTex.GetData(colorArray);

            rand = new Random();

            int value = rand.Next(1, 3);
            if (value == 1)
                velocity = new Vector2(1, 0);
            else if (value == 2)
                velocity = new Vector2(-1, 0);

            colRange = 4;
        }

        public virtual void Update(float elapsedTime)
        {
            position.X += velocity.X;
        }

        public virtual void Update(float elapsedTime, Player player)
        {
            List<Rectangle> CollisionList = MapManager.GenerateCollisionList((int)position.X, (int)position.Y, colRange, colRange);

            CheckIfOnGround(CollisionList);

            position.X += velocity.X;
            HorizontalCollision(CollisionList);

            velocity.Y += gravity;

            position.Y += velocity.Y;
            VerticalCollision(CollisionList);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(enemyTex, position, Color.White);
        }

        public virtual void CollideWithPlayer(Player player)
        {
            if (PixelCol(HitBox(), colorArray, player.HitBox(), player.ColorArray))
                player.gotKilled = true;     
        }

        public virtual void SpecialAbility(Player player) { }

        public virtual Rectangle HitBox()
        {
            return new Rectangle((int)position.X, (int)position.Y, enemyTex.Width, enemyTex.Height);
        }

        public virtual Vector2 Position
        {
            get
            {
                return position;
            }
        }

        private void CheckIfOnGround(List<Rectangle> collisionList)
        {
            Point collisionPointLeft = new Point(HitBox().Left + 1, HitBox().Top + HitBox().Height);
            Point collisionPointRight = new Point(HitBox().Right - 1, HitBox().Top + HitBox().Height);

            isOnGround = false;

            for (int i = 0; i < collisionList.Count; i++)
            {
                if (collisionList[i].Contains(collisionPointLeft) || collisionList[i].Contains(collisionPointRight))
                {
                    isOnGround = true;
                    break;
                }
            }
        }

        //Collision in the X-Axis
        private void HorizontalCollision(List<Rectangle> collisionList)
        {
            Point collisionPointLeft = new Point(HitBox().Left - 2, HitBox().Top + HitBox().Height / 2);
            Point collisionPointRight = new Point(HitBox().Right + 2, HitBox().Top + HitBox().Height / 2);

            for (int i = 0; i < collisionList.Count; i++)
            {
                if (collisionList[i].Intersects(HitBox()))
                {
                    //if (PixelCol(HitBox(), colorArray, collisionList[i], collisionList[i].ColorArray))
                    //{
                        
                    //}

                    if (velocity.X >= 0)
                        position.X = collisionList[i].Left - HitBox().Width;
                    else if (velocity.X < 0)
                        position.X = collisionList[i].Left + collisionList[i].Width;

                    velocity.X *= -1;
                    break;
                }
            }
        }

        //Collision in the Y-Axis
        private void VerticalCollision(List<Rectangle> collisionList)
        {
            if (!canFall)
            {
                //We use these points to check if the unit is on his way to falling of a tile
                Point firstLeftPoint = new Point(HitBox().Left-2, HitBox().Top + HitBox().Height);
                Point secondLeftPoint = new Point(HitBox().Left + 2, HitBox().Top + HitBox().Height);
                Point firstRightPoint = new Point(HitBox().Right + 2, HitBox().Top + HitBox().Height);
                Point secondRightPoint = new Point(HitBox().Right - 2, HitBox().Top + HitBox().Height);

                //By default we say that the unis is falling of both sides of some tile (which he isnt ofc, but we start this way)
                bool SomethingContainsRightPoint = false;
                bool SomethingContainsLeftPoint = false;

                foreach (Rectangle t in collisionList)
                {
                    if (t.Contains(firstRightPoint) || t.Contains(secondRightPoint))
                    {
                        SomethingContainsRightPoint = true;
                    }


                    if (t.Contains(firstLeftPoint) || t.Contains(secondLeftPoint))
                    {
                        SomethingContainsLeftPoint = true;
                    }
                    //If any one of them contains the above collisionpoints that means the unit is NOT falling of that side
                    //So we set the corresponding bool to true
                }
                //if either of the boolens is still false after we have checked all the tiles that means the unit is falling of the tile soon
                //so we simply revers its x-position, dosent matter which side its falling of, just make him turn around
                if (!SomethingContainsRightPoint || !SomethingContainsLeftPoint)
                    velocity.X *= -1;
            }

            //and after that we do the normal collisiondetection and handling
            for (int i = 0; i < collisionList.Count; i++)
            {
                if (collisionList[i].Intersects(HitBox()))
                {
                    //if (PixelCol(HitBox(), colorArray, collisionList[i], collisionList[i].ColorArray))
                    //{
                        
                    //}

                    if (velocity.Y >= 0)
                        position.Y = collisionList[i].Top - HitBox().Height;
                    else if (velocity.Y < 0)
                        position.Y = collisionList[i].Top + collisionList[i].Height;

                    velocity.Y = 0;
                    break;
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
