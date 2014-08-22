﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MobileGame.FileManagement;
using MobileGame.CameraManagement;
using MobileGame.Units;

using GUI_System.GameStateManagement;

namespace MobileGame.Managers
{
    class GameManager
    {
        public static Player Player;

        internal MapManager mapManager;
        private static bool gameWon, gameLost;

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
            Camera.ResetValues(mapManager.PlayerStartPos);
            Camera.DefaultFocus = Player;
            Camera.Position = mapManager.PlayerStartPos;
            Camera.Limits = new Rectangle(0, 0, MapManager.MapWidth, MapManager.MapHeight); 
        }

        public void Update(float elapsedTime)
        {
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
            Player.Update(elapsedTime);

            Console.WriteLine("FPS: " + (1000 / elapsedTime));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            ScreenManager.Game.GraphicsDevice.SetRenderTarget(Game1.ShaderTarget);
            ScreenManager.Game.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Camera.Get_Transformation());
            Vector2 drawPos;
            if (Player != null)
            {
                drawPos = new Vector2(Player.Position.X - TextureManager.LightSource.Width / 2, Player.Position.Y - TextureManager.LightSource.Height / 2);
                spriteBatch.Draw(TextureManager.LightSource, drawPos, Color.White);
            }
            drawPos = new Vector2(MapManager.GoalPos.X - TextureManager.LightSource.Width / 2, MapManager.GoalPos.Y - TextureManager.LightSource.Height / 2);
            spriteBatch.Draw(TextureManager.LightSource, drawPos, Color.White);
            spriteBatch.End();

            ScreenManager.Game.GraphicsDevice.SetRenderTarget(Game1.MainTarget);
            ScreenManager.Game.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Camera.Get_Transformation());
            mapManager.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Camera.Get_Transformation());
            EnemyManager.Draw(spriteBatch);

            if (Player != null)
                Player.Draw(spriteBatch);
            spriteBatch.End();
            
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
