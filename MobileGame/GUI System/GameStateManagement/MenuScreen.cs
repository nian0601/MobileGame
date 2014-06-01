using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using GUI_System.GUIObjects;

namespace GUI_System.GameStateManagement
{
    public abstract class MenuScreen : GameScreen
    {
        List<GUIObject> menuEntries = new List<GUIObject>();
        int selectedEntry = 0;
        string menuTitle;

        protected IList<GUIObject> MenuEntries
        {
            get { return menuEntries; }
        }

        public MenuScreen(string menuTitle)
        {
            this.menuTitle = menuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void HandleInput(GameTime gameTime)
        {
            if (KeyMouseReader.KeyClick(Keys.Escape))
                OnCancel();
        }

        protected virtual void OnCancel()
        {
            ExitScreen();
        }

        protected virtual void UpdateMenyEntryLocations()
        {
            //This makes the menu slide into place. Should experiment with this function to see what other cool effects you could achive
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            //We start at Y = 175, which then increments after each menuEntry
            //The X-value is generated per entry
            Vector2 position = new Vector2(0, 175);

            for (int i = 0; i < menuEntries.Count; i++)
            {
                GUIObject menuEntry = menuEntries[i];

                //each entry gets centered horizontally
                position.X = ScreenManager.Game.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

                if (CurrentScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                //set the entry's position
                menuEntry.Position = position;

                //increment the Y-value for the next entry
                position.Y += menuEntry.GetHeight(this);
            }

        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            for (int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);

                menuEntries[i].Update(this, gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            UpdateMenyEntryLocations();

            GraphicsDevice graphics = ScreenManager.Game.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();

            for (int i = 0; i < menuEntries.Count; i++)
            {
                GUIObject menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, gameTime);
            }

            //This is used to make the menuTitle slide into place, just like the entries
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}