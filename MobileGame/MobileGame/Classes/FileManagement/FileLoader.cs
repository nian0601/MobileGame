using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Xna.Framework.Storage;

using MobileGame.Managers;
using MobileGame.LevelEditor;
using MobileGame.Lights;

namespace MobileGame.FileManagement
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

        public static float[,] LoadedPointLights
        {
            get { return ConvertToMultiArray(LoadedLevel.PointLights); }
        }

        public static float[,] LoadedAmbientLights
        {
            get { return ConvertToMultiArray(LoadedLevel.AmbientLights); }
        }


        public static int LoadedLevelTileSize{ get { return LoadedLevel.TileSize; } }

        public static int LoadedLevelMapHeight { get { return LoadedLevel.MapHeight; } }

        public static int LoadedLevelMapWidth { get { return LoadedLevel.MapWidth; } }

        public static int LoadedLevelNumPointLights { get { return LoadedLevel.NumPointLights; } }

        public static int LoadedLevelNumAmbientLights { get { return LoadedLevel.NumAmbientLights; } }

        public static GameData LoadedGameData { get { return gameData; } }
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

            LoadLevel(gameData.CurrentMap);
        }

        public static void LoadLevel(int LevelNumber)
        {
            //Get the path to the map we want to load
            string LoadPath = LevelDirectoryPath + @"\" + gameData.MapList[LevelNumber-1] + ".xml";
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
            int MapHeight = EditorMapManager.NumYTiles;
            int MapWidth = EditorMapManager.NumXTiles;

            byte[,] CollisionLayer = EditorMapManager.CollisionLayer;
            byte[,] BackgroundLayer = EditorMapManager.BackgroundLayer;
            byte[,] PlatformLayer = EditorMapManager.PlatformLayer;
            byte[,] SpecialsLayer = EditorMapManager.SpecialsLayer;

            float[,] PointLights = new float[LightingManager.PointLights.Count, 7];

            for (int y = 0; y < LightingManager.PointLights.Count; y++)
            {
                PointLights[y, 0] = LightingManager.PointLights[y].Position.X;
                PointLights[y, 1] = LightingManager.PointLights[y].Position.Y;
                PointLights[y, 2] = LightingManager.PointLights[y].Radius;
                PointLights[y, 3] = LightingManager.PointLights[y].Power;
                PointLights[y, 4] = LightingManager.PointLights[y].Color.R;
                PointLights[y, 5] = LightingManager.PointLights[y].Color.G;
                PointLights[y, 6] = LightingManager.PointLights[y].Color.B;
            }

            float[,] AmbientLights = new float[LightingManager.AmbientLights.Count, 8];

            for (int y = 0; y < LightingManager.AmbientLights.Count; y++)
            {
                AmbientLights[y, 0] = LightingManager.AmbientLights[y].Position.X;
                AmbientLights[y, 1] = LightingManager.AmbientLights[y].Position.Y;
                AmbientLights[y, 2] = LightingManager.AmbientLights[y].Width;
                AmbientLights[y, 3] = LightingManager.AmbientLights[y].Height;
                AmbientLights[y, 4] = LightingManager.AmbientLights[y].Color.R;
                AmbientLights[y, 5] = LightingManager.AmbientLights[y].Color.G;
                AmbientLights[y, 6] = LightingManager.AmbientLights[y].Color.B;
                AmbientLights[y, 7] = LightingManager.AmbientLights[y].Power;
            }


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
            LevelData.TileSize = 20;
            LevelData.MapHeight = MapHeight;
            LevelData.MapWidth = MapWidth;
            LevelData.CollisionLayer = ConvertToJaggedArray(CollisionLayer);
            LevelData.BackgroundLayer = ConvertToJaggedArray(BackgroundLayer);
            LevelData.PlatformLayer = ConvertToJaggedArray(PlatformLayer);
            LevelData.SpecialsLayer = ConvertToJaggedArray(SpecialsLayer);
            LevelData.PointLights = ConvertToJaggedArray(PointLights);
            LevelData.NumPointLights = LightingManager.PointLights.Count;
            LevelData.AmbientLights = ConvertToJaggedArray(AmbientLights);
            LevelData.NumAmbientLights = LightingManager.AmbientLights.Count;

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
            if (!TempGameData.MapList.Contains(LevelName))
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

            string savePath = SaveFilesDirectoryPath + @"\GameData.xml";

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

            string savePath = SaveFilesDirectoryPath + @"\GameData.xml";

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

        private static float[][] ConvertToJaggedArray(float[,] multiArray)
        {
            int mapHeight = multiArray.GetLength(0);
            int mapWidth = multiArray.GetLength(1);

            float[][] tempJaggedArray = new float[mapHeight][];

            for (int y = 0; y < mapHeight; y++)
            {
                tempJaggedArray[y] = new float[mapWidth];
                for (int x = 0; x < mapWidth; x++)
                {
                    tempJaggedArray[y][x] = multiArray[y, x];
                }
            }

            return tempJaggedArray;
        }

        private static float[,] ConvertToMultiArray(float[][] jaggedArray)
        {
            int mapHeight = jaggedArray.Length;
            int mapWidth = jaggedArray[0].Length;

            float[,] tempArray = new float[mapHeight, mapWidth];

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
