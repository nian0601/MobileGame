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
            EnemyManager.Reset();
            Camera.ClearFocusList();

            if (mapManager == null)
                mapManager = new MapManager();

            mapManager.Initialize();

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

            LightRenderer = new LightingSystem.LightRenderer(Game1.graphics);
            LightRenderer.Initialize();
            LightRenderer.minLight = 0.0f;
            LightRenderer.lightBias = 10f;

            spotLightDir = Vector2.UnitX * -1.00001f;

            pointLightHandles.Add(new PointLight(new Vector2(ScreenManager.Game.GraphicsDevice.Viewport.Width / 2, ScreenManager.Game.GraphicsDevice.Viewport.Height / 2), 1000f, 300f, Color.White));
            spotLightHandles.Add(new SpotLight(new Vector2(200, 200), spotLightDir, 1f, 2f, 2f, 500f, Color.LightBlue));
            spotLightHandles.Add(new SpotLight(new Vector2(200, 200), spotLightDir, 1f, 2f, 2f, 500f, Color.Red));
            LightRenderer.pointLights.Add(pointLightHandles[0]);
            LightRenderer.spotLights.Add(spotLightHandles[0]);
            LightRenderer.spotLights.Add(spotLightHandles[1]);


            LightRenderer.LoadContent(ScreenManager.Game.Content);

            LightingManager.DefinePlayerLight(600, 600, Color.White, true);
            LightingManager.AddAmbientLight(Color.White * 0.5f);

            myAffectLightDir = false;
            myCurrLight = 0;
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
                myAffectLightDir = !myAffectLightDir;

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


            if (KeyMouseReader.KeyClick(Keys.D1))
                myCurrLight = 0;
            if (KeyMouseReader.KeyClick(Keys.D2))
                myCurrLight = 1;

            //spotLightHandles[myCurrLight].Position.X = lightPos.X;
            //spotLightHandles[myCurrLight].Position.Y = lightPos.Y;
            //spotLightHandles[myCurrLight].direction = spotLightDir;
            pointLightHandles[0].Position = lightPos;
            Console.WriteLine("FPS: " + (1000 / elapsedTime));
        }

        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    //Draw all the Lights we have added to LightingManager.LightTarget
        //    LightingManager.Draw();

        //    //Change RenderTarget to the MainTarget-Render2D, here we draw all the tiles
        //    ScreenManager.Game.GraphicsDevice.SetRenderTarget(Game1.MainTarget);
        //    ScreenManager.Game.GraphicsDevice.Clear(Color.CornflowerBlue);

        //    //And draw all tiles, the player and enemies
        //    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Camera.Get_Transformation());
        //    mapManager.Draw(spriteBatch);

        //    EnemyManager.Draw(spriteBatch);

        //    if (Player != null)
        //        Player.Draw(spriteBatch);
        //    spriteBatch.End();
            
        //    //Now we need to change rendertarget to the backbuffer, draw the MainTarget with the shader enabled
        //    ScreenManager.Game.GraphicsDevice.SetRenderTarget(null);
        //    ScreenManager.Game.GraphicsDevice.Clear(Color.CornflowerBlue);

        //    TextureManager.PixelShader.Parameters["lightMask"].SetValue(LightingManager.LightingTarget);
        //    //TextureManager.PixelShader.Parameters["lightMask"].SetValue(Game1.ShaderTarget);

        //    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, TextureManager.PixelShader);
        //    spriteBatch.Draw(Game1.MainTarget, Vector2.Zero, Color.White);
        //    spriteBatch.End();

        //}

        public void Draw(SpriteBatch spriteBatch)
        {
            LightRenderer.BeginDrawBackground();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            mapManager.DrawBackground(spriteBatch);
            mapManager.DrawForeground(spriteBatch);

            spriteBatch.End();

            LightRenderer.EndDrawBackground();


            LightRenderer.BeginDrawShadowCasters();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            mapManager.DrawMiddle(spriteBatch);

            spriteBatch.End();

            LightRenderer.EndDrawShadowCasters();

            LightRenderer.DrawLitScene();

            //And draw all tiles, the player and enemies
            
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
            ScreenManager.Game.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Camera.Get_Transformation());
            spriteBatch.Draw(finalRenderTarget, finalDrawPos, finalSource, Color.White);
            if (Player != null)
                Player.Draw(spriteBatch);

            spriteBatch.Draw(TextureManager.FilledSquare, new Rectangle((int)lightPos.X - 10, (int)lightPos.Y - 10, 20, 20), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            EnemyManager.Draw(spriteBatch);

            
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
