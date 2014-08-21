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
        public JumpThroughMaker()
        {
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
            if (MapManager.CollisionLayer[X, Y] == 1)
                MapManager.CollisionLayer[X, Y] = 2;
        }

        private void MakeSeveralJumpthruable(int startX, int startY, int endX, int endY)
        {
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (MapManager.CollisionLayer[x, y] == 1)
                        MapManager.CollisionLayer[x, y] = 2;
                }
            }
        }

        private void MakeNotJumpthruable(int X, int Y)
        {
            if(MapManager.CollisionLayer[X, Y] == 2)
                MapManager.CollisionLayer[X, Y] = 1;
        }

        private void MakeSeveralNotJumpthruable(int startX, int startY, int endX, int endY)
        {
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (MapManager.CollisionLayer[x, y] == 2)
                        MapManager.CollisionLayer[x, y] = 1;
                }
            }
        }
    }
}
