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
        public static Player Player;
        private AnimatedObject testAnimation;
        internal MapManager mapManager;
        private static bool gameWon, gameLost;

        public GameManager(){}

        public void Initialize()
        {
            testAnimation = new AnimatedObject();

            EnemyManager.Reset();
            Camera.ClearFocusList();

            if (mapManager == null)
                mapManager = new MapManager();

            mapManager.Initialize();

            if (Player == null)
                Player = new Player();

            Player.SetStartPos(mapManager.PlayerStartPos);

            Camera.Position = Player.Position;
            Camera.DefaultFocus = Player;
            Camera.Limits = new Rectangle(0, 0, MapManager.MapWidth, MapManager.MapHeight);

           
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

            EnemyManager.Update(Player, elapsedTime);

            //End the game if the player got killed by something
            if (Player.GotKilled)
                gameLost = true;

            //The player handles collision against the generic platforms itself inside the update.
            Player.Update();

            testAnimation.Update(elapsedTime);

            //Console.WriteLine("FPS: " + (1000 / elapsedTime));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mapManager.Draw(spriteBatch);

            EnemyManager.Draw(spriteBatch);

            Player.Draw(spriteBatch);

            testAnimation.Draw(spriteBatch);
        }

        public static void RestarLevel()
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
