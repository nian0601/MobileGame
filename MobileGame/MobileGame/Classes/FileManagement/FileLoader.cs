using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Xna.Framework.Storage;

namespace MobileGame.FileManagement
{
    static class FileLoader
    {
        private static string SaveFilesDirectoryPath = @"C:\Users\" + Environment.UserName + @"\MobileGame";
        private static string LevelDirectoryPath = @"C:\Users\" + Environment.UserName + @"\MobileGame\Levels";
        private static LevelData LoadedLevel;
        private static GameData gameData;

        #region Properties
        public static int[, ,] LoadedLevelArray { get { return ConvertToMultiArray(LoadedLevel.LevelArray); } }

        public static int LoadedLevelTileSize{ get { return LoadedLevel.TileSize; } }

        public static int LoadedLevelMapHeight { get { return LoadedLevel.MapHeight; } }

        public static int LoadedLevelMapWidth { get { return LoadedLevel.MapWidth; } }

        public static GameData LoadedGameData { get { return gameData; } }
        #endregion

        //This method loads the "newest" map, that is the map the player last played on/unlocked
        //Gets called when the game boots up (in MainMenuScreen)
        public static void Initialize()
        {
            gameData = new GameData();

            string savePath = SaveFilesDirectoryPath + @"\GameData";

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

            LoadLevel(gameData.CurrentMap);
        }

        public static void LoadLevel(int LevelNumber)
        {
            string LoadPath = LevelDirectoryPath + @"\" + gameData.MapList[LevelNumber-1];

            FileStream stream = File.Open(LoadPath, FileMode.Open);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
                LoadedLevel = (LevelData)serializer.Deserialize(stream);
            }
            finally
            {
                GameManager.Player = null;
                stream.Close();
            }
        }

        public static void SaveLevel(int[,,] LevelArray)
        {
            //First we read the GameData file so that we get access to the MapList
            //We will use that to propperly name the new map
            string GameDataPath = SaveFilesDirectoryPath + @"\GameData";
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
            LevelData.LevelArray = ConvertToJaggedArray(LevelArray);
            LevelData.TileSize = TextureManager.TileSize;

            //And now its time to build the SavePath
            //The new level we want to save should get number "MapList.Count + 1"
            //So if the MapList is empty, that is we have no maps, 
            //then the new map we save will get number "0 + 1 = 1"
            //If there are 8 maps in the list then the new map will get number "8 + 1 = 9"
            string savePath = LevelDirectoryPath + @"\Level" + (TempGameData.MapList.Count + 1);
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
            TempGameData.MapList.Add("Level" + (TempGameData.MapList.Count + 1));
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

        public static void UpdateGameData()
        {
            //We get the filePath to the nextlevel (if there is one)
            //string levelPath = LevelDirectoryPath + @"\Level" + (gameData.CurrentMap + 1).ToString();
            //string levelPath = LevelDirectoryPath + @"\" + gameData.MapList[gameData.CurrentMap];

            //Instansiate a new GameData object that we later will serialize
            GameData GameData = new GameData();

            if (gameData.MapList.Count > gameData.CurrentMap)
            {
                //So we set AllMapsDone to FALSE
                GameData.AllMapsDone = false;
                //And set LastMapPlayed to the next level (increment it with one)
                GameData.CurrentMap = gameData.CurrentMap + 1;
            }
            else
            {   
                //So we set AllMapsDone to true
                GameData.AllMapsDone = true;
                //And keep the last level as the lastMapPlayed
                GameData.CurrentMap = gameData.CurrentMap;
            }

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

        private static int[][][] ConvertToJaggedArray(int[, ,] multiArray)
        {
            int layers = multiArray.GetLength(0);
            int mapHeight = multiArray.GetLength(1);
            int mapWidth = multiArray.GetLength(2);

            int[][][] tempJaggedArray = new int[layers][][];

            for (int z = 0; z < layers; z++)
            {
                tempJaggedArray[z] = new int[mapHeight][];
                for (int y = 0; y < mapHeight; y++)
                {
                    tempJaggedArray[z][y] = new int[mapWidth];
                    for (int x = 0; x < mapWidth; x++)
                    {
                        tempJaggedArray[z][y][x] = multiArray[z, y, x];
                    }
                }
            }

            return tempJaggedArray;
        }

        private static int[, ,] ConvertToMultiArray(int[][][] jaggedArray)
        {
            int layers = jaggedArray.Length;
            int mapHeight = jaggedArray[0].Length;
            int mapWidth = jaggedArray[0][0].Length;

            int[, ,] tempArray = new int[layers, mapHeight, mapWidth];

            for (int z = 0; z < layers; z++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    for (int x = 0; x < mapWidth; x++)
                    {
                        tempArray[z, y, x] = jaggedArray[z][y][x];
                    }
                }
            }

            return tempArray;
        }
    }
}
