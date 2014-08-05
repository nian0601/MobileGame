using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MobileGame.CameraManagement;
using MobileGame.Tiles;
using MobileGame.Managers;
using MobileGame.Animations;

namespace MobileGame.Units
{
    class Player : IFocusable
    {
        private Texture2D playerTex;

        private Vector2 position;
        private Vector2 startPos;
        private Vector2 velocity;

        private float maxSpeed;
        private float acceleration;
        private float friction;
        private float gravity;
        private float jumpPower;

        private bool isOnGround;

        internal bool foundGoal;
        internal bool gotKilled;

        private Color[] colorArray;

        private Animator animator;
        private int FrameWidth, FrameHeight;

        int colRange;

        #region Properties

        public Vector2 Position
        {
            get { return position; }
            private set { position = value; }
        }

        public Color[] ColorArray
        {
            get { return colorArray; }
        }

        public bool FoundGoal
        {
            get { return foundGoal; }
        }

        public bool GotKilled
        {
            get { return gotKilled; }
        }

        public int InterestRadius
        {
            get { return 200; }
        }

        public int ControlRadius
        {
            get { return 100; }
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

        public Player() : base()
        {
            Position = new Vector2(150, 120);
            playerTex = TextureManager.PlayerSheet;

            velocity = new Vector2(0, 0);
            maxSpeed = 3f;
            acceleration = 0.1f;
            friction = 0.95f;
            gravity = 0.4f;
            jumpPower = 6.0f;

            isOnGround = false;

            colorArray = new Color[playerTex.Width * playerTex.Height];
            TextureManager.PlayerColTex.GetData(colorArray);

            colRange = 7;

            FrameWidth = 22;
            FrameHeight = 60;

            animator = new Animator(FrameWidth, FrameHeight);
            animator.AddAnimation(new Animation("right", 100, 0, 0, 8, 0));
            animator.AddAnimation(new Animation("left", 100, 0, 1, 8, 1));
        }

        public void Update(float ElapsedTime)
        {
            animator.Update(ElapsedTime);

            //Generate the CollisionList
            List<Tile> CollisionList = MapManager.GenerateCollisionList((int)Position.X, (int)Position.Y, colRange, colRange);

            ListenToInput(ElapsedTime);
            CheckIfOnGround(CollisionList);

            //Apply horizontal velocity
            position.X += velocity.X;
            //Make sure position isnt outside the map
            position.X = MathHelper.Clamp(Position.X, -100, MapManager.MapWidth);
            //And set the players speed to 0 if he is at the edge of the map
            if (Position.X == 0 || Position.X == MapManager.MapWidth - FrameWidth)
                velocity.X = 0;
            //Run CollisionCheck
            HorizontalCollision(CollisionList);

            //Apply gravity and vertical velocity
            velocity.Y += gravity;
            position.Y += velocity.Y;
            //Make sure position isnt outside the map
            position.Y = MathHelper.Clamp(Position.Y, -100, MapManager.MapHeight);
            if (Position.Y == 0)
                velocity.Y = 0;
            else if (Position.Y >= MapManager.MapHeight - FrameHeight)
            {
                gotKilled = true;
            }
            //Run CollisionCheck
            VerticalCollision(CollisionList);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(playerTex, Position, SourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(playerTex, Position, animator.SourceRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        public void Jump(float jumpMultiplier)
        {
            if (isOnGround)
            {
                velocity.Y -= jumpPower * jumpMultiplier;
                isOnGround = false;
            }
        }

        public void SetStartPos(Vector2 Pos)
        {
            startPos.X = Pos.X + FrameWidth;
            startPos.Y = Pos.Y - FrameHeight;
            ResetPosition();
        }

        public void ResetPosition()
        {
            Position = startPos;
            gotKilled = false;
            foundGoal = false;
        }

        public Rectangle HitBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, FrameWidth, FrameHeight);
        }

        private void ListenToInput(float ElapsedTime)
        {
            if (KeyMouseReader.KeyClick(Keys.Y))
                animator.StopAnimation();

            if (!KeyMouseReader.isKeyDown(Keys.A) && !KeyMouseReader.isKeyDown(Keys.D))
            {
                velocity.X *= friction;
                animator.StopAnimation();
            }
                

            if (Math.Abs(velocity.X) < 0.05f)
                velocity.X = 0;

            if (KeyMouseReader.isKeyDown(Keys.W))
                Jump(1);

            if (KeyMouseReader.isKeyDown(Keys.A))
            {
                if (velocity.X > -maxSpeed)
                    velocity.X -= acceleration;

                animator.StopAnimation("right");
                animator.StartAnimation("left");
            }

            if (KeyMouseReader.isKeyDown(Keys.D))
            {
                velocity.X += acceleration;
                if (velocity.X > maxSpeed)
                    velocity.X = maxSpeed;

                animator.StopAnimation("left");
                animator.StartAnimation("right");
            }

            if (Math.Abs(velocity.Y) > 1.5f)
                animator.StopAnimation();
        }

        private void CheckIfOnGround(List<Tile> collisionList)
        {
            Point collisionPointLeft = new Point(HitBox().Left + 4, HitBox().Top + HitBox().Height);
            Point collisionPointRight = new Point(HitBox().Right - 4, HitBox().Top + HitBox().Height);
            Point collisionPointCenter = new Point(HitBox().Right - HitBox().Width / 2, HitBox().Top + HitBox().Height);

            isOnGround = false;

            for (int i = 0; i < collisionList.Count; i++)
            {
                if (collisionList[i].HitBox().Contains(collisionPointLeft) || collisionList[i].HitBox().Contains(collisionPointRight) || collisionList[i].HitBox().Contains(collisionPointCenter))
                {
                    isOnGround = true;
                    break;
                }
            }
        }

        //Collision in the X-Axis
        private void HorizontalCollision(List<Tile> collisionList)
        {
            for (int i = 0; i < collisionList.Count; i++)
            {
                if (collisionList[i].HitBox().Intersects(HitBox()) && !collisionList[i].canJumpThrough)
                {
                    if (PixelCol(HitBox(), colorArray, collisionList[i].HitBox(), collisionList[i].ColorArray))
                    {
                        if (velocity.X >= 0)
                            position.X = collisionList[i].HitBox().Left - HitBox().Width;
                        else if (velocity.X < 0)
                            position.X = collisionList[i].HitBox().Left + collisionList[i].HitBox().Width;

                        velocity.X = 0;
                        break;
                    }
                }
            }
        }

        //Collision in the Y-Axis
        private void VerticalCollision(List<Tile> collisionList)
        {
            for (int i = 0; i < collisionList.Count; i++)
            {
                if (collisionList[i].HitBox().Intersects(HitBox()))
                {
                    if (PixelCol(HitBox(), colorArray, collisionList[i].HitBox(), collisionList[i].ColorArray))
                    {
                        if (velocity.Y >= 0 && HitBox().Bottom > collisionList[i].HitBox().Top)
                        {
                            position.Y = collisionList[i].HitBox().Top - HitBox().Height;
                            velocity.Y = 0;
                        }
                        else if (collisionList[i].canJumpThrough && HitBox().Bottom > collisionList[i].HitBox().Top-2)
                        {

                        }
                        //Jumping
                        else if (velocity.Y < 0)
                        {
                            position.Y = collisionList[i].HitBox().Top + collisionList[i].HitBox().Height;
                            velocity.Y = 0;
                        }
                        
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
