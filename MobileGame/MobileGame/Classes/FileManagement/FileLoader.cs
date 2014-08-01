using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Xna.Framework.Storage;

using MobileGame.Managers;

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
            //Get the path to the map we want to load
            string LoadPath = LevelDirectoryPath + @"\" + gameData.MapList[LevelNumber-1];
            //Open the stream
            FileStream stream = File.Open(LoadPath, FileMode.Open);

            //And then we do the serializing process
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
                LoadedLevel = (LevelData)serializer.Deserialize(stream);
            }
            finally
            {
                //Before closing the stream we set the Player to null
                //This forces the GameManager to create a new player object which
                //will make sure that the player gets spawnd with all the correct values on the new map
                GameManager.Player = null;
                stream.Close();
            }
        }

        public static void UpdateGameData()
        {
            //Instansiate a new GameData object that we later will serialize
            GameData GameData = new GameData();

            //If the MapListCount is greater then the current mapvalue that means have more maps to play
            if (gameData.MapList.Count > gameData.CurrentMap)
            {
                //So we set AllMapsDone to FALSE
                GameData.AllMapsDone = false;
                //And set LastMapPlayed to the next level (increment it with one)
                GameData.CurrentMap = gameData.CurrentMap + 1;
            }
            //and if it isnt then we just played the last map in the game
            else
            {   
                //So we set AllMapsDone to true
                GameData.AllMapsDone = true;
                //And keep the last level as the lastMapPlayed
                GameData.CurrentMap = gameData.CurrentMap;
            }

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

        public static void ResetSaveFile()
        {
            //Instansiate a new GameData object that we later will serialize
            GameData GameData = new GameData();

            GameData.AllMapsDone = false;
            GameData.CurrentMap = 1;

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

            Console.WriteLine("Reset Save File! (CurrentMap = 1, AllMapsDone = false)");
        }

        //Because reasons you cant serialize a multidimentional-array ( [, , ] ), but you can serialize a jagged array ( [][][] )
        //So we use these helperfunction to convert between the two
        //Convert to jaggedArray when we want to save data and convert to MultiArray when we want to load data
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
