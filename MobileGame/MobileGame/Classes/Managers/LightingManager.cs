using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        private static List<BasicLight> BasicLights;
        private static List<AmbientLight> AmbientLights;

        private static int PlayerLightWidth;
        private static int PlayerLightHeight;
        private static Color PlayerLightColor;
        private static bool ShowPlayerLight;

        private static int LightMode;

        public static void Initialize()
        {
            BasicLights = new List<BasicLight>();
            AmbientLights = new List<AmbientLight>();

            PlayerLightWidth = 400;
            PlayerLightHeight = 400;
            ShowPlayerLight = true;

            LightMode = 0;
        }

        public static void AddBasicLight(int X, int Y, int Width, int Height, Color Color)
        {
            BasicLights.Add(new BasicLight(X, Y, Width, Height, TextureManager.LightSource, Color));
        }

        public static void AddAmbientLight(int X, int Y, int Width, int Height, Color Color)
        {
            AmbientLights.Add(new AmbientLight(X, Y, Width, Height, Color));
        }

        public static void AddAmbientLight(Color Color)
        {
            AmbientLights.Add(new AmbientLight(Color));
        }

        public static void DefinePlayerLight(int Width, int Height, Color Color, bool Show)
        {
            PlayerLightWidth = Width;
            PlayerLightHeight = Height;
            PlayerLightColor = Color;
            ShowPlayerLight = Show;
        }

        public static void Update()
        {
            if (KeyMouseReader.isKeyDown(Keys.LeftShift) && KeyMouseReader.KeyClick(Keys.L))
                LightMode++;

            if (LightMode > 4)
                LightMode = 0;
        }

        public static void Draw()
        {
            //Make the GraphicsDevice draw onto our LightingTarget
            GraphicsDevice.SetRenderTarget(LightingTarget);
            //Clear the new rendertarget to a solid black
            GraphicsDevice.Clear(Color.Transparent);

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Camera.Get_Transformation());

            if (LightMode != 2 && LightMode != 3 && LightMode != 4)
            {
                foreach (BasicLight Light in BasicLights)
                    Light.Draw(SpriteBatch);
            }

            if (LightMode != 1 && LightMode != 3 && LightMode != 4)
            {
                foreach (AmbientLight Light in AmbientLights)
                    Light.Draw(SpriteBatch);
            }

            if (LightMode != 1 && LightMode != 2 && LightMode != 4)
            {
                if (ShowPlayerLight && GameManager.Player != null)
                    SpriteBatch.Draw(TextureManager.LightSource, new Rectangle((int)GameManager.Player.Position.X - PlayerLightWidth / 2, (int)GameManager.Player.Position.Y - PlayerLightHeight / 2, PlayerLightWidth, PlayerLightHeight), PlayerLightColor);

            }
            
            SpriteBatch.End();
        }
    }
}
