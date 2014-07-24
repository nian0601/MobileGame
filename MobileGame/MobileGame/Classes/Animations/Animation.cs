using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MobileGame.Animations
{
    class Animation
    {
        private string name;
        private int timePerFrame;
        private int startCol, startRow;
        private int endCol, endRow;

        public string Name { get { return name; } }
        public int TimePerFrame { get { return timePerFrame; } }
        public int StartCol { get { return startCol; } }
        public int StartRow { get { return startRow; } }
        public int EndCol { get { return endCol; } }
        public int EndRow { get { return endRow; } }

        public Animation(string Name, int TimePerFrame, int StartCol, int StartRow, int EndCol, int EndRow)
        {
            name = Name;
            timePerFrame = TimePerFrame;
            startCol = StartCol;
            startRow = StartRow;
            endCol = EndCol;
            endRow = EndRow;
        }
    }
}
