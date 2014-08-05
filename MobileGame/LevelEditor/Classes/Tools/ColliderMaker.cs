using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using LevelEditor.Managers;

namespace LevelEditor.Tools
{
    class ColliderMaker
    {
        private Tile[, ,] TileArray;

        public ColliderMaker(Tile[, ,] tileArray)
        {
            TileArray = tileArray;
        }

        public void Update(int mouseX, int mouseY)
        {
            if (ToolManager.HasActiveSelection && KeyMouseReader.LeftClick())
            {
                MakeSeveralCollidable(ToolManager.SelectionTopLeft.X, ToolManager.SelectionTopLeft.Y, ToolManager.SelectionBottomRight.X, ToolManager.SelectionBottomRight.Y);
                ToolManager.ClearSelection();
            }
            else if (ToolManager.HasActiveSelection && KeyMouseReader.RightClick())
            {
                MakeSeveralUnCollidable(ToolManager.SelectionTopLeft.X, ToolManager.SelectionTopLeft.Y, ToolManager.SelectionBottomRight.X, ToolManager.SelectionBottomRight.Y);
                ToolManager.ClearSelection();
            }
            else if (KeyMouseReader.LeftMouseDown())
                MakeCollidable(mouseX, mouseY);
            else if (KeyMouseReader.RightMouseDown())
                MakeUnCollidable(mouseX, mouseY);
        }

        private void MakeCollidable(int X, int Y)
        {
            TileArray[0, X, Y].Collidable = true;
        }

        private void MakeSeveralCollidable(int startX, int startY, int endX, int endY)
        {
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    TileArray[0, x, y].Collidable = true;
                }
            }
        }

        private void MakeUnCollidable(int X, int Y)
        {
            TileArray[0, X, Y].Collidable = false;
        }

        private void MakeSeveralUnCollidable(int startX, int startY, int endX, int endY)
        {
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    TileArray[0, x, y].Collidable = false;
                }
            }
        }
    }
}
