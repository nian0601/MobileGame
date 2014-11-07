using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MobileGame.Managers;
using MobileGame.Units;

namespace MobileGame.Tiles
{
    class JumpTile : SpecialTile
    {
        public JumpTile(int x, int y): base(x, y)
        {
            myPixelPos = new Vector2(x* MapManager.TileSize, y * MapManager.TileSize);
        }

        public override void CollideWithUnit(Player Unit)
        {
            Point unitCollisionPointLeft = new Point(Unit.HitBox().Left + 15, Unit.HitBox().Top + Unit.HitBox().Height - 2);
            Point unitCollisionPointRight = new Point(Unit.HitBox().Right - 15, Unit.HitBox().Top + Unit.HitBox().Height - 2);

            if(HitBox().Contains(unitCollisionPointLeft) && HitBox().Contains(unitCollisionPointRight))
                Unit.Jump(1.5f);

            base.CollideWithUnit(Unit);
        }

        public override Rectangle HitBox()
        {
            return new Rectangle((int)myPixelPos.X, (int)myPixelPos.Y, MapManager.TileSize, MapManager.TileSize);
        }
    }
}
