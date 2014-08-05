using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using LevelEditor.FileManagement;
using LevelEditor.Tools;

namespace LevelEditor.Managers
{
    class MapManager
    {
        private static Vector2 offset;
        public static Vector2 Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        //The array that contains all the tiles and such
        //This get converted to an array of TileData's right before we try to save it
        private static Tile[, ,] tileArray;
        public static Tile[, ,] TileArray
        {
            get { return tileArray; }
        }

        //The number of layers our map has
        private int Layers;
        //The number of tiles high the map is
        public static int mapHeight { get { return mapYTiles; } }
        //The number of tiles wide the map is
        public static int mapWidth { get { return mapXTiles; } }
        //The size of one tile (since they are rectangular, we only need the size of one side)
        public static int TileSize;

        //How many tiles the entire editor cover
        private int editorXTiles, editorYTiles;
        //How many tiles that is in the ENTIER map, even the ones not being displayed
        private static int mapXTiles, mapYTiles;
        //Used to only loop through the tiles that are currently being displayed in the editor
        private int xDisplayMin, xDisplayMax;
        private int yDisplayMin, yDisplayMax;
        //The amount of tiles we should offset when we have moved the map
        public static int xOffset, yOffset;
        //The TILE that the mouse is currently hoovering, different from the mouse's pixelpos
        private int mouseX, mouseY;

        //The tileValue of the currently selected tile, this gets set by the different tilebuttons
        public static int SelectedTileValue;

        //Used when we are trying to move the map around using space+mouse
        //Need to know in which direction the mouse is moving so we move the map in the correct way
        //We get that by getting the difference between these two variables
        private Point mousePos, prevMousePos;

        private bool HasActiveSelection;
        private Point selectionTopLeft, selectionBottomRight;

        #region ContentManager, SpriteBatch and Textures
        private ContentManager Content;
        public static SpriteBatch Spritebatch;

        private Texture2D GridTexture;
        private Texture2D Background;
        private static Texture2D CursorTexture;

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
        public static bool PlayerPlaced;
        public static bool GoalPlaced;

        public MapManager(ContentManager Content, SpriteBatch spriteBatch)
        {
            this.Content = Content;
            Spritebatch = spriteBatch;

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

            SelectedTileValue = 0;

            HasActiveSelection = false;
            selectionTopLeft = new Point(0, 0);
            selectionBottomRight = new Point(0, 0);
        }

        public void Initialize()
        {
            if (!Initialized)
            {
                TextureManager.LoadContents(Content);

                GridTexture = TextureManager.GridTexture;
                Background = TextureManager.Background;
                CursorTexture = TextureManager.TileTextures[SelectedTileValue];

                TileSize = TextureManager.TileSize;
                ResetMap();

                ToolManager.Initialize(tileArray);
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

                #region Support for dragging map around with space+mouse
                if (KeyMouseReader.isKeyDown(Keys.Space))
                {
                    mousePos = KeyMouseReader.GetMousePos();

                    if (KeyMouseReader.LeftMouseDown())
                    {
                        if (mousePos.X - prevMousePos.X <= -10)
                        {
                            MoveMapHorizontaly(2);
                        }
                        else if (mousePos.X - prevMousePos.X >= 10)
                        {
                            MoveMapHorizontaly(-2);
                        }

                        if (mousePos.Y - prevMousePos.Y <= -10)
                        {
                            MoveMapVerticaly(2);
                        }
                        else if (mousePos.Y - prevMousePos.Y >= 10)
                        {
                            MoveMapVerticaly(-2);
                        }
                    }

                    prevMousePos = mousePos;
                }
                #endregion
                else
                    ToolManager.Update();
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

            if(Game1.EditMode == 0)
                Spritebatch.Draw(CursorTexture, new Vector2((mouseX - xOffset) * TileSize + Offset.X, (mouseY - yOffset) * TileSize + Offset.Y), Color.White);

            ToolManager.Draw(Spritebatch);

            Spritebatch.End();
        }

        public static void SetCursorTexture(int TileValue)
        {
            CursorTexture = TextureManager.TileTextures[TileValue];
            SelectedTileValue = TileValue;
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
            //xOffset += MoveAmount;
            //All this offset shit is used to make sure we add tiles in the correct places when the map has been moved
            int maxXOffset = mapXTiles - editorXTiles;
            if (xOffset + MoveAmount < 0)
                xOffset = 0;
            else if (xOffset + MoveAmount > maxXOffset)
                xOffset = maxXOffset;
            else
                xOffset += MoveAmount;
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
    }
}
