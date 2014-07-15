using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MobileGame.FileManagement;
using MobileGame.CameraManagement;
using Microsoft.Xna.Framework.Input;

namespace MobileGame
{
    class GameManager
    {
        private MapManager mapManager;
        public static Player Player;
        private bool gameWon, gameLost;

        public GameManager(){}

        public void Initialize()
        {
            EnemyManager.Reset();
            Camera.ClearFocusList();

            if (mapManager == null)
                mapManager = new MapManager();

            mapManager.Initialize();

            if (Player == null)
                Player = new Player();

            Player.SetStartPos(mapManager.PlayerStartPos);

            Camera.XBoundary = MapManager.MapWidth;
            Camera.YBoundary = MapManager.MapHeight;
            Camera.Position = Player.Position;
            Camera.DefaultFocus = Player;

            Game1.graphics.PreferredBackBufferHeight = Camera.CameraHeight;
            Game1.graphics.PreferredBackBufferWidth = Camera.CameraWidth;
            Game1.graphics.ApplyChanges();
        }

        public void Update(float elapsedTime)
        {
            mapManager.Update();

            //Collision against specialblocks is handled outside the player class
            for (int i = 0; i < mapManager.SpecialBlocksList.Count; i++)
            {
                if (mapManager.SpecialBlocksList[i].HitBox().Intersects(Player.HitBox()))
                {
                    mapManager.SpecialBlocksList[i].CollideWithUnit(Player);

                    if (Player.FoundGoal)
                        gameWon = true; 
                }
            }

            EnemyManager.Update(Player, elapsedTime, mapManager.ColliderList);

            if (Player.GotKilled)
                gameLost = true;

            List<Tile> CollisionList = mapManager.FindConnectedTiles(mapManager.ConvertPixelsToIndex(Player.Position));

            //The player handles collision against the generic platforms itself inside the update.
            Player.Update(mapManager.ColliderList);
            //Player.Update(CollisionList);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mapManager.Draw(spriteBatch);

            EnemyManager.Draw(spriteBatch);

            Player.Draw(spriteBatch);
        }

        public void RestarLevel()
        {
            gameWon = false;
            gameLost = false;
            Player.ResetPosition();
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
