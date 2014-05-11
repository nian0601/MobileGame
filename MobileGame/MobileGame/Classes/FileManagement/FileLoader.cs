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
        public static int[, ,] LoadedLevelArray
        {
            get { return ConvertToMultiArray(LoadedLevel.LevelArray); }
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
            string savePath = LevelDirectoryPath + @"\Level" + LevelNumber.ToString();

            FileStream stream = File.Open(savePath, FileMode.Open);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
                LoadedLevel = (LevelData)serializer.Deserialize(stream);
            }
            finally
            {
                stream.Close();
            }
        }

        public static void SaveLevel(int[,,] LevelArray)
        {
            System.IO.Directory.CreateDirectory(LevelDirectoryPath);

            LevelData LevelData = new LevelData();
            LevelData.LevelArray = ConvertToJaggedArray(LevelArray);
            LevelData.TileSize = TextureManager.TileSize;

            string savePath = LevelDirectoryPath + @"\Level3";

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
        }

        public static void UpdateGameData()
        {
            //We get the filePath to the nextlevel (if there is one)
            string levelPath = LevelDirectoryPath + @"\Level" + (gameData.CurrentMap + 1).ToString();

            //Instansiate a new GameData object that we later will serialize
            GameData GameData = new GameData();
            
            //If the file for the next level does not exist that means we have ran out of levels
            if (!File.Exists(levelPath))
            {
                //So we set AllMapsDone to true
                GameData.AllMapsDone = true;
                //And keep the last level as the lastMapPlayed
                GameData.CurrentMap = gameData.CurrentMap;
            }
            //If the file for the next level DOES exist that means we have more levels to play
            else
            {
                //So we set AllMapsDone to FALSE
                GameData.AllMapsDone = false;
                //And set LastMapPlayed to the next level (increment it with one)
                GameData.CurrentMap = gameData.CurrentMap + 1;
            }

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
