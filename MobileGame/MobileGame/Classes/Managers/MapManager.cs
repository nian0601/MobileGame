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
        private static int[, ,] currentMap;
        private static Tile[, ,] tileArray;
        private static int tileSize;
        private static int mapWidth;
        private static int mapHeight;

        private List<SimpleTile> platformList;
        private List<SpecialTile> specialBlockList;
        private int mapYTiles;
        private int mapXTiles;
        private Vector2 playerStartPos;

        internal GraphicsDevice graphicsDevice;

        #region Properties

        /// <summary>
        /// This array is used in the loading and building of maps and contains the 1's and 0's that gets converted into tiles in the BuildMap-function
        /// </summary>
        public int[, ,] CurrentMap
        {
            get { return currentMap; }
        }

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

        #endregion

        public MapManager()
        {
            platformList = new List<SimpleTile>();
            specialBlockList = new List<SpecialTile>();
            playerStartPos = new Vector2(200, 200);
        }

        public void Initialize()
        {
            tileSize = FileLoader.LoadedLevelTileSize;
            currentMap = FileLoader.LoadedLevelArray;
            mapYTiles = FileLoader.LoadedLevelMapHeight;
            mapXTiles = FileLoader.LoadedLevelMapWidth;

            mapHeight = mapYTiles * tileSize;
            mapWidth = mapXTiles * tileSize;

            platformList.Clear();
            specialBlockList.Clear();

            BuildLevel(currentMap);
        }

        //This entire function should not be in the final game, we do not want the player to be able to draw on the map while playing
        public void Update()
        {
            Vector2 mousePos = Camera.ScreenToWorldCoordinates(new Vector2(KeyMouseReader.mousePos.X, KeyMouseReader.mousePos.Y));

            int mouseX = (int)ConvertPixelsToIndex(mousePos).X;
            int mouseY = (int)ConvertPixelsToIndex(mousePos).Y;

            if (KeyMouseReader.isKeyDown(Keys.LeftAlt) && KeyMouseReader.isKeyDown(Keys.LeftShift) && KeyMouseReader.LeftClick())
                EnemyManager.AddEnemy(new SimpleEnemy(mouseX, mouseY, true));
            else if (KeyMouseReader.isKeyDown(Keys.LeftShift) &&  KeyMouseReader.LeftClick())
                EnemyManager.AddEnemy(new SimpleEnemy(mouseX, mouseY, false));

            if (KeyMouseReader.isKeyDown(Keys.LeftShift) && KeyMouseReader.MouseWheelDown())
                RemoveSimpleTile(mouseX, mouseY);

            else if (KeyMouseReader.MouseWheelDown())
                CreateSimpleTile(mouseX, mouseY);

            if (KeyMouseReader.isKeyDown(Keys.LeftShift) && KeyMouseReader.KeyClick(Keys.F))
                Camera.RemoveFocusObject(FindTileAtIndex(mouseX, mouseY));
            else if (KeyMouseReader.KeyClick(Keys.F))
                Camera.AddFocusObject(FindTileAtIndex(mouseX, mouseY));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < mapXTiles; x++)
            {
                for (int y = 0; y < mapYTiles; y++)
                {
                    tileArray[0, x, y].Draw(spriteBatch);
                }
            }

            foreach (Tile T in specialBlockList)
                T.Draw(spriteBatch);
        }

        /// <summary>
        /// Use this to generate a list with all the tiles surrounding a certain pixelpos (playerpos, enemypos etc)
        /// </summary>
        /// <param name="x">The x-value</param>
        /// <param name="y">The y-value</param>
        /// <param name="xRange">The amount of tiles on the x-axis we want to include</param>
        /// <param name="yRange">The amount of tiles on the y-axis we want to include</param>
        /// <returns></returns>
        public static List<Tile> GenerateCollisionList(int x, int y, int xRange, int yRange)
        {
            //Create a tempList which will hold the tiles we wanna send back
            List<Tile> tempList = new List<Tile>();

            //Convert the given x and y values into a vector for convinence
            Vector2 originPoint = new Vector2(x, y);
            //Convert the pixelposition into indexposition
            Vector2 index = ConvertPixelsToIndex(originPoint);

            //Then we get all the surrounding tiles, going xRange-num tiles to the left and right aswell as going yRange-num tiles up and down
            tempList = FindSurroundingTiles(index, xRange, yRange);

            for (int i = tempList.Count - 1; i > 0; i--)
            {
                if (!tempList[i].collidable)
                    tempList.RemoveAt(i);
            }

            //If the game is in debugmode we simply color the tiles we have found red
            //(the tile-class makes sure that the tile gets their normal color if they arent in this list)
            if (Game1.Debugging)
            {
                foreach (Tile t in tempList)
                {
                    if(t.canJumpThrough)
                        t.Color = Color.Red;
                }
            }
            

            return tempList;
        }

        private void BuildLevel(int[, ,] level)
        {
            tileArray = new Tile[2, mapXTiles, mapYTiles];

            //LOOPS THROUGH THE PLATFORM LAYER
            for (int x = 0; x < mapXTiles; x++)
            {
                for (int y = 0; y < mapYTiles; y++)
                {
                    int tileType = level[0, x, y];

                    //If the tiletype is 9 that means we found the place where the player should spawn
                    //So we do some stuff that we dont do with the normal tiles and then continue on with the loops
                    if (tileType == 9)
                    {
                        playerStartPos = ConvertIndexToPixels(x, y);
                        tileArray[0, x, y] = new SimpleTile(x, y, 0);
                        continue;
                    }

                    //We create an empty simpletile before we check tiletypes
                    //Then we define the simpleTile after the tileType checks
                    SimpleTile tempTile;
                    if (tileType != 0)
                    {
                        tempTile = new SimpleTile(x, y, tileType);
                        platformList.Add(tempTile);
                    }
                    else
                    {
                        tempTile = new SimpleTile(x, y, tileType);
                    }
                    
                    //And adds it to the levelarray
                    tileArray[0, x, y] = tempTile;
                }
            }

            //LOOPS THROUGH THE SPECIAL BLOCK'S LAYER
            for (int x = 0; x < mapXTiles; x++)
            {
                for (int y = 0; y < mapYTiles; y++)
                {
                    int tileType = level[1, x, y];

                    if (tileType == 1)
                        specialBlockList.Add(new JumpTile(x, y));
                    else if (tileType == 2)
                        specialBlockList.Add(new TeleportTile(x, y));
                    else if (tileType == 3)
                    {
                        GoalTile temp = new GoalTile(x, y);
                        specialBlockList.Add(temp);
                        Camera.AddFocusObject(temp);
                    }

                    else if (tileType == 4)
                        EnemyManager.AddEnemy(new SimpleEnemy(x, y, false));
                }
            }

            //To make sure that all the tiles get their correct texture we have to wait until all the tiles have been added
            //and then loop through them all and set TileTextures
            foreach (SimpleTile Tile in platformList)
                Tile.SetTileBitType(CalculateTileValue((int)Tile.IndexPos.X, (int)Tile.IndexPos.Y));

            #region Performance Check Code
            //PERFORMANCE CHECK!!
            //for (int i = 0; i < 1000; i++)
            //{
            //    TimeSpan begin = Process.GetCurrentProcess().TotalProcessorTime;
            //    Stopwatch watch = new Stopwatch();
            //    watch.Start();

            //    foreach (SimpleTile Tile in colliderList)
            //        AssignTileType(Tile);

            //    watch.Stop();
            //    TimeSpan end = Process.GetCurrentProcess().TotalProcessorTime;

            //    //Console.WriteLine("Measured time: " + watch.ElapsedMilliseconds + " ms.");
            //    Console.WriteLine("Measured time: " + (end - begin).TotalMilliseconds + " ms.");
            //}
            #endregion
        }

        /// <summary>
        /// Simple Helperfunction that takes an index and converts it into pixels
        /// </summary>
        public Vector2 ConvertIndexToPixels(int X, int Y)
        {
            int x = X * tileSize;
            int y = Y * tileSize;

            return new Vector2(x, y);
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
        /// Simple helperfunction that finds the tile at an given index
        /// </summary>
        private Tile FindTileAtIndex(int x, int y)
        {
            return tileArray[0, x, y];
        }

        /// <summary>
        /// This function takes the index of a tile and calulates its value based on its neighbours, used to set the tiletextures
        /// </summary>
        private int CalculateTileValue(int x, int y)
        {
            int TileValue = 0;

            //NORTH
            if (y - 1 >= 0)
            {
                Tile northTile = FindTileAtIndex(x, y - 1);

                if (platformList.Contains(northTile))
                    TileValue += 1;
            }

            //EAST
            if (x + 1 <= mapXTiles - 1)
            {
                Tile eastTile = FindTileAtIndex(x + 1, y);

                if (platformList.Contains(eastTile))
                    TileValue += 2;
            }

            //SOUTH
            if (y + 1 <= mapYTiles - 1)
            {
                Tile southTile = FindTileAtIndex(x, y + 1);

                if (platformList.Contains(southTile))
                    TileValue += 4;
            }

            //WEST
            if (x - 1 >= 0)
            {
                Tile westTile = FindTileAtIndex(x - 1, y);

                if (platformList.Contains(westTile))
                    TileValue += 8;
            }
            return TileValue;
        }

        /// <summary>
        /// Simple helperfunction that creates a simpletile (a platform) at a given index
        /// </summary>
        private void CreateSimpleTile(int x, int y)
        {
            SimpleTile tempTile = new SimpleTile(x, y, 1);

            bool newTile = true;
            Vector2 tempVector = new Vector2(x, y);

            //This is to make sure that we only add ONE tile to the colliderList
            //Is needed if we want it to be possible to "draw" the map, i.e not having to click each tile
            for (int i = 0; i < platformList.Count; i++)
                if (platformList[i].IndexPos == tempVector)
                    newTile = false;

            if (newTile)
            {
                tileArray[0, x, y] = tempTile;
                platformList.Add(tempTile);

                tempTile.SetTileBitType(CalculateTileValue((int)tempTile.IndexPos.X, (int)tempTile.IndexPos.Y));

                List<Tile> tempTileList = FindSurroundingTiles(tempTile.IndexPos, 1, 1);
                foreach (Tile T in tempTileList)
                    T.SetTileBitType(CalculateTileValue((int)T.IndexPos.X, (int)T.IndexPos.Y));

                currentMap[0, x, y] = 1;
            }

        }

        /// <summary>
        /// A simple helperfunction that removes a simpletile (platform) at a given index
        /// </summary>
        private void RemoveSimpleTile(int x, int y)
        {
            tileArray[0, x, y] = new SimpleTile(x, y, 0);
            currentMap[0, x, y] = 0;

            Vector2 tempVector = new Vector2(x, y);

            for (int i = 0; i < platformList.Count; i++)
            {
                if (platformList[i].IndexPos == tempVector)
                {
                    List<Tile> tempTileList = FindSurroundingTiles(platformList[i].IndexPos, 1, 1);
                    foreach (Tile T in tempTileList)
                        T.SetTileBitType(CalculateTileValue((int)T.IndexPos.X, (int)T.IndexPos.Y));


                    platformList.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// A function that takes a index and then returns all the surrounding simpletiles(platforms) within the defined ranges
        /// </summary>
        /// <param name="centerIndex"></param>
        private static List<Tile> FindSurroundingTiles(Vector2 centerIndex, int xRange, int yRange)
        {
            List<Tile> tempList = new List<Tile>();

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
                    if (IsXInsideArray(currentX, tileArray) && IsYInsideArray(currentY, tileArray))
                    {
                        if (tileArray[0, currentX, currentY].shouldDraw)
                            tempList.Add(tileArray[0, currentX, currentY]);      
                    }
                }
            }
            #endregion

            return tempList;
        }

        /// <summary>
        /// A simple helperfunction that checks if a given value is inside the y-bounds of the given array
        /// </summary>
        private static bool IsYInsideArray(int y, Tile[, ,] array)
        {
            if (y < 0 || y > array.GetUpperBound(2))
                return false;
            return true;
        }
        
        /// <summary>
        /// A simple helperfunction that checks if a given value is inside the x-bounds of the given array
        /// </summary>
        private static bool IsXInsideArray(int x, Tile[, ,] array)
        {
            if (x < 0 || x > array.GetUpperBound(1))
                return false;
            return true;
        }
    }
}
