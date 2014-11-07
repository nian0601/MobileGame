using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GUI_System.GameStateManagement
{
    static class KeyMouseReader
    {
        private static KeyboardState keyState, oldKeyState = Keyboard.GetState();
        private static MouseState mouseState, oldMouseState = Mouse.GetState();
        private static Point mousePos;
        private static int scrollValue, oldScrollValue = oldMouseState.ScrollWheelValue;

        public static bool KeyClick(Keys key)
        {
            return keyState.IsKeyDown(key) && oldKeyState.IsKeyUp(key);
        }

        public static bool isKeyDown(Keys key)
        {
            return keyState.IsKeyDown(key);
        }

        public static bool LeftClick()
        {
            return mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
        }

        public static bool LeftMouseDown()
        {
            return mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool RightClick()
        {
            return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
        }

        public static bool RightMouseDown()
        {
            return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Pressed;
        }

        public static bool MouseWheelClick()
        {
            return mouseState.MiddleButton == ButtonState.Pressed && oldMouseState.MiddleButton == ButtonState.Released;
        }

        public static bool MouseWheelDown()
        {
            return mouseState.MiddleButton == ButtonState.Pressed && oldMouseState.MiddleButton == ButtonState.Pressed;
        }

        public static Point GetMousePos()
        {
            return mousePos;
        }

        public static bool ScrolledUp()
        {
            return scrollValue > oldScrollValue;
        }

        public static bool ScrolledDown()
        {
            return scrollValue < oldScrollValue;
        }

        public static void Update()
        {
            oldKeyState = keyState;
            keyState = Keyboard.GetState();
            oldMouseState = mouseState;
            mouseState = Mouse.GetState();
            mousePos.X = mouseState.X;
            mousePos.Y = mouseState.Y;

            oldScrollValue = scrollValue;
            scrollValue = Mouse.GetState().ScrollWheelValue;
        }
    }
}
