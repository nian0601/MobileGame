using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using MobileGame.Lights;
using MobileGame.Managers;

namespace MobileGame.LevelEditor.Tools
{
    class LightsPlacer
    {
        private List<Rectangle> pointLightHitBoxes;
        private List<Rectangle> ambientLightHitBoxes;


        public LightsPlacer()
        {
            pointLightHitBoxes = new List<Rectangle>();
            ambientLightHitBoxes = new List<Rectangle>();
        }

        public void Init()
        {
            pointLightHitBoxes.Clear();
            for (int i = 0; i < LightingManager.PointLights.Count; i++)
            {
                Rectangle hitBox = new Rectangle((int)LightingManager.PointLights[i].Position.X - 10 + (int)EditorMapManager.Offset.X, (int)LightingManager.PointLights[i].Position.Y - 10 + (int)EditorMapManager.Offset.Y, 20, 20);
                pointLightHitBoxes.Add(hitBox);
            }

            ambientLightHitBoxes.Clear();
            int startX = 10;
            int startY = 10;
            for (int i = 0; i < LightingManager.AmbientLights.Count; i++)
            {
                startX += startX * 0 + 40;

                Rectangle hitBox = new Rectangle(startX + (int)EditorMapManager.Offset.X, startY + (int)EditorMapManager.Offset.Y, 20, 20);
                ambientLightHitBoxes.Add(hitBox);
            }
        }

        public void Update()
        {
            if (ToolManager.HasActiveSelection && KeyMouseReader.LeftClick())
            {
                int x1 = ToolManager.SelectionTopLeftIndex.X * EditorMapManager.TileSize;
                int y1 = ToolManager.SelectionTopLeftIndex.Y * EditorMapManager.TileSize;

                int x2 = ToolManager.SelectionBottomRightIndex.X * EditorMapManager.TileSize;
                int y2 = ToolManager.SelectionBottomRightIndex.Y * EditorMapManager.TileSize;

                int centerx = x1 + ((x2 - x1) / 2);
                int centery = y1 + ((y2 - y1) / 2);

                float radius = (x2 - x1) / 2;
                Vector2 pos = new Vector2(centerx, centery);

                PointLight tempLight = new PointLight(pos, radius, 0.5f, EditorScreen.ColorPicker.SelectedColor, false);
                LightingManager.PointLights.Add(tempLight);


                Rectangle hitBox = new Rectangle((int)tempLight.Position.X - 10 + (int)EditorMapManager.Offset.X, (int)tempLight.Position.Y - 10 + (int)EditorMapManager.Offset.Y, 20, 20);
                pointLightHitBoxes.Add(hitBox);
            }
            else if (KeyMouseReader.KeyClick(Keys.Q))
            {
                LightingManager.AmbientLights.Add(new AmbientLight(0, 0, EditorMapManager.NumXTiles * 20, EditorMapManager.NumYTiles * 20, EditorScreen.ColorPicker.SelectedColor, 0.5f));

                int x = 10 + (LightingManager.AmbientLights.Count * 30);
                int y = 10;

                Rectangle hitBox = new Rectangle(x + (int)EditorMapManager.Offset.X, y + (int)EditorMapManager.Offset.Y, 20, 20);
                ambientLightHitBoxes.Add(hitBox);
            }

            else if (KeyMouseReader.LeftClick())
            {
                Vector2 mousePos = new Vector2(KeyMouseReader.GetMousePos().X, KeyMouseReader.GetMousePos().Y) - EditorMapManager.Offset;
                Rectangle hitBox = new Rectangle((int)mousePos.X - 10 + (int)EditorMapManager.Offset.X, (int)mousePos.Y - 10 + (int)EditorMapManager.Offset.Y, 20, 20);
                PointLight tempLight = new PointLight(mousePos, 300, 0.5f, EditorScreen.ColorPicker.SelectedColor, false);
                LightingManager.PointLights.Add(tempLight);
                pointLightHitBoxes.Add(hitBox);
            }
            else if (KeyMouseReader.RightClick())
            {
                RemovePointLight(KeyMouseReader.GetMousePos());
                RemoveAmbientLight(KeyMouseReader.GetMousePos());
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < pointLightHitBoxes.Count; i++)
            {
                spriteBatch.Draw(TextureManager.FilledSquare, pointLightHitBoxes[i], Color.White);
            }

            for (int i = 0; i < ambientLightHitBoxes.Count; i++)
            {
                spriteBatch.Draw(TextureManager.FilledSquare, ambientLightHitBoxes[i], Color.White);
            }
        }

        private void RemovePointLight(Point aMousePos)
        {
            for (int i = 0; i < pointLightHitBoxes.Count; i++)
            {
                if (pointLightHitBoxes[i].Contains(aMousePos))
                {
                    LightingManager.PointLights.RemoveAt(i);
                    pointLightHitBoxes.RemoveAt(i);
                    return;
                }
            }
        }

        private void RemoveAmbientLight(Point aMousePos)
        {
            for (int i = 0; i < ambientLightHitBoxes.Count; i++)
            {
                if (ambientLightHitBoxes[i].Contains(aMousePos))
                {
                    LightingManager.AmbientLights.RemoveAt(i);
                    ambientLightHitBoxes.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
