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
using MobileGame.LightingSystem;

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

        //How many tiles the entire editor cover
        private static int xTiles, yTiles;

        //The number of tiles high the map is
        public static int NumYTiles { get { return yTiles; } }
        //The number of tiles wide the map is
        public static int NumXTiles { get { return xTiles; } }
        //The size of one tile (since they are rectangular, we only need the size of one side)
        public static int TileSize;

        //The TILE that the mouse is currently hoovering, different from the mouse's pixelpos
        private int mouseX, mouseY;
        private static Rectangle mouseSourceRect;

        //The tileValue of the currently selected tile, this gets set by the different tilebuttons
        public static int SelectedTileValue;

        //Used when we are trying to move the map around using space+mouse
        //Need to know in which direction the mouse is moving so we move the map in the correct way
        //We get that by getting the difference between these two variables
        //private Point mousePos, prevMousePos;

        private Point selectionTopLeft, selectionBottomRight;

        #region ContentManager, SpriteBatch and Textures
        private ContentManager Content;
        public static SpriteBatch Spritebatch;

        private Texture2D GridTexture;
        private Texture2D Background;
        private Texture2D GridCell;
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

        private static LightRenderer myLightRenderer;
        public static LightRenderer LightRenderer { get { return myLightRenderer; } }
        
        private static Texture2D myTileSetTexture;

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

            xTiles = 80;
            yTiles = 50;

            collisionLayer = new byte[xTiles, yTiles];
            backgroundLayer = new byte[xTiles, yTiles];
            platformLayer = new byte[xTiles, yTiles];
            specialsLayer = new byte[xTiles, yTiles];
            selectedLayer = backgroundLayer;
            selectedLayerNum = 0;

            SelectedTileValue = 0;

            selectionTopLeft = new Point(0, 0);
            selectionBottomRight = new Point(0, 0);

            //myTileSetTexture = Content.Load<Texture2D>("Tiles/TileSet");
            //myTileSetTexture = Content.Load<Texture2D>("Tiles/LabTileSet");
            myTileSetTexture = Content.Load<Texture2D>("Tiles/LabTileSet40x40");

            myLightRenderer = new LightRenderer(Game1.graphics);
            myLightRenderer.Initialize((int)Offset.X, (int)Offset.Y, xTiles * 20, yTiles * 20);
            myLightRenderer.LoadContent(Content);

            myLightRenderer.minLight = 0.20f;
            myLightRenderer.lightBias = 10f;
        }

        public void Initialize()
        {
            if (!Initialized)
            {
                GridTexture = Content.Load<Texture2D>("Editor/GridTexture");
                Background = Content.Load<Texture2D>("Editor/Background");
                GridCell = Content.Load<Texture2D>("Editor/GridCell");
                CursorTexture = TextureManager.GameTextures[SelectedTileValue];

                TileSize = TextureManager.TileSize;
                ResetMap();

                ToolManager.Initialize();
                Initialized = true;

                TileSize = 40;
            }
        }

        public void Update()
        {
            if (MapBounds().Contains(KeyMouseReader.GetMousePos()))
            {
                mouseX = ConvertPixelsToIndex(KeyMouseReader.GetMousePos()).X;
                mouseY = ConvertPixelsToIndex(KeyMouseReader.GetMousePos()).Y;

                ToolManager.Update(myLightRenderer);
            }

            if (KeyMouseReader.isKeyDown(Keys.LeftShift) && KeyMouseReader.isKeyDown(Keys.LeftAlt) && KeyMouseReader.KeyClick(Keys.R))
                ToolPositionsManager.SaveData();
            else if (KeyMouseReader.KeyClick(Keys.R))
                ResetMap();

            
        }

        public void Draw()
        {
            //LightingMode
            if (EditorScreen.EditMode == 4)
            {
                //BACKGROUND (STUFF THAT WONT BE CAST SHADOWS)
                myLightRenderer.BeginDrawBackground();

                Spritebatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

                for (int x = 0; x < xTiles; x++)
                {
                    for (int y = 0; y < yTiles; y++)
                    {
                        Vector2 Pos = new Vector2(x * TileSize, y * TileSize) + offset;

                        #region Background
                        byte value = backgroundLayer[x, y];

                        if (value != 255)
                        {
                            int sourceX = value % 8;
                            int sourceY = value / 8;
                            //Rectangle sourceRect = new Rectangle(sourceX * 20, sourceY * 20, 20, 20);
                            Rectangle sourceRect = new Rectangle(sourceX * TileSize, sourceY * TileSize, TileSize, TileSize);

                            Spritebatch.Draw(myTileSetTexture, Pos, sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.15f);
                        }
                        #endregion

                        #region Platforms
                        value = platformLayer[x, y];

                        if (value != 255)
                        {
                            Color color = Color.White;
                            if (selectedLayerNum == 0)
                                color *= 0.5f;

                            int sourceX = value % 8;
                            int sourceY = value / 8;
                            //Rectangle sourceRect = new Rectangle(sourceX * 20, sourceY * 20, 20, 20);
                            Rectangle sourceRect = new Rectangle(sourceX * TileSize, sourceY * TileSize, TileSize, TileSize);

                            Spritebatch.Draw(myTileSetTexture, Pos, sourceRect, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.25f);
                        }
                        #endregion

                        #region Specials
                        value = specialsLayer[x, y];

                        if (value != 255)
                        {
                            Color color = Color.White;

                            if (selectedLayerNum == 0)
                                color *= 0.25f;
                            else if (selectedLayerNum == 1)
                                color *= 0.5f;

                            int sourceX = value % 8;
                            int sourceY = value / 8;
                            //Rectangle sourceRect = new Rectangle(sourceX * 20, sourceY * 20, 20, 20);
                            Rectangle sourceRect = new Rectangle(sourceX * TileSize, sourceY * TileSize, TileSize, TileSize);

                            Spritebatch.Draw(myTileSetTexture, Pos, sourceRect, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
                        }
                        #endregion
                    }
                }

                Spritebatch.End();

                myLightRenderer.EndDrawBackground();

                //SHADOW CASTERS (STUFF THAT WILL CAST SHADOWS)
                myLightRenderer.BeginDrawShadowCasters();

                Spritebatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

                for (int x = 0; x < xTiles; x++)
                {
                    for (int y = 0; y < yTiles; y++)
                    {
                        #region Platforms
                        Vector2 Pos = new Vector2(x * TileSize, y * TileSize) + offset;
                       
                        byte value = platformLayer[x, y];

                        if (value != 255)
                        {
                            Color color = Color.White;
                            if (selectedLayerNum == 0)
                                color *= 0.5f;

                            int sourceX = value % 8;
                            int sourceY = value / 8;
                            //Rectangle sourceRect = new Rectangle(sourceX * 20, sourceY * 20, 20, 20);
                            Rectangle sourceRect = new Rectangle(sourceX * TileSize, sourceY * TileSize, TileSize, TileSize);

                            Spritebatch.Draw(myTileSetTexture, Pos, sourceRect, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.25f);
                        }
                        #endregion
                    }
                }

                Spritebatch.End();

                myLightRenderer.EndDrawShadowCasters();

                myLightRenderer.DrawLitScene();
            }
            //Non-Lighing mode
            else
            {
                Spritebatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

                for (int x = 0; x < xTiles; x++)
                {
                    for (int y = 0; y < yTiles; y++)
                    {
                        Vector2 Pos = new Vector2(x * TileSize, y * TileSize) + offset;

                        #region Background
                        byte value = backgroundLayer[x, y];

                        if (value != 255)
                        {
                            int sourceX = value % 8;
                            int sourceY = value / 8;
                            //Rectangle sourceRect = new Rectangle(sourceX * 20, sourceY * 20, 20, 20);
                            Rectangle sourceRect = new Rectangle(sourceX * 40, sourceY * 40, 40, 40);

                            Spritebatch.Draw(myTileSetTexture, Pos, sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.15f);
                        }
                        #endregion

                        #region Platforms
                        value = platformLayer[x, y];

                        if (value != 255)
                        {
                            Color color = Color.White;
                            if (selectedLayerNum == 0)
                                color *= 0.5f;

                            int sourceX = value % 8;
                            int sourceY = value / 8;
                            //Rectangle sourceRect = new Rectangle(sourceX * 20, sourceY * 20, 20, 20);
                            Rectangle sourceRect = new Rectangle(sourceX * 40, sourceY * 40, 40, 40);

                            Spritebatch.Draw(myTileSetTexture, Pos, sourceRect, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.25f);
                        }
                        #endregion

                        #region Specials
                        value = specialsLayer[x, y];

                        if (value != 255)
                        {
                            Color color = Color.White;

                            if (selectedLayerNum == 0)
                                color *= 0.25f;
                            else if (selectedLayerNum == 1)
                                color *= 0.5f;

                            int sourceX = value % 8;
                            int sourceY = value / 8;
                            //Rectangle sourceRect = new Rectangle(sourceX * 20, sourceY * 20, 20, 20);
                            Rectangle sourceRect = new Rectangle(sourceX * 40, sourceY * 40, 40, 40);

                            Spritebatch.Draw(myTileSetTexture, Pos, sourceRect, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
                        }
                        #endregion
                    }
                }

                Spritebatch.End();
            }

            Spritebatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            if (EditorScreen.EditMode == 1)
            {
                for (int x = 0; x < xTiles; x++)
                {
                    for (int y = 0; y < yTiles; y++)
                    {
                        Vector2 Pos = new Vector2(x * TileSize, y * TileSize) + offset;
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
                for (int x = 0; x < xTiles; x++)
                {
                    for (int y = 0; y < yTiles; y++)
                    {
                        Vector2 Pos = new Vector2(x * TileSize, y * TileSize) + offset;
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
                Spritebatch.Draw(CursorTexture, new Vector2((mouseX * TileSize) + Offset.X, (mouseY * TileSize) + Offset.Y), mouseSourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);

            ToolManager.Draw(Spritebatch);

            Spritebatch.End();
        }

        public static void SetCursorTexture(int TileValue)
        {
            if (TileValue != 255)
            {
                //CursorTexture = TextureManager.GameTextures[TileValue];
                CursorTexture = myTileSetTexture;
                SelectedTileValue = TileValue;

                int x = TileValue % 8;
                int y = TileValue / 8;
                mouseSourceRect = new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);
                //mouseSourceRect = new Rectangle(x * 20, y * 20, 20, 20);
            }

        }

        public static void BuildMap()
        {
            ResetMap();

            collisionLayer = FileLoader.LoadedCollisionLayer;
            backgroundLayer = FileLoader.LoadedBackgroundLayer;
            platformLayer = FileLoader.LoadedPlatformLayer;
            specialsLayer = FileLoader.LoadedSpecialsLayer;

            for (int i = 0; i < FileLoader.LoadedLevelNumPointLights; i++)
            {
                float x = FileLoader.LoadedPointLights[i, 0] + Offset.X;
                float y = FileLoader.LoadedPointLights[i, 1] + Offset.Y;
                float radius = FileLoader.LoadedPointLights[i, 2];
                float power = FileLoader.LoadedPointLights[i, 3];
                int r = (int)FileLoader.LoadedPointLights[i, 4];
                int g = (int)FileLoader.LoadedPointLights[i, 5];
                int b = (int)FileLoader.LoadedPointLights[i, 6];

                Color color = new Color(r, g, b);

                PointLight newLight = new PointLight(new Vector2(x, y), power, radius, color);
                myLightRenderer.pointLights.Add(newLight);
            }

            GoalPlaced = false;
            PlayerPlaced = false;
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
            for (int x = 0; x < xTiles; x++)
            {
                for (int y = 0; y < yTiles; y++)
                {
                    collisionLayer[x, y] = 0;
                }
            }

            //Rest of layuers
            for (int x = 0; x < xTiles; x++)
            {
                for (int y = 0; y < yTiles; y++)
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
            tempRect.Width = xTiles * TileSize;
            tempRect.Height = yTiles * TileSize;

            return tempRect;
        }

        private Point ConvertPixelsToIndex(Point pos)
        {
            int x = (int)(pos.X - offset.X) / TileSize;
            int y = (int)(pos.Y - offset.Y) / TileSize;

            return new Point(x, y);
        }
    }
}
