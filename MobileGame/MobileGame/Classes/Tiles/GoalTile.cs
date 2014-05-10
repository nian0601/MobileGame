using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MobileGame
{
    class GoalTile : SpecialTile
    {
        public GoalTile(int x, int y) : base(x, y)
        {
            tileTexture = TextureManager.GoalTile;

            pixelPos = new Vector2(index.X * tileSize, index.Y * tileSize);
        }

        public override void CollideWithUnit(Player Unit)
        {
            Unit.foundGoal = true;
        }
    }
}
