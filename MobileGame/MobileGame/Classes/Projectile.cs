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
            projTex = TextureManager.SmallerEnemyTex;
            position = Position;
            target = Target;
            speed = Speed;
            direction = CalculateDirection();
            active = true;

            lifeTime = 2000;
            timeLived = 0;
        }

        public void Update(Player player, float elapsedTime)
        {
            timeLived += elapsedTime;

            position.X += direction.X * speed;
            position.Y += direction.Y * speed;

            if (timeLived >= lifeTime)
                active = false;

            if (HitBox().Intersects(player.HitBox()))
                if (AccuratePlayerHitCheck(player.HitBox()))
                    active = false;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(projTex, position, Color.White);
        }

        public void ResetValues(Vector2 Position, float Speed, Vector2 Target)
        {
            position = Position;
            target = Target;
            speed = Speed;
            direction = CalculateDirection();
            active = true;
            timeLived = 0;
        }

        private Vector2 CalculateDirection()
        {
            Vector2 tempDir;

            tempDir.X = target.X - position.X;
            tempDir.Y = target.Y - position.Y;

            float directionLenght = tempDir.Length();

            tempDir.X /= directionLenght;
            tempDir.Y /= directionLenght;

            Vector2 finalDir = Vector2.Normalize(tempDir);

            return finalDir;
        }

        private Rectangle HitBox()
        {
            return new Rectangle((int)position.X, (int)position.Y, projTex.Width, projTex.Height);
        }

        public bool Active
        {
            get { return active; }
        }

        //This could use some work, perhaps implement pixelperfect collision?
        //or atleast use like 4 points
        private bool AccuratePlayerHitCheck(Rectangle PlayerHitBox)
        {
            Point projCenterPoint = new Point(HitBox().Left + HitBox().Width / 2, HitBox().Top + HitBox().Height / 2);

            if(PlayerHitBox.Contains(projCenterPoint))
                return true;

            return false;
        }
    }
}
