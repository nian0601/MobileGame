using GUI_System.GameStateManagement;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUI_System.GUIObjects
{
    public interface GUIObject
    {
        Vector2 Position { get; set; }

        void Update(MenuScreen screen, GameTime gameTime);

        void Draw(MenuScreen screen, GameTime gameTime);

        int GetHeight(MenuScreen screen);
        int GetWidth(MenuScreen screen);
    }
}
