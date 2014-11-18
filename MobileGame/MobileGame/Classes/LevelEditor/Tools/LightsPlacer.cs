using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using MobileGame.LightingSystem;
using MobileGame.Managers;

namespace MobileGame.LevelEditor.Tools
{
    class LightsPlacer
    {
        private List<Rectangle> pointLightHitBoxes;
        private List<Rectangle> ambientLightHitBoxes;
        private List<Rectangle> spotLightHitBoxes;


        public LightsPlacer()
        {
            pointLightHitBoxes = new List<Rectangle>();
            ambientLightHitBoxes = new List<Rectangle>();
            spotLightHitBoxes = new List<Rectangle>();
        }

        public void Init(LightRenderer aLightRenderer)
        {
            pointLightHitBoxes.Clear();
            for (int i = 0; i < aLightRenderer.pointLights.Count; i++)
            {
                Rectangle hitBox = new Rectangle((int)aLightRenderer.pointLights[i].Position.X - 10, (int)aLightRenderer.pointLights[i].Position.Y - 10, 20, 20);
                pointLightHitBoxes.Add(hitBox);
            }


            spotLightHitBoxes.Clear();
            for(int i = 0; i < aLightRenderer.spotLights.Count; i++)
            {
                Rectangle hitBox = new Rectangle((int)aLightRenderer.spotLights[i].Position.X - 10, (int)aLightRenderer.spotLights[i].Position.Y - 10, 20, 20);
                spotLightHitBoxes.Add(hitBox);
            }

            //ambientLightHitBoxes.Clear();
            //int startX = 10;
            //int startY = 10;
            //for (int i = 0; i < LightingManager.AmbientLights.Count; i++)
            //{
            //    startX += startX * 0 + 40;

            //    Rectangle hitBox = new Rectangle(startX + (int)EditorMapManager.Offset.X, startY + (int)EditorMapManager.Offset.Y, 20, 20);
            //    ambientLightHitBoxes.Add(hitBox);
            //}
        }

        public void Update(LightRenderer aLightRenderer)
        {
            //POINTLIGHTS
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

                PointLight tempPointLight = new PointLight(pos, 1f, radius, EditorScreen.ColorPicker.SelectedColor);
                aLightRenderer.pointLights.Add(tempPointLight);


                Rectangle hitBox = new Rectangle((int)tempPointLight.Position.X - 10, (int)tempPointLight.Position.Y - 10, 20, 20);
                pointLightHitBoxes.Add(hitBox);
            }
            else if (KeyMouseReader.KeyClick(Keys.Q))
            {
                //LightingManager.AmbientLights.Add(new AmbientLight(0, 0, EditorMapManager.NumXTiles * 20, EditorMapManager.NumYTiles * 20, EditorScreen.ColorPicker.SelectedColor, 0.5f));

                //int x = 10 + (LightingManager.AmbientLights.Count * 30);
                //int y = 10;

                //Rectangle hitBox = new Rectangle(x + (int)EditorMapManager.Offset.X, y + (int)EditorMapManager.Offset.Y, 20, 20);
                //ambientLightHitBoxes.Add(hitBox);
            }

            else if (KeyMouseReader.LeftClick())
            {
                Vector2 mousePos = new Vector2(KeyMouseReader.GetMousePos().X, KeyMouseReader.GetMousePos().Y);
                Rectangle hitBox = new Rectangle((int)mousePos.X - 10, (int)mousePos.Y - 10, 20, 20);
                PointLight tempPointLight = new PointLight(mousePos, 1f, 250f, EditorScreen.ColorPicker.SelectedColor);
                aLightRenderer.pointLights.Add(tempPointLight);
                pointLightHitBoxes.Add(hitBox);
            }
            else if (KeyMouseReader.RightClick())
            {
                RemovePointLight(KeyMouseReader.GetMousePos(), aLightRenderer);
                RemoveAmbientLight(KeyMouseReader.GetMousePos(), aLightRenderer);
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

        private void RemovePointLight(Point aMousePos, LightRenderer aLightRenderer)
        {
            for (int i = 0; i < pointLightHitBoxes.Count; i++)
            {
                if (pointLightHitBoxes[i].Contains(aMousePos))
                {
                    aLightRenderer.pointLights.RemoveAt(i);
                    pointLightHitBoxes.RemoveAt(i);
                    return;
                }
            }
        }

        private void RemoveAmbientLight(Point aMousePos, LightRenderer aLightRenderer)
        {
            for (int i = 0; i < ambientLightHitBoxes.Count; i++)
            {
                if (ambientLightHitBoxes[i].Contains(aMousePos))
                {
                    //LightingManager.AmbientLights.RemoveAt(i);
                    ambientLightHitBoxes.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
