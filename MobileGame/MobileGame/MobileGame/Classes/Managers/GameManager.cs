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
using MobileGame.Lights;

using GUI_System.GameStateManagement;

namespace MobileGame.Managers
{
    class GameManager
    {
        public static Player Player;

        internal MapManager mapManager;
        private static bool gameWon, gameLost;

        private List<AmbientLight> ambientLightHandles;
        private List<PointLight> spotLightHandles;

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

            ambientLightHandles = new List<AmbientLight>();
            spotLightHandles = new List<PointLight>();

            ambientLightHandles.Add(new AmbientLight(Color.White * 0.40f));
            ambientLightHandles.Add(new AmbientLight(Color.White));
            spotLightHandles.Add(new PointLight(Player.Position, 200, 0.8f, Color.White));

            LightingManager.AmbientLights.Add(ambientLightHandles[0]);
            LightingManager.PointLights.Add(spotLightHandles[0]);
        }

        public void Update(float elapsedTime)
        {
            LightingManager.Update();
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
            Player.Update(elapsedTime);

            if (KeyMouseReader.KeyClick(Keys.F))
            {
                if (LightingManager.AmbientLights.Contains(ambientLightHandles[1]))
                    LightingManager.AmbientLights.Remove(ambientLightHandles[1]);
                else
                    LightingManager.AmbientLights.Add(ambientLightHandles[1]);
            }

            spotLightHandles[0].Position.X = Player.Position.X;
            spotLightHandles[0].Position.Y = Player.Position.Y;
            //Console.WriteLine("FPS: " + (1000 / elapsedTime));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw all the Lights we have added to LightingManager.LightTarget
            LightingManager.BeginDrawMainTarget();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Camera.Get_Transformation());

            mapManager.DrawBackground(spriteBatch);
            mapManager.DrawMiddle(spriteBatch);
            mapManager.DrawForeground(spriteBatch);

            EnemyManager.Draw(spriteBatch);

            if (Player != null)
                Player.Draw(spriteBatch);
            spriteBatch.End();

            LightingManager.EndDrawingMainTarget();

            LightingManager.DrawLitScreen();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Camera.Get_Transformation());
            Camera.Draw(spriteBatch);
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
