using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

using MobileGame.FileManagement;
using MobileGame.CameraManagement;
using MobileGame.Tiles;
using MobileGame.Enemies;

namespace MobileGame.Managers
{
    class MapManager
    {
        private static byte[,] collisionLayer;
        private static byte[,] backgroundLayer;
        private static byte[,] platformLayer;
        private static byte[,] specialsLayer;

        private static int tileSize;
        private static int mapWidth;
        private static int mapHeight;

        private List<SpecialTile> specialBlockList;
        private int mapYTiles;
        private int mapXTiles;
        private Vector2 playerStartPos;

        internal GraphicsDevice graphicsDevice;

        #region Properties

        public List<SpecialTile> SpecialBlocksList
        {
            get { return specialBlockList; }
        }

        public Vector2 PlayerStartPos
        {
            get { return playerStartPos; }
        }

        public static int MapWidth
        {
            get { return mapWidth; }
        }

        public static int MapHeight
        {
            get { return mapHeight; }
        }

        public static int TileSize { get { return tileSize; } }

        public static byte[,] CollisionLayer { get { return collisionLayer; } }

        public static Vector2 GoalPos { get; private set; }

        #endregion

        public MapManager()
        {
            specialBlockList = new List<SpecialTile>();
            playerStartPos = new Vector2(200, 200);
        }

        public void Initialize()
        {
            tileSize = FileLoader.LoadedLevelTileSize;
            mapYTiles = FileLoader.LoadedLevelMapHeight;
            mapXTiles = FileLoader.LoadedLevelMapWidth;

            mapHeight = mapYTiles * tileSize;
            mapWidth = mapXTiles * tileSize;

            //PerformanceCheck();

            BuildLevel();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < mapXTiles; x++)
            {
                for (int y = 0; y < mapYTiles; y++)
                {
                    Point pixelIndex = ConvertIndexToPixels(x, y);
                    Color Color = Color.White;

                    if (Game1.Debugging)
                    {
                        if (collisionLayer[x, y] == 1)
                            Color = Color.PaleVioletRed;
                    }

                    Vector2 Pos = new Vector2(x * tileSize, y * tileSize);
                    Texture2D Texture;

                    byte value = backgroundLayer[x, y];
                    if (value != 255)
                    {
                        Texture = TextureManager.GameTextures[value];
                        spriteBatch.Draw(Texture, Pos, null, Color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.15f);
                    }

                    value = platformLayer[x, y];
                    if (value != 255)
                    {
                        Texture = TextureManager.GameTextures[value];
                        spriteBatch.Draw(Texture, Pos, null, Color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.25f);
                    }

                    value = specialsLayer[x, y];
                    if (value != 255)
                    {
                        Texture = TextureManager.GameTextures[value];
                        spriteBatch.Draw(Texture, Pos, null, Color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
                    }
                }
            }
        }

        /// <summary>
        /// Use this to generate rectangles for all collisionflags within a certain range of a point in the game
        /// </summary>
        /// <param name="x">The x-value</param>
        /// <param name="y">The y-value</param>
        /// <param name="xRange">The amount of tiles on the x-axis we want to include</param>
        /// <param name="yRange">The amount of tiles on the y-axis we want to include</param>
        /// <returns></returns>
        public static List<Rectangle> GenerateCollisionList(int x, int y, int xRange, int yRange)
        {
            //Create a temp list which will hold all the indices that we want to build rects for
            List<Vector2> tempList = new List<Vector2>();

            //Create a list that will hold all the rects we wanna send back
            List<Rectangle> RectList = new List<Rectangle>();

            //Convert the given x and y values into a vector for convinence
            Vector2 originPoint = new Vector2(x, y);
            //Convert the pixelposition into indexposition
            Vector2 index = ConvertPixelsToIndex(originPoint);

            //Then we get all the surrounding tiles, going xRange-num tiles to the left and right aswell as going yRange-num tiles up and down
            tempList = FindSurroundingCollisionFlags(index, xRange, yRange);

            foreach (Vector2 Index in tempList)
            {
                Rectangle temp = new Rectangle((int)Index.X * tileSize, (int)Index.Y * tileSize, tileSize, tileSize);
                RectList.Add(temp);
            }

            return RectList;
        }

        /// <summary>
        /// The function which takes the loaded level and builds it
        /// </summary>
        private void BuildLevel()
        {
            collisionLayer = new byte[mapXTiles, mapYTiles];
            backgroundLayer = new byte[mapXTiles, mapYTiles];
            platformLayer = new byte[mapXTiles, mapYTiles];
            specialsLayer = new byte[mapXTiles, mapYTiles];

            specialBlockList.Clear();

            byte backgroundValue, platformValue, specialsValue;

            //LOOPS THROUGH THE PLATFORM LAYER
            for (int x = 0; x < mapXTiles; x++)
            {
                for (int y = 0; y < mapYTiles; y++)
                {
                    collisionLayer[x, y] = FileLoader.LoadedCollisionLayer[x, y];

                    backgroundValue = FileLoader.LoadedBackgroundLayer[x, y];
                    platformValue = FileLoader.LoadedPlatformLayer[x, y];
                    specialsValue = FileLoader.LoadedSpecialsLayer[x, y];

                    #region BackgroundLayer
                    if (backgroundValue == 16)
                    {
                        specialBlockList.Add(new JumpTile(x, y));
                        backgroundLayer[x, y] = backgroundValue;
                    }
                    
                    else if (backgroundValue == 17)
                    {
                        GoalTile temp = new GoalTile(x, y);
                        Camera.AddFocusObject(temp);
                        specialBlockList.Add(temp);
                        backgroundLayer[x, y] = backgroundValue;
                        GoalPos = new Vector2(x * tileSize, y * tileSize);
                    }
                    else if (backgroundValue == 29)
                    {
                        playerStartPos = new Vector2(x * tileSize, y * tileSize);
                        backgroundLayer[x, y] = 255;
                    }
                    else if (backgroundValue == 30)
                    {
                        EnemyManager.AddEnemy(new SimpleEnemy(x, y, false));
                        backgroundLayer[x, y] = 255;
                    }
                    else if (backgroundValue == 31)
                    {
                        specialBlockList.Add(new TeleportTile(x, y));
                        backgroundLayer[x, y] = backgroundValue;
                    }
                    else
                        backgroundLayer[x, y] = backgroundValue;
                    #endregion

                    #region PlatformLayer
                    if (platformValue == 16)
                    {
                        specialBlockList.Add(new JumpTile(x, y));
                        platformLayer[x, y] = platformValue;
                    }
                    else if (platformValue == 31)
                    {
                        specialBlockList.Add(new TeleportTile(x, y));
                        platformLayer[x, y] = platformValue;
                    }
                    else if (platformValue == 17)
                    {
                        GoalTile temp = new GoalTile(x, y);
                        Camera.AddFocusObject(temp);
                        specialBlockList.Add(temp);
                        platformLayer[x, y] = platformValue;
                        GoalPos = new Vector2(x * tileSize, y * tileSize);
                    }
                    else if (platformValue == 29)
                    {
                        playerStartPos = new Vector2(x * tileSize, y * tileSize);
                        platformLayer[x, y] = 255;
                    }
                    else if (platformValue == 30)
                    {
                        EnemyManager.AddEnemy(new SimpleEnemy(x, y, false));
                        platformLayer[x, y] = 255;
                    }
                    else
                        platformLayer[x, y] = platformValue;
                    #endregion

                    #region SpecialsLayer
                    if (specialsValue == 16)
                    {
                        specialBlockList.Add(new JumpTile(x, y));
                        specialsLayer[x, y] = specialsValue;
                    }
                    else if (specialsValue == 31)
                    {
                        specialBlockList.Add(new TeleportTile(x, y));
                        specialsLayer[x, y] = specialsValue;
                    }
                    else if (specialsValue == 17)
                    {
                        GoalTile temp = new GoalTile(x, y);
                        Camera.AddFocusObject(temp);
                        specialBlockList.Add(temp);
                        specialsLayer[x, y] = specialsValue;
                        GoalPos = new Vector2(x * tileSize, y * tileSize);
                    }
                    else if (specialsValue == 29)
                    {
                        playerStartPos = new Vector2(x * tileSize, y * tileSize);
                        specialsLayer[x, y] = 255;
                    }
                    else if (specialsValue == 30)
                    {
                        EnemyManager.AddEnemy(new SimpleEnemy(x, y, false));
                        specialsLayer[x, y] = 255;
                    }
                    else
                        specialsLayer[x, y] = specialsValue;
                    #endregion

                }
            }

            
        }

        /// <summary>
        /// Simple Helperfunction that takes an index and converts it into pixels
        /// </summary>
        public Point ConvertIndexToPixels(int X, int Y)
        {
            int x = X * tileSize;
            int y = Y * tileSize;

            return new Point(x, y);
        }

        /// <summary>
        /// A simple helperfunction that takes an vector (pixelpos) and converts it into an index
        /// </summary>
        public static Vector2 ConvertPixelsToIndex(Vector2 pos)
        {
            int x = (int)pos.X / tileSize;
            int y = (int)pos.Y / tileSize;

            return new Vector2(x, y);
        }

        /// <summary>
        /// A function that takes a index and then returns all the surrounding indices which are flagged for collision
        /// </summary>
        /// <param name="centerIndex"></param>
        private static List<Vector2> FindSurroundingCollisionFlags(Vector2 centerIndex, int xRange, int yRange)
        {
            List<Vector2> tempList = new List<Vector2>();

            int currentY = 0;
            int currentX = 0;
            //We use the xRange and yRange variables to determine how many tiles we loop through.
            //we start at -xRange and -yRange and loop all the way to xRange(positive value now) and yRange(positive value aswell)

            //So if xRange gets the value of 5, we will loop from -5 to 5 in the xLoop
            //and if yRange gets the value of 3, we will loop from -3 to 3 in the yLoop

            //This functionality makes the function usuable in multiple situations.
            //Its used to find the tiles directly connected to a certain tile, by setting both ranges to 1, used to set tiletypes
            //and its also used to generate CollisionLists, by sending in the position of the player and a larger range, like 5 or so

            //If we find a tile that is air or a tile that is outside the arrayIndex we simply ignore it
            //If we find a platformtile we add it to the list
            #region Loop
            for (int x = -xRange; x <= xRange; x++)
            {
                currentX = (int)centerIndex.X + x;

                for (int y = -yRange; y <= yRange; y++)
                {
                    currentY = (int)centerIndex.Y + y;

                    //Is the tile we are looking at inside the map-array?
                    if (IsXInsideArray(currentX, collisionLayer) && IsYInsideArray(currentY, collisionLayer))
                    {
                        if (collisionLayer[currentX, currentY] != 0)
                            tempList.Add(new Vector2(currentX, currentY));  
                    }
                }
            }
            #endregion

            return tempList;
        }

        /// <summary>
        /// A simple helperfunction that checks if a given value is inside the y-bounds of the given array
        /// </summary>
        private static bool IsYInsideArray(int y, byte[,] array)
        {
            if (y < 0 || y > array.GetUpperBound(1))
                return false;
            return true;
        }
        
        /// <summary>
        /// A simple helperfunction that checks if a given value is inside the x-bounds of the given array
        /// </summary>
        private static bool IsXInsideArray(int x, byte[,] array)
        {
            if (x < 0 || x > array.GetUpperBound(0))
                return false;
            return true;
        }



        private void PerformanceCheck()
        {
            for (int i = 0; i < 1000; i++)
            {
                TimeSpan begin = Process.GetCurrentProcess().TotalProcessorTime;
                Stopwatch watch = new Stopwatch();
                watch.Start();

                //PASTE WHAT EVER CODE YOU WANNA TEST HERE

                BuildLevel();

                //AND STOP HERE

                watch.Stop();
                TimeSpan end = Process.GetCurrentProcess().TotalProcessorTime;

                //Console.WriteLine("Measured time: " + watch.ElapsedMilliseconds + " ms.");
                Console.WriteLine("Measured time: " + (end - begin).TotalMilliseconds + " ms.");
            }
        }
    }
}
