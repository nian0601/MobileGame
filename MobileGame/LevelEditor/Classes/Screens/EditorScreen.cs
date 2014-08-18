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
        private MenuButton LayerUpButton, LayerDownButton;

        private List<MenuButton> TileButtons;

        private MapManager mapManager;

        public EditorScreen(): base("Editor Screen")
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            ExitButton = new MenuButton(new ExitButtonStyle(ScreenManager.Game.Content));
            SaveButton = new MenuButton(new SaveMapButtonStyle(ScreenManager.Game.Content));
            LoadButton = new MenuButton(new LoadMapButtonStyle(ScreenManager.Game.Content));

            LayerDownButton = new MenuButton(new LayerDownButtonStyle(ScreenManager.Game.Content));
            LayerUpButton = new MenuButton(new LayerUpButtonStyle(ScreenManager.Game.Content));

            MenuEntries.Add(ExitButton);
            MenuEntries.Add(SaveButton);
            MenuEntries.Add(LoadButton);

            #region TileButtons
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
            #endregion
        }

        public override void Activate()
        {
            FileManagement.FileLoader.Initialize();
            ToolPositionsManager.LoadData();

            if(mapManager == null)
                mapManager = new MapManager(ScreenManager.Game.Content, ScreenManager.SpriteBatch);

            mapManager.Initialize();
            MapManager.Offset = new Vector2(218, 22);

            for (int i = 0; i < TileButtons.Count; i++)
            {
                GUIObject button = TileButtons[i];
                button.Position = ToolPositionsManager.GetPosition("PlatformButton" + i);
            }
        }

        public override void HandleInput(GameTime gameTime)
        {
            mapManager.Update();

            if (SaveButton.LeftClick())
            {
                ConfirmationPopUp ProccedToSaveScreenBox = new ConfirmationPopUp();

                if (!MapManager.PlayerPlaced)
                    ProccedToSaveScreenBox = new ConfirmationPopUp("You have not placed the Player");
                else if (!MapManager.GoalPlaced)
                    ProccedToSaveScreenBox = new ConfirmationPopUp("You have not place the Goal");
                else if (!MapManager.HasSufficentCollisionFlags)
                    ProccedToSaveScreenBox = new ConfirmationPopUp("There is no or very few collisionflags");
                else
                {
                    ScreenManager.AddScreen(new SaveMapScreen(mapManager));
                    return;
                }

                ProccedToSaveScreenBox.Accepted = ProccedToSaveScreenAccepted;
                ScreenManager.AddScreen(ProccedToSaveScreenBox);
            }


            else if (LoadButton.LeftClick())
                ScreenManager.AddScreen(new LoadMapScreen(mapManager));

            else if (ExitButton.LeftClick())
                ScreenManager.Game.Exit();

            else if (LayerUpButton.LeftClick())
                Game1.ChangeEditMode(1);

            else if (LayerDownButton.LeftClick())
                Game1.ChangeEditMode(-1);

            for (int i = 0; i < TileButtons.Count; i++)
            {
                TileButtons[i].Update(this, gameTime);

                if (TileButtons[i].LeftClick())
                    MapManager.SetCursorTexture(i);
            }

            base.HandleInput(gameTime);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            LayerUpButton.Update(this, gameTime);
            LayerDownButton.Update(this, gameTime);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        protected override void UpdateMenyEntryLocations()
        {
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            #region Exit/Save/Load Buttons

            int spaceBetweenEntries = 10;
            int yStartPos = ScreenManager.Game.GraphicsDevice.Viewport.Height - MenuEntries[0].GetHeight(this);
            int xStartPos = MenuEntries[0].GetWidth(this) / 2 + 15;

            Vector2 position = new Vector2();
            position.Y = yStartPos;

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                GUIObject menuEntry = MenuEntries[i];

                position.X = xStartPos + (menuEntry.GetWidth(this) + spaceBetweenEntries) * i;

                if (CurrentScreenState == ScreenState.TransitionOn)
                    position.Y -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                menuEntry.Position = position;
            }

            #endregion

            xStartPos = LayerUpButton.GetWidth(this) / 2 + 15;
            yStartPos = yStartPos - LayerUpButton.GetHeight(this) - 10;

            position = new Vector2(xStartPos, yStartPos);
            LayerUpButton.Position = position;

            position.X += LayerDownButton.GetWidth(this) + spaceBetweenEntries;
            LayerDownButton.Position = position;

            if (KeyMouseReader.KeyClick(Microsoft.Xna.Framework.Input.Keys.M))
            {
                for (int i = 0; i < TileButtons.Count; i++)
                {
                    GUIObject button = TileButtons[i];
                    ToolPositionsManager.AddPosition("PlatformButton" + i, button.Position);
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

            LayerDownButton.Draw(this, gameTime);
            LayerUpButton.Draw(this, gameTime);

            foreach (GUIObject Object in TileButtons)
            {
                Object.Draw(this, gameTime);
            }

            spriteBatch.End();

            
        }

        private void ProccedToSaveScreenAccepted(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new SaveMapScreen(mapManager));
        }
    }
}
