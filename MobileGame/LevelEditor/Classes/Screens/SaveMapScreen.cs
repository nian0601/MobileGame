using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using GUI_System.GameStateManagement;
using GUI_System.GUIObjects;

using LevelEditor.Managers;

namespace LevelEditor.Screens
{
    class SaveMapScreen : MenuScreen
    {
        private MenuButton SaveButton, CancelButton;
        private TextInputField NameInput;
        private MapManager mapManager;

        public SaveMapScreen(MapManager MapManager) : base("")
        {
            SaveButton = new MenuButton(new SaveButtonStyle(ScreenManager.Game.Content));
            CancelButton = new MenuButton(new CancelButtonStyle(ScreenManager.Game.Content));
            NameInput = new TextInputField(new MapNameInputStyle(ScreenManager.Game.Content));
            
            MenuEntries.Add(SaveButton);
            MenuEntries.Add(CancelButton);
            MenuEntries.Add(NameInput);

            IsPopup = true;

            mapManager = MapManager;
        }

        public override void HandleInput(GameTime gameTime)
        {
            if (CancelButton.LeftClick())
            {
                NameInput.Clear();
                ExitScreen();
            } 

            if (SaveButton.LeftClick() && NameInput.Text.Length > 0)
            {
                FileManagement.FileLoader.SaveLevel(MapManager.TileArray, MapManager.mapHeight, MapManager.mapWidth, NameInput.Text);
            }
                

            NameInput.Update(this, gameTime);

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
            Vector2 SavePosition = new Vector2(SaveXPos, 300);

            int CancelXPos = SaveXPos + CancelButton.GetWidth(this) + 10;
            Vector2 CancelPos = new Vector2(CancelXPos, 300);

            int InputXPos = ScreenManager.Game.GraphicsDevice.Viewport.Width / 2 - NameInput.GetWidth(this) / 2;
            Vector2 InputPos = new Vector2(InputXPos, 200);
            

            if (CurrentScreenState == ScreenState.TransitionOn)
            {
                SavePosition.Y -= transitionOffset * 256;
                CancelPos.Y -= transitionOffset * 256;
                InputPos.Y -= transitionOffset * 256;
            }
            else
            {
                SavePosition.Y += transitionOffset * 512;
                CancelPos.Y += transitionOffset * 512;
                InputPos.Y += transitionOffset * 512;
            }

            SaveButton.Position = SavePosition;
            CancelButton.Position = CancelPos;
            NameInput.Position = InputPos;
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


            

            spriteBatch.End();
        }
    }
}
