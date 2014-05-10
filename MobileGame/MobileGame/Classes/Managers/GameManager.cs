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
        private static Player player;
        private static bool gameWon, gameLost;

        public GameManager()
        {
            mapManager = new MapManager();
            player = new Player();
            player.SetStartPos(mapManager.PlayerStartPos);
        }

        public void Update(float elapsedTime)
        {
            mapManager.Update();

            //Collision against specialblocks is handled outside the player class
            for (int i = 0; i < mapManager.SpecialBlocksList.Count; i++)
            {
                if (mapManager.SpecialBlocksList[i].HitBox().Intersects(player.HitBox()))
                {
                    mapManager.SpecialBlocksList[i].CollideWithUnit(player);

                    if (player.FoundGoal)
                        gameWon = true;

                    
                }
            }

            EnemyManager.Update(player, elapsedTime, mapManager.ColliderList);

            if (player.GotKilled)
                gameLost = true;
            

            //The player handles collision against the generic platforms itself inside the update.
            player.Update(mapManager.ColliderList); 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mapManager.Draw(spriteBatch);

            EnemyManager.Draw(spriteBatch);

            player.Draw(spriteBatch);
        }

        public static void RestarLevel()
        {
            gameWon = false;
            gameLost = false;
            player.ResetPosition();
        }

        public bool GameWon
        {
            get { return gameWon; }
        }

        public bool GameLost
        {
            get { return gameLost; }
        }
    }
}
