using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GUI_System.GameStateManagement;
using GUI_System.GUIObjects;

namespace MobileGame.Screens
{
    public class MainMenuScreen : MenuScreen
    {
        private MenuButton playButton, levelSelectButton;
        Texture2D TitleTexture;

        public MainMenuScreen() : base("Main menu")
        {
            playButton = new MenuButton(new PlayButtonStyle(ScreenManager.Game.Content));
            levelSelectButton = new MenuButton(new LevelSelectStyle(ScreenManager.Game.Content));

            MenuEntries.Add(playButton);
            MenuEntries.Add(levelSelectButton);

            TitleTexture = ScreenManager.Game.Content.Load<Texture2D>("GUI Textures/MainMenu/MainMenuTitle");
        }

        public override void HandleInput(GameTime gameTime)
        {
            if (levelSelectButton.LeftClick())
                ScreenManager.AddScreen(new LevelSelectScreen());

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
                MenuEntry menuEntry = MenuEntries[i];

                //each entry gets centered horizontally
                position.X = ScreenManager.Game.GraphicsDevice.Viewport.Width / 2 - (menuEntry.GetWidth(this) / 2) + ((menuEntry.GetWidth(this) * i));

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
            UpdateMenyEntryLocations();

            GraphicsDevice graphics = ScreenManager.Game.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntry menuEntry = MenuEntries[i];

                bool isSelected = false;

                menuEntry.Draw(this, isSelected, gameTime);
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

        protected override void OnCancel()
        {
            ScreenManager.Game.Exit();
            base.OnCancel();
        }
    }
}