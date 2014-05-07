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
        public static KeyboardState keyState, oldKeyState = Keyboard.GetState();
        public static MouseState mouseState, oldMouseState = Mouse.GetState();
        public static Point mousePos;

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

        public static bool RightClick()
        {
            return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
        }

        public static Point GetMousePos()
        {
            return mousePos;
        }

        public static void Update()
        {
            oldKeyState = keyState;
            keyState = Keyboard.GetState();
            oldMouseState = mouseState;
            mouseState = Mouse.GetState();
            mousePos.X = mouseState.X;
            mousePos.Y = mouseState.Y;
        }
    }
}
