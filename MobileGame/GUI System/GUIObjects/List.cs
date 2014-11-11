using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using GUI_System.GUIObjects;
using GUI_System.GameStateManagement;
using Microsoft.Xna.Framework.Content;

namespace GUI_System.GUIObjects
{
    public class List : GUIObject
    {
        private ListStyle ListStyle;
        private List<ListItem> ListItems;
        internal MouseState oldState, currState;
        internal Point mousePos;

        public Vector2 Position
        {
            get { return ListStyle.Position; }
            set { ListStyle.Position = value; UpdateItemPositions(); }
        }

        private ListItem selectedItem;
        public ListItem SelectedItem { get { return selectedItem; } }

        public List(ListStyle ListStyle)
        {
            this.ListStyle = ListStyle;
            ListItems = new List<ListItem>();
            selectedItem = null;
        }

        public void Update(MenuScreen screen, GameTime gameTime)
        {
            oldState = currState;
            currState = Mouse.GetState();

            mousePos.X = Mouse.GetState().Position.X;
            mousePos.Y = Mouse.GetState().Position.Y;

            foreach (ListItem Item in ListItems)
                Item.Update(this, screen, gameTime);

            //selectedItem = null;

            for (int i = 0; i < ListItems.Count; i++)
            {
                if (ListItems[i].Selected)
                    selectedItem = ListItems[i];
            }
        }

        public void Draw(MenuScreen screen, GameTime gameTime)
        {
            ListStyle.Draw(screen.ScreenManager.SpriteBatch);

            foreach (ListItem Item in ListItems)
            {
                if (Item.Selected)
                    Item.Color = Color.Red;
                Item.Draw(screen, gameTime);
            }
                
        }

        public void AddItem(string text, ListItemStyle ListItemStyle)
        {

            ListItem NewItem = new ListItem(ListItemStyle, text);
            NewItem.Width = ListStyle.Width - ((int)ListStyle.ItemOffset.X * 2);

            Vector2 Pos = new Vector2();
            Pos.X = Position.X - ListStyle.Origin.X/2 + ListStyle.ItemOffset.X;
            Pos.Y = Position.Y - ListStyle.Origin.Y/2 + ListStyle.ItemOffset.Y + (ListItems.Count * NewItem.Height);

            NewItem.Position = Pos;
            ListItems.Add(NewItem);
        }

        public void RemoveItem(ListItem Item)
        {
            ListItems.Remove(Item);
        }

        public void Clear()
        {
            ListItems.Clear();
        }

        public int GetHeight(MenuScreen screen)
        {
            return ListStyle.Height;
        }

        public int GetWidth(MenuScreen screen)
        {
            return ListStyle.Width;
        }

        private void UpdateItemPositions()
        {
            for (int i = 0; i < ListItems.Count; i++)
            {
                Vector2 Pos = new Vector2();
                Pos.X = Position.X + ListStyle.ItemOffset.X;
                Pos.Y = Position.Y + ListStyle.ItemOffset.Y + (i * ListItems[i].Height);

                ListItems[i].Position = Pos;
            }
        }
    }
}
