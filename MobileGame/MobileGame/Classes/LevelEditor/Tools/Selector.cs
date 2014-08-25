using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MobileGame.Managers;

namespace MobileGame.LevelEditor.Tools
{
    class Selector
    {
        private Point firstPoint;

        private Point secondPoint;

        private bool pointOnePlaced;

        private bool pointTwoPlaced;

        public Selector()
        {
            Reset();
        }

        public void Update()
        {
            if (KeyMouseReader.LeftMouseDown() && !pointOnePlaced)
            {
                firstPoint = KeyMouseReader.GetMousePos();
                firstPoint.X *= EditorMapManager.TileSize;
                firstPoint.X += (int)EditorMapManager.Offset.X;
                firstPoint.Y *= EditorMapManager.TileSize;
                firstPoint.Y += (int)EditorMapManager.Offset.Y;

                pointOnePlaced = true;
            }

            if (KeyMouseReader.LeftMouseDown() && pointOnePlaced)
            {
                secondPoint = KeyMouseReader.GetMousePos();
                secondPoint.X *= EditorMapManager.TileSize;
                secondPoint.X += (int)EditorMapManager.Offset.X;
                secondPoint.Y *= EditorMapManager.TileSize;
                secondPoint.Y += (int)EditorMapManager.Offset.Y;

            }
            else if (!KeyMouseReader.LeftMouseDown() && pointOnePlaced)
            {
                secondPoint = KeyMouseReader.GetMousePos();
                secondPoint.X *= EditorMapManager.TileSize;
                secondPoint.X += (int)EditorMapManager.Offset.X;
                secondPoint.Y *= EditorMapManager.TileSize;
                secondPoint.Y += (int)EditorMapManager.Offset.Y;
                pointTwoPlaced = true;
            }

            if (pointOnePlaced && pointTwoPlaced)
            {
                ToolManager.HasActiveSelection = true;
                ToolManager.SelectionTopLeft = ToolManager.ConvertPixelsToIndex(new Point(GetSelectionRect().Left, GetSelectionRect().Top));
                ToolManager.SelectionBottomRight = ToolManager.ConvertPixelsToIndex(new Point(GetSelectionRect().Right, GetSelectionRect().Bottom));
                Reset();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (pointOnePlaced)
            {
                Point topLeft = ToolManager.ConvertPixelsToIndex(new Point(GetSelectionRect().Left, GetSelectionRect().Top));
                Point bottomRight = ToolManager.ConvertPixelsToIndex(new Point(GetSelectionRect().Right, GetSelectionRect().Bottom));

                for (int x = topLeft.X; x < bottomRight.X; x++)
                {
                    for (int y = topLeft.Y; y < bottomRight.Y; y++)
                    {
                        Texture2D Texture = TextureManager.FilledSquare;
                        Vector2 Pos = new Vector2(x * EditorMapManager.TileSize, y * EditorMapManager.TileSize) + EditorMapManager.Offset;
                        Rectangle Source = new Rectangle((int)Pos.X, (int)Pos.Y, EditorMapManager.TileSize, EditorMapManager.TileSize);
                        Color color = Color.Red * 0.25f;

                        spriteBatch.Draw(Texture, Source, null, color, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                    }
                }
            }
            else if (ToolManager.HasActiveSelection)
            {
                Point topLeft = ToolManager.SelectionTopLeft;
                Point bottomRight = ToolManager.SelectionBottomRight;

                for (int x = topLeft.X; x < bottomRight.X; x++)
                {
                    for (int y = topLeft.Y; y < bottomRight.Y; y++)
                    {
                        Texture2D Texture = TextureManager.FilledSquare;
                        Vector2 Pos = new Vector2(x * EditorMapManager.TileSize, y * EditorMapManager.TileSize) + EditorMapManager.Offset;
                        Rectangle Source = new Rectangle((int)Pos.X, (int)Pos.Y, EditorMapManager.TileSize, EditorMapManager.TileSize);
                        Color color = Color.Red * 0.25f;

                        spriteBatch.Draw(Texture, Source, null, color, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                    }
                }
            }
        }

        public void Reset()
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
