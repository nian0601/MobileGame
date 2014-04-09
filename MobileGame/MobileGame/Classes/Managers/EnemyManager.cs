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

        //Is not used at the moment, each enemy is updated individually in GameManager.cs when the enemyList is being looped through anyway
        //Might be useful later though, if i change my mind. Lets keep it for now
        public static void Update()
        {
            for (int i = 0; i < enemyList.Count; i++)
                enemyList[i].Update();
        }

        //This is not being used at the moment either, for the same reason as Update()
        //Should I however move away from looping through everything in GameManager this will be very useful, so lets keep it for now aswell
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
