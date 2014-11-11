using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using MobileGame.Managers;

namespace MobileGame.LevelEditor.Tools
{
    class TileCreator
    {
        public TileCreator() {}

        public void Update(int mouseX, int mouseY)
        {
            if (ToolManager.HasActiveSelection && KeyMouseReader.LeftClick())
            {
                Point pixelOffset = new Point((int)EditorMapManager.Offset.X, (int)EditorMapManager.Offset.Y);
                Point indexOffset = ToolManager.ConvertPixelsToIndex(pixelOffset);

                CreatePlatforms(ToolManager.SelectionTopLeftIndex.X + indexOffset.X, ToolManager.SelectionTopLeftIndex.Y + indexOffset.Y, ToolManager.SelectionBottomRightIndex.X + indexOffset.X, ToolManager.SelectionBottomRightIndex.Y + indexOffset.Y, 0, EditorMapManager.SelectedTileValue);
                ToolManager.ClearSelection();
            }
            else if (KeyMouseReader.LeftMouseDown())
                CreatePlatform(mouseX, mouseY, EditorMapManager.SelectedTileValue);
            else if (ToolManager.HasActiveSelection && KeyMouseReader.RightClick())
            {
                CreateAirs(ToolManager.SelectionTopLeftIndex.X, ToolManager.SelectionTopLeftIndex.Y, ToolManager.SelectionBottomRightIndex.X, ToolManager.SelectionBottomRightIndex.Y, 0, EditorMapManager.SelectedTileValue);
                ToolManager.ClearSelection();
            }
            else if (KeyMouseReader.RightMouseDown())
                CreateAir(mouseX, mouseY);
            else if (KeyMouseReader.KeyClick(Keys.J))
                CreateJumpTile(mouseX, mouseY);
            else if (KeyMouseReader.KeyClick(Keys.T))
                CreateTeleportTile(mouseX, mouseY);
            else if (KeyMouseReader.KeyClick(Keys.E))
                CreateEnemy(mouseX, mouseY);
            else if (KeyMouseReader.KeyClick(Keys.G))
                CreateGoalTile(mouseX, mouseY);
            else if (KeyMouseReader.KeyClick(Keys.P))
                CreatePlayerSpawn(mouseX, mouseY);
        }

        private void CreateAir(int X, int Y)
        {
            if (EditorMapManager.SelectedLayer[X, Y] == 26)
                EditorMapManager.GoalPlaced = false;
            if (EditorMapManager.SelectedLayer[X, Y] == 29)
                EditorMapManager.PlayerPlaced = false;

            EditorMapManager.SelectedLayer[X, Y] = 255;
        }

        private void CreateAirs(int startX, int startY, int endX, int endY, int layer, int TileValue)
        {
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    CreateAir(x, y);
                }
            }
        }

        private void CreatePlatform(int X, int Y, int TileValue)
        {
            if (TileValue == 26)
            {
                CreateGoalTile(X, Y);
            }
            else if (TileValue == 29)
            {
                CreatePlayerSpawn(X, Y);
            }
            else
                EditorMapManager.SelectedLayer[X, Y] = (byte)TileValue;
        }

        private void CreatePlatforms(int startX, int startY, int endX, int endY, int layer, int TileValue)
        {
            if (TileValue != 17 && TileValue != 29)
            {
                for (int x = startX; x < endX; x++)
                {
                    for (int y = startY; y < endY; y++)
                    {
                        EditorMapManager.SelectedLayer[x, y] = (byte)TileValue;
                    }
                }
            }

        }

        private void CreateJumpTile(int X, int Y)
        {
            EditorMapManager.SelectedLayer[X, Y] = 25;
        }

        private void CreateTeleportTile(int X, int Y)
        {
            EditorMapManager.SelectedLayer[X, Y] = 31;
        }

        private void CreateGoalTile(int X, int Y)
        {
            if (!EditorMapManager.GoalPlaced)
            {
                EditorMapManager.SelectedLayer[X, Y] = 26;
                EditorMapManager.GoalPlaced = true;
            }
        }

        private void CreatePlayerSpawn(int X, int Y)
        {
            if (!EditorMapManager.PlayerPlaced)
            {
                EditorMapManager.SelectedLayer[X, Y] = 29;
                EditorMapManager.PlayerPlaced = true;
            }

        }

        private void CreateEnemy(int X, int Y)
        {
            EditorMapManager.SelectedLayer[X, Y] = 30;
        }
    }
}
