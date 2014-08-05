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
    class Selector
    {
        private Point firstPoint;

        private Point secondPoint;

        private bool pointOnePlaced;

        private bool pointTwoPlaced;

        private Tile[, ,] TileArray;

        public Selector(Tile[, ,] tileArray)
        {
            TileArray = tileArray;
            Reset();
        }

        public void Update()
        {
            if (KeyMouseReader.LeftMouseDown() && !pointOnePlaced)
            {
                firstPoint = KeyMouseReader.GetMousePos();
                firstPoint.X += MapManager.xOffset * MapManager.TileSize;
                firstPoint.Y += MapManager.yOffset * MapManager.TileSize;
                pointOnePlaced = true;
            }

            if (KeyMouseReader.LeftMouseDown() && pointOnePlaced)
            {
                secondPoint = KeyMouseReader.GetMousePos();
                secondPoint.X += MapManager.xOffset * MapManager.TileSize;
                secondPoint.Y += MapManager.yOffset * MapManager.TileSize;
                
            }
            else if (!KeyMouseReader.LeftMouseDown() && pointOnePlaced)
            {
                secondPoint = KeyMouseReader.GetMousePos();
                secondPoint.X += MapManager.xOffset * MapManager.TileSize;
                secondPoint.Y += MapManager.yOffset * MapManager.TileSize;
                pointTwoPlaced = true;
            }

            if (pointOnePlaced)
            {
                Point topLeft = ToolManager.ConvertPixelsToIndex(new Point(GetSelectionRect().Left, GetSelectionRect().Top));
                Point bottomRight = ToolManager.ConvertPixelsToIndex(new Point(GetSelectionRect().Right, GetSelectionRect().Bottom));

                ToolManager.ClearSelection();

                for (int x = topLeft.X; x < bottomRight.X; x++)
                {
                    for (int y = topLeft.Y; y < bottomRight.Y; y++)
                    {
                        TileArray[0, x, y].Selected = true;
                    }
                }

                if (pointTwoPlaced)
                {
                    ToolManager.HasActiveSelection = true;
                    ToolManager.SelectionTopLeft = ToolManager.ConvertPixelsToIndex(new Point(GetSelectionRect().Left, GetSelectionRect().Top));
                    ToolManager.SelectionBottomRight = ToolManager.ConvertPixelsToIndex(new Point(GetSelectionRect().Right, GetSelectionRect().Bottom));
                    Reset();
                }
            }
        }

        private void Reset()
        {
            firstPoint = new Point(0, 0);
            secondPoint = new Point(0, 0);

            pointOnePlaced = false;
            pointTwoPlaced = false;
        }

        private Rectangle GetSelectionRect()
        {
            Point topLeft = new Point();

            if (firstPoint.X < secondPoint.X)
                topLeft.X = firstPoint.X;
            else
                topLeft.X = secondPoint.X;

            if (firstPoint.Y < secondPoint.Y)
                topLeft.Y = firstPoint.Y;
            else
                topLeft.Y = secondPoint.Y;

            int width = Math.Abs(secondPoint.X - firstPoint.X);
            int height = Math.Abs(secondPoint.Y - firstPoint.Y);

            return new Rectangle(topLeft.X, topLeft.Y, width, height);
        }
    }
}