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
        private static List<ShootingEnemy> shootingEnemies = new List<ShootingEnemy>();
        private static List<Projectile> projectileList = new List<Projectile>();

        //Is not used at the moment, each enemy is updated individually in GameManager.cs when the enemyList is being looped through anyway
        //Might be useful later though, if i change my mind. Lets keep it for now
        public static void Update(Player player, List<SpecialTile> collideList)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].Update();

                if (enemyList[i].HitBox().Intersects(player.HitBox()))
                    enemyList[i].CollideWithPlayer(player);

                //Loops through all special blocks and checks if any enemies collides with them. This only used to check if an enemy collides with an EnemyCollideTile for the moment
                for (int t = 0; t < collideList.Count; t++)
                {
                    if (enemyList[i].HitBox().Intersects(collideList[t].HitBox()))
                        collideList[t].CollideWithUnit(enemyList[i]);
                }
            }

            for (int i = 0; i < shootingEnemies.Count; i++)
            {
                shootingEnemies[i].Update();

                if (GetDistance(shootingEnemies[i].Position, player.Position) < shootingEnemies[i].Range)
                    projectileList.Add(shootingEnemies[i].Shoot(player.Position));
            }

            for (int i = 0; i < projectileList.Count; i++)
                projectileList[i].Update();
        }

        //This is not being used at the moment either, for the same reason as Update()
        //Should I however move away from looping through everything in GameManager this will be very useful, so lets keep it for now aswell
        public static void Draw(SpriteBatch spritebatch)
        {
            for (int i = 0; i < enemyList.Count; i++)
                enemyList[i].Draw(spritebatch);

            for (int i = 0; i < shootingEnemies.Count; i++)
                shootingEnemies[i].Draw(spritebatch);

            for (int i = 0; i < projectileList.Count; i++)
                projectileList[i].Draw(spritebatch);
        }

        public static void AddEnemy(SimpleEnemy newEnemy)
        {
            enemyList.Add(newEnemy);
        }

        public static void AddEnemy(ShootingEnemy newEnemy)
        {
            shootingEnemies.Add(newEnemy);
        }

        public static void RemoveEnemy(SimpleEnemy remEnemy)
        {
            enemyList.Remove(remEnemy);
        }

        public static List<SimpleEnemy> EnemyList
        {
            get
            {
                return enemyList;
            }
        }

        private static float GetDistance(Vector2 PosOne, Vector2 PosTwo)
        {
            Vector2 tempDis;

            tempDis.X = PosTwo.X - PosOne.X;
            tempDis.Y = PosTwo.Y - PosTwo.Y;

            float dist = tempDis.Length();

            return dist;
        }
    }
}
