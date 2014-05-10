using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GUI_System.GameStateManagement;
using GUI_System.GUIObjects;

namespace MobileGame.Screens
{
    class LevelSelectScreen : MenuScreen
    {
        MenuButton returnButton;
        Texture2D TitleTexture, levelTexture;

        public LevelSelectScreen() : base("")
        {
            returnButton = new MenuButton(new MenuButtonStyle(ScreenManager.Game.Content));

            MenuEntries.Add(returnButton);

            TitleTexture = ScreenManager.Game.Content.Load<Texture2D>("GUI Textures/LevelSelection/LevelSelectionTitle");
            levelTexture = ScreenManager.Game.Content.Load<Texture2D>("GUI Textures/MainMenu/PlayButton");
        }

        public override void HandleInput(GameTime gameTime)
        {
            if (returnButton.LeftClick())
                ExitScreen();

            base.HandleInput(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = ScreenManager.Game.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            int xCounter = 0;
            int yCounter = 0;
            Vector2 startPos = new Vector2(215, 150);
            Vector2 drawPos;

            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            spriteBatch.Begin();

            for (int i = 0; i < 12; i++)
            {
                drawPos.X = startPos.X + (levelTexture.Width + 10) * xCounter;
                drawPos.Y = startPos.Y + (levelTexture.Height + 10) * yCounter;

                if (CurrentScreenState == ScreenState.TransitionOn)
                    drawPos.X += transitionOffset * 256;
                else
                    drawPos.X -= transitionOffset * 512;

                spriteBatch.Draw(levelTexture, drawPos, Color.White * TransitionAlpha);

                xCounter++;
                if (xCounter == 4)
                {
                    xCounter = 0;
                    yCounter++;
                }
            }

            Vector2 tempPos = new Vector2(returnButton.GetWidth(this) + 10, graphics.Viewport.Height - returnButton.GetWidth(this) - 10);
            if (CurrentScreenState == ScreenState.TransitionOn)
                tempPos.X += transitionOffset * 256;
            else
                tempPos.X -= transitionOffset * 512;

            returnButton.Position = tempPos;
            returnButton.Draw(this, false, gameTime);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 100);
            Vector2 titleOrigin = new Vector2(TitleTexture.Width / 2, TitleTexture.Height / 2);
            Color titleColor = Color.White;

            titlePosition.Y -= transitionOffset * 200;

            spriteBatch.Draw(TitleTexture, titlePosition, null, titleColor, 0, titleOrigin, 1f, SpriteEffects.None, 0);


            spriteBatch.End();
        }
    }
}
