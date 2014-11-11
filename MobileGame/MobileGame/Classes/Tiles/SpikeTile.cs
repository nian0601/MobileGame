using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MobileGame.CameraManagement;
using MobileGame.Managers;
using MobileGame.Units;

namespace MobileGame.Tiles
{
    class SpikeTile : SpecialTile
    {
        public SpikeTile(int x, int y): base(x, y)
        {
            myPixelPos = new Vector2(x * MapManager.TileSize, y * MapManager.TileSize);
        }

        public override void CollideWithUnit(Player Unit)
        {
            Unit.gotKilled = true;
        }

        public override Rectangle HitBox()
        {
            return new Rectangle((int)myPixelPos.X, (int)myPixelPos.Y, MapManager.TileSize, MapManager.TileSize);
        }
    }
}
