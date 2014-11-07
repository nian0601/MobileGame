using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using GUI_System.GameStateManagement;

namespace GUI_System.GUIObjects
{
    public class ColorPicker : GUIObject
    {
        private MouseState oldState, currState;
        private Point mousePos;
        private Color[] colorData;

        private ColorPickerStyle pickerStyle;

        private Color selectedColor;
        public Color SelectedColor { get { return selectedColor; } }

        public Vector2 Position
        {
            get { return pickerStyle.Position; }
            set { pickerStyle.Position = value; }
        }

        public ColorPicker(ColorPickerStyle style)
        {
            oldState = currState;
            currState = Mouse.GetState();

            pickerStyle = style;

            colorData = GetPixelData(pickerStyle.TextureToPickFrom);
        }

        public void Update(MenuScreen screen, GameTime gameTime)
        {
            oldState = currState;
            currState = Mouse.GetState();

            mousePos.X = currState.X;
            mousePos.Y = currState.Y;

            if (oldState.LeftButton == ButtonState.Pressed && currState.LeftButton == ButtonState.Released)
            {
                if (HitBox().Contains(mousePos))
                {
                    Color[] retrivedColor = new Color[1];
                    Rectangle sourceRect = new Rectangle(mousePos.X - HitBox().X, mousePos.Y - HitBox().Y, 1, 1);
                    pickerStyle.TextureToPickFrom.GetData<Color>(0, sourceRect, retrivedColor, 0, 1);
                    selectedColor = retrivedColor[0];
                }

            }
        }

        public void Draw(MenuScreen screen, GameTime gameTime)
        {
            pickerStyle.Draw(screen.ScreenManager.SpriteBatch);
        }

        public int GetHeight(MenuScreen screen)
        {
            return HitBox().Height;
        }

        public int GetWidth(MenuScreen screen)
        {
            return HitBox().Width;
        }

        public Rectangle HitBox()
        {
            return pickerStyle.ClickableArea();
        }

        private Color[] GetPixelData(Texture2D texture)
        {
            Color[] pixels = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(pixels);
            return pixels;
        }
    }
}
