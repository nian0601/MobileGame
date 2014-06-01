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
        public int mapHeight { get { return MapHeight; } }
        public int mapWidth { get { return MapWidth; } }
        public static int TileSize;
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

            currentMap = new int[Layers, MapWidth, MapHeight];
            TileArray = new Tile[Layers, MapWidth, MapHeight];
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
        }

        public void Draw()
        {
            Spritebatch.Begin();

            Spritebatch.Draw(Background, Vector2.Zero, Color.White);

            for (int z = 0; z < Layers; z++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    for (int y = 0; y < MapHeight; y++)
                    {
                        TileArray[z, x, y].Draw(Spritebatch, Offset);
                    }
                }
            }

            Spritebatch.Draw(GridTexture, offset, Color.White);

            Spritebatch.End();
        }

        private void ResetMap()
        {
            //Platform-layer
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    currentMap[0, x, y] = 0;
                    TileArray[0, x, y] = new Tile(x, y, AirTexture, TileSize, false);
                }
            }

            //Special-layer
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    currentMap[1, x, y] = 0;
                    TileArray[1, x, y] = new Tile(x, y, AirTexture, TileSize, false);
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

        #region Create-functions
        private void CreateAir(int X, int Y)
        {
            TileArray[0, X, Y] = new Tile(X, Y, AirTexture, TileSize, false);
            TileArray[1, X, Y] = new Tile(X, Y, AirTexture, TileSize, false);

            CurrentMap[0, X, Y] = 0;
            CurrentMap[1, X, Y] = 0;

            List<Tile> tempTileList = FindConnectedTiles(TileArray[0, X, Y].IndexPos);
            foreach (Tile T in tempTileList)
                AssignTileType(T);
        }

        private void CreatePlatform(int X, int Y)
        {
            TileArray[0, X, Y] = new Tile(X, Y, PlatformTexture, TileSize, true);
            CurrentMap[0, X, Y] = 1;

            AssignTileType(TileArray[0, X, Y]);

            List<Tile> tempTileList = FindConnectedTiles(TileArray[0, X, Y].IndexPos);
            foreach (Tile T in tempTileList)
                AssignTileType(T);
        }

        private void CreateJumpTile(int X, int Y)
        {
            TileArray[1, X, Y] = new Tile(X, Y, JumpTileTexture, TileSize, true);

            CurrentMap[1, X, Y] = 1;
        }

        private void CreateTeleportTile(int X, int Y)
        {
            TileArray[1, X, Y] = new Tile(X, Y, TeleportTileTexture, TileSize, true);

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
            TileArray[1, X, Y] = new Tile(X, Y, EnemyTexture, TileSize, true);

            CurrentMap[1, X, Y] = 4;
        }
        #endregion

        #region Functions that assigns TileTypes
        private void AssignTileType(Tile Tile)
        {
            //ST = SurroundingTiles, made the name shorted to make the fking if-statements shorter aswell
            List<int> ST = FindConnectedTileTypes(Tile.IndexPos);

            if (IsBottomClosedLeftCornerTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.BottomClosedLeftCornerTile]);
            else if (IsBottomClosedRightCornerTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.BottomClosedRightCornerTile]);
            else if (IsBottomClosedTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.BottomClosedTile]);
            else if (IsBottomLeftCorner(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.BottomLeftCorner]);
            else if (IsBottomleftOpenTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.BottomLeftOpenTile]);
            else if (IsBottomLeftTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.BottomLeftTile]);
            else if (IsBottomMiddleTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.BottomMiddleTile]);
            else if (IsBottomOpenTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.BottomOpenTile]);
            else if (IsBottomRightCornerTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.BottomRightCorner]);
            else if (IsBottomRightOpenTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.BottomRightOpenTile]);
            else if (IsBottomRightTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.BottomRightTile]);
            else if (IsBottomTopOpenTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.BottomTopOpenTile]);
            else if (IsFourCornersTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.FourCornersTile]);
            else if (IsLeftClosedBottomCornerTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.LeftClosedBottomCornerTile]);
            else if (IsLeftClosedTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.LeftClosedTile]);
            else if (IsLeftClosedTopCornerTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.LeftClosedTopCornerTile]);
            else if (IsLeftOpenTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.LeftOpenTile]);
            else if (IsLeftRightOpenTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.LeftRightOpenTile]);
            else if (IsMiddleLeftTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.MiddleLeftTile]);
            else if (IsMiddleRightTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.MiddleRightTile]);
            else if (IsMiddleTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.MiddleTile]);
            else if (IsRightClosedBottomCornerTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.RightClosedBottomCenterTile]);
            else if (IsRightClosedTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.RightClosedTile]);
            else if (IsRightClosedTopCornerTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.RightClosedTopCenterTile]);
            else if (IsRightOpenTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.RightOpenTile]);
            else if (IsSingleTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.SingleTile]);
            else if (IsThreeCornersBottomLeftTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.ThreeCornersBottomLeftTile]);
            else if (IsThreeCornersBottomRightTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.ThreeCornersBottomRightTile]);
            else if (IsThreeCornersTopLeftTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.ThreeCornersTopLeftTile]);
            else if (IsThreeCornersTopRightTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.ThreeCornersTopRightTile]);
            else if (IsTopClosedLeftCornerTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.TopClosedLeftCornerTile]);
            else if (IsTopClosedRightCornerTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.TopClosedrightCornerTile]);
            else if (IsTopClosedTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.TopClosedTile]);
            else if (IsTopLeftBottomRightCornerTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.TopLeftBottomRightCornerTile]);
            else if (IsTopLeftCornerTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.TopLeftCorner]);
            else if (IsTopLeftOpenTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.TopLeftOpenTile]);
            else if (IsTopLeftTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.TopLeftTile]);
            else if (IsTopMiddleTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.TopMiddleTile]);
            else if (IsTopOpenTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.TopOpenTile]);
            else if (IsTopRightBottomLeftCornerTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.TopRightBottomLeftCornerTile]);
            else if (IsTopRightCornerTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.TopRightCorner]);
            else if (IsTopRightOpenTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.TopRightOpenTile]);
            else if (IsTopRightTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.TopRightTile]);
            else if (IsTwoCornersBottomTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.TwoCornersBottomTile]);
            else if (IsTwoCornersLeftTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.TwoCornersLeftTile]);
            else if (IsTwoCornersRightTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.TwoCornersRightTile]);
            else if (IsTwoCornersTopTile(ST))
                Tile.SetTexture(TileTextures[(int)TileTypes.TwoCornersTopTile]);
        }

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
            #region TileTextures.Add
            TileTextures = new List<Texture2D>();

            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BottomClosedLeftCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BottomClosedRightCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BottomClosedTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BottomLeftCorner"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BottomLeftOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BottomLeftTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BottomMiddleTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BottomOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BottomRightCorner"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BottomRightOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BottomRightTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BottomTopOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/FourCornersTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/LeftClosedBottomCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/LeftClosedTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/LeftClosedTopCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/LeftOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/LeftRightOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/MiddleLeftTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/MiddleRightTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/MiddleTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/RightClosedBottomCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/RightClosedTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/RightClosedTopCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/RightOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/SingleTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/ThreeCornersBottomLeftTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/ThreeCornersBottomRightTIle"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/ThreeCornersTopLeftTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/ThreeCornersTopRightTIle"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TopClosedLeftCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TopClosedRightCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TopClosedTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TopLeftBottomRightCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TopLeftCorner"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TopLeftOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TopLeftTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TopMiddleTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TopOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TopRightBottomLeftCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TopRightCorner"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TopRightOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TopRightTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TwoCornersBottomTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TwoCornersLeftTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TwoCornersRightTile"));
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/TwoCornersTopTile"));
            #endregion
        }

        #region All the bool-functions regarding Tile-Types
        private bool IsBottomClosedLeftCornerTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;

            return false;
        }

        private bool IsBottomClosedRightCornerTile(List<int> ST)
        {
            if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsBottomClosedTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsBottomLeftCorner(List<int> ST)
        {
            return (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1);
        }

        private bool IsBottomleftOpenTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;

            return false;
        }

        private bool IsBottomLeftTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsBottomMiddleTile(List<int> ST)
        {
            if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsBottomOpenTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;

            return false;
        }

        private bool IsBottomRightCornerTile(List<int> ST)
        {
            return (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0);
        }

        private bool IsBottomRightOpenTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsBottomRightTile(List<int> ST)
        {
            if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;

            return false;
        }

        private bool IsBottomTopOpenTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsFourCornersTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;

            return false;
        }

        private bool IsLeftClosedBottomCornerTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;

            return false;
        }

        private bool IsLeftClosedTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;

            return false;
        }

        private bool IsLeftClosedTopCornerTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsLeftOpenTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsLeftRightOpenTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsMiddleLeftTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsMiddleRightTile(List<int> ST)
        {
            if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsMiddleTile(List<int> ST)
        {
            return (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1);
        }

        private bool IsRightClosedBottomCornerTile(List<int> ST)
        {
            if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;

            return false;
        }

        private bool IsRightClosedTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;

            return false;
        }

        private bool IsRightClosedTopCornerTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsRightOpenTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsSingleTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsThreeCornersBottomLeftTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;

            return false;
        }

        private bool IsThreeCornersBottomRightTile(List<int> ST)
        {
            return (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1);
        }

        private bool IsThreeCornersTopLeftTile(List<int> ST)
        {
            if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;

            return false;
        }

        private bool IsThreeCornersTopRightTile(List<int> ST)
        {
            return (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0);
        }

        private bool IsTopClosedLeftCornerTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsTopClosedRightCornerTile(List<int> ST)
        {
            if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;

            return false;
        }

        private bool IsTopClosedTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;

            return false;
        }

        private bool IsTopLeftBottomRightCornerTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;

            return false;
        }

        private bool IsTopLeftCornerTile(List<int> ST)
        {
            return (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1);
        }

        private bool IsTopLeftOpenTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsTopLeftTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            return false;
        }

        private bool IsTopMiddleTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsTopOpenTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsTopRightBottomLeftCornerTile(List<int> ST)
        {
            if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsTopRightCornerTile(List<int> ST)
        {
            return (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1);
        }

        private bool IsTopRightOpenTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 0 && ST[2] == 1 && ST[3] == 1 && ST[5] == 0 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsTopRightTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 0 && ST[8] == 0)
                return true;
            else if (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 0 && ST[5] == 1 && ST[6] == 1 && ST[7] == 0 && ST[8] == 1)
                return true;
            else if (ST[0] == 1 && ST[1] == 0 && ST[2] == 0 && ST[3] == 0 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsTwoCornersBottomTile(List<int> ST)
        {
            if (ST[0] == 1 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 0)
                return true;

            return false;
        }

        private bool IsTwoCornersLeftTile(List<int> ST)
        {
            if (ST[0] == 0 && ST[1] == 1 && ST[2] == 0 && ST[3] == 1 && ST[5] == 1 && ST[6] == 1 && ST[7] == 1 && ST[8] == 1)
                return true;

            return false;
        }

        private bool IsTwoCornersRightTile(List<int> ST)
        {
            return (ST[0] == 1 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 0);
        }

        private bool IsTwoCornersTopTile(List<int> ST)
        {
            return (ST[0] == 0 && ST[1] == 1 && ST[2] == 1 && ST[3] == 1 && ST[5] == 1 && ST[6] == 0 && ST[7] == 1 && ST[8] == 1);
        }
        #endregion

        #endregion
    }
}
