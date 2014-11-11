using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using GUI_System.GameStateManagement;

using MobileGame.FileManagement;
using MobileGame.CameraManagement;
using MobileGame.Managers;

namespace MobileGame.Screens
{
    class GamePlayScreen : GameScreen
    {
        ContentManager content;
        GameManager gameManager;
        float pausAlpha;

        public GamePlayScreen(): base()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void Activate()
        {
            //Only create a new content if we dont have one allready
            //Good to have this check as we will Activate and Deactivate GamePlayScreen's a fair bit
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            if(gameManager == null)
                gameManager = new GameManager();

            gameManager.Initialize();
            gameManager.mapManager.graphicsDevice = ScreenManager.Game.GraphicsDevice;
            GameManager.RestarLevel();
        }

        public override void Unload()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (coveredByOtherScreen)
                pausAlpha = Math.Min(pausAlpha + 1f / 32, 1);
            else
                pausAlpha = Math.Min(pausAlpha - 1f / 32, 0);
        }

        public override void HandleInput(GameTime gameTime)
        {
            gameManager.Update(gameTime.ElapsedGameTime.Milliseconds);
            Camera.Update(gameTime);

            if (KeyMouseReader.KeyClick(Keys.P))
                ScreenManager.AddScreen(new PausScreen());

            if (gameManager.GameWon)
                ScreenManager.AddScreen(new WinScreen());
            
            if (gameManager.GameLost)
                ScreenManager.AddScreen(new DefeatedScreen(gameManager));   
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            

            gameManager.Draw(spriteBatch);

            if (TransitionPosition > 0 || pausAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pausAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
