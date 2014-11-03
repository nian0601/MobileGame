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
        private List<Rectangle> pointLightHitboxes;


        public LightsPlacer()
        {
            pointLightHitboxes = new List<Rectangle>();
        }

        public void Update()
        {
            if (KeyMouseReader.KeyClick(Keys.Q))
                LightingManager.AmbientLights.Add(new Lights.AmbientLight(0, 0, EditorMapManager.NumXTiles * 20, EditorMapManager.NumYTiles * 20, EditorScreen.ColorPicker.SelectedColor * 0.5f));
            else if (KeyMouseReader.LeftClick())
            {
                Vector2 mousePos = new Vector2(KeyMouseReader.GetMousePos().X, KeyMouseReader.GetMousePos().Y) - EditorMapManager.Offset;
                Rectangle hitBox = new Rectangle((int)mousePos.X - 10 + (int)EditorMapManager.Offset.X, (int)mousePos.Y - 10 + (int)EditorMapManager.Offset.Y, 20, 20);
                PointLight tempLight = new PointLight(mousePos, 300, 0.5f, EditorScreen.ColorPicker.SelectedColor);
                LightingManager.PointLights.Add(tempLight);
                pointLightHitboxes.Add(hitBox);
            }
            else if (KeyMouseReader.RightClick())
            {
                Vector2 mousePos = new Vector2(KeyMouseReader.GetMousePos().X, KeyMouseReader.GetMousePos().Y);
                RemovePointLight(mousePos);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < pointLightHitboxes.Count; i++)
            {
                spriteBatch.Draw(TextureManager.FilledSquare, pointLightHitboxes[i], Color.White);
            }
        }

        private void RemovePointLight(Vector2 aMousePos)
        {
            for (int i = 0; i < pointLightHitboxes.Count; i++)
            {
                if (pointLightHitboxes[i].Contains(aMousePos))
                {
                    LightingManager.PointLights.RemoveAt(i);
                    pointLightHitboxes.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
