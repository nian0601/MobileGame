using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MobileGame.CameraManagement;

namespace MobileGame
{
    class GoalTile : SpecialTile
    {
        public GoalTile(int x, int y) : base(x, y, false)
        {
            tileTexture = TextureManager.GoalTile;

            pixelPos = new Vector2(index.X * tileSize, index.Y * tileSize - tileTexture.Height / 2);

            interestRadius = 600;
            controlRadius = 300;
        }

        public override void CollideWithUnit(Player Unit)
        {
            Unit.foundGoal = true;
        }

        public override Rectangle HitBox()
        {
            return new Rectangle((int)pixelPos.X, (int)pixelPos.Y, tileTexture.Width, tileTexture.Height);
        }
    }
}
