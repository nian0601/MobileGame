using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;


namespace MobileGame.LevelEditor.Tools
{
    class ColliderMaker
    {
        public ColliderMaker()
        {
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
            if (EditorMapManager.CollisionLayer[X, Y] != 1)
                EditorMapManager.NumCollisionFlags++;

            EditorMapManager.CollisionLayer[X, Y] = 1;
        }

        private void MakeSeveralCollidable(int startX, int startY, int endX, int endY)
        {
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (EditorMapManager.CollisionLayer[x, y] != 1)
                        EditorMapManager.NumCollisionFlags++;

                    EditorMapManager.CollisionLayer[x, y] = 1;
                }
            }
        }

        private void MakeUnCollidable(int X, int Y)
        {
            if (EditorMapManager.CollisionLayer[X, Y] != 0)
                EditorMapManager.NumCollisionFlags--;

            EditorMapManager.CollisionLayer[X, Y] = 0;
        }

        private void MakeSeveralUnCollidable(int startX, int startY, int endX, int endY)
        {
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (EditorMapManager.CollisionLayer[x, y] != 0)
                        EditorMapManager.NumCollisionFlags--;

                    EditorMapManager.CollisionLayer[x, y] = 0;
                }
            }
        }
    }
}
