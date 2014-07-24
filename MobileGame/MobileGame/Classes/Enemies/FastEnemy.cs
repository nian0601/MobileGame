using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MobileGame.Managers;

namespace MobileGame.Enemies
{
    class FastEnemy : SimpleEnemy
    {
        public FastEnemy(int x, int y): base(x, y, false)
        {
            enemyTex = TextureManager.SmallerEnemyTex;
            velocity *= 3;
        }
    }
}
