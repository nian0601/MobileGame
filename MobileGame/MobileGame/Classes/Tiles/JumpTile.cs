using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MobileGame
{
    class JumpTile : SpecialTile
    {
        public JumpTile(int x, int y): base(x, y)
        {
            tileTexture = TextureManager.JumpTile;

            pixelPos = new Vector2(index.X * tileSize, index.Y * tileSize);
        }

        public override void CollideWithUnit(Player Unit)
        {
            Point unitCollisionPointLeft = new Point(Unit.HitBox().Left + 15, Unit.HitBox().Top + Unit.HitBox().Height - 2);
            Point unitCollisionPointRight = new Point(Unit.HitBox().Right - 15, Unit.HitBox().Top + Unit.HitBox().Height - 2);

            if(HitBox().Contains(unitCollisionPointLeft) || HitBox().Contains(unitCollisionPointRight))
                Unit.Jump(1.5f);

            base.CollideWithUnit(Unit);
        }
    }
}
