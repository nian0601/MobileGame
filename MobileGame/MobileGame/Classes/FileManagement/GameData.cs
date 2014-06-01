using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobileGame.FileManagement
{
    public class GameData
    {
        public int CurrentMap;
        public bool AllMapsDone;
        public List<String> MapList;

        public GameData()
        {
            CurrentMap = 0;
            AllMapsDone = false;
            MapList = new List<string>();
            //MapList.Add("Test");
        }
    }
}
