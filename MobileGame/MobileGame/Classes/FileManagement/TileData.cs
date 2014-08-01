using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobileGame.FileManagement
{
    public class TileData
    {
        public int TileValue;
        public int TileType;
        public bool Collidable;
        public bool CanJumpThrough;

        public TileData()
        {
            TileValue = 0;
            TileType = 0;
            Collidable = false;
            CanJumpThrough = false;
        }
    }
}
