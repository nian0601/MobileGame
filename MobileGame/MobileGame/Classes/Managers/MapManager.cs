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
using MobileGame.LightingSystem;

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

        private List<SpecialTile> mySpecialTiles;
        private List<AnimatedTile> myAnimatedTiles;
        private int mapYTiles;
        private int mapXTiles;
        private Vector2 playerStartPos;

        internal GraphicsDevice graphicsDevice;

        #region Properties

        public List<SpecialTile> SpecialBlocksList
        {
            get { return mySpecialTiles; }
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

        #endregion

        public MapManager()
        {
            mySpecialTiles = new List<SpecialTile>();
            myAnimatedTiles = new List<AnimatedTile>();
            playerStartPos = new Vector2(200, 200);
        }

        public void Initialize(LightRenderer aLightRenderer)
        {
            tileSize = FileLoader.LoadedLevelTileSize;
            mapYTiles = FileLoader.LoadedLevelMapHeight;
            mapXTiles = FileLoader.LoadedLevelMapWidth;

            tileSize = 40;
            mapHeight = mapYTiles * tileSize;
            mapWidth = mapXTiles * tileSize;

            

            //PerformanceCheck();

            BuildLevel(aLightRenderer);
        }

        public void Update(float aElaspedTime)
        {
            int mouseX = KeyMouseReader.GetMousePos().X;
            int mouseY = KeyMouseReader.GetMousePos().Y;

            for (int i = 0; i < myAnimatedTiles.Count; i++)
            {
                myAnimatedTiles[i].Update(aElaspedTime);
            }
        }


        public void DrawBackground(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < mapXTiles; x++)
            {
                for (int y = 0; y < mapYTiles; y++)
                {
                    Color Color = Color.White;

                    if (Game1.Debugging)
                    {
                        if (collisionLayer[x, y] == 1)
                            Color = Color.PaleVioletRed;
                    }

                    byte value = backgroundLayer[x, y];
                    if (value != 255)
                    {
                        Vector2 Pos = new Vector2(x * tileSize, y * tileSize);
                        int sourceX = value % 8;
                        int sourceY = value / 8;
                        Rectangle sourceRect = new Rectangle(sourceX * tileSize, sourceY * tileSize, tileSize, tileSize);
                        //Rectangle sourceRect = new Rectangle(sourceX * 40, sourceY * 40, 40, 40);

                        spriteBatch.Draw(TextureManager.TileSet, Pos, sourceRect, Color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.15f);

                    }
                }
            }
        }

        public void DrawMiddle(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < mapXTiles; x++)
            {
                for (int y = 0; y < mapYTiles; y++)
                {
                    Color Color = Color.White;

                    if (Game1.Debugging)
                    {
                        if (collisionLayer[x, y] == 1)
                            Color = Color.PaleVioletRed;
                    }

                    byte value = platformLayer[x, y];
                    if (value != 255)
                    {
                        Vector2 Pos = new Vector2(x * tileSize, y * tileSize);
                        int sourceX = value % 8;
                        int sourceY = value / 8;
                        //Rectangle sourceRect = new Rectangle(sourceX * tileSize, sourceY * tileSize, tileSize, tileSize);
                        Rectangle sourceRect = new Rectangle(sourceX * 40, sourceY * 40, 40, 40);

                        spriteBatch.Draw(TextureManager.TileSet, Pos, sourceRect, Color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.25f);
                    }
                }
            }
        }

        public void DrawForeground(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < mapXTiles; x++)
            {
                for (int y = 0; y < mapYTiles; y++)
                {
                    Color Color = Color.White;

                    if (Game1.Debugging)
                    {
                        if (collisionLayer[x, y] == 1)
                            Color = Color.PaleVioletRed;
                    }


                    byte value = specialsLayer[x, y];
                    if (value != 255)
                    {
                        Vector2 Pos = new Vector2(x * tileSize, y * tileSize);
                        int sourceX = value % 8;
                        int sourceY = value / 8;
                        //Rectangle sourceRect = new Rectangle(sourceX * tileSize, sourceY * tileSize, tileSize, tileSize);
                        Rectangle sourceRect = new Rectangle(sourceX * 40, sourceY * 40, 40, 40);

                        spriteBatch.Draw(TextureManager.TileSet, Pos, sourceRect, Color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f); ;
                    }
                }
            }
        }

        public void DrawAnimatedTiles(SpriteBatch aSpriteBatch)
        {
            for (int i = 0; i < myAnimatedTiles.Count; i++)
            {
                myAnimatedTiles[i].Draw(aSpriteBatch);
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
            Point originPoint = new Point(x, y);
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
        private void BuildLevel(LightRenderer aLightRenderer)
        {
            collisionLayer = new byte[mapXTiles, mapYTiles];
            backgroundLayer = new byte[mapXTiles, mapYTiles];
            platformLayer = new byte[mapXTiles, mapYTiles];
            specialsLayer = new byte[mapXTiles, mapYTiles];

            mySpecialTiles.Clear();

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

                    if (backgroundValue == 25 || platformValue == 25 || specialsValue == 25) //JumpTile
                    {
                        mySpecialTiles.Add(new JumpTile(x, y));
                    }
                    else if (backgroundValue == 26 || platformValue == 26 || specialsValue == 26) //GoalTile
                    {
                        GoalTile temp = new GoalTile(x, y);
                        Camera.AddFocusObject(temp);
                        mySpecialTiles.Add(temp);
                        //LightingManager.PointLights.Add(new PointLight(new Vector2(x * TileSize, y * tileSize), 250, 0.7f, Color.White, false));
                    }
                    else if (backgroundValue == 27 || backgroundValue == 28 || backgroundValue == 29
                            || platformValue == 27 || platformValue == 28 || platformValue == 29
                            || specialsValue == 27 || specialsValue == 28 || specialsValue == 29) //SpikeTile
                    {
                        mySpecialTiles.Add(new SpikeTile(x, y));
                    }


                    if (backgroundValue == 41 || platformValue == 41 || specialsValue == 41) // Torch
                    {
                        myAnimatedTiles.Add(new AnimatedTile(x, y, TextureManager.BurningTorch, 0, 0, 5, 0, 100));
                        //LightingManager.PointLights.Add(new PointLight(new Vector2(x * TileSize, y * tileSize), 150, 0.7f, Color.White, true));
                        //Right now the torch is getting drawn two times:
                        //The first is a static image of the first frame
                        //The second is the acctual animation
                        //This is because i did not want to do alot of if-checks for each backgroundvalue/platformvalue/specialvalue
                        //and set the corresponding layer to 255 if a toirch was found
                        //should figure out something to do this in a pretty way later
                    }

                    backgroundLayer[x, y] = backgroundValue;
                    platformLayer[x, y] = platformValue;
                    specialsLayer[x, y] = specialsValue;

                }
            }

            for (int i = 0; i < FileLoader.LoadedLevelNumPointLights; i++)
            {
                float x = FileLoader.LoadedPointLights[i, 0];
                float y = FileLoader.LoadedPointLights[i, 1];
                float radius = FileLoader.LoadedPointLights[i, 2];
                float power = FileLoader.LoadedPointLights[i, 3];
                int r = (int)FileLoader.LoadedPointLights[i, 4];
                int g = (int)FileLoader.LoadedPointLights[i, 5];
                int b = (int)FileLoader.LoadedPointLights[i, 6];

                Color color = new Color(r, g, b);

                PointLight newLight = new PointLight(new Vector2(x, y), power, radius, color);
                aLightRenderer.pointLights.Add(newLight); 
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
        public static Vector2 ConvertPixelsToIndex(Point pos)
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

                //BuildLevel();

                //AND STOP HERE

                watch.Stop();
                TimeSpan end = Process.GetCurrentProcess().TotalProcessorTime;

                //Console.WriteLine("Measured time: " + watch.ElapsedMilliseconds + " ms.");
                Console.WriteLine("Measured time: " + (end - begin).TotalMilliseconds + " ms.");
            }
        }
    }
}
