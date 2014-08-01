using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using LevelEditor.FileManagement;

namespace LevelEditor.Managers
{
    class MapManager
    {
        private Vector2 offset;
        public Vector2 Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        //This is used just to make it easier to display the map and wont be used in the serializing process
        private static Tile[, ,] tileArray;
        public static Tile[, ,] TileArray
        {
            get { return tileArray; }
        }

        private int Layers;
        public int layers { get { return Layers; } }
        public static int mapHeight { get { return mapYTiles; } }
        public static int mapWidth { get { return mapXTiles; } }
        public static int TileSize;

        private int editorXTiles, editorYTiles;
        private static int mapXTiles, mapYTiles;
        private int xDisplayMin, xDisplayMax;
        private int yDisplayMin, yDisplayMax;
        private int xOffset, yOffset;
        private int mouseX, mouseY;

        private int selectedTileValue;

        #region ContentManager, SpriteBatch and Textures
        private ContentManager Content;
        private SpriteBatch Spritebatch;

        private Texture2D GridTexture;
        private Texture2D Background;
        private Texture2D CursorTexture;

        public static List<Texture2D> TileTextures;

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
            TileSize = 0;

            mapXTiles = 100;
            mapYTiles = 60;

            tileArray = new Tile[Layers, mapXTiles, mapYTiles];

            editorXTiles = 40;
            editorYTiles = 30;

            xDisplayMin = 0;
            xDisplayMax = mapXTiles;

            yDisplayMin = 0;
            yDisplayMax = mapYTiles;

            xOffset = 0;
            yOffset = 0;

            selectedTileValue = 0;
        }

        public void Initialize()
        {
            if (!Initialized)
            {
                TextureManager.LoadContents(Content);

                GridTexture = TextureManager.GridTexture;
                Background = TextureManager.Background;
                CursorTexture = TextureManager.TileTextures[selectedTileValue];

                TileSize = TextureManager.TileSize;
                ResetMap();

                Initialized = true;
            }
        }

        public void Update()
        {
            if (MapBounds().Contains(KeyMouseReader.GetMousePos()))
            {
                mouseX = ConvertPixelsToIndex(KeyMouseReader.GetMousePos()).X;
                mouseY = ConvertPixelsToIndex(KeyMouseReader.GetMousePos()).Y;

                mouseX += xOffset;
                mouseY += yOffset;

                if (KeyMouseReader.ScrolledUp())
                {
                    selectedTileValue++;
                    if (selectedTileValue > 15)
                        selectedTileValue = 15;

                    CursorTexture = TextureManager.TileTextures[selectedTileValue];
                }
                else if (KeyMouseReader.ScrolledDown())
                {
                    selectedTileValue--;
                    if (selectedTileValue < 0)
                        selectedTileValue = 0;

                    CursorTexture = TextureManager.TileTextures[selectedTileValue];
                }
                    

                Console.WriteLine(selectedTileValue);

                #region EditMode 0 (Placing stuff)
                if (Game1.EditMode == 0)
                {
                    if (KeyMouseReader.LeftMouseDown())
                        CreatePlatform(mouseX, mouseY);
                    else if (KeyMouseReader.RightMouseDown())
                    {
                        if (tileArray[1, mouseX, mouseY] is GoalTile)
                            GoalPlaced = false;

                        if (tileArray[0, mouseX, mouseY] is PlayerTile)
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
                }
                #endregion

                #region EditMode 1 (Make Collidable)
                else if (Game1.EditMode == 1)
                {
                    if (KeyMouseReader.LeftMouseDown())
                        MakeCollidable(mouseX, mouseY);
                    else if (KeyMouseReader.RightMouseDown())
                        MakeNotCollidable(mouseX, mouseY);
                }
                #endregion

                #region EditMode 2 (Make Jumpthruable)
                else if (Game1.EditMode == 2)
                {
                    if (KeyMouseReader.LeftMouseDown())
                        MakeJumpthruable(mouseX, mouseY);
                    else if (KeyMouseReader.RightMouseDown())
                        MakeNotJumpthruable(mouseX, mouseY);
                }
                #endregion

            }

            if (KeyMouseReader.KeyClick(Keys.R))
                ResetMap();
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
            //Platforms
            for (int x = xDisplayMin; x < xDisplayMax; x++)
            {
                for (int y = yDisplayMin; y < yDisplayMax; y++)
                {
                    tileArray[0, x, y].Draw(Spritebatch, x - xOffset, y - yOffset, Offset);
                }
            }

            //Specials
            for (int x = xDisplayMin; x < xDisplayMax; x++)
            {
                for (int y = yDisplayMin; y < yDisplayMax; y++)
                {
                    tileArray[1, x, y].Draw(Spritebatch, x - xOffset, y - yOffset, Offset);
                }
            }

            Spritebatch.Draw(GridTexture, offset, Color.White);
            Spritebatch.Draw(CursorTexture, new Vector2(mouseX*TileSize + Offset.X, mouseY*TileSize + Offset.Y), Color.White);

            Spritebatch.End();
        }

        private void ResetMap()
        {
            //Platform-layer
            for (int x = 0; x < mapXTiles; x++)
            {
                for (int y = 0; y < mapYTiles; y++)
                {
                    tileArray[0, x, y] = new Tile(x, y, false);
                }
            }

            //Special-layer
            for (int x = 0; x < mapXTiles; x++)
            {
                for (int y = 0; y < mapYTiles; y++)
                {
                    tileArray[1, x, y] = new Tile(x, y, false);
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
            tempRect.Width = editorXTiles * TileSize;
            tempRect.Height = editorYTiles * TileSize;

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
            tileArray[0, X, Y] = new Tile(X, Y, false);
            tileArray[1, X, Y] = new Tile(X, Y, false);

            //List<Tile> tempTileList = FindConnectedTiles(tileArray[0, X, Y].IndexPos);
            //foreach (Tile T in tempTileList)
            //    T.SetTileType(CalculateTileValue((int)T.IndexPos.X, (int)T.IndexPos.Y));
        }

        private void CreatePlatform(int X, int Y)
        {
            tileArray[0, X, Y] = new Tile(X, Y, true);

            //tileArray[0, X, Y].SetTileType(CalculateTileValue(X, Y));
            tileArray[0, X, Y].SetTileType(selectedTileValue);

            //List<Tile> tempTileList = FindConnectedTiles(tileArray[0, X, Y].IndexPos);
            //foreach (Tile T in tempTileList)
            //    T.SetTileType(CalculateTileValue((int)T.IndexPos.X, (int)T.IndexPos.Y));
        }

        private void CreateJumpTile(int X, int Y)
        {
            tileArray[1, X, Y] = new JumpTile(X, Y, true);
        }

        private void CreateTeleportTile(int X, int Y)
        {
            tileArray[1, X, Y] = new TeleportTile(X, Y, true);
        }

        private void CreateGoalTile(int X, int Y)
        {
            tileArray[1, X, Y] = new GoalTile(X, Y, true);
        }

        private void CreatePlayerSpawn(int X, int Y)
        {
            tileArray[0, X, Y] = new PlayerTile(X, Y, true);
        }

        private void CreateEnemy(int X, int Y)
        {
            tileArray[1, X, Y] = new EnemyTile(X, Y, true);
        }
        #endregion

        #region Make Collidable Shit

        private void MakeCollidable(int X, int Y)
        {
            tileArray[0, X, Y].Collidable = true;
        }

        private void MakeNotCollidable(int X, int Y)
        {
            tileArray[0, X, Y].Collidable = false;
        }

        #endregion

        #region Make Jumpthruable Shit

        private void MakeJumpthruable(int X, int Y)
        {
            tileArray[0, X, Y].CanJumpThrough = true;
        }

        private void MakeNotJumpthruable(int X, int Y)
        {
            tileArray[0, X, Y].CanJumpThrough = false;
        }

        #endregion

        private Tile FindTileAtIndex(int x, int y)
        {
            return tileArray[0, x, y];
        }

        private int CalculateTileValue(int x, int y)
        {
            int TileValue = 0;

            //NORTH
            if (y - 1 >= 0)
            {
                Tile northTile = FindTileAtIndex(x, y - 1);

                if (northTile.ShouldDraw)
                    TileValue += 1;
            }

            //EAST
            if (x + 1 <= mapXTiles - 1)
            {
                Tile eastTile = FindTileAtIndex(x + 1, y);

                if (eastTile.ShouldDraw)
                    TileValue += 2;
            }

            //SOUTH
            if (y + 1 <= mapYTiles - 1)
            {
                Tile southTile = FindTileAtIndex(x, y + 1);

                if (southTile.ShouldDraw)
                    TileValue += 4;
            }

            //WEST
            if (x - 1 >= 0)
            {
                Tile westTile = FindTileAtIndex(x - 1, y);

                if (westTile.ShouldDraw)
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
                    if (IsXInsideArray(currentX, tileArray) && IsYInsideArray(currentY, tileArray))
                    {
                        //Is the tile a platform or not? (Add(1) = is a platform)
                        if (tileArray[0, currentX, currentY].TileType == 1)
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
                    if (IsXInsideArray(currentX, tileArray) && IsYInsideArray(currentY, tileArray))
                    {
                        //Console.WriteLine("Current Index is inside array");
                        if (tileArray[0, currentX, currentY].TileType == 1)
                            tempList.Add(tileArray[0, currentX, currentY]);
                    }
                    else //The tile we are currently looking at is NOT inside the map-array, i.e the tile does not exsit!
                    {
                    }
                }
            }
            #endregion

            return tempList;
        }

        private bool IsYInsideArray(int y, Tile[, ,] array)
        {
            if (y < 0 || y > array.GetUpperBound(2))
                return false;
            return true;
        }

        private bool IsXInsideArray(int x, Tile[, ,] array)
        {
            if (x < 0 || x > array.GetUpperBound(1))
                return false;
            return true;
        }

        private void LoadTileTextures()
        {
            TileTextures = new List<Texture2D>();
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_00"));     // 0
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_01"));     // 1
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_02"));     // 2
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_03"));     // 3
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_04"));     // 4
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_05"));     // 5
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_06"));     // 6
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_07"));     // 7
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_08"));     // 8
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_09"));     // 9
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_10"));    // 10
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_11"));    // 11
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_12"));    // 12
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_13"));    // 13
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_14"));    // 14
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_15"));    // 15

            TileSize = TileTextures[0].Width;
        }
        #endregion
    }
}
