using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using GUI_System.GameStateManagement;

namespace GUI_System.GUIObjects
{
    public class ListItem
    {
        private ListItemStyle ItemStyle;

        public Vector2 Position
        {
            get { return ItemStyle.Position; }
            set { ItemStyle.Position = value; }
        }

        public int Width
        {
            get { return ItemStyle.Width; }
            set { ItemStyle.Width = value; }
        }

        public int Height
        {
            get { return ItemStyle.Height; }
            set { ItemStyle.Height = value; }
        }

        public string Text
        {
            get { return ItemStyle.Text; }
            set { ItemStyle.Text = value; }
        }

        public Color Color
        {
            set { ItemStyle.Color = value; }
        }

        internal bool Selected;

        public ListItem(ListItemStyle ItemStyle, string text)
        {
            this.ItemStyle = ItemStyle;
            Text = text;
            Height = (int)ItemStyle.Font.MeasureString(Text).Y;
            Selected = false;
        }

        public void Update(List List, MenuScreen screen, GameTime gameTime)
        {
            if (List.currState.LeftButton == ButtonState.Pressed && List.oldState.LeftButton == ButtonState.Released)
            {
                Selected = false;
                
                if (ItemStyle.ClickableArea().Contains(List.mousePos))
                {
                    ItemStyle.Color = Color.Gray;
                    Selected = true;
                    Console.WriteLine("Selected list item: " + Text);
                }
            }
            else if (ItemStyle.ClickableArea().Contains(List.mousePos))
                ItemStyle.Color = Color.Gray;
            
        }

        public void Draw(MenuScreen screen, GameTime gameTime)
        {
            ItemStyle.Draw(screen.ScreenManager.SpriteBatch);
            ItemStyle.Color = ItemStyle.DefaultColor;
        }
    }
}
