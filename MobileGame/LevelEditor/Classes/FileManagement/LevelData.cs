﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelEditor.FileManagement
{
    public class LevelData
    {
        public int TileSize;
        public int MapWidth;
        public int MapHeight;
        public int[][][] LevelArray;

        public LevelData()
        {
            TileSize = 0;
            MapWidth = 0;
            MapHeight = 0;
        }
    }
}
