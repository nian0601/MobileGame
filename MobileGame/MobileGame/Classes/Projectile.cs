using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MobileGame
{
    class Projectile
    {
        private Vector2 position;
        private Vector2 target;
        private Vector2 direction;
        private float speed;

        private Texture2D projTex;

        private float lifeTime;
        private float timeLived;

        private bool active;

        public Projectile(Vector2 Position, float Speed, Vector2 Target)
        {
            projTex = TextureManager.LayerdGoalTile;
            position = Position;
            target = Target;
            speed = Speed;
            direction = CalculateDirection();
        }

        public void Update()
        {
            position.X += direction.X * speed;
            position.Y += direction.Y * speed;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(projTex, position, Color.White);
        }

        private Vector2 CalculateDirection()
        {
            Vector2 tempDir;

            tempDir.X = target.X - position.X;
            tempDir.Y = target.Y - position.Y;

            float directionLenght = (float)Math.Sqrt(tempDir.X * tempDir.X + tempDir.Y + tempDir.Y);

            tempDir.X /= directionLenght;
            tempDir.Y /= directionLenght;

            Console.WriteLine(tempDir);

            return tempDir;
        }
    }
}
