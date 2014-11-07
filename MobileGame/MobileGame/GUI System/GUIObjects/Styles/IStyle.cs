using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GUI_System.GUIObjects
{
    public interface IStyle
    {
        Vector2 Position { get; set; }

        void Draw(SpriteBatch spriteBatch);
        Rectangle ClickableArea();
    }
}
