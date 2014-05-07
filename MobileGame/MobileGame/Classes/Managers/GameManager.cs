﻿using System;
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
                }
            }

            EnemyManager.Update(player, elapsedTime, mapManager.ColliderList);

            //The player handles collision against the generic platforms itself inside the update.
            player.Update(mapManager.ColliderList); 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mapManager.Draw(spriteBatch);

            EnemyManager.Draw(spriteBatch);

            player.Draw(spriteBatch);
        }
    }
}
