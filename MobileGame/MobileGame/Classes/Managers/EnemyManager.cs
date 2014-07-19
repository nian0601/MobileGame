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

        public static void Update(Player player, float elapsedTime)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].Update(elapsedTime, player);

                if (enemyList[i].HitBox().Intersects(player.HitBox()))
                    enemyList[i].CollideWithPlayer(player);

                enemyList[i].SpecialAbility(player);
            }
        }

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
