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
using MobileGame.Managers;

namespace MobileGame.LevelEditor
{
    class LoadMapScreen : MenuScreen
    {
        private MenuButton LoadButton, DeleteButton, CancelButton;
        private List SavedMapList;

        private Texture2D TitleTexture;
        private Vector2 TitlePos;

        private EditorMapManager mapManager;

        public LoadMapScreen(EditorMapManager MapManager)
            : base("")
        {
            LoadButton = new MenuButton(new LoadButtonStyle(ScreenManager.Game.Content));
            DeleteButton = new MenuButton(new DeleteButtonStyle(ScreenManager.Game.Content));
            CancelButton = new MenuButton(new CancelButtonStyle(ScreenManager.Game.Content));
            SavedMapList = new List(new LoadMapListStyle(ScreenManager.Game.Content));

            TitleTexture = ScreenManager.Game.Content.Load<Texture2D>("Editor/LoadMap/LoadLable");

            for (int i = 0; i < FileLoader.LoadedGameData.MapList.Count; i++)
            {
                SavedMapList.AddItem(FileLoader.LoadedGameData.MapList[i], new LoadMapListItemStyle(ScreenManager.Game.Content));
            }

            MenuEntries.Add(LoadButton);
            MenuEntries.Add(DeleteButton);
            MenuEntries.Add(CancelButton);
            MenuEntries.Add(SavedMapList);

            IsPopup = true;

            mapManager = MapManager;
        }

        public override void HandleInput(GameTime gameTime)
        {
            if (LoadButton.LeftClick())
            {
                if (SavedMapList.SelectedItem != null)
                {
                    ConfirmationPopUp confirmLoadBox = new ConfirmationPopUp();
                    confirmLoadBox.Accepted += ConfirmLoadBoxAccept;
                    ScreenManager.AddScreen(confirmLoadBox);
                }
            }
            else if (DeleteButton.LeftClick())
            {
                if (SavedMapList.SelectedItem != null)
                {
                    ConfirmationPopUp confirmDeleteBox = new ConfirmationPopUp();
                    confirmDeleteBox.Accepted += ConfirmDeletionBoxAccept;
                    ScreenManager.AddScreen(confirmDeleteBox);
                }
            }
            if (CancelButton.LeftClick())
            {
                ExitScreen();
            }

            SavedMapList.Update(this, gameTime);

            base.HandleInput(gameTime);
        }

        protected override void UpdateMenyEntryLocations()
        {
            //This makes the menu slide into place. Should experiment with this function to see what other cool effects you could achive
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            //We start at Y = 250, which then increments after each menuEntry
            //The X-value is generated per entry
            Vector2 position = new Vector2(0, 250);

            int LoadXPos = ScreenManager.Game.GraphicsDevice.Viewport.Width / 2 - LoadButton.GetWidth(this);
            Vector2 LoadPos = new Vector2(LoadXPos, 400);

            int DeleteXPos = LoadXPos + DeleteButton.GetWidth(this) + 5;
            Vector2 DeletePos = new Vector2(DeleteXPos, 400);

            int CancelXPos = (int)LoadPos.X + 10;
            int CancelYPos = (int)LoadPos.Y + CancelButton.GetHeight(this) + 5;
            Vector2 CancelPos = new Vector2(CancelXPos, CancelYPos);

            int ListXPos = (int)LoadPos.X - LoadButton.GetWidth(this) / 2;
            int ListYPos = (int)LoadPos.Y - LoadButton.GetHeight(this) / 2 - SavedMapList.GetHeight(this) - 5;
            Vector2 ListPos = new Vector2(ListXPos, ListYPos);

            int LableXPos = ListXPos;
            int LableYPos = ListYPos - TitleTexture.Height - 5;
            TitlePos = new Vector2(LableXPos, LableYPos);

            if (CurrentScreenState == ScreenState.TransitionOn)
            {
                LoadPos.Y -= transitionOffset * 256;
                DeletePos.Y -= transitionOffset * 256;
                CancelPos.Y -= transitionOffset * 256;
                ListPos.Y -= transitionOffset * 256;
                TitlePos.Y -= transitionOffset * 256;
            }
            else
            {
                LoadPos.Y += transitionOffset * 512;
                DeletePos.Y += transitionOffset * 512;
                CancelPos.Y += transitionOffset * 512;
                ListPos.Y += transitionOffset * 512;
                TitlePos.Y += transitionOffset * 512;
            }

            LoadButton.Position = LoadPos;
            DeleteButton.Position = DeletePos;
            CancelButton.Position = CancelPos;
            SavedMapList.Position = ListPos;
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

            spriteBatch.Draw(TitleTexture, TitlePos, Color.White);

            spriteBatch.End();
        }

        private void ConfirmDeletionBoxAccept(Object sender, EventArgs e)
        {
            FileLoader.DeleteLevel(SavedMapList.SelectedItem.Text);

            SavedMapList.Clear();

            for (int i = 0; i < FileLoader.LoadedGameData.MapList.Count; i++)
            {
                SavedMapList.AddItem(FileLoader.LoadedGameData.MapList[i], new LoadMapListItemStyle(ScreenManager.Game.Content));
            }
        }

        private void ConfirmLoadBoxAccept(Object sender, EventArgs e)
        {
            FileLoader.LoadLevelToEditor(SavedMapList.SelectedItem.Text);
            ExitScreen();
        }
    }
}
