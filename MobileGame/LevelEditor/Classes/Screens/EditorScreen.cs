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
        }

        public override void Activate()
        {
            if(mapManager == null)
                mapManager = new MapManager(ScreenManager.Game.Content, ScreenManager.SpriteBatch);

            mapManager.Initialize();
            mapManager.Offset = new Vector2(218, 22);
        }

        public override void HandleInput(GameTime gameTime)
        {
            mapManager.Update();

            if (SaveButton.LeftClick())
                ScreenManager.AddScreen(new SaveMapScreen());

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
                MenuEntry menuEntry = MenuEntries[i];

                position.Y = yStartPos + (menuEntry.GetHeight(this) + spaceBetweenEntries) * i;

                if (CurrentScreenState == ScreenState.TransitionOn)
                    position.Y -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                menuEntry.Position = position;
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
                MenuEntry menuEntry = MenuEntries[i];

                bool isSelected = false;

                menuEntry.Draw(this, isSelected, gameTime);
            }

            spriteBatch.End();

            
        }
    }
}
