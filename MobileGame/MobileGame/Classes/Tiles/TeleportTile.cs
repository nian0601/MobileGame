using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using MobileGame.Managers;
using MobileGame.Units;

namespace MobileGame.Tiles
{
    class TeleportTile : SpecialTile
    {
        public TeleportTile(int x, int y) : base(x, y)
        {
            myPixelPos = new Vector2(x * MapManager.TileSize, y * MapManager.MapHeight);
        }

        public override void CollideWithUnit(Player Unit)
        {
            Point unitCollisionPoint = new Point(Unit.HitBox().Left + Unit.HitBox().Width/2, Unit.HitBox().Top + Unit.HitBox().Height/2);

            if(HitBox().Contains(unitCollisionPoint) && KeyMouseReader.KeyClick(Keys.Space))
                Unit.ResetPosition();
        }

        public override Rectangle HitBox()
        {
            return new Rectangle((int)myPixelPos.X, (int)myPixelPos.Y, MapManager.TileSize, MapManager.TileSize);
        }
    }
}
