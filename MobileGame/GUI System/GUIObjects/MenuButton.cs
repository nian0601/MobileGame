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
    public class MenuButton : MenuEntry
    {
        private MouseState oldState, currState;
        private Point mousePos;

        private Style buttonStyle;

        public override Vector2 Position
        {
            get { return buttonStyle.Position; }
            set { buttonStyle.Position = value; }
        }

        public MenuButton(Style style): base("")
        {
            oldState = currState;
            currState = Mouse.GetState();

            buttonStyle = style;
        }

        public override void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            oldState = currState;
            currState = Mouse.GetState();

            mousePos.X = currState.X;
            mousePos.Y = currState.Y;

            buttonStyle.NormalTexture();

            if (HitBox().Contains(mousePos))
                buttonStyle.HooverTexture();
        }

        public override void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            Vector2 origin = new Vector2(HitBox().Width / 2, HitBox().Height / 2);
            buttonStyle.Draw(screen.ScreenManager.SpriteBatch, origin);
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

        public override int GetHeight(MenuScreen screen)
        {
            return HitBox().Height;
        }

        public override int GetWidth(MenuScreen screen)
        {
            return HitBox().Width;
        }

        protected Rectangle HitBox()
        {
            return buttonStyle.HitBox();
        }
    }
}