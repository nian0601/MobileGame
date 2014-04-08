using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MobileGame
{
    class EnemyCollideTile : SpecialTile
    {
        public EnemyCollideTile(int x, int y): base(x, y)
        {
            tileTexture = TextureManager.LayerdGoalTile;
            shouldDraw = false;

            tileSize = tileTexture.Height;
            pixelPos = new Vector2(index.X * tileSize, index.Y * tileSize);
        }

        public override void CollideWithUnit(Enemy Unit)
        {
            Point unitCollisionPointLeft = new Point(Unit.HitBox().Left + 2, Unit.HitBox().Top + Unit.HitBox().Height - 2);
            Point unitCollisionPointRight = new Point(Unit.HitBox().Right - 2, Unit.HitBox().Top + Unit.HitBox().Height - 2);

            if(HitBox().Contains(unitCollisionPointLeft) && HitBox().Contains(unitCollisionPointRight))
                Unit.FlipVelocity();

            base.CollideWithUnit(Unit);
        }
    }
}
