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
            player.Update(mapManager.ColliderList);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mapManager.Draw(spriteBatch);
            player.Draw(spriteBatch);
        }
    }
}
