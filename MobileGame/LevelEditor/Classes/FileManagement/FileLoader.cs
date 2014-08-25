using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Xna.Framework.Storage;

using LevelEditor.Managers;

namespace LevelEditor.FileManagement
{
    static class FileLoader
    {
        private static string SaveFilesDirectoryPath = @"C:\Users\" + Environment.UserName + @"\MobileGame";
        private static string LevelDirectoryPath = @"C:\Users\" + Environment.UserName + @"\MobileGame\Levels";
        private static LevelData LoadedLevel;
        private static GameData gameData;

        #region Properties
        public static byte[,] LoadedCollisionLayer
        {
            get { return ConvertToMultiArray(LoadedLevel.CollisionLayer); }
        }

        public static byte[,] LoadedBackgroundLayer
        {
            get { return ConvertToMultiArray(LoadedLevel.BackgroundLayer); }
        }

        public static byte[,] LoadedPlatformLayer
        {
            get { return ConvertToMultiArray(LoadedLevel.PlatformLayer); }
        }

        public static byte[,] LoadedSpecialsLayer
        {
            get { return ConvertToMultiArray(LoadedLevel.SpecialsLayer); }
        }

        public static int LoadedLevelTileSize
        {
            get { return LoadedLevel.TileSize; }
        }

        public static GameData LoadedGameData
        {
            get { return gameData; }
        }
        #endregion

        //This method loads the "newest" map, that is the map the player last played on/unlocked
        //Gets called when the game boots up (in MainMenuScreen)
        public static void Initialize()
        {
            gameData = new GameData();

            string savePath = SaveFilesDirectoryPath + @"\GameData.xml";

            FileStream stream = File.Open(savePath, FileMode.Open);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(GameData));
                gameData = (GameData)serializer.Deserialize(stream);
            }
            finally
            {
                stream.Close();
            }
        }

        public static void LoadLevel(string MapName)
        {
            string LoadPath = LevelDirectoryPath + @"\" + MapName + ".xml";

            FileStream stream = File.Open(LoadPath, FileMode.Open);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
                LoadedLevel = (LevelData)serializer.Deserialize(stream);
            }
            finally
            {
                stream.Close();
            }

            EditorMapManager.BuildMap();
        }

        public static void SaveLevel(string LevelName)
        {
            int MapHeight = EditorMapManager.mapHeight;
            int MapWidth = EditorMapManager.mapWidth;

            byte[,] CollisionLayer = EditorMapManager.CollisionLayer;
            byte[,] BackgroundLayer = EditorMapManager.BackgroundLayer;
            byte[,] PlatformLayer = EditorMapManager.PlatformLayer;
            byte[,] SpecialsLayer = EditorMapManager.SpecialsLayer;


            //First we read the GameData file so that we get access to the MapList
            //We will use that to propperly name the new map
            string GameDataPath = SaveFilesDirectoryPath + @"\GameData.xml";
            GameData TempGameData;
            FileStream GameDataStream = File.Open(GameDataPath, FileMode.Open);

            try
            {
                XmlSerializer GameDataSerializer = new XmlSerializer(typeof(GameData));
                TempGameData = (GameData)GameDataSerializer.Deserialize(GameDataStream); 
            }
            finally
            {
                GameDataStream.Close();
            }

            //Now we make sure that we have a Levels directory
            System.IO.Directory.CreateDirectory(LevelDirectoryPath);

            //Get all the necessary LevelData
            LevelData LevelData = new LevelData();
            LevelData.TileSize = EditorMapManager.TileSize;
            LevelData.MapHeight = MapHeight;
            LevelData.MapWidth = MapWidth;
            LevelData.CollisionLayer = ConvertToJaggedArray(CollisionLayer);
            LevelData.BackgroundLayer = ConvertToJaggedArray(BackgroundLayer);
            LevelData.PlatformLayer = ConvertToJaggedArray(PlatformLayer);
            LevelData.SpecialsLayer = ConvertToJaggedArray(SpecialsLayer);

            //And now its time to build the SavePath
            //The new level we want to save should get number "MapList.Count + 1"
            //So if the MapList is empty, that is we have no maps, 
            //then the new map we save will get number "0 + 1 = 1"
            //If there are 8 maps in the list then the new map will get number "8 + 1 = 9"
            string savePath = LevelDirectoryPath + @"\" + LevelName + ".xml";
            FileStream stream = File.Open(savePath, FileMode.Create);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
                serializer.Serialize(stream, LevelData);
            }
            finally
            {
                stream.Close();
            }

            //After we have saved the new level we need to update the GameData file (update the MapList)
            TempGameData.MapList = gameData.MapList;
            if(!TempGameData.MapList.Contains(LevelName))
                TempGameData.MapList.Add(LevelName);

            GameDataStream = File.Open(GameDataPath, FileMode.Create);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(GameData));
                serializer.Serialize(GameDataStream, TempGameData);
            }
            finally
            {
                GameDataStream.Close();
            }

            gameData = TempGameData;
        }

        public static void DeleteLevel(string MapName)
        {
            string GameDataPath = SaveFilesDirectoryPath + @"\GameData";

            gameData.MapList.Remove(MapName);

            UpdateGameData();

            string MapPath = LevelDirectoryPath + @"\" + MapName;
            File.Delete(MapPath);
        }

        public static void UpdateGameData()
        {
            //Instansiate a new GameData object that we later will serialize
            GameData GameData = new GameData();

            GameData.AllMapsDone = gameData.AllMapsDone;
            GameData.CurrentMap = gameData.CurrentMap;

            //Make sure that we transfer the MapList between GameData's
            GameData.MapList = gameData.MapList;

            //Make sure that the gameData variable that the game actually uses gets updated aswell!
            gameData = GameData;

            //
            //After we have set the two values we simply do the serializing process
            //

            string savePath = SaveFilesDirectoryPath + @"\GameData";

            FileStream stream = File.Open(savePath, FileMode.Create);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(GameData));
                serializer.Serialize(stream, GameData);
            }
            finally
            {
                stream.Close();
            }
        }


        private static byte[][] ConvertToJaggedArray(byte[,] multiArray)
        {
            int mapHeight = multiArray.GetLength(0);
            int mapWidth = multiArray.GetLength(1);

            byte[][] tempJaggedArray = new byte[mapHeight][];

            for (int y = 0; y < mapHeight; y++)
            {
                tempJaggedArray[y] = new byte[mapWidth];
                for (int x = 0; x < mapWidth; x++)
                {
                    tempJaggedArray[y][x] = multiArray[y, x];
                }
            }

            return tempJaggedArray;
        }

        private static byte[,] ConvertToMultiArray(byte[][] jaggedArray)
        {
            int mapHeight = jaggedArray.Length;
            int mapWidth = jaggedArray[0].Length;

            byte[,] tempArray = new byte[mapHeight, mapWidth];

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    tempArray[y, x] = jaggedArray[y][x];
                }
            }

            return tempArray;
        }
    }
}
