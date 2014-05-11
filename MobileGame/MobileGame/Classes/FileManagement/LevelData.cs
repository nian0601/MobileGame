using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobileGame.FileManagement
{
    public class LevelData
    {
        /// <summary>
        /// This needs to be a JAGGED Array for the XMLSerializer to accept it
        /// </summary>
        public int[][][] LevelArray;
        public int TileSize;

        public LevelData()
        {
            TileSize = 0;
        }
    }
}
