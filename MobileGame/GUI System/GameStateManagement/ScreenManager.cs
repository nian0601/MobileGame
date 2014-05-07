using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GUI_System.GameStateManagement
{
    public class ScreenManager
    {
        public static Game Game;

        List<GameScreen> screens = new List<GameScreen>();
        List<GameScreen> tempScreensList = new List<GameScreen>();

        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D blankTexture;

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            set { spriteBatch = value; }
        }

        public SpriteFont Font
        {
            get { return font; }
            set { font = value; }
        }

        public Texture2D BlankTexture
        {
            get { return blankTexture; }
            set { blankTexture = value; }
        }

        public ScreenManager(Game game)
        {
            Game = game;

        }

        public void Update(GameTime gameTime)
        {
            //We create a copy of the screens-list to make sure that the adding and removing
            //of screens doesent interfere with the updating
            tempScreensList.Clear();

            foreach (GameScreen screen in screens)
                tempScreensList.Add(screen);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            //As long as the list of screens isnt empty we should keep updating
            while (tempScreensList.Count > 0)
            {
                //Pop out the screen at the top from the waitinglist (think of a stack) (Should probbably refactor this to use an actual stack in the future)
                GameScreen screen = tempScreensList[tempScreensList.Count - 1];
                tempScreensList.RemoveAt(tempScreensList.Count - 1);

                //Now we update the screen
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.CurrentScreenState == GameScreen.ScreenState.TransitionOn || screen.CurrentScreenState == GameScreen.ScreenState.Active)
                {
                    //If this is the first active screen we found we should give it a shot at handling input/updating gamelogic and what not
                    if (!otherScreenHasFocus)
                    {
                        //Here we should call an update function that only should be ran when we have focus on the screen in question
                        //I.E an updateloop that contains gamelogic or such
                        screen.HandleInput(gameTime);
                        otherScreenHasFocus = true;
                    }

                    coveredByOtherScreen = true;
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in screens)
            {
                if (screen.CurrentScreenState == GameScreen.ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }

        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            screen.IsExiting = false;
            screen.Activate();

            screens.Add(screen);
        }

        public void RemoveScreen(GameScreen screen)
        {
            screen.Unload();

            screens.Remove(screen);
            tempScreensList.Remove(screen);
        }

        public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }
    }
}
