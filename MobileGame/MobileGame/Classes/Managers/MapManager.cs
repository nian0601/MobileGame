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
        private int[,] currentMap;
        private Tile[,] tileArray;
        private List<Tile> colliderList;
        private int tileSize;


        public MapManager()
        {
            tileSize = TextureManager.PlatformTile.Height;
            colliderList = new List<Tile>();

            LoadLevel();

            Console.WriteLine(colliderList.Count);
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < tileArray.GetLength(0); x++)
            {
                for (int y = 0; y < tileArray.GetLength(1); y++)
                {
                    tileArray[x, y].Draw(spriteBatch, ConvertIndexToPixels(x, y));
                }
            }
        }

        public List<Tile> ColliderList
        {
            get
            {
                return colliderList;
            }
        }

        //Should expand this function in the future to include reading a level from file and such
        private void LoadLevel()
        {
            currentMap = new int[,] { 
                                        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
                                        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 1},
                                        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
                                        {1, 0, 1, 1, 1, 1, 0, 0, 1, 0, 1, 0, 1, 1, 1, 1, 0, 0, 0, 1},
                                        {1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1},
                                        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                        {1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
                                        {1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                        {1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 2, 2, 1, 0, 0, 0, 0, 0, 1},
                                        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                    };

            BuildLevel(currentMap);
        }

        private void BuildLevel(int[,] level)
        {
            int[,] levelToBuild = level;

            int mapHeight = levelToBuild.GetLength(0);
            int mapWidth = levelToBuild.GetLength(1);
            tileArray = new Tile[mapWidth, mapHeight];

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    int tileType = levelToBuild[y, x];

                    Tile tempTile = new Tile(x, y, tileType);

                    tileArray[x, y] = tempTile;

                    if (tileType != 0)
                        colliderList.Add(tempTile);
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
