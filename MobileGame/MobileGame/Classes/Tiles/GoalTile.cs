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
    class GoalTile : SpecialTile
    {
        public GoalTile(int x, int y) : base(x, y)
        {
            myPixelPos = new Vector2(x * MapManager.TileSize, y * MapManager.TileSize);

            myInterestRadius = 450;
            myControlRadius = 250;
        }

        public override void CollideWithUnit(Player Unit)
        {
            Unit.foundGoal = true;
        }

        public override Rectangle HitBox()
        {
            return new Rectangle((int)myPixelPos.X, (int)myPixelPos.Y, MapManager.TileSize, MapManager.TileSize);
        }
    }
}
