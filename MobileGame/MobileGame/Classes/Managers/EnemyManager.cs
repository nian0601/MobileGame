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
        private static List<SimpleEnemy> enemyList = new List<SimpleEnemy>();

        //Is not used at the moment, each enemy is updated individually in GameManager.cs when the enemyList is being looped through anyway
        //Might be useful later though, if i change my mind. Lets keep it for now
        public static void Update(Player player, float elapsedTime, List<SimpleTile> simpleCollideList)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].Update(elapsedTime, player, simpleCollideList);

                if (enemyList[i].HitBox().Intersects(player.HitBox()))
                    enemyList[i].CollideWithPlayer(player);

                enemyList[i].SpecialAbility(player);
            }
        }

        //This is not being used at the moment either, for the same reason as Update()
        //Should I however move away from looping through everything in GameManager this will be very useful, so lets keep it for now aswell
        public static void Draw(SpriteBatch spritebatch)
        {
            for (int i = 0; i < enemyList.Count; i++)
                enemyList[i].Draw(spritebatch);
        }

        public static void AddEnemy(SimpleEnemy newEnemy)
        {
            enemyList.Add(newEnemy);
        }

        public static void RemoveEnemy(SimpleEnemy remEnemy)
        {
            enemyList.Remove(remEnemy);
        }

        public static void Reset()
        {
            enemyList.Clear();
        }

        public static List<SimpleEnemy> EnemyList
        {
            get
            {
                return enemyList;
            }
        }
    }
}
