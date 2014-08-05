﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using LevelEditor.Managers;

namespace LevelEditor.Tools
{
    class CopyPaster
    {
        private bool active;
        public bool Active { get { return active; } }

        private Tile[, ,] TileArray;

        private Tile[,] CopiedArraySegment;
        private int CopyWidth, CopyHeight;

        private bool showPasteTarget;
        public bool DisplayPasteTarget { get { return showPasteTarget; } }

        public CopyPaster(Tile[, ,] tileArray)
        {
            active = false;
            TileArray = tileArray;
            CopyWidth = 0;
            CopyHeight = 0;
        }

        public void Update(int mouseX, int mouseY)
        {
            active = false;

            if (showPasteTarget && KeyMouseReader.LeftClick())
            {
                Console.WriteLine("Try to paste saved selection");
                showPasteTarget = false;
                Paste(mouseX, mouseY, 0);

                active = true;
            }
            else if (showPasteTarget && KeyMouseReader.RightClick())
            {
                Console.WriteLine("Cancel copy process");
                showPasteTarget = false;

                active = true;
            }
            else if (ToolManager.HasActiveSelection && KeyMouseReader.isKeyDown(Keys.LeftControl) && KeyMouseReader.KeyClick(Keys.C))
            {
                Console.WriteLine("Try to save current selection");
                MakeCopy(ToolManager.SelectionTopLeft.X, ToolManager.SelectionTopLeft.Y, ToolManager.SelectionBottomRight.X, ToolManager.SelectionBottomRight.Y, 0);

                active = true;
            }
            else if (KeyMouseReader.isKeyDown(Keys.LeftControl) && KeyMouseReader.KeyClick(Keys.V))
            {
                Console.WriteLine("Showing the saved selection");
                showPasteTarget = true;
                //This is used in the drawfunction to then draw the tiles that we are trying to paste

                active = true;
            }
        }

        public void ShowPasteTarget(int startX, int startY, int layer, SpriteBatch spritebatch)
        {
            int xCounter = 0;
            int yCounter = 0;

            int endX = startX + CopyWidth;
            int endY = startY + CopyHeight;

            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (IsXInsideArray(x) && IsYInsideArray(y))
                    {
                        CopiedArraySegment[xCounter, yCounter].Draw(spritebatch, x - MapManager.xOffset, y - MapManager.yOffset, MapManager.Offset);
                    }

                    yCounter++;
                }
                yCounter = 0;
                xCounter++;
            }
        }

        private void MakeCopy(int startX, int startY, int endX, int endY, int layer)
        {
            int xCounter = 0;
            int yCounter = 0;

            Point topLeft = ToolManager.SelectionTopLeft;
            Point bottomRight = ToolManager.SelectionBottomRight;

            CopyWidth = bottomRight.X - topLeft.X;
            CopyHeight = bottomRight.Y - topLeft.Y;

            CopiedArraySegment = new Tile[CopyWidth, CopyHeight];

            for (int x = topLeft.X; x < bottomRight.X; x++)
            {
                for (int y = topLeft.Y; y < bottomRight.Y; y++)
                {
                    Tile TempTile = new Tile(x, y, false);
                    
                    TempTile.Collidable = TileArray[layer, x, y].Collidable;
                    TempTile.CanJumpThrough = TileArray[layer, x, y].CanJumpThrough;
                    TempTile.TileType = TileArray[layer, x, y].TileType;
                    TempTile.Selected = false;

                    if (TempTile.TileType != 0)
                    {
                        TempTile.SetTileType(TileArray[layer, x, y].TileValue);
                        TempTile.ShouldDraw = true;
                    }
                    else if (TempTile.TileType == 0)
                    {
                        TempTile.SetTileType(15);
                    }

                    CopiedArraySegment[xCounter, yCounter] = TempTile;

                    yCounter++;
                }
                yCounter = 0;
                xCounter++;
            }

            Console.WriteLine("Made a copy of the current selection");
        }

        private void Paste(int startX, int startY, int layer)
        {
            int xCounter = 0;
            int yCounter = 0;

            int endX = startX + CopyWidth;
            int endY = startY + CopyHeight;

            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (IsXInsideArray(x) && IsYInsideArray(y))
                    {
                        Tile TempTile = new Tile(x, y, false);

                        TempTile.Collidable = CopiedArraySegment[xCounter, yCounter].Collidable;
                        TempTile.CanJumpThrough = CopiedArraySegment[xCounter, yCounter].CanJumpThrough;
                        TempTile.TileType = CopiedArraySegment[xCounter, yCounter].TileType;
                        TempTile.Selected = false;

                        if (TempTile.TileType != 0)
                        {
                            TempTile.SetTileType(CopiedArraySegment[xCounter, yCounter].TileValue);
                            TempTile.ShouldDraw = true;
                        }
                        else if (TempTile.TileType == 0)
                        {
                            TempTile.SetTileType(15);
                        }

                        TileArray[layer, x, y] = TempTile;

                    }
                    
                    yCounter++;
                }
                yCounter = 0;
                xCounter++;
            }

            Console.WriteLine("Pasted the saved selection");
        }

        private bool IsXInsideArray(int X)
        {
            if (X < TileArray.GetLowerBound(1) || X > TileArray.GetUpperBound(1))
                return false;

            return true;
        }

        private bool IsYInsideArray(int Y)
        {
            if (Y < TileArray.GetLowerBound(2) || Y > TileArray.GetUpperBound(2))
                return false;

            return true;
        }
    }
}