﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MobileGame.LevelEditor;
using MobileGame.Managers;

namespace MobileGame.LevelEditor.Tools
{
    class CopyPaster
    {
        private bool active;
        public bool Active { get { return active; } }

        private byte[,] CopiedLayerSegment;
        private int CopyWidth, CopyHeight;

        private bool showPasteTarget;
        public bool DisplayPasteTarget { get { return showPasteTarget; } }

        public CopyPaster()
        {
            active = false;
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
                MakeCopy(ToolManager.SelectionTopLeftIndex.X, ToolManager.SelectionTopLeftIndex.Y, ToolManager.SelectionBottomRightIndex.X, ToolManager.SelectionBottomRightIndex.Y, 0);

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
                        Vector2 Pos = new Vector2(x * EditorMapManager.TileSize, y * EditorMapManager.TileSize) + EditorMapManager.Offset;
                        byte value = CopiedLayerSegment[xCounter, yCounter];

                        if (value != 255)
                        {
                            int sourceX = value % 8;
                            int sourceY = value / 8;
                            Rectangle sourceRect = new Rectangle(sourceX * EditorMapManager.TileSize, sourceY * EditorMapManager.TileSize, EditorMapManager.TileSize, EditorMapManager.TileSize);

                            spritebatch.Draw(TextureManager.TileSet, Pos, sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.15f);
                        }
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

            Point topLeft = ToolManager.SelectionTopLeftIndex;
            Point bottomRight = ToolManager.SelectionBottomRightIndex;

            CopyWidth = bottomRight.X - topLeft.X;
            CopyHeight = bottomRight.Y - topLeft.Y;

            CopiedLayerSegment = new byte[CopyWidth, CopyHeight];

            for (int x = topLeft.X; x < bottomRight.X; x++)
            {
                for (int y = topLeft.Y; y < bottomRight.Y; y++)
                {
                    CopiedLayerSegment[xCounter, yCounter] = EditorMapManager.SelectedLayer[x, y];

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
                        EditorMapManager.SelectedLayer[x, y] = CopiedLayerSegment[xCounter, yCounter];
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
            if (X < EditorMapManager.SelectedLayer.GetLowerBound(0) || X > EditorMapManager.SelectedLayer.GetUpperBound(0))
                return false;

            return true;
        }

        private bool IsYInsideArray(int Y)
        {
            if (Y < EditorMapManager.SelectedLayer.GetLowerBound(1) || Y > EditorMapManager.SelectedLayer.GetUpperBound(1))
                return false;

            return true;
        }
    }
}
