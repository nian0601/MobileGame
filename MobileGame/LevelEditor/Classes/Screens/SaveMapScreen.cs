using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using GUI_System.GameStateManagement;
using GUI_System.GUIObjects;

namespace LevelEditor.Screens
{
    class SaveMapScreen : MenuScreen
    {
        private MenuButton SaveButton, CancelButton;

        public SaveMapScreen() : base("")
        {
            SaveButton = new MenuButton(new SaveButtonStyle(ScreenManager.Game.Content));
            CancelButton = new MenuButton(new CancelButtonStyle(ScreenManager.Game.Content));

            MenuEntries.Add(SaveButton);
            MenuEntries.Add(CancelButton);

            IsPopup = true;
        }

        public override void HandleInput(GameTime gameTime)
        {
            if (CancelButton.LeftClick())
                ExitScreen();   

            base.HandleInput(gameTime);
        }

        protected override void UpdateMenyEntryLocations()
        {
            //This makes the menu slide into place. Should experiment with this function to see what other cool effects you could achive
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            //We start at Y = 250, which then increments after each menuEntry
            //The X-value is generated per entry
            Vector2 position = new Vector2(0, 250);

            int SaveXPos = ScreenManager.Game.GraphicsDevice.Viewport.Width / 2 - SaveButton.GetWidth(this) / 2;
            Vector2 SavePosition = new Vector2(SaveXPos, 250);

            int CancelXPos = SaveXPos + CancelButton.GetWidth(this) + 10;
            Vector2 CancelPos = new Vector2(CancelXPos, 250);

            if (CurrentScreenState == ScreenState.TransitionOn)
            {
                SavePosition.Y -= transitionOffset * 256;
                CancelPos.Y -= transitionOffset * 256;
            }
            else
            {
                SavePosition.Y += transitionOffset * 512;
                CancelPos.Y += transitionOffset * 512;
            }

            SaveButton.Position = SavePosition;
            CancelButton.Position = CancelPos;
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
                MenuEntry menuEntry = MenuEntries[i];

                bool isSelected = false;

                menuEntry.Draw(this, isSelected, gameTime);
            }

            spriteBatch.End();
        }
    }
}
