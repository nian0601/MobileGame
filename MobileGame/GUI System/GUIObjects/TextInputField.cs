using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using GUI_System.GameStateManagement;


namespace GUI_System.GUIObjects
{
    public class TextInputField : GUIObject
    {
        public Vector2 Position
        {
            get { return FieldStyle.Position; }
            set { FieldStyle.Position = value; }
        }

        private TextInputObject InputObject;
        public string Text { get { return InputObject.Text; } }

        private InputFieldStyle FieldStyle;

        private MouseState oldState, currState;
        private Vector2 mousePos;

        public TextInputField(InputFieldStyle FieldStyle)
        {
            InputObject = new TextInputObject();
            InputObject.CharNumLimit = 12;
            this.FieldStyle = FieldStyle;
        }

        public void Update(MenuScreen screen, GameTime gameTime)
        {
            oldState = currState;
            currState = Mouse.GetState();

            mousePos.X = currState.X;
            mousePos.Y = currState.Y;

            FieldStyle.NormalTexture();

            if (FieldStyle.ClickableArea().Contains(mousePos))
                FieldStyle.HooverTexture();  
                
            InputObject.Update();
            FieldStyle.Text = InputObject.Text;
        }

        public void Draw(MenuScreen screen, GameTime gameTime)
        {
            FieldStyle.Draw(screen.ScreenManager.SpriteBatch);
        }

        public void Clear()
        {
            FieldStyle.Text = "";
            InputObject.Text = "";
        }

        public int GetHeight(MenuScreen screen)
        {
            return FieldStyle.Size().Height;
        }

        public int GetWidth(MenuScreen screen)
        {
            return FieldStyle.Size().Width;
        }
    }
}
