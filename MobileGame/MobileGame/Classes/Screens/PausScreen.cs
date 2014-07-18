using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using GUI_System.GameStateManagement;
using GUI_System.GUIObjects;

namespace MobileGame.Screens
{
    class PausScreen : MenuScreen
    {
        private MenuButton returnToMenuButton, restartLevelButton, exitPausButton;
        private Texture2D TitleTexture;

        public PausScreen() : base("")
        {
            returnToMenuButton = new MenuButton(new MenuButtonStyle(ScreenManager.Game.Content));
            restartLevelButton = new MenuButton(new RestartButtonStyle(ScreenManager.Game.Content));
            exitPausButton = new MenuButton(new CrossButtonStyle(ScreenManager.Game.Content));

            MenuEntries.Add(returnToMenuButton);
            MenuEntries.Add(restartLevelButton);
            MenuEntries.Add(exitPausButton);

            TitleTexture = ScreenManager.Game.Content.Load<Texture2D>("GUI Textures/PausScreen/PausScreenTitle");

            IsPopup = true;
        }

        public override void HandleInput(GameTime gameTime)
        {
            if (returnToMenuButton.LeftClick())
                LoadingScreen.Load(ScreenManager, true, new BackgroundScreen(), new MainMenuScreen());

            else if (exitPausButton.LeftClick() || KeyMouseReader.KeyClick(Microsoft.Xna.Framework.Input.Keys.P))
                ExitScreen();

            else if (restartLevelButton.LeftClick())
            {
                GameManager.RestarLevel();
                ExitScreen();
            }
                

            base.HandleInput(gameTime);
        }

        protected override void UpdateMenyEntryLocations()
        {
            //This makes the menu slide into place. Should experiment with this function to see what other cool effects you could achive
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Viewport viewPort = ScreenManager.Game.GraphicsDevice.Viewport;

            Vector2 firstButtonPos = new Vector2(viewPort.Width / 2 - (returnToMenuButton.GetWidth(this) / 2) - 10, 250);
            returnToMenuButton.Position = firstButtonPos;
            Vector2 restartButtonPos = new Vector2(returnToMenuButton.Position.X + restartLevelButton.GetWidth(this) + 10, returnToMenuButton.Position.Y);
            restartLevelButton.Position = restartButtonPos;

            Vector2 exitButtonPos = new Vector2(viewPort.Width - exitPausButton.GetWidth(this)/2 - 10, exitPausButton.GetHeight(this)/2 + 10);
            exitPausButton.Position = exitButtonPos;

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                GUIObject menuEntry = MenuEntries[i];
                Vector2 position = menuEntry.Position;

                if (CurrentScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                //set the entry's position
                menuEntry.Position = position;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 4);

            UpdateMenyEntryLocations();

            GraphicsDevice graphics = ScreenManager.Game.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                GUIObject menuEntry = MenuEntries[i];

                menuEntry.Draw(this, gameTime);
            }

            //This is used to make the menuTitle slide into place, just like the entries
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 150);
            Vector2 titleOrigin = new Vector2(TitleTexture.Width / 2, TitleTexture.Height / 2);
            Color titleColor = Color.White;

            titlePosition.Y -= transitionOffset * 200;

            spriteBatch.Draw(TitleTexture, titlePosition, null, titleColor, 0, titleOrigin, 1f, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
