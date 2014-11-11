using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using GUI_System.GameStateManagement;

namespace GUI_System.GUIObjects
{
    public class MenuButton : GUIObject
    {
        private MouseState oldState, currState;
        private Point mousePos;

        private ButtonStyle buttonStyle;

        public Vector2 Position
        {
            get { return buttonStyle.Position; }
            set { buttonStyle.Position = value; }
        }

        public MenuButton(ButtonStyle style)
        {
            oldState = currState;
            currState = Mouse.GetState();

            buttonStyle = style;
        }

        public void Update(MenuScreen screen, GameTime gameTime)
        {
            oldState = currState;
            currState = Mouse.GetState();

            mousePos.X = currState.X;
            mousePos.Y = currState.Y;

            buttonStyle.NormalTexture();

            if (HitBox().Contains(mousePos))
            {
                buttonStyle.HooverTexture();
                if (KeyMouseReader.isKeyDown(Keys.LeftShift) && KeyMouseReader.LeftMouseDown())
                {
                    Vector2 newPos = new Vector2(KeyMouseReader.GetMousePos().X, KeyMouseReader.GetMousePos().Y);
                    Position = newPos;
                }
            }
                
        }

        public void Draw(MenuScreen screen, GameTime gameTime)
        {
            buttonStyle.Draw(screen.ScreenManager.SpriteBatch);
        }

        public bool LeftClick()
        {
            if (HitBox().Contains(mousePos))
                return currState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released;

            return false;
        }

        public bool RightClick()
        {
            if (HitBox().Contains(mousePos))
                return currState.RightButton == ButtonState.Pressed && oldState.RightButton == ButtonState.Released;

            return false;
        }

        public int GetHeight(MenuScreen screen)
        {
            return HitBox().Height;
        }

        public int GetWidth(MenuScreen screen)
        {
            return HitBox().Width;
        }

        protected Rectangle HitBox()
        {
            return buttonStyle.ClickableArea();
        }
    }
}