using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using GUI_System.GameStateManagement;
using GUI_System.GUIObjects;

using MobileGame.FileManagement;

namespace MobileGame.Screens
{
    class WinScreen : MenuScreen
    {
        private MenuButton MenuButton, LevelSelectButton, RestartButton, ContinueButton;
        private Texture2D TitleTexture, ScoreTexture;
        private GameManager GameManager;

        public WinScreen(GameManager gameManager): base("")
        {
            GameManager = gameManager;

            MenuButton = new MenuButton(new MenuButtonStyle(ScreenManager.Game.Content));
            LevelSelectButton = new MenuButton(new LevelSelectStyle(ScreenManager.Game.Content));
            RestartButton = new MenuButton(new RestartButtonStyle(ScreenManager.Game.Content));
            ContinueButton = new MenuButton(new PlayButtonStyle(ScreenManager.Game.Content));

            MenuEntries.Add(MenuButton);
            MenuEntries.Add(LevelSelectButton);
            MenuEntries.Add(RestartButton);
            MenuEntries.Add(ContinueButton);

            TitleTexture = ScreenManager.Game.Content.Load<Texture2D>("GUI Textures/WinScreen/WinTitle");
            ScoreTexture = ScreenManager.Game.Content.Load<Texture2D>("GUI Textures/WinScreen/ScoreText");

            IsPopup = true;
        }

        public override void Activate()
        {
            FileLoader.UpdateGameData();
            base.Activate();
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
            else if (ContinueButton.LeftClick())
            {
                //If we have comepleted all the maps we should come to a "No more maps available atm"-screen
                if (FileLoader.LoadedGameData.AllMapsDone)
                    LoadingScreen.Load(ScreenManager, true, new BackgroundScreen(), new MainMenuScreen());
                //If we havnt completed all maps we should instead load the next one
                //Please note that the LoadedGameData variable gets updated in the Activate method
                //So when we call LoadLevel on LoadedGameData.CurrentMap we are actually loading the next map,
                //because the variable is updated to count the next map as the current map allready
                //And when we have loaded the new map we call LoadingScreen to make a fancy transition into the new map
                else
                {
                    FileLoader.LoadLevel(FileLoader.LoadedGameData.CurrentMap);
                    LoadingScreen.Load(ScreenManager, true, new GamePlayScreen());
                }
            }

            base.HandleInput(gameTime);
        }

        protected override void UpdateMenyEntryLocations()
        {
            //This makes the menu slide into place. Should experiment with this function to see what other cool effects you could achive
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            //We start at Y = 250, which then increments after each menuEntry
            //The X-value is generated per entry
            Vector2 position = new Vector2(0, 350);

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                GUIObject menuEntry = MenuEntries[i];

                //each entry gets centered horizontally
                position.X = ScreenManager.Game.GraphicsDevice.Viewport.Width / 2 - ((menuEntry.GetWidth(this) / 2) * 3) + ((menuEntry.GetWidth(this) * i));

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
            Vector2 scoreOrigin = new Vector2(ScoreTexture.Width, ScoreTexture.Height / 2);
            Color scoreColor = Color.White;

            titlePosition.Y -= transitionOffset * 200;
            scorePosition.Y -= transitionOffset * 200;

            spriteBatch.Draw(TitleTexture, titlePosition, null, titleColor, 0, titleOrigin, 1f, SpriteEffects.None, 0);
            spriteBatch.Draw(ScoreTexture, scorePosition, null, scoreColor, 0, scoreOrigin, 1f, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
