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
using Microsoft.Xna.Framework.Content;

namespace MobileGame.Managers
{
    static class LightingManager
    {
        public static List<BasicLight> BasicLights;
        public static List<AmbientLight> AmbientLights;
        public static Vector2 DrawOffset;
        public static bool EditorMode;

        private static GraphicsDevice graphicsDevice;

        private static RenderTarget2D mainTarget;
        private static RenderTarget2D lightingTarget;

        private static Effect pixelShader;
      
        private static SpriteBatch spriteBatch;        

        private static int LightMode;

        public static void Initialize(GraphicsDevice GraphicsDevice)
        {
            graphicsDevice = GraphicsDevice;
            spriteBatch = new SpriteBatch(graphicsDevice);

            lightingTarget = new RenderTarget2D(graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);
            mainTarget = new RenderTarget2D(graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);
            
            BasicLights = new List<BasicLight>();
            AmbientLights = new List<AmbientLight>();
            DrawOffset = Vector2.Zero;
            EditorMode = false;

            LightMode = 0;
        }

        public static void Initialize(GraphicsDevice GraphicsDevice, int RenderTargetWidth, int RenderTargetHeight, Vector2 drawOffset)
        {
            graphicsDevice = GraphicsDevice;
            spriteBatch = new SpriteBatch(graphicsDevice);

            lightingTarget = new RenderTarget2D(graphicsDevice, RenderTargetWidth, RenderTargetHeight);
            mainTarget = new RenderTarget2D(graphicsDevice, RenderTargetWidth, RenderTargetHeight);

            BasicLights = new List<BasicLight>();
            AmbientLights = new List<AmbientLight>();
            DrawOffset = drawOffset;
            EditorMode = true;

            LightMode = 0;
        }

        public static void LoadContent(ContentManager Content)
        {
            pixelShader = Content.Load<Effect>("Shaders/PixelShader.mgfxo");
        }

        public static void Update()
        {
            if (KeyMouseReader.isKeyDown(Keys.LeftShift) && KeyMouseReader.KeyClick(Keys.L))
                LightMode++;

            if (LightMode > 3)
                LightMode = 0;
        }

        public static void BeginDrawMainTarget()
        {
            graphicsDevice.SetRenderTarget(mainTarget);
            graphicsDevice.Clear(Color.CornflowerBlue);
        }

        public static void EndDrawingMainTarget() { }

        public static void DrawLitScreen()
        {
            DrawLights();

            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(Color.CornflowerBlue);

            pixelShader.Parameters["lightMask"].SetValue(lightingTarget);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, pixelShader);
            spriteBatch.Draw(mainTarget, DrawOffset, Color.White);
            spriteBatch.End();
        }

        private static void DrawLights()
        {
            //Make the GraphicsDevice draw onto our LightingTarget
            graphicsDevice.SetRenderTarget(lightingTarget);
            //Clear the new rendertarget to a solid black
            graphicsDevice.Clear(Color.Black);

            if(!EditorMode)
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Camera.Get_Transformation());
            else
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            if (LightMode != 2 && LightMode != 3 && LightMode != 4)
            {
                foreach (BasicLight Light in BasicLights)
                    Light.Draw(spriteBatch);
            }

            if (LightMode != 1 && LightMode != 3 && LightMode != 4)
            {
                foreach (AmbientLight Light in AmbientLights)
                    Light.Draw(spriteBatch);
            }
            
            spriteBatch.End();
        }
    }
}
