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

            //The player handles collision against the generic platforms itself inside the update.
            player.Update(mapManager.ColliderList); 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mapManager.Draw(spriteBatch);
            player.Draw(spriteBatch);
        }
    }
}
