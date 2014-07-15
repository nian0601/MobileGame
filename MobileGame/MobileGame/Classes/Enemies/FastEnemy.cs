using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobileGame
{
    class FastEnemy : SimpleEnemy
    {
        public FastEnemy(int x, int y): base(x, y)
        {
            enemyTex = TextureManager.SmallerEnemyTex;
            velocity *= 3;
        }
    }
}
