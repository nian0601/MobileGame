using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MobileGame.FileManagement;
using MobileGame.CameraManagement;
using MobileGame.Units;
using MobileGame.LightingSystem;

using GUI_System.GameStateManagement;

namespace MobileGame.Managers
{
    class GameManager
    {
        public static Player Player;

        internal MapManager mapManager;
        private static bool gameWon, gameLost;


        private LightRenderer LightRenderer;
        private List<PointLight> pointLightHandles;
        private List<SpotLight> spotLightHandles;

        private Vector2 spotLightDir;
        private Vector2 lightPos;
        private bool myAffectLightDir;
        private int myCurrLight;

        public static RenderTarget2D finalRenderTarget;

        public GameManager(){}

        public void Initialize()
        {
            LightRenderer = new LightRenderer(Game1.graphics);
            EnemyManager.Reset();
            Camera.ClearFocusList();

            if (mapManager == null)
                mapManager = new MapManager();

            mapManager.Initialize(LightRenderer);
            finalRenderTarget = new RenderTarget2D(ScreenManager.Game.GraphicsDevice, MapManager.MapWidth, MapManager.MapHeight);

            if (Player == null)
                Player = new Player();

            Player.SetStartPos(mapManager.PlayerStartPos);
            lightPos = mapManager.PlayerStartPos;

            Camera.ResetValues(mapManager.PlayerStartPos);
            Camera.DefaultFocus = Player;
            Camera.Position = mapManager.PlayerStartPos;
            Camera.Limits = new Rectangle(0, 0, MapManager.MapWidth, MapManager.MapHeight);


            pointLightHandles = new List<PointLight>();
            spotLightHandles = new List<SpotLight>();

            
            LightRenderer.Initialize(0, 0, MapManager.MapWidth, MapManager.MapHeight);

            LightRenderer.minLight = 0.15f;
            LightRenderer.lightBias = 10f;
            spotLightDir = Vector2.UnitX * -1.00001f;

            //pointLightHandles.Add(new PointLight(new Vector2(200, 200), 1f, 300f, Color.White));

            pointLightHandles.Add(new PointLight(new Vector2(200, 200), 1f, 200f, Color.Red));
            pointLightHandles.Add(new PointLight(new Vector2(200, 200), 1f, 250f, Color.LightGreen));
            pointLightHandles.Add(new PointLight(new Vector2(200, 200), 1f, 250f, Color.LightGreen));


            spotLightHandles.Add(new SpotLight(new Vector2(200, 200), spotLightDir, 1f, 2f, 0.5f, 500f, Color.LightBlue));
            spotLightHandles.Add(new SpotLight(new Vector2(200, 200), spotLightDir, 1f, 2f, 0.5f, 500f, Color.Red));

            //LightRenderer.pointLights.Add(pointLightHandles[0]);
            //LightRenderer.pointLights.Add(pointLightHandles[1]);
            //LightRenderer.pointLights.Add(pointLightHandles[2]);
            //LightRenderer.spotLights.Add(spotLightHandles[0]);
            //LightRenderer.spotLights.Add(spotLightHandles[1]);


            LightRenderer.LoadContent(ScreenManager.Game.Content);
            Console.WriteLine("PointLights.Count: " + LightRenderer.pointLights.Count);
            myAffectLightDir = false;
            myCurrLight = 0;
        }

        public void Update(float elapsedTime)
        {
            mapManager.Update(elapsedTime);

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

            if (KeyMouseReader.isKeyDown(Keys.Up))
            {
                lightPos.Y -= 5;
                if(myAffectLightDir)
                    spotLightDir = Vector2.UnitY * -1.00001f;
            }
                
            if (KeyMouseReader.isKeyDown(Keys.Down))
            {
                lightPos.Y += 5;
                if (myAffectLightDir)
                    spotLightDir = Vector2.UnitY * 1.00001f;
            }
                
            if (KeyMouseReader.isKeyDown(Keys.Left))
            {
                lightPos.X -= 5;
                if (myAffectLightDir)
                    spotLightDir = Vector2.UnitX * 1.00001f;
            }
                
            if (KeyMouseReader.isKeyDown(Keys.Right))
            {
                lightPos.X += 5;
                if (myAffectLightDir)
                    spotLightDir = Vector2.UnitX * -1.00001f;
            }

            if (KeyMouseReader.KeyClick(Keys.F))
                myAffectLightDir = !myAffectLightDir;


            if (KeyMouseReader.KeyClick(Keys.F))
                myAffectLightDir = !myAffectLightDir;

            if (KeyMouseReader.KeyClick(Keys.D1))
                myCurrLight = 0;
            if (KeyMouseReader.KeyClick(Keys.D2))
                myCurrLight = 1;
            if (KeyMouseReader.KeyClick(Keys.D3))
                myCurrLight = 2;

            //spotLightHandles[myCurrLight].Position.X = lightPos.X;
            //spotLightHandles[myCurrLight].Position.Y = lightPos.Y;
            //spotLightHandles[myCurrLight].direction = spotLightDir;
            pointLightHandles[myCurrLight].Position = lightPos;

            //Console.WriteLine("FPS: " + (1000 / elapsedTime));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Begin to draw background (stuff that WONT cast shadows)
            LightRenderer.BeginDrawBackground();
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            //Draw the background
            mapManager.DrawBackground(spriteBatch);
            mapManager.DrawMiddle(spriteBatch);
            mapManager.DrawForeground(spriteBatch);
            mapManager.DrawAnimatedTiles(spriteBatch);

            if (Player != null)
                Player.Draw(spriteBatch);
            EnemyManager.Draw(spriteBatch);

            spriteBatch.End();
            LightRenderer.EndDrawBackground();
            //Finished drawing background

            //Begin to draw shadowcasters (stuff that WILL cast shadows)
            LightRenderer.BeginDrawShadowCasters();
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            //Draw shadwocasters
            mapManager.DrawMiddle(spriteBatch);

            spriteBatch.End();
            LightRenderer.EndDrawShadowCasters();
            //Finished drawing shadowcasters

            //Draw the finished, lit screen
            LightRenderer.DrawLitScene();


            Vector2 finalDrawPos = new Vector2
            (
                Camera.Position.X - Camera.ViewPort.Width / 2,
                Camera.Position.Y - Camera.ViewPort.Height / 2
            );
            Rectangle finalSource = new Rectangle
            (
                (int)finalDrawPos.X,
                (int)finalDrawPos.Y,
                Camera.ViewPort.Width,
                Camera.ViewPort.Height
            );

            ScreenManager.Game.GraphicsDevice.SetRenderTarget(null);
            ScreenManager.Game.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Camera.Get_Transformation());

            spriteBatch.Draw(finalRenderTarget, finalDrawPos, finalSource, Color.White);
            
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
