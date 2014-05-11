using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Xna.Framework.Storage;

namespace MobileGame.LevelManagement
{
    static class LevelLoader
    {
        private static string LevelDirectoryPath = @"C:\Users\" + Environment.UserName + @"\MobileGame\Levels";
        private static LevelData LoadedLevel;

        public static int[, ,] LoadedLevelArray
        {
            get { return ConvertToMultiArray(LoadedLevel.LevelArray); }
        }

        public static int LoadedLevelTileSize
        {
            get { return LoadedLevel.TileSize; }
        }

        public static void Load()
        {
            string savePath = LevelDirectoryPath + @"\LevelOne";

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

        public static void Save(int[,,] LevelArray)
        {
            System.IO.Directory.CreateDirectory(LevelDirectoryPath);

            LevelData LevelData = new LevelData();
            LevelData.LevelArray = ConvertToJaggedArray(LevelArray);
            LevelData.TileSize = TextureManager.TileSize;

            Console.WriteLine(LevelData.LevelArray[1][0].Length);

            string savePath = LevelDirectoryPath + @"\Level1";

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
