using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MobileGame
{
    class Player
    {
        private Texture2D playerTex;

        private Vector2 position;
        private Vector2 velocity;

        private float maxSpeed;
        private float acceleration;
        private float friction;
        private float gravity;
        private float jumpPower;

        private bool isOnGround;

        public Player()
        {
            position = new Vector2(50, 100);
            playerTex = TextureManager.PlayerTile;

            velocity = new Vector2(2, 1);
            maxSpeed = 3f;
            acceleration = 0.1f;
            friction = 0.95f;
            gravity = 0.4f;
            jumpPower = 5.0f;

            isOnGround = false;
        }

        public void Update(List<Tile> collisionList)
        {
            ListenToInput();
            CheckIfOnGround(collisionList);

            position.X += velocity.X;
            HorizontalCollision(collisionList);

            velocity.Y += gravity;

            position.Y += velocity.Y;
            VerticalCollision(collisionList); 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerTex, position, Color.White);
        }

        private void ListenToInput()
        {
            if (!KeyMouseReader.isKeyDown(Keys.A) && !KeyMouseReader.isKeyDown(Keys.D))
                velocity.X *= friction;

            if (Math.Abs(velocity.X) < 0.05f)
                velocity.X = 0;

            if (KeyMouseReader.isKeyDown(Keys.W))
                Jump();

            if (KeyMouseReader.isKeyDown(Keys.A))
            {
                if (velocity.X > -maxSpeed)
                    velocity.X -= acceleration;
            }    

            if (KeyMouseReader.isKeyDown(Keys.D))
            {
                velocity.X += acceleration;
                if (velocity.X > maxSpeed)
                    velocity.X = maxSpeed;
            }
                
        }

        private void CheckIfOnGround(List<Tile> collisionList)
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

        private void Jump()
        {
            if (isOnGround)
            {
                velocity.Y -= jumpPower;

                Console.WriteLine("Vel: " + velocity.Y + ", Power: " + jumpPower);
            }
        }

        private void HorizontalCollision(List<Tile> collisionList)
        {
            for (int i = 0; i < collisionList.Count; i++)
            {
                if(collisionList[i].HitBox().Intersects(HitBox()))
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

        private void VerticalCollision(List<Tile> collisionList)
        {
            for (int i = 0; i < collisionList.Count; i++)
            {
                if (collisionList[i].HitBox().Intersects(HitBox()))
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

        //Returns a rectangle representing the player hitbox
        public Rectangle HitBox()
        {
            Rectangle temp = new Rectangle((int)position.X, (int)position.Y, playerTex.Width, playerTex.Height);

            return new Rectangle((int)position.X, (int)position.Y, playerTex.Width, playerTex.Height);
        }
    }
}
