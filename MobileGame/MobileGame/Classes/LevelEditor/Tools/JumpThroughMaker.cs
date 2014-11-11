using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using MobileGame.Managers;

namespace MobileGame.LevelEditor.Tools
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
                MakeSeveralJumpthruable(ToolManager.SelectionTopLeftIndex.X, ToolManager.SelectionTopLeftIndex.Y, ToolManager.SelectionBottomRightIndex.X, ToolManager.SelectionBottomRightIndex.Y);
                ToolManager.ClearSelection();
            }
            else if (ToolManager.HasActiveSelection && KeyMouseReader.RightClick())
            {
                MakeSeveralNotJumpthruable(ToolManager.SelectionTopLeftIndex.X, ToolManager.SelectionTopLeftIndex.Y, ToolManager.SelectionBottomRightIndex.X, ToolManager.SelectionBottomRightIndex.Y);
                ToolManager.ClearSelection();
            }
            else if (KeyMouseReader.LeftMouseDown())
                MakeJumpthruable(mouseX, mouseY);
            else if (KeyMouseReader.RightMouseDown())
                MakeNotJumpthruable(mouseX, mouseY);
        }

        private void MakeJumpthruable(int X, int Y)
        {
            if (EditorMapManager.CollisionLayer[X, Y] == 1)
                EditorMapManager.CollisionLayer[X, Y] = 2;
        }

        private void MakeSeveralJumpthruable(int startX, int startY, int endX, int endY)
        {
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (EditorMapManager.CollisionLayer[x, y] == 1)
                        EditorMapManager.CollisionLayer[x, y] = 2;
                }
            }
        }

        private void MakeNotJumpthruable(int X, int Y)
        {
            if(EditorMapManager.CollisionLayer[X, Y] == 2)
                EditorMapManager.CollisionLayer[X, Y] = 1;
        }

        private void MakeSeveralNotJumpthruable(int startX, int startY, int endX, int endY)
        {
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (EditorMapManager.CollisionLayer[x, y] == 2)
                        EditorMapManager.CollisionLayer[x, y] = 1;
                }
            }
        }
    }
}
