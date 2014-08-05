using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using LevelEditor.Managers;

namespace LevelEditor.Tools
{
    class TileCreator
    {
        private Tile[, ,] TileArray;

        public TileCreator(Tile[, ,] tileArray)
        {
            TileArray = tileArray;
        }

        public void Update(int mouseX, int mouseY)
        {
            if (ToolManager.HasActiveSelection && KeyMouseReader.LeftClick())
            {
                CreatePlatforms(ToolManager.SelectionTopLeft.X, ToolManager.SelectionTopLeft.Y, ToolManager.SelectionBottomRight.X, ToolManager.SelectionBottomRight.Y, 0, MapManager.SelectedTileValue);
                ToolManager.ClearSelection();
            }
            else if (KeyMouseReader.LeftMouseDown())
                CreatePlatform(mouseX, mouseY, MapManager.SelectedTileValue);
            else if (ToolManager.HasActiveSelection && KeyMouseReader.RightClick())
            {
                CreateAirs(ToolManager.SelectionTopLeft.X, ToolManager.SelectionTopLeft.Y, ToolManager.SelectionBottomRight.X, ToolManager.SelectionBottomRight.Y, 0, MapManager.SelectedTileValue);
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
            if (TileArray[1, X, Y] is GoalTile)
                MapManager.GoalPlaced = false;
            if (TileArray[1, X, Y] is PlayerTile)
                MapManager.PlayerPlaced = false;

            TileArray[0, X, Y] = new Tile(X, Y, false);
            TileArray[1, X, Y] = new Tile(X, Y, false);
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
            Tile temp = new Tile(X, Y, true);
            temp.SetTileType(TileValue);

            TileArray[0, X, Y] = temp;
        }

        private void CreatePlatforms(int startX, int startY, int endX, int endY, int layer, int TileValue)
        {
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    Tile temp = new Tile(x, y, true);
                    temp.SetTileType(TileValue);

                    TileArray[layer, x, y] = temp;
                }
            }
        }

        private void CreateJumpTile(int X, int Y)
        {
            TileArray[1, X, Y] = new JumpTile(X, Y, true);
        }

        private void CreateTeleportTile(int X, int Y)
        {
            TileArray[1, X, Y] = new TeleportTile(X, Y, true);
        }

        private void CreateGoalTile(int X, int Y)
        {
            if (!MapManager.GoalPlaced)
            {
                TileArray[1, X, Y] = new GoalTile(X, Y, true);
                MapManager.GoalPlaced = true;
            }
                
        }

        private void CreatePlayerSpawn(int X, int Y)
        {
            if (!MapManager.PlayerPlaced)
            {
                TileArray[1, X, Y] = new PlayerTile(X, Y, true);
                MapManager.PlayerPlaced = true;
            }
                
        }

        private void CreateEnemy(int X, int Y)
        {
            TileArray[1, X, Y] = new EnemyTile(X, Y, true);
        }
    }
}
