using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GUI_System.GameStateManagement;
using GUI_System.GUIObjects;

using MobileGame.Managers;
using MobileGame.Screens;

namespace MobileGame.LevelEditor
{
    public class EditorScreen : MenuScreen
    {
        private MenuButton LoadButton, SaveButton, ExitButton;
        private MenuButton BGLayerButton, MiddleLayerButton, FGLayerButton;
        private MenuButton TileLayerButton, CollisionLayerButton, JumpLayerButton;

        private List<MenuButton> ToolButtons;

        private EditorMapManager EditorMapManager;

        public static int EditMode;

        public EditorScreen() : base("Editor Screen")
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            EditMode = 0;

            ExitButton = new MenuButton(new ExitButtonStyle(ScreenManager.Game.Content));
            SaveButton = new MenuButton(new SaveMapButtonStyle(ScreenManager.Game.Content));
            LoadButton = new MenuButton(new LoadMapButtonStyle(ScreenManager.Game.Content));

            BGLayerButton = new MenuButton(new BGLayerButton(ScreenManager.Game.Content));
            MiddleLayerButton = new MenuButton(new MiddleLayerBytton(ScreenManager.Game.Content));
            FGLayerButton = new MenuButton(new FGLayerButton(ScreenManager.Game.Content));

            TileLayerButton = new MenuButton(new TileLayerButton(ScreenManager.Game.Content));
            CollisionLayerButton = new MenuButton(new CollisionLayerButton(ScreenManager.Game.Content));
            JumpLayerButton = new MenuButton(new JumpLayerButton(ScreenManager.Game.Content));

            MenuEntries.Add(ExitButton);
            MenuEntries.Add(SaveButton);
            MenuEntries.Add(LoadButton);

            #region TileButtons
            ToolButtons = new List<MenuButton>();
            ToolButtons.Add(new MenuButton(new Tile0(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new Tile1(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new Tile2(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new Tile3(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new Tile4(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new Tile5(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new Tile6(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new Tile7(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new Tile8(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new Tile9(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new Tile10(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new Tile11(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new Tile12(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new Tile13(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new Tile14(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new Tile15(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new JumpTileButton(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new GoalTileButton(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new Spike1Button(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new Spike2Button(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new BGTile0(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new BGTile1(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new BGTile2(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new BGTile3(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new BGTile4(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new BGTile5(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new BGTile6(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new BGTile7(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new BGTile8(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new PlayerButton(ScreenManager.Game.Content)));
            ToolButtons.Add(new MenuButton(new EnemyButton(ScreenManager.Game.Content)));

            #endregion
        }

        public override void Activate()
        {
            FileManagement.FileLoader.Initialize();
            ToolPositionsManager.LoadData();

            if (EditorMapManager == null)
                EditorMapManager = new EditorMapManager(ScreenManager.Game.Content, ScreenManager.SpriteBatch);

            EditorMapManager.Initialize();
            EditorMapManager.Offset = new Vector2(240, 22);

            for (int i = 0; i < ToolButtons.Count; i++)
            {
                GUIObject button = ToolButtons[i];
                button.Position = ToolPositionsManager.GetPosition("Tool" + i);
            }
        }

        public override void HandleInput(GameTime gameTime)
        {
            //KeyMouseReader.Update();
            EditorMapManager.Update();

            if (SaveButton.LeftClick())
            {
                ConfirmationPopUp ProccedToSaveScreenBox = new ConfirmationPopUp();

                if (!EditorMapManager.PlayerPlaced)
                    ProccedToSaveScreenBox = new ConfirmationPopUp("You have not placed the Player");
                else if (!EditorMapManager.GoalPlaced)
                    ProccedToSaveScreenBox = new ConfirmationPopUp("You have not place the Goal");
                else if (!EditorMapManager.HasSufficentCollisionFlags)
                    ProccedToSaveScreenBox = new ConfirmationPopUp("There is no or very few collisionflags");
                else
                {
                    ScreenManager.AddScreen(new SaveMapScreen(EditorMapManager));
                    return;
                }

                ProccedToSaveScreenBox.Accepted = ProccedToSaveScreenAccepted;
                ScreenManager.AddScreen(ProccedToSaveScreenBox);
            }


            else if (LoadButton.LeftClick())
                ScreenManager.AddScreen(new LoadMapScreen(EditorMapManager));

            else if (ExitButton.LeftClick())
                LoadingScreen.Load(ScreenManager, true, new BackgroundScreen(), new MainMenuScreen());

            else if (TileLayerButton.LeftClick())
                ChangeEditMode(0);
            else if (CollisionLayerButton.LeftClick())
                ChangeEditMode(1);
            else if (JumpLayerButton.LeftClick())
                ChangeEditMode(2);

            if (EditMode == 0)
            {
                if (BGLayerButton.LeftClick())
                    EditorMapManager.SetSelectedLayer(0);
                else if (MiddleLayerButton.LeftClick())
                    EditorMapManager.SetSelectedLayer(1);
                else if (FGLayerButton.LeftClick())
                    EditorMapManager.SetSelectedLayer(2);
            }

            for (int i = 0; i < ToolButtons.Count; i++)
            {
                ToolButtons[i].Update(this, gameTime);

                if (ToolButtons[i].LeftClick())
                    EditorMapManager.SetCursorTexture(i);
            }

            base.HandleInput(gameTime);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            TileLayerButton.Update(this, gameTime);
            CollisionLayerButton.Update(this, gameTime);
            JumpLayerButton.Update(this, gameTime);

            if (EditMode == 0)
            {
                BGLayerButton.Update(this, gameTime);
                MiddleLayerButton.Update(this, gameTime);
                FGLayerButton.Update(this, gameTime);
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        protected override void UpdateMenyEntryLocations()
        {
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            #region Exit/Save/Load Buttons

            int xSpacing = 10;
            int ySpacing = 5;
            int yStartPos = ScreenManager.Game.GraphicsDevice.Viewport.Height - MenuEntries[0].GetHeight(this) + 10;
            int xStartPos = MenuEntries[0].GetWidth(this) / 2 + 15;

            Vector2 position = new Vector2();
            position.Y = yStartPos;

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                GUIObject menuEntry = MenuEntries[i];

                position.X = xStartPos + (menuEntry.GetWidth(this) + xSpacing) * i;

                if (CurrentScreenState == ScreenState.TransitionOn)
                    position.Y -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                menuEntry.Position = position;
            }

            #endregion

            xStartPos = TileLayerButton.GetWidth(this) / 2 + 15;
            yStartPos = yStartPos - TileLayerButton.GetHeight(this) - ySpacing;

            position = new Vector2(xStartPos, yStartPos);
            TileLayerButton.Position = position;

            position.X += CollisionLayerButton.GetWidth(this) + xSpacing;
            CollisionLayerButton.Position = position;

            position.X += JumpLayerButton.GetHeight(this) + xSpacing;
            JumpLayerButton.Position = position;

            position.X = BGLayerButton.GetWidth(this) / 2 + 15;
            position.Y -= BGLayerButton.GetHeight(this) + ySpacing;
            BGLayerButton.Position = position;

            position.Y -= MiddleLayerButton.GetHeight(this) + ySpacing;
            MiddleLayerButton.Position = position;

            position.Y -= FGLayerButton.GetHeight(this) + ySpacing;
            FGLayerButton.Position = position;

            if (KeyMouseReader.KeyClick(Microsoft.Xna.Framework.Input.Keys.M))
            {
                for (int i = 0; i < ToolButtons.Count; i++)
                {
                    GUIObject button = ToolButtons[i];
                    ToolPositionsManager.AddPosition("Tool" + i, button.Position);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            UpdateMenyEntryLocations();

            GraphicsDevice graphics = ScreenManager.Game.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            EditorMapManager.Draw();

            spriteBatch.Begin();

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                GUIObject menuEntry = MenuEntries[i];
                menuEntry.Draw(this, gameTime);
            }

            TileLayerButton.Draw(this, gameTime);
            CollisionLayerButton.Draw(this, gameTime);
            JumpLayerButton.Draw(this, gameTime);

            if (EditMode == 0)
            {
                BGLayerButton.Draw(this, gameTime);
                MiddleLayerButton.Draw(this, gameTime);
                FGLayerButton.Draw(this, gameTime);
            }

            foreach (GUIObject Object in ToolButtons)
            {
                Object.Draw(this, gameTime);
            }

            spriteBatch.End();


        }

        private void ProccedToSaveScreenAccepted(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new SaveMapScreen(EditorMapManager));
        }

        public static void ChangeEditMode(int value)
        {
            EditMode = value;
            if (EditMode > 4)
                EditMode = 4;

            else if (EditMode < 0)
                EditMode = 0;

            Console.WriteLine("EditMode: " + EditMode);
        }
    }
}
