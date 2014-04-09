using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MobileGame
{
    class GameManager
    {
        private MapManager mapManager;
        private Player player;

        public GameManager()
        {
            mapManager = new MapManager();
            player = new Player();
        }

        public void Update()
        {
            //Collision against specialblocks is handled outside the player class. Enemy vs specialblocks and enemy vs player should be handled out here aswell
            for (int i = 0; i < mapManager.SpecialBlocksList.Count; i++)
            {
                if (mapManager.SpecialBlocksList[i].HitBox().Intersects(player.HitBox()))
                {
                    mapManager.SpecialBlocksList[i].CollideWithUnit(player);
                }
            }

            for (int i = 0; i < EnemyManager.EnemyList.Count; i++)
            {
                EnemyManager.EnemyList[i].Update();

                if (EnemyManager.EnemyList[i].HitBox().Intersects(player.HitBox()))
                    EnemyManager.EnemyList[i].CollideWithPlayer(player);

                //Loops through all special blocks and checks if any enemies collides with them. This only used to check if an enemy collides with an EnemyCollideTile for the moment
                for (int t = 0; t < mapManager.SpecialBlocksList.Count; t++)
                {
                    if (EnemyManager.EnemyList[i].HitBox().Intersects(mapManager.SpecialBlocksList[t].HitBox()))
                        mapManager.SpecialBlocksList[t].CollideWithUnit(EnemyManager.EnemyList[i]);
                }
            }
                

            //The player handles collision against the generic platforms itself inside the update.
            player.Update(mapManager.ColliderList); 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mapManager.Draw(spriteBatch);

            for (int i = 0; i < EnemyManager.EnemyList.Count; i++)
                EnemyManager.EnemyList[i].Draw(spriteBatch);

            player.Draw(spriteBatch);
        }
    }
}
