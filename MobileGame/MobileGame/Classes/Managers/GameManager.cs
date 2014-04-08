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
            for (int i = 0; i < mapManager.SpecialBlocksList.Count; i++)
            {
                if (mapManager.SpecialBlocksList[i].HitBox().Intersects(player.HitBox()))
                {
                    Console.WriteLine("COLLIDING WITH SPECIAL BLOCK!");
                    player.Jump();
                }
            }

            player.Update(mapManager.ColliderList);

            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mapManager.Draw(spriteBatch);
            player.Draw(spriteBatch);
        }
    }
}
