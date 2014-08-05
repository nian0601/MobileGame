using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using LevelEditor.Managers;

namespace LevelEditor.Tools
{
    class JumpThroughMaker
    {
        private Tile[, ,] TileArray;

        public JumpThroughMaker(Tile[, ,] tileArray)
        {
            TileArray = tileArray;
        }

        public void Update(int mouseX, int mouseY)
        {
            if (ToolManager.HasActiveSelection && KeyMouseReader.LeftClick())
            {
                MakeSeveralJumpthruable(ToolManager.SelectionTopLeft.X, ToolManager.SelectionTopLeft.Y, ToolManager.SelectionBottomRight.X, ToolManager.SelectionBottomRight.Y);
                ToolManager.ClearSelection();
            }
            else if (ToolManager.HasActiveSelection && KeyMouseReader.RightClick())
            {
                MakeSeveralNotJumpthruable(ToolManager.SelectionTopLeft.X, ToolManager.SelectionTopLeft.Y, ToolManager.SelectionBottomRight.X, ToolManager.SelectionBottomRight.Y);
                ToolManager.ClearSelection();
            }
            else if (KeyMouseReader.LeftMouseDown())
                MakeJumpthruable(mouseX, mouseY);
            else if (KeyMouseReader.RightMouseDown())
                MakeNotJumpthruable(mouseX, mouseY);
        }

        private void MakeJumpthruable(int X, int Y)
        {
            TileArray[0, X, Y].CanJumpThrough = true;
        }

        private void MakeSeveralJumpthruable(int startX, int startY, int endX, int endY)
        {
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if(TileArray[0, x, y].Collidable)
                        TileArray[0, x, y].CanJumpThrough = true;
                }
            }
        }

        private void MakeNotJumpthruable(int X, int Y)
        {
            TileArray[0, X, Y].CanJumpThrough = false;
        }

        private void MakeSeveralNotJumpthruable(int startX, int startY, int endX, int endY)
        {
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    TileArray[0, x, y].CanJumpThrough = false;
                }
            }
        }
    }
}
