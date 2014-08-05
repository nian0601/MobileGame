using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GUI_System.GameStateManagement;
using GUI_System.GUIObjects;

using LevelEditor.Managers;

namespace LevelEditor.Screens
{
    class EditorScreen : MenuScreen
    {
        private MenuButton LoadButton, SaveButton, ExitButton;

        private List<MenuButton> TileButtons;
        private List<MenuButton> TileButtonsDisplayList;

        private MapManager mapManager;

        public EditorScreen(): base("Editor Screen")
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            LoadButton = new MenuButton(new LoadMapButtonStyle(ScreenManager.Game.Content));
            SaveButton = new MenuButton(new SaveMapButtonStyle(ScreenManager.Game.Content));
            ExitButton = new MenuButton(new ExitButtonStyle(ScreenManager.Game.Content));

            MenuEntries.Add(LoadButton);
            MenuEntries.Add(SaveButton);
            MenuEntries.Add(ExitButton);

            TileButtons = new List<MenuButton>();
            TileButtons.Add(new MenuButton(new Tile0(ScreenManager.Game.Content)));
            TileButtons.Add(new MenuButton(new Tile1(ScreenManager.Game.Content)));
            TileButtons.Add(new MenuButton(new Tile2(ScreenManager.Game.Content)));
            TileButtons.Add(new MenuButton(new Tile3(ScreenManager.Game.Content)));
            TileButtons.Add(new MenuButton(new Tile4(ScreenManager.Game.Content)));
            TileButtons.Add(new MenuButton(new Tile5(ScreenManager.Game.Content)));
            TileButtons.Add(new MenuButton(new Tile6(ScreenManager.Game.Content)));
            TileButtons.Add(new MenuButton(new Tile7(ScreenManager.Game.Content)));
            TileButtons.Add(new MenuButton(new Tile8(ScreenManager.Game.Content)));
            TileButtons.Add(new MenuButton(new Tile9(ScreenManager.Game.Content)));
            TileButtons.Add(new MenuButton(new Tile10(ScreenManager.Game.Content)));
            TileButtons.Add(new MenuButton(new Tile11(ScreenManager.Game.Content)));
            TileButtons.Add(new MenuButton(new Tile12(ScreenManager.Game.Content)));
            TileButtons.Add(new MenuButton(new Tile13(ScreenManager.Game.Content)));
            TileButtons.Add(new MenuButton(new Tile14(ScreenManager.Game.Content)));
            TileButtons.Add(new MenuButton(new Tile15(ScreenManager.Game.Content)));

            TileButtonsDisplayList = new List<MenuButton>();
            TileButtonsDisplayList.Add(TileButtons[6]);
            TileButtonsDisplayList.Add(TileButtons[14]);
            TileButtonsDisplayList.Add(TileButtons[12]);
            TileButtonsDisplayList.Add(TileButtons[4]);
            TileButtonsDisplayList.Add(TileButtons[7]);
            TileButtonsDisplayList.Add(TileButtons[15]);
            TileButtonsDisplayList.Add(TileButtons[13]);
            TileButtonsDisplayList.Add(TileButtons[5]);
            TileButtonsDisplayList.Add(TileButtons[3]);
            TileButtonsDisplayList.Add(TileButtons[11]);
            TileButtonsDisplayList.Add(TileButtons[9]);
            TileButtonsDisplayList.Add(TileButtons[1]);
            TileButtonsDisplayList.Add(TileButtons[2]);
            TileButtonsDisplayList.Add(TileButtons[10]);
            TileButtonsDisplayList.Add(TileButtons[8]);
            TileButtonsDisplayList.Add(TileButtons[0]);
        }

        public override void Activate()
        {
            FileManagement.FileLoader.Initialize();

            if(mapManager == null)
                mapManager = new MapManager(ScreenManager.Game.Content, ScreenManager.SpriteBatch);

            mapManager.Initialize();
            MapManager.Offset = new Vector2(218, 22);
        }

        public override void HandleInput(GameTime gameTime)
        {
            mapManager.Update();

            if (SaveButton.LeftClick())
                ScreenManager.AddScreen(new SaveMapScreen(mapManager));

            for (int i = 0; i < TileButtons.Count; i++)
            {
                TileButtons[i].Update(this, gameTime);

                if (TileButtons[i].LeftClick())
                    MapManager.SetCursorTexture(i);
            }

            base.HandleInput(gameTime);
        }

        protected override void UpdateMenyEntryLocations()
        {
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Vector2 position = new Vector2(13 + MenuEntries[0].GetWidth(this)/2, 0);
            int spaceBetweenEntries = 10;
            int yStartPos = 480;

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                GUIObject menuEntry = MenuEntries[i];

                position.Y = yStartPos + (menuEntry.GetHeight(this) + spaceBetweenEntries) * i;

                if (CurrentScreenState == ScreenState.TransitionOn)
                    position.Y -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                menuEntry.Position = position;
            }

            int startX = 30;
            int startY = 50;
            int padding = 2;
            int counter = 0;

            position.X = startX;
            position.Y = startY;

            for (int i = 0; i < TileButtonsDisplayList.Count; i++)
            {
                GUIObject button = TileButtonsDisplayList[i];

                position.X = startX + (button.GetWidth(this) + padding) * counter;

                button.Position = position;

                counter++;
                if (counter >= 4)
                {
                    position.Y += button.GetHeight(this) + padding;
                    counter = 0;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            UpdateMenyEntryLocations();

            GraphicsDevice graphics = ScreenManager.Game.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;


            mapManager.Draw();


            spriteBatch.Begin();

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                GUIObject menuEntry = MenuEntries[i];
                menuEntry.Draw(this, gameTime);
            }

            foreach (GUIObject Object in TileButtonsDisplayList)
            {
                Object.Draw(this, gameTime);
            }

            spriteBatch.End();

            
        }
    }
}
