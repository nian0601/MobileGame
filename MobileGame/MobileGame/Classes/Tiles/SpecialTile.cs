using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MobileGame.Managers;
using MobileGame.Enemies;
using MobileGame.Units;

namespace MobileGame.Tiles
{
    class SpecialTile : Tile
    {
        public SpecialTile(int x, int y) : base(x, y) { }

        public virtual void CollideWithUnit(Player Unit) { }

        public virtual void CollideWithUnit(Player Unit, float ElapsedTime) { }

        public virtual void CollideWithUnit(IEnemy Unit) { }
    }
}
