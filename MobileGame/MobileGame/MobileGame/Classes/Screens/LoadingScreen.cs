using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GUI_System.GameStateManagement;

namespace MobileGame.Screens
{
    class LoadingScreen : GameScreen
    {
        bool loadingIsSlow;
        bool otherScreensAreGone;

        GameScreen[] screensToLoad;

        //The constructor is private because when we want to use a loadingscreen we should use the static Load function
        private LoadingScreen(ScreenManager screenManager, bool loadingIsSlow, GameScreen[] screensToLoad)
        {
            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }

        //This function is to be used when we want to use a loadingscreen
        public static void Load(ScreenManager screenManager, bool loadingIsSlow, params GameScreen[] screensToLoad)
        {
            //Tell all screens to transition off
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            //Create the loadingscreen
            LoadingScreen loadingScreen = new LoadingScreen(screenManager, loadingIsSlow, screensToLoad);

            //and finally activate the loadingscreen
            screenManager.AddScreen(loadingScreen);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (otherScreensAreGone)
            {
                ScreenManager.RemoveScreen(this);

                foreach (GameScreen screen in screensToLoad)
                {
                    if (screen != null)
                        ScreenManager.AddScreen(screen);
                }

                ScreenManager.Game.ResetElapsedTime();
            }   
        }

        public override void Draw(GameTime gameTime)
        {
            // If we are the only active screen that means that all other screens has transitioned off.
            // We check this in the draw function to make sure it looks good: for it to look good we must have drawn a frame
            // without the other screens before we load the new screen
            if ((CurrentScreenState == ScreenState.Active) && ScreenManager.GetScreens().Length == 1)
            {
                otherScreensAreGone = true;
            }


            if (loadingIsSlow)
            {
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                SpriteFont font = ScreenManager.Font;

                const string message = "Loading...";

                Viewport viewPort = ScreenManager.Game.GraphicsDevice.Viewport;
                Vector2 viewPortSize = new Vector2(viewPort.Width, viewPort.Height);
                Vector2 textSize = font.MeasureString(message);
                Vector2 textPosition = (viewPortSize - textSize) / 2;

                Color color = Color.White * TransitionAlpha;

                spriteBatch.Begin();
                spriteBatch.DrawString(font, message, textPosition, color);
                spriteBatch.End();
            }
        }
    }
}
