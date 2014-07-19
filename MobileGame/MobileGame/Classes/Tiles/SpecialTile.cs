using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobileGame
{
    class SpecialTile : Tile
    {
        public SpecialTile(int x, int y) : base(x, y) { }

        public virtual void CollideWithUnit(Player Unit) { }

        public virtual void CollideWithUnit(IEnemy Unit) { }
    }
}
