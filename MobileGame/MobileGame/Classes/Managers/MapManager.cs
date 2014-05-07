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

namespace MobileGame
{
    class MapManager
    {
        private int[,,] currentMap;
        private Tile[,,] tileArray;
        private List<SimpleTile> colliderList;
        private List<SpecialTile> specialBlockList;
        private int tileSize;
        private Vector2 playerStartPos;

        #region Properties


        public List<SimpleTile> ColliderList
        {
            get
            {
                return colliderList;
            }
        }

        public List<SpecialTile> SpecialBlocksList
        {
            get
            {
                return specialBlockList;
            }
        }

        public Vector2 PlayerStartPos
        {
            get { return playerStartPos; }
        }


        #endregion

        public MapManager()
        {
            tileSize = TextureManager.PlatformTile.Height;
            colliderList = new List<SimpleTile>();
            specialBlockList = new List<SpecialTile>();
            playerStartPos = new Vector2(200, 200);
            LoadLevel();
        }

        public void Update()
        {
            int mouseX = ConvertPixelsToIndex(KeyMouseReader.mousePos).X;
            int mouseY = ConvertPixelsToIndex(KeyMouseReader.mousePos).Y;

            if (KeyMouseReader.isKeyDown(Keys.LeftShift) && KeyMouseReader.LeftClick())
                EnemyManager.AddEnemy(new SimpleEnemy(mouseX, mouseY));
            else if (KeyMouseReader.isKeyDown(Keys.LeftControl) && KeyMouseReader.LeftClick())
                EnemyManager.AddEnemy(new ShootingEnemy(mouseX, mouseY));

            else if (KeyMouseReader.isKeyDown(Keys.LeftAlt) && KeyMouseReader.LeftClick())
                specialBlockList.Add(new JumpTile(mouseX, mouseY));

            else if (KeyMouseReader.isKeyDown(Keys.LeftShift) && KeyMouseReader.MouseWheelDown())
                RemoveSimpleTile(mouseX, mouseY);

            else if (KeyMouseReader.MouseWheelDown())
                CreateSimpleTile(mouseX, mouseY);

            else if (KeyMouseReader.RightClick())
            {
                List<int> tempList = FindConnectedTileTypes(new Vector2(mouseX, mouseY));
                Console.WriteLine("ST[0] == " + tempList[0] + " && ST[1] == " + tempList[1] + " && ST[2] == " + tempList[2] + " && ST[3] == " + tempList[3] + " && ST[5] == " + tempList[5] + " && ST[6] == " + tempList[6] + " && ST[7] == " + tempList[7] + " && ST[8] == " + tempList[8]);
            }      
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < tileArray.GetLength(1); x++)
            {
                for (int y = 0; y < tileArray.GetLength(2); y++)
                {
                    tileArray[0, x, y].Draw(spriteBatch);
                }
            }

            foreach (Tile T in specialBlockList)
                T.Draw(spriteBatch);
        }

        

        //Should expand this function in the future to include reading a level from file and such
        private void LoadLevel()
        {
            currentMap = new int[,,] {
                                        {
                                            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                            {1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                            {1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                            {1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                            {1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 1},
                                            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1},
                                            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                            {1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1},
                                            {1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                            {1, 9, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},

                                        },
                                        {
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
                                        }
                                        
                                    };

            BuildLevel(currentMap);
        }

        private void BuildLevel(int[,,] level)
        {
            int[,,] levelToBuild = level;

            int layers = levelToBuild.GetLength(0);
            int mapHeight = levelToBuild.GetLength(1);
            int mapWidth = levelToBuild.GetLength(2);
            tileArray = new Tile[layers, mapWidth, mapHeight];

            //LOOPS THROUGH THE PLATFORM LAYER
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    int tileType = levelToBuild[0, y, x];

                    if (tileType == 9)
                    {
                        playerStartPos = ConvertIndexToPixels(x, y);
                        tileArray[0, x, y] = new SimpleTile(x, y, 0);
                        continue;
                    }
                        

                    SimpleTile tempTile = new SimpleTile(x, y, tileType);

                    tileArray[0, x, y] = tempTile;

                    
                    if (tileType != 0)
                        colliderList.Add(tempTile);
                        
                }
            }

            //LOOPS THROUGH THE SPECIAL BLOCK'S LAYER
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    int tileType = levelToBuild[1, y, x];

                    if (tileType == 1)
                        specialBlockList.Add(new JumpTile(x, y));
                    else if (tileType == 2)
                        specialBlockList.Add(new TeleportTile(x, y));
                    else if (tileType == 3)
                        specialBlockList.Add(new GoalTile(x, y));
                    else if (tileType == 4)
                        EnemyManager.AddEnemy(new SimpleEnemy(x, y));   
                }
            }

            foreach (SimpleTile Tile in colliderList)
                AssignTileType(Tile);

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
        }

        private Vector2 ConvertIndexToPixels(int X, int Y)
        {
            int x = X * tileSize;
            int y = Y * tileSize;

            return new Vector2(x, y);
        }

        private Point ConvertPixelsToIndex(Point pos)
        {
            int x = (int)pos.X / tileSize;
            int y = (int)pos.Y / tileSize;

            return new Point(x, y);
        }

        private Tile FindTileAtIndex(int x, int y)
        {
            return tileArray[0, x, y];
        }

        //This function tries to determine what texture all of the platforms should use, so that they fit together and form proper platforms
        private void AssignTileType(Tile Tile)
        {
            //ST = SurroundingTiles, made the name shorted to make the fking if-statements shorter aswell
            List<int> ST = FindConnectedTileTypes(Tile.IndexPos);

            if (IsBottomClosedLeftCornerTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.BottomClosedLeftCornerTile);
            else if (IsBottomClosedRightCornerTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.BottomClosedRightCornerTile);
            else if (IsBottomClosedTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.BottomClosedTile);
            else if (IsBottomLeftCorner(ST))
                Tile.SetTileType(TextureManager.TileTypes.BottomLeftCorner);
            else if (IsBottomleftOpenTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.BottomLeftOpenTile);
            else if (IsBottomLeftTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.BottomLeftTile);
            else if (IsBottomMiddleTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.BottomMiddleTile);
            else if (IsBottomOpenTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.BottomOpenTile);
            else if (IsBottomRightCornerTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.BottomRightCorner);
            else if (IsBottomRightOpenTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.BottomRightOpenTile);
            else if (IsBottomRightTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.BottomRightTile);
            else if (IsBottomTopOpenTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.BottomTopOpenTile);
            else if (IsFourCornersTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.FourCornersTile);
            else if (IsLeftClosedBottomCornerTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.LeftClosedBottomCornerTile);
            else if (IsLeftClosedTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.LeftClosedTile);
            else if (IsLeftClosedTopCornerTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.LeftClosedTopCornerTile);
            else if (IsLeftOpenTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.LeftOpenTile);
            else if (IsLeftRightOpenTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.LeftRightOpenTile);
            else if (IsMiddleLeftTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.MiddleLeftTile);
            else if (IsMiddleRightTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.MiddleRightTile);
            else if (IsMiddleTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.MiddleTile);
            else if (IsRightClosedBottomCornerTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.RightClosedBottomCenterTile);
            else if (IsRightClosedTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.RightClosedTile);
            else if (IsRightClosedTopCornerTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.RightClosedTopCenterTile);
            else if (IsRightOpenTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.RightOpenTile);
            else if (IsSingleTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.SingleTile);
            else if (IsThreeCornersBottomLeftTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.ThreeCornersBottomLeftTile);
            else if (IsThreeCornersBottomRightTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.ThreeCornersBottomRightTile);
            else if (IsThreeCornersTopLeftTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.ThreeCornersTopLeftTile);
            else if (IsThreeCornersTopRightTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.ThreeCornersTopRightTile);
            else if (IsTopClosedLeftCornerTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.TopClosedLeftCornerTile);
            else if (IsTopClosedRightCornerTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.TopClosedrightCornerTile);
            else if (IsTopClosedTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.TopClosedTile);
            else if (IsTopLeftBottomRightCornerTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.TopLeftBottomRightCornerTile);
            else if (IsTopLeftCornerTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.TopLeftCorner);
            else if (IsTopLeftOpenTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.TopLeftOpenTile);
            else if (IsTopLeftTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.TopLeftTile);
            else if (IsTopMiddleTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.TopMiddleTile);
            else if (IsTopOpenTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.TopOpenTile);
            else if (IsTopRightBottomLeftCornerTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.TopRightBottomLeftCornerTile);
            else if (IsTopRightCornerTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.TopRightCorner);
            else if (IsTopRightOpenTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.TopRightOpenTile);
            else if (IsTopRightTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.TopRightTile);
            else if (IsTwoCornersBottomTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.TwoCornersBottomTile);
            else if (IsTwoCornersLeftTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.TwoCornersLeftTile);
            else if (IsTwoCornersRightTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.TwoCornersRightTile);
            else if (IsTwoCornersTopTile(ST))
                Tile.SetTileType(TextureManager.TileTypes.TwoCornersTopTile);
        }

        private void CreateSimpleTile(int x, int y)
        {
            SimpleTile tempTile = new SimpleTile(x, y, 1);

            bool newTile = true;
            Vector2 tempVector = new Vector2(x, y);

            //This is to make sure that we only add ONE tile to the colliderList
            //Is needed if we want it to be possible to "draw" the map, i.e not having to click each tile
            for (int i = 0; i < colliderList.Count; i++)
                if (colliderList[i].IndexPos == tempVector)
                    newTile = false;

            if (newTile)
            {
                tileArray[0, x, y] = tempTile;
                colliderList.Add(tempTile);

                AssignTileType(tempTile);

                List<Tile> tempTileList = FindConnectedTiles(tempTile.IndexPos);
                foreach (Tile T in tempTileList)
                    AssignTileType(T);
            }
            
        }

        private void RemoveSimpleTile(int x, int y)
        {
            tileArray[0, x, y] = new SimpleTile(x, y, 0);

            Vector2 tempVector = new Vector2(x, y);

            for (int i = 0; i < colliderList.Count; i++)
            {
                if (colliderList[i].IndexPos == tempVector)
                {
                    List<Tile> tempTileList = FindConnectedTiles(colliderList[i].IndexPos);
                    foreach (Tile T in tempTileList)
                        AssignTileType(T);


                    colliderList.RemoveAt(i);
                    Console.WriteLine(colliderList.Count);
                    break;
                }
            }    
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
                    if (IsXInsideArray(currentX, tileArray) && IsYInsideArray(currentY, tileArray))
                    {
                        //Console.WriteLine("Current Index is inside array");
                        if (colliderList.Contains(tileArray[0, currentX, currentY]))
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
                        if (colliderList.Contains(tileArray[0, currentX, currentY]))
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

        private void RemoveSimpleTileInListByIndex(int x, int y, List<SimpleTile> tileList)
        {
            Vector2 tempVector = new Vector2(x, y);

            for (int i = 0; i < tileList.Count; i++)
            {
                if (tileList[i].IndexPos == tempVector)
                {
                    tileList.RemoveAt(i);
                    break;
                }  
            }
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
    }
}
