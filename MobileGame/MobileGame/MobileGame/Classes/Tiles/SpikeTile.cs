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
        public SpikeTile(int x, int y, int type): base(x, y)
        {
            tileTexture = TextureManager.SpikeTextures[type];

            pixelPos = new Vector2(index.X * tileSize, index.Y * tileSize);
        }

        public override void CollideWithUnit(Player Unit)
        {
            Unit.gotKilled = true;
        }

        public override Rectangle HitBox()
        {
            return new Rectangle((int)pixelPos.X, (int)pixelPos.Y, tileTexture.Width, tileTexture.Height);
        }
    }
}
