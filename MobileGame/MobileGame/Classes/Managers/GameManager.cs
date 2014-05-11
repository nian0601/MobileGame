using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MobileGame.FileManagement;

namespace MobileGame
{
    class GameManager
    {
        private MapManager mapManager;
        private Player player;
        private bool gameWon, gameLost;

        public GameManager(){}

        public void Initialize()
        {
            EnemyManager.Reset();

            if (mapManager == null)
                mapManager = new MapManager();

            mapManager.Initialize();

            if (player == null)
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

            if (KeyMouseReader.KeyClick(Microsoft.Xna.Framework.Input.Keys.T))
                FileLoader.SaveLevel(mapManager.CurrentMap);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mapManager.Draw(spriteBatch);

            EnemyManager.Draw(spriteBatch);

            player.Draw(spriteBatch);
        }

        public void RestarLevel()
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
