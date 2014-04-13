using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


        public MapManager()
        {
            tileSize = TextureManager.PlatformTile.Height;
            colliderList = new List<SimpleTile>();
            specialBlockList = new List<SpecialTile>();

            LoadLevel();
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

        //Should expand this function in the future to include reading a level from file and such
        private void LoadLevel()
        {
            currentMap = new int[,,] {
                                        {
                                            {13, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 12},
                                            {6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
                                            {6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
                                            {6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 0, 0, 0, 0, 0, 4},
                                            {6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 9, 0, 0, 0, 0, 0, 4},
                                            {6, 0, 1, 2, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
                                            {6, 0, 7, 8, 8, 9, 0, 0, 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
                                            {6, 0, 0, 0, 0, 0, 0, 0, 4, 11, 2, 2, 3, 0, 0, 0, 0, 0, 0, 4},
                                            {6, 0, 0, 0, 0, 0, 0, 0, 7, 8, 8, 8, 9, 0, 0, 1, 3, 0, 0, 4},
                                            {6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 6, 0, 0, 4},
                                            {6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 6, 0, 0, 4},
                                            {11, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 10},
                                        },
                                        {
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
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
                        EnemyManager.AddEnemy(new ShootingEnemy(x, y));   
                }
            }

            FindConnectedTiles(new Vector2(0, 11));
        }

        private Vector2 ConvertIndexToPixels(int X, int Y)
        {
            int x = X * tileSize;
            int y = Y * tileSize;

            return new Vector2(x, y);
        }

        private Point ConvertPixelsToIndex(Vector2 pos)
        {
            int x = (int)pos.X / tileSize;
            int y = (int)pos.Y / tileSize;

            return new Point(x, y);
        }

        //This function tries to determine what texture all of the platforms should use, so that they fit together and form proper platforms
        private void AssignTileTypes()
        {
            for (int i = 0; i < colliderList.Count; i++)
            {

            }
        }

        private void FindConnectedTiles(Vector2 centerIndex)
        {
            List<int> tempList = new List<int>();

            //These variables will be used to check if we are trying to look at tiles that are outside the array-index
            //If that is the case we will treat that "tile" as air, since air dosnt affect which kind of texture we should use
            int xMax = 1;
            int xMin = -1;
            int yMax = 1;
            int yMin = -1;

            
            if (centerIndex.X == 0)
            {
                xMin = 0;
                xMax = 1;
            }
            else if (centerIndex.X == tileArray.GetUpperBound(1))
            {
                xMin = -1;
                xMax = 0;
            }

            if (centerIndex.Y == 0)
            {
                yMin = 0;
                yMax = 1;
            }
            else if (centerIndex.Y == tileArray.GetUpperBound(2))
            {
                yMin = -1;
                yMax = 0;
            }

            //Loops through the 9 tiles that form a 3x3 square around the centerIndex (included the centerTile in the loop)
            //If we find a tile that is air or a tile that is outside the arrayIndex we add a "0" to the list
            //If we find a platformtile we add a "1" to the list
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        Console.WriteLine("Found centertile");
                    }
                    else if (x > xMax || x < xMin)
                    {
                        Console.WriteLine("Outside x-bounds");
                        tempList.Add(0);
                    }
                    else if (y > yMax || y < yMin)
                    {
                        Console.WriteLine("Outside y-bounds");
                        tempList.Add(0);
                    }
                    else if (colliderList.Contains(tileArray[0, (int)centerIndex.X + x, (int)centerIndex.Y + y]))
                    {
                        Console.WriteLine("Found platform-tile");
                        tempList.Add(1);
                    }
                    else
                    {
                        Console.WriteLine("Found empty tile");
                        tempList.Add(0);
                    }
                }
            }

            Console.WriteLine(tempList.Count);
        }
    }
}
