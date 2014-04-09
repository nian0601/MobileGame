using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MobileGame
{
    static class EnemyManager
    {
        private static List<Enemy> enemyList = new List<Enemy>();

        public static void Update()
        {
            for (int i = 0; i < enemyList.Count; i++)
                enemyList[i].Update();
        }

        public static void Draw(SpriteBatch spritebatch)
        {
            for (int i = 0; i < enemyList.Count; i++)
                enemyList[i].Draw(spritebatch);
        }

        public static void AddEnemy(Enemy newEnemy)
        {
            enemyList.Add(newEnemy);
        }

        public static void RemoveEnemy(Enemy remEnemy)
        {
            enemyList.Remove(remEnemy);
        }

        public static List<Enemy> EnemyList
        {
            get
            {
                return enemyList;
            }
        }
    }
}
