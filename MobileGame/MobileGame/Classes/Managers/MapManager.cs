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
        private List<Enemy> enemyList;
        private int tileSize;


        public MapManager()
        {
            tileSize = TextureManager.PlatformTile.Height;
            colliderList = new List<SimpleTile>();
            specialBlockList = new List<SpecialTile>();
            enemyList = new List<Enemy>();

            LoadLevel();

            Console.WriteLine(colliderList.Count);
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

        public List<Enemy> EnemiesList
        {
            get
            {
                return enemyList;
            }
        }

        //Should expand this function in the future to include reading a level from file and such
        private void LoadLevel()
        {
            currentMap = new int[,,] {
                                        {
                                            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
                                            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 1},
                                            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
                                            {1, 0, 1, 1, 1, 1, 0, 0, 1, 0, 1, 0, 1, 1, 1, 1, 0, 0, 0, 1},
                                            {1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1},
                                            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                            {1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
                                            {1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                            {1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1},
                                            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                        },
                                        {
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            {0, 0, 4, 0, 3, 0, 4, 0, 3, 0, 0, 0, 0, 4, 0, 0, 0, 2, 0, 0},
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
                        enemyList.Add(new Enemy(x, y));
                    else if (tileType == 4)
                        specialBlockList.Add(new EnemyCollideTile(x, y));
                }
            }

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
    }
}
