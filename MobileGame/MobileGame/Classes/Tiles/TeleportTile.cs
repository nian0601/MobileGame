using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MobileGame
{
    class TeleportTile : SpecialTile
    {
        public TeleportTile(int x, int y) : base(x, y)
        {
            tileTexture = TextureManager.TeleportTile;

            tileSize = tileTexture.Width;
            pixelPos = new Vector2(index.X * tileSize, index.Y * tileSize);
        }

        public override void CollideWithUnit(Player Unit)
        {
            Point unitCollisionPoint = new Point(Unit.HitBox().Left + Unit.HitBox().Width/2, Unit.HitBox().Top + Unit.HitBox().Height/2);

            if(HitBox().Contains(unitCollisionPoint) && KeyMouseReader.KeyClick(Keys.Space))
                Unit.ResetPosition();
            base.CollideWithUnit(Unit);
        }
    }
}
