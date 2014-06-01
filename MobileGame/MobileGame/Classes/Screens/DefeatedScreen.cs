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
    class DefeatedScreen : MenuScreen
    {
        private MenuButton MenuButton, LevelSelectButton, RestartButton;
        private Texture2D TitleTexture;
        private GameManager GameManager;

        public DefeatedScreen(GameManager gameManager): base("")
        {
            GameManager = gameManager;

            MenuButton = new MenuButton(new MenuButtonStyle(ScreenManager.Game.Content));
            LevelSelectButton = new MenuButton(new LevelSelectStyle(ScreenManager.Game.Content));
            RestartButton = new MenuButton(new RestartButtonStyle(ScreenManager.Game.Content));

            MenuEntries.Add(MenuButton);
            MenuEntries.Add(LevelSelectButton);
            MenuEntries.Add(RestartButton);

            TitleTexture = ScreenManager.Game.Content.Load<Texture2D>("GUI Textures/DefeatedScreen/DefeatedTitle");

            IsPopup = true;
        }

        public override void HandleInput(GameTime gameTime)
        {
            if (MenuButton.LeftClick())
                LoadingScreen.Load(ScreenManager, true, new BackgroundScreen(), new MainMenuScreen());
            else if (LevelSelectButton.LeftClick())
                LoadingScreen.Load(ScreenManager, true, new BackgroundScreen(), new MainMenuScreen(), new LevelSelectScreen());
            else if (RestartButton.LeftClick())
            {
                ExitScreen();
                GameManager.RestarLevel();
            }
                

            base.HandleInput(gameTime);
        }

        protected override void UpdateMenyEntryLocations()
        {
            //This makes the menu slide into place. Should experiment with this function to see what other cool effects you could achive
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            //We start at Y = 250, which then increments after each menuEntry
            //The X-value is generated per entry
            Vector2 position = new Vector2(0, 250);

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                GUIObject menuEntry = MenuEntries[i];

                //each entry gets centered horizontally
                position.X = ScreenManager.Game.GraphicsDevice.Viewport.Width / 2 - ((menuEntry.GetWidth(this) / 2) * 2) + ((menuEntry.GetWidth(this) * i));

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

            Vector2 scorePosition = new Vector2(graphics.Viewport.Width / 2, 225);
            Color scoreColor = Color.White;

            titlePosition.Y -= transitionOffset * 200;
            scorePosition.Y -= transitionOffset * 200;

            spriteBatch.Draw(TitleTexture, titlePosition, null, titleColor, 0, titleOrigin, 1f, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
