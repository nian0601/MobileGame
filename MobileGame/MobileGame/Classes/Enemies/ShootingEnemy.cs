using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MobileGame
{
    class ShootingEnemy : SimpleEnemy
    {
        private float range;

        public ShootingEnemy(int x, int y): base(x, y)
        {
            range = 200;
        }

        public Projectile Shoot(Vector2 Target)
        {
            return new Projectile(position, 5, Target);
        }

        public float Range
        {
            get
            {
                return range;
            }
        }
    }
}
