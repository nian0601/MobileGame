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
        public static TileData[, ,] LoadedLevelArray
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
        }

        public static void LoadLevel(string MapName)
        {
            string LoadPath = LevelDirectoryPath + @"\" + MapName;

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

            MapManager.BuildMap(LoadedLevelArray);
        }

        public static void SaveLevel(Tile[,,] LevelArray, int MapHeight, int MapWidth, string LevelName)
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

            TileData[, ,] ArrayToSave = ConvertToTileDataArray(LevelArray);

            //Get all the necessary LevelData
            LevelData LevelData = new LevelData();
            LevelData.TileSize = MapManager.TileSize;
            LevelData.LevelArray = ConvertToJaggedArray(ArrayToSave);
            LevelData.MapHeight = MapHeight;
            LevelData.MapWidth = MapWidth;

            //And now its time to build the SavePath
            //The new level we want to save should get number "MapList.Count + 1"
            //So if the MapList is empty, that is we have no maps, 
            //then the new map we save will get number "0 + 1 = 1"
            //If there are 8 maps in the list then the new map will get number "8 + 1 = 9"
            string savePath = LevelDirectoryPath + @"\" + LevelName;
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

        private static TileData[][][] ConvertToJaggedArray(TileData[, ,] multiArray)
        {
            int layers = multiArray.GetLength(0);
            int mapHeight = multiArray.GetLength(1);
            int mapWidth = multiArray.GetLength(2);

            TileData[][][] tempJaggedArray = new TileData[layers][][];

            for (int z = 0; z < layers; z++)
            {
                tempJaggedArray[z] = new TileData[mapHeight][];
                for (int y = 0; y < mapHeight; y++)
                {
                    tempJaggedArray[z][y] = new TileData[mapWidth];
                    for (int x = 0; x < mapWidth; x++)
                    {
                        tempJaggedArray[z][y][x] = multiArray[z, y, x];
                    }
                }
            }

            return tempJaggedArray;
        }

        private static TileData[, ,] ConvertToMultiArray(TileData[][][] jaggedArray)
        {
            int layers = jaggedArray.Length;
            int mapHeight = jaggedArray[0].Length;
            int mapWidth = jaggedArray[0][0].Length;

            TileData[, ,] tempArray = new TileData[layers, mapHeight, mapWidth];

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

        private static TileData[, ,] ConvertToTileDataArray(Tile[, ,] TileArray)
        {
            int layers = TileArray.GetLength(0);
            int yTiles = TileArray.GetLength(1);
            int xTiles = TileArray.GetLength(2);

            TileData[, ,] TileDataArray = new TileData[layers, yTiles, xTiles];

            for (int z = 0; z < layers; z++)
            {
                for (int y = 0; y < yTiles; y++)
                {
                    for (int x = 0; x < xTiles; x++)
                    {
                        TileData TempData = new TileData();
                        TempData.TileType = TileArray[z, y, x].TileType;
                        TempData.TileValue = TileArray[z, y, x].TileValue;
                        TempData.Collidable = TileArray[z, y, x].Collidable;
                        TempData.CanJumpThrough = TileArray[z, y, x].CanJumpThrough;

                        TileDataArray[z, y, x] = TempData;
                    }
                }
            }

            return TileDataArray;
        }
    }
}
