using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MobileGame.Lights;
using MobileGame.CameraManagement;
using MobileGame.FileManagement;

using GUI_System.GameStateManagement;

namespace MobileGame.Managers
{
    static class LightingManager
    {
        public static RenderTarget2D LightingTarget { get; set; }
        public static GraphicsDevice GraphicsDevice { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }

        private static List<BasicLight> Lights;

        public static void Initialize(int RenderWidth, int RenderHeight)
        {
            //LightingTarget = new RenderTarget2D(ScreenManager.Game.GraphicsDevice, FileLoader.LoadedLevelMapWidth * FileLoader.LoadedLevelTileSize, FileLoader.LoadedLevelMapHeight * FileLoader.LoadedLevelTileSize);
            Lights = new List<BasicLight>();
        }

        public static void AddLight(int X, int Y, int Width, int Height, Color Color)
        {
            Lights.Add(new BasicLight(X, Y, Width, Height, TextureManager.LightSource, Color));
        }

        public static void Update(GameTime gameTime)
        {

        }

        public static void Draw()
        {
            //Make the GraphicsDevice draw onto our LightingTarget
            GraphicsDevice.SetRenderTarget(LightingTarget);
            //Clear the new rendertarget to a solid black
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Camera.Get_Transformation());

            foreach (BasicLight Light in Lights)
                Light.Draw(SpriteBatch);

            SpriteBatch.Draw(TextureManager.LightSource, new Rectangle((int)GameManager.Player.position.X - 200, (int)GameManager.Player.position.Y - 200, 400, 400), Color.White * 0.75f);

            SpriteBatch.End();
        }
    }
}
