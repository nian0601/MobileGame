using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace LevelEditor.Managers
{
    class MapManager
    {
        //This is used in the serializing process when we want to save the map
        //To reduce filesize we save the map as int and then rebuild the map when we load it the next time
        private int[, ,] currentMap;
        public int[, ,] CurrentMap
        {
            get { return currentMap; }
        }

        private Vector2 offset;
        public Vector2 Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        //This is used just to make it easier to display the map and wont be used in the serializing process
        private Tile[, ,] TileArray;

        private int Layers, MapHeight, MapWidth;
        public int layers { get { return Layers; } }
        public int mapHeight { get { return mapYTiles; } }
        public int mapWidth { get { return mapXTiles; } }
        public static int TileSize;

        private int editorXTiles, editorYTiles;
        private int mapXTiles, mapYTiles;
        private int xDisplayMin, xDisplayMax;
        private int yDisplayMin, yDisplayMax;
        private int xOffset, yOffset;

        #region ContentManager, SpriteBatch and Textures
        private ContentManager Content;
        private SpriteBatch Spritebatch;

        private Texture2D AirTexture;
        private Texture2D PlatformTexture;
        private Texture2D EnemyTexture;
        private Texture2D JumpTileTexture;
        private Texture2D TeleportTileTexture;
        private Texture2D GoalTexture;
        private Texture2D PlayerTexture;
        private Texture2D GridTexture;
        private Texture2D Background;

        public static List<Texture2D> TileTextures;
        public static List<Texture2D> TileBitTextures;

        public enum TileTypes
        {
            BottomClosedLeftCornerTile,
            BottomClosedRightCornerTile,
            BottomClosedTile,
            BottomLeftCorner,
            BottomLeftOpenTile,
            BottomLeftTile,
            BottomMiddleTile,
            BottomOpenTile,
            BottomRightCorner,
            BottomRightOpenTile,
            BottomRightTile,
            BottomTopOpenTile,
            FourCornersTile,
            LeftClosedBottomCornerTile,
            LeftClosedTile,
            LeftClosedTopCornerTile,
            LeftOpenTile,
            LeftRightOpenTile,
            MiddleLeftTile,
            MiddleRightTile,
            MiddleTile,
            RightClosedBottomCenterTile,
            RightClosedTile,
            RightClosedTopCenterTile,
            RightOpenTile,
            SingleTile,
            ThreeCornersBottomLeftTile,
            ThreeCornersBottomRightTile,
            ThreeCornersTopLeftTile,
            ThreeCornersTopRightTile,
            TopClosedLeftCornerTile,
            TopClosedrightCornerTile,
            TopClosedTile,
            TopLeftBottomRightCornerTile,
            TopLeftCorner,
            TopLeftOpenTile,
            TopLeftTile,
            TopMiddleTile,
            TopOpenTile,
            TopRightBottomLeftCornerTile,
            TopRightCorner,
            TopRightOpenTile,
            TopRightTile,
            TwoCornersBottomTile,
            TwoCornersLeftTile,
            TwoCornersRightTile,
            TwoCornersTopTile
        }

        #endregion

        private bool Initialized;
        private bool PlayerPlaced;
        private bool GoalPlaced;

        public MapManager(ContentManager Content, SpriteBatch spriteBatch)
        {
            this.Content = Content;
            this.Spritebatch = spriteBatch;

            Initialized = false;
            PlayerPlaced = false;
            GoalPlaced = false;

            Layers = 2;
            MapHeight = 15;
            MapWidth = 20;
            TileSize = 0;

            mapXTiles = 40;
            mapYTiles = 30;

            currentMap = new int[Layers, mapXTiles, mapYTiles];
            TileArray = new Tile[Layers, mapXTiles, mapYTiles];

            editorXTiles = 20;
            editorYTiles = 15;

            xDisplayMin = 0;
            xDisplayMax = editorXTiles;

            yDisplayMin = 0;
            yDisplayMax = editorYTiles;

            xOffset = 0;
            yOffset = 0;
        }

        public void Initialize()
        {
            if (!Initialized)
            {
                AirTexture = Content.Load<Texture2D>("GameTextures/AirTile");
                PlatformTexture = Content.Load<Texture2D>("GameTextures/PlatformTile");
                EnemyTexture = Content.Load<Texture2D>("GameTextures/SmallerEnemy");
                JumpTileTexture = Content.Load<Texture2D>("GameTextures/JumpTile");
                TeleportTileTexture = Content.Load<Texture2D>("GameTextures/TeleportTile");
                GoalTexture = Content.Load<Texture2D>("GameTextures/GoalTile");
                PlayerTexture = Content.Load<Texture2D>("GameTextures/SmallerPlayer");
                GridTexture = Content.Load<Texture2D>("Editor/GridTexture");
                Background = Content.Load<Texture2D>("Editor/Background");

                TileSize = AirTexture.Width;

                LoadTileTextures();

                ResetMap();

                Initialized = true;
            }
        }

        public void Update()
        {
            if (MapBounds().Contains(KeyMouseReader.mousePos))
            {
                int mouseX = ConvertPixelsToIndex(KeyMouseReader.mousePos).X;
                int mouseY = ConvertPixelsToIndex(KeyMouseReader.mousePos).Y;

                mouseX += xOffset;
                mouseY += yOffset;

                if (KeyMouseReader.LeftMouseDown())
                    CreatePlatform(mouseX, mouseY);
                else if (KeyMouseReader.RightMouseDown())
                {
                    if (TileArray[1, mouseX, mouseY] is GoalTile)
                        GoalPlaced = false;

                    if (TileArray[0, mouseX, mouseY] is PlayerTile)
                        PlayerPlaced = false;

                    CreateAir(mouseX, mouseY);
                }
                else if (KeyMouseReader.KeyClick(Keys.J))
                    CreateJumpTile(mouseX, mouseY);
                else if (KeyMouseReader.KeyClick(Keys.T))
                    CreateTeleportTile(mouseX, mouseY);
                else if (KeyMouseReader.KeyClick(Keys.E))
                    CreateEnemy(mouseX, mouseY);
                else if (KeyMouseReader.KeyClick(Keys.G))
                {
                    if (!GoalPlaced)
                    {
                        CreateGoalTile(mouseX, mouseY);
                        GoalPlaced = true;
                    }
                }
                else if (KeyMouseReader.KeyClick(Keys.P))
                {
                    if (!PlayerPlaced)
                    {
                        CreatePlayerSpawn(mouseX, mouseY);
                        PlayerPlaced = true;
                    }
                }
                else if (KeyMouseReader.KeyClick(Keys.R))
                    ResetMap();
            }

            if (KeyMouseReader.isKeyDown(Keys.Up))
                MoveMapVerticaly(-1);
            if (KeyMouseReader.isKeyDown(Keys.Down))
                MoveMapVerticaly(1);
            if (KeyMouseReader.isKeyDown(Keys.Left))
                MoveMapHorizontaly(-1);
            if (KeyMouseReader.isKeyDown(Keys.Right))
                MoveMapHorizontaly(1);
        }

        public void Draw()
        {
            Spritebatch.Begin();

            Spritebatch.Draw(Background, Vector2.Zero, Color.White);

            for (int z = 0; z < Layers; z++)
            {
                for (int x = xDisplayMin; x < xDisplayMax; x++)
                {
                    for (int y = yDisplayMin; y < yDisplayMax; y++)
                    {
                        TileArray[z, x, y].Draw(Spritebatch, x - xOffset, y - yOffset, Offset);
                    }
                }
            }

            Spritebatch.Draw(GridTexture, offset, Color.White);

            Spritebatch.End();
        }

        private void ResetMap()
        {
            //Platform-layer
            for (int x = 0; x < mapXTiles; x++)
            {
                for (int y = 0; y < mapYTiles; y++)
                {
                    currentMap[0, x, y] = 0;
                    TileArray[0, x, y] = new Tile(x, y, AirTexture, TileSize, false, false);
                }
            }

            //Special-layer
            for (int x = 0; x < mapXTiles; x++)
            {
                for (int y = 0; y < mapYTiles; y++)
                {
                    currentMap[1, x, y] = 0;
                    TileArray[1, x, y] = new Tile(x, y, AirTexture, TileSize, false, false);
                }
            }

            PlayerPlaced = false;
            GoalPlaced = false;
        }

        private Rectangle MapBounds()
        {
            Rectangle tempRect = new Rectangle();

            tempRect.X = (int)Offset.X;
            tempRect.Y = (int)Offset.Y;
            tempRect.Width = MapWidth * TileSize;
            tempRect.Height = MapHeight* TileSize;

            return tempRect;
        }

        private Vector2 ConvertIndexToPixels(int X, int Y)
        {
            int x = X * TileSize + (int)offset.X;
            int y = Y * TileSize + (int)offset.Y;

            return new Vector2(x, y);
        }

        private Point ConvertPixelsToIndex(Point pos)
        {
            int x = (int)(pos.X - offset.X) / TileSize;
            int y = (int)(pos.Y - offset.Y) / TileSize;

            return new Point(x, y);
        }

        private void MoveMapHorizontaly(int MoveAmount)
        {
            xDisplayMin += MoveAmount;
            xDisplayMax += MoveAmount;
            xOffset += MoveAmount;
            //All this offset shit is used to make sure we add tiles in the correct places when the map has been moved
            int maxXOffset = mapXTiles - editorXTiles;
            if (xOffset < 0)
                xOffset = 0;
            else if (xOffset > maxXOffset)
                xOffset = maxXOffset;

            //We also want to keep the number of displayed tiles the same
            //So make sure we stop at the right times
            if (xDisplayMin > mapXTiles - editorXTiles)
                xDisplayMin = mapXTiles - editorXTiles;
            //We never want to display tiles that dont exist
            //So make sure the minDisplay never goes below 0
            else if (xDisplayMin < 0)
                xDisplayMin = 0;

            if (xDisplayMax < editorXTiles)
                xDisplayMax = editorXTiles;
            //Also make sure that maxDisplay never is greater
            //then the width of the map
            else if (xDisplayMax > mapXTiles)
                xDisplayMax = mapXTiles;

            Console.WriteLine("xDisplayMin: " + xDisplayMin + ", xDisplayMax: " + xDisplayMax + ", xMaxOffset: " + maxXOffset + ", xOffset: " + xOffset);
        }

        private void MoveMapVerticaly(int MoveAmount)
        {
            yDisplayMin += MoveAmount;
            yDisplayMax += MoveAmount;
            yOffset += MoveAmount;
            //All this offset shit is used to make sure we add tiles in the correct places when the map has been moved
            int maxYOffset = mapYTiles - editorYTiles;
            if (yOffset < 0)
                yOffset = 0;
            else if (yOffset > maxYOffset)
                yOffset = maxYOffset;

            if (yDisplayMin < 0)
                yDisplayMin = 0;

            if (yDisplayMax > mapYTiles)
                yDisplayMax = mapYTiles;

            if (yDisplayMin > mapYTiles - editorYTiles)
                yDisplayMin = mapYTiles - editorYTiles;

            if (yDisplayMax < editorYTiles)
                yDisplayMax = editorYTiles;

            Console.WriteLine("yDisplayMin: " + yDisplayMin + ", yDisplayMax: " + yDisplayMax + ", yMaxOffset: " + maxYOffset + ", yOffset: " + yOffset);
        }

        #region Create-functions
        private void CreateAir(int X, int Y)
        {
            TileArray[0, X, Y] = new Tile(X, Y, AirTexture, TileSize, false, false);
            TileArray[1, X, Y] = new Tile(X, Y, AirTexture, TileSize, false, false);

            CurrentMap[0, X, Y] = 0;
            CurrentMap[1, X, Y] = 0;

            List<Tile> tempTileList = FindConnectedTiles(TileArray[0, X, Y].IndexPos);
            foreach (Tile T in tempTileList)
                T.SetTexture(TileBitTextures[CalculateTileValue((int)T.IndexPos.X, (int)T.IndexPos.Y)]);
                //AssignTileType(T);
        }

        private void CreatePlatform(int X, int Y)
        {
            TileArray[0, X, Y] = new Tile(X, Y, PlatformTexture, TileSize, true, true);
            CurrentMap[0, X, Y] = 1;

            TileArray[0, X, Y].SetTexture(TileBitTextures[CalculateTileValue(X, Y)]);

            List<Tile> tempTileList = FindConnectedTiles(TileArray[0, X, Y].IndexPos);
            foreach (Tile T in tempTileList)
                T.SetTexture(TileBitTextures[CalculateTileValue((int)T.IndexPos.X, (int)T.IndexPos.Y)]);
        }

        private void CreateJumpTile(int X, int Y)
        {
            TileArray[1, X, Y] = new Tile(X, Y, JumpTileTexture, TileSize, true, false);

            CurrentMap[1, X, Y] = 1;
        }

        private void CreateTeleportTile(int X, int Y)
        {
            TileArray[1, X, Y] = new Tile(X, Y, TeleportTileTexture, TileSize, true, false);

            CurrentMap[1, X, Y] = 2;
        }

        private void CreateGoalTile(int X, int Y)
        {
            TileArray[1, X, Y] = new GoalTile(X, Y, GoalTexture, TileSize, true);

            CurrentMap[1, X, Y] = 3;
        }

        private void CreatePlayerSpawn(int X, int Y)
        {
            TileArray[0, X, Y] = new PlayerTile(X, Y, PlayerTexture, TileSize, true);

            CurrentMap[0, X, Y] = 9;
        }

        private void CreateEnemy(int X, int Y)
        {
            TileArray[1, X, Y] = new Tile(X, Y, EnemyTexture, TileSize, true, false);

            CurrentMap[1, X, Y] = 4;
        }
        #endregion

        private Tile FindTileAtIndex(int x, int y)
        {
            return TileArray[0, x, y];
        }

        private int CalculateTileValue(int x, int y)
        {
            int TileValue = 0;

            //NORTH
            if (y - 1 >= 0)
            {
                Tile northTile = FindTileAtIndex(x, y - 1);

                if (northTile.Collidable)
                    TileValue += 1;
            }

            //EAST
            if (x + 1 <= mapXTiles - 1)
            {
                Tile eastTile = FindTileAtIndex(x + 1, y);

                if (eastTile.Collidable)
                    TileValue += 2;
            }

            //SOUTH
            if (y + 1 <= mapYTiles - 1)
            {
                Tile southTile = FindTileAtIndex(x, y + 1);

                if (southTile.Collidable)
                    TileValue += 4;
            }

            //WEST
            if (x - 1 >= 0)
            {
                Tile westTile = FindTileAtIndex(x - 1, y);

                if (westTile.Collidable)
                    TileValue += 8;
            }
            return TileValue;
        }

        #region Functions that assigns TileTypes
        private List<int> FindConnectedTileTypes(Vector2 centerIndex)
        {
            List<int> tempList = new List<int>();

            int currentY = 0;
            int currentX = 0;
            //Loops through the 9 tiles that form a 3x3 square around the centerIndex (included the centerTile in the loop)
            //If we find a tile that is air or a tile that is outside the arrayIndex we add a "0" to the list
            //If we find a platformtile we add a "1" to the list
            #region Loop
            for (int x = -1; x <= 1; x++)
            {
                currentX = (int)centerIndex.X + x;

                for (int y = -1; y <= 1; y++)
                {
                    currentY = (int)centerIndex.Y + y;

                    //Is the tile we are looking at inside the map-array?
                    if (IsXInsideArray(currentX, CurrentMap) && IsYInsideArray(currentY, CurrentMap))
                    {
                        //Is the tile a platform or not? (Add(1) = is a platform)
                        if (CurrentMap[0, currentX, currentY] == 1)
                            tempList.Add(1);
                        else
                            tempList.Add(0);
                    }
                    else //The tile we are currently looking at is NOT inside the map-array, i.e the tile does not exsit!
                    {
                        //Then we should treat it as a tile (platform)!
                        tempList.Add(1);
                    }
                }
            }
            #endregion

            //Console.WriteLine("ST[0] == " + tempList[0] + " && ST[1] == " + tempList[1] + " && ST[2] == " + tempList[2] + " && ST[3] == " + tempList[3] + " && ST[5] == " + tempList[5] + " && ST[6] == " + tempList[6] + " && ST[7] == " + tempList[7] + " && ST[8] == " + tempList[8]);

            return tempList;
        }

        private List<Tile> FindConnectedTiles(Vector2 centerIndex)
        {
            List<Tile> tempList = new List<Tile>();

            int currentY = 0;
            int currentX = 0;
            //Loops through the 9 tiles that form a 3x3 square around the centerIndex (included the centerTile in the loop)
            //If we find a tile that is air or a tile that is outside the arrayIndex we add a "0" to the list
            //If we find a platformtile we add a "1" to the list
            #region Loop
            for (int x = -1; x <= 1; x++)
            {
                currentX = (int)centerIndex.X + x;

                for (int y = -1; y <= 1; y++)
                {
                    currentY = (int)centerIndex.Y + y;

                    //Is the tile we are looking at inside the map-array?
                    if (IsXInsideArray(currentX, CurrentMap) && IsYInsideArray(currentY, CurrentMap))
                    {
                        //Console.WriteLine("Current Index is inside array");
                        if (CurrentMap[0, currentX, currentY] == 1)
                            tempList.Add(TileArray[0, currentX, currentY]);
                    }
                    else //The tile we are currently looking at is NOT inside the map-array, i.e the tile does not exsit!
                    {
                    }
                }
            }
            #endregion

            return tempList;
        }

        private bool IsYInsideArray(int y, int[, ,] array)
        {
            if (y < 0 || y > array.GetUpperBound(2))
                return false;
            return true;
        }

        private bool IsXInsideArray(int x, int[, ,] array)
        {
            if (x < 0 || x > array.GetUpperBound(1))
                return false;
            return true;
        }

        private void LoadTileTextures()
        {
            TileBitTextures = new List<Texture2D>();
            TileBitTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/SingleTile"));          // 0
            TileBitTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TopOpenTile"));         // 1
            TileBitTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/RightOpenTile"));       // 2
            TileBitTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TopRightOpenTile"));    // 3
            TileBitTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BottomOpenTile"));      // 4
            TileBitTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BottomTopOpenTile"));   // 5
            TileBitTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BottomRightOpenTile")); // 6
            TileBitTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/LeftClosedTile"));      // 7
            TileBitTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/LeftOpenTile"));        // 8
            TileBitTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TopLeftOpenTile"));     // 9
            TileBitTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/LeftRightOpenTile"));   // 10
            TileBitTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BottomClosedTile"));    // 11
            TileBitTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BottomLeftOpenTile"));  // 12
            TileBitTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/RightClosedTile"));     // 13
            TileBitTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TopClosedTile"));       // 14
            TileBitTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/MiddleTile"));          // 15
        }
        #endregion
    }
}
