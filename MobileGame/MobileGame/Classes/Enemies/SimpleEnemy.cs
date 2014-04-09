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

        protected Color[] colorArray;

        protected Random rand;

        public SimpleEnemy(int x, int y)
        {
            enemyTex = TextureManager.EnemyTex;
            position = new Vector2(x * enemyTex.Width, y * enemyTex.Height);

            colorArray = new Color[enemyTex.Width * enemyTex.Height];
            enemyTex.GetData(colorArray);

            rand = new Random();

            int value = rand.Next(1, 3);
            if (value == 1)
                velocity = new Vector2(1, 0);
            else if (value == 2)
                velocity = new Vector2(-1, 0);
        }

        public virtual void Update()
        {
            position += velocity;
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

        public virtual void CollideWithEnemyCollider()
        {
            velocity *= -1;
        }

        public virtual Rectangle HitBox()
        {
            Rectangle temp = new Rectangle((int)position.X, (int)position.Y, enemyTex.Width, enemyTex.Height);

            return new Rectangle((int)position.X, (int)position.Y, enemyTex.Width, enemyTex.Height);
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
