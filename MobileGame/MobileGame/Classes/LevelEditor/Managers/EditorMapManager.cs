using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using MobileGame.FileManagement;
using MobileGame.Managers;

namespace MobileGame.LevelEditor
{
    class EditorMapManager
    {
        //The offset for the entire map, so that it begins drawing in the right spot
        //usually in the right side of the editor, so we can place tools/other ui stuff to the left
        private static Vector2 offset;
        public static Vector2 Offset
        {
            get { return offset; }
            set { offset = value; }
        }

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

        private Point selectionTopLeft, selectionBottomRight;

        #region ContentManager, SpriteBatch and Textures
        private ContentManager Content;
        public static SpriteBatch Spritebatch;

        private Texture2D GridTexture;
        private Texture2D Background;
        private static Texture2D CursorTexture;
        #endregion

        private bool Initialized;
        public static bool PlayerPlaced;
        public static bool GoalPlaced;
        public static bool HasSufficentCollisionFlags
        {
            get
            {
                if (NumCollisionFlags > 10)
                    return true;
                else
                    return false;
            }
        }

        public static int NumCollisionFlags { get; set; }


        // LAYERING STUFF
        private static byte[,] collisionLayer;
        public static byte[,] CollisionLayer
        {
            get { return collisionLayer; }
        }

        private static byte[,] backgroundLayer;
        public static byte[,] BackgroundLayer
        {
            get { return backgroundLayer; }
        }

        private static byte[,] platformLayer;
        public static byte[,] PlatformLayer
        {
            get { return platformLayer; }
        }

        private static byte[,] specialsLayer;
        public static byte[,] SpecialsLayer
        {
            get { return specialsLayer; }
        }

        private static byte[,] selectedLayer;
        public static byte[,] SelectedLayer
        {
            get { return selectedLayer; }
        }

        private static int selectedLayerNum;

        public EditorMapManager(ContentManager Content, SpriteBatch spriteBatch)
        {
            this.Content = Content;
            Spritebatch = spriteBatch;

            Initialized = false;
            PlayerPlaced = false;
            GoalPlaced = false;

            TileSize = 40;

            mapXTiles = 100;
            mapYTiles = 60;

            collisionLayer = new byte[mapXTiles, mapYTiles];
            backgroundLayer = new byte[mapXTiles, mapYTiles];
            platformLayer = new byte[mapXTiles, mapYTiles];
            specialsLayer = new byte[mapXTiles, mapYTiles];
            selectedLayer = backgroundLayer;
            selectedLayerNum = 0;

            editorXTiles = 40;
            editorYTiles = 36;

            xDisplayMin = 0;
            xDisplayMax = mapXTiles;

            yDisplayMin = 0;
            yDisplayMax = mapYTiles;

            xOffset = 0;
            yOffset = 0;

            SelectedTileValue = 0;

            selectionTopLeft = new Point(0, 0);
            selectionBottomRight = new Point(0, 0);
        }

        public void Initialize()
        {
            if (!Initialized)
            {
                GridTexture = Content.Load<Texture2D>("Editor/GridTexture");
                Background = Content.Load<Texture2D>("Editor/Background");
                CursorTexture = TextureManager.GameTextures[SelectedTileValue];

                TileSize = TextureManager.TileSize;
                ResetMap();

                ToolManager.Initialize();
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

            if (KeyMouseReader.isKeyDown(Keys.LeftShift) && KeyMouseReader.isKeyDown(Keys.LeftAlt) && KeyMouseReader.KeyClick(Keys.R))
                ToolPositionsManager.SaveData();
            else if (KeyMouseReader.KeyClick(Keys.R))
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
            Spritebatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            Spritebatch.Draw(TextureManager.FilledSquare, new Rectangle((int)Offset.X, (int)Offset.Y, editorXTiles * TileSize, editorYTiles * TileSize), Color.CornflowerBlue);

            //Background
            for (int x = xDisplayMin; x < xDisplayMax; x++)
            {
                for (int y = yDisplayMin; y < yDisplayMax; y++)
                {
                    Vector2 Pos = new Vector2((x - xOffset) * TileSize, (y - yOffset) * TileSize) + Offset;
                    Texture2D Texture;

                    #region Background
                    byte value = backgroundLayer[x, y];

                    if (value != 255)
                    {
                        Texture = TextureManager.GameTextures[value];

                        Spritebatch.Draw(Texture, Pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.15f);
                    }
                    #endregion

                    #region Platforms
                    value = platformLayer[x, y];

                    if (value != 255)
                    {
                        Texture = TextureManager.GameTextures[value];
                        Color color = Color.White;
                        if (selectedLayerNum == 0)
                            color *= 0.5f;

                        Spritebatch.Draw(Texture, Pos, null, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.25f);
                    }
                    #endregion

                    #region Specials
                    value = specialsLayer[x, y];

                    if (value != 255)
                    {
                        Texture = TextureManager.GameTextures[value];
                        Color color = Color.White;

                        if (selectedLayerNum == 0)
                            color *= 0.25f;
                        else if (selectedLayerNum == 1)
                            color *= 0.5f;

                        Spritebatch.Draw(Texture, Pos, null, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
                    }
                    #endregion
                }
            }

            if (EditorScreen.EditMode == 1)
            {
                for (int x = xDisplayMin; x < xDisplayMax; x++)
                {
                    for (int y = yDisplayMin; y < yDisplayMax; y++)
                    {
                        Vector2 Pos = new Vector2((x - xOffset) * TileSize, (y - yOffset) * TileSize) + Offset;
                        Rectangle Source = new Rectangle((int)Pos.X, (int)Pos.Y, TileSize, TileSize);
                        Texture2D Texture = TextureManager.FilledSquare;
                        Color color;

                        #region Background
                        byte value = collisionLayer[x, y];

                        if (value == 1 || value == 2)
                        {
                            color = Color.Red * 0.50f;

                            Spritebatch.Draw(Texture, Source, null, color, 0f, Vector2.Zero, SpriteEffects.None, 0.85f);
                        }
                        #endregion
                    }
                }
            }
            else if (EditorScreen.EditMode == 2)
            {
                for (int x = xDisplayMin; x < xDisplayMax; x++)
                {
                    for (int y = yDisplayMin; y < yDisplayMax; y++)
                    {
                        Vector2 Pos = new Vector2((x - xOffset) * TileSize, (y - yOffset) * TileSize) + Offset;
                        Rectangle Source = new Rectangle((int)Pos.X, (int)Pos.Y, TileSize, TileSize);
                        Texture2D Texture = TextureManager.FilledSquare;
                        Color color;

                        #region Background
                        byte value = collisionLayer[x, y];

                        if (value == 0)
                        {
                            color = Color.Black * 0.65f;

                            Spritebatch.Draw(Texture, Source, null, color, 0f, Vector2.Zero, SpriteEffects.None, 0.85f);
                        }
                        else if (value == 2)
                        {
                            color = Color.Red * 0.50f;

                            Spritebatch.Draw(Texture, Source, null, color, 0f, Vector2.Zero, SpriteEffects.None, 0.85f);
                        }
                        #endregion
                    }
                }
            }


            Spritebatch.Draw(GridTexture, offset, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);

            if (EditorScreen.EditMode == 0)
                Spritebatch.Draw(CursorTexture, new Vector2((mouseX - xOffset) * TileSize + Offset.X, (mouseY - yOffset) * TileSize + Offset.Y), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);

            ToolManager.Draw(Spritebatch);

            Spritebatch.End();
        }

        public static void SetCursorTexture(int TileValue)
        {
            if (TileValue != 255)
            {
                CursorTexture = TextureManager.GameTextures[TileValue];
                SelectedTileValue = TileValue;
            }

        }

        public static void BuildMap()
        {
            ResetMap();

            collisionLayer = FileLoader.LoadedCollisionLayer;
            backgroundLayer = FileLoader.LoadedBackgroundLayer;
            platformLayer = FileLoader.LoadedPlatformLayer;
            specialsLayer = FileLoader.LoadedSpecialsLayer;

            GoalPlaced = true;
            PlayerPlaced = true;
        }

        public static void SetSelectedLayer(int Value)
        {
            if (Value == 0)
            {
                selectedLayer = backgroundLayer;
                selectedLayerNum = 0;
            }
            else if (Value == 1)
            {
                selectedLayer = platformLayer;
                selectedLayerNum = 1;
            }
            else if (Value == 2)
            {
                selectedLayer = specialsLayer;
                selectedLayerNum = 2;
            }
        }

        private static void FadeLayer(int layer)
        {

        }

        private static void ResetMap()
        {
            //Collision-Layer
            for (int x = 0; x < mapXTiles; x++)
            {
                for (int y = 0; y < mapYTiles; y++)
                {
                    collisionLayer[x, y] = 0;
                }
            }

            //Rest of layuers
            for (int x = 0; x < mapXTiles; x++)
            {
                for (int y = 0; y < mapYTiles; y++)
                {
                    backgroundLayer[x, y] = 255;
                    platformLayer[x, y] = 255;
                    specialsLayer[x, y] = 255;
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
    }
}
