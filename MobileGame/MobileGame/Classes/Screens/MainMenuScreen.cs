using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GUI_System.GameStateManagement;
using GUI_System.GUIObjects;

using MobileGame.FileManagement;
using MobileGame.Managers;
using MobileGame.LevelEditor;

namespace MobileGame.Screens
{
    public class MainMenuScreen : MenuScreen
    {
        private MenuButton playButton, levelSelectButton;
        Texture2D TitleTexture;

        public MainMenuScreen() : base("Main menu")
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            playButton = new MenuButton(new PlayButtonStyle(ScreenManager.Game.Content));
            levelSelectButton = new MenuButton(new LevelSelectStyle(ScreenManager.Game.Content));

            MenuEntries.Add(playButton);
            MenuEntries.Add(levelSelectButton);

            TitleTexture = ScreenManager.Game.Content.Load<Texture2D>("GUI Textures/MainMenu/MainMenuTitle");

            TextureManager.LoadContents(ScreenManager.Game.Content);
            FileLoader.Initialize();
            LightingManager.Initialize();
        }

        public override void Activate()
        {
            Game1.SetScreenSize(800, 600);
            
            base.Activate();
        }

        public override void HandleInput(GameTime gameTime)
        {
            //This will only ever be true when we leave the editor
            if (Game1.ScreenHeight != 600 && Game1.ScreenWidth != 800)
            {
                Game1.SetScreenSize(800, 600);
                FileLoader.Initialize();
            }
                

            if (playButton.LeftClick())
                LoadingScreen.Load(ScreenManager, true, new GamePlayScreen());

            if (levelSelectButton.LeftClick())
                ScreenManager.AddScreen(new LevelSelectScreen());

            if (KeyMouseReader.KeyClick(Microsoft.Xna.Framework.Input.Keys.R))
                FileLoader.ResetSaveFile();

            if (KeyMouseReader.KeyClick(Microsoft.Xna.Framework.Input.Keys.E))
            {
                Game1.SetScreenSize(1100, 800);
                LoadingScreen.Load(ScreenManager, true, new EditorScreen());
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
                GUIObject menuEntry = MenuEntries[i];

                menuEntry.Draw(this, gameTime);
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