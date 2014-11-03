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
using System.IO;

namespace MobileGame.Lights
{
    static class LightingManager
    {
        public static List<AmbientLight> AmbientLights;
        public static List<PointLight> PointLights;
        public static Vector2 DrawOffset;
        public static bool EditorMode;

        private static GraphicsDevice graphicsDevice;

        private static RenderTarget2D mainTarget;
        private static RenderTarget2D lightingTarget;
        private static int renderTargetWidth, renderTargetHeight;

        private static Effect pixelShader;
        private static Effect pointLight;
      
        private static SpriteBatch spriteBatch;        

        private static int LightMode;

        public static void Initialize(GraphicsDevice GraphicsDevice)
        {
            graphicsDevice = GraphicsDevice;
            spriteBatch = new SpriteBatch(graphicsDevice);
            renderTargetWidth = graphicsDevice.PresentationParameters.BackBufferWidth;
            renderTargetHeight = graphicsDevice.PresentationParameters.BackBufferHeight;
            lightingTarget = new RenderTarget2D(graphicsDevice, renderTargetWidth, renderTargetHeight);
            mainTarget = new RenderTarget2D(graphicsDevice, renderTargetWidth, renderTargetHeight);
            
            AmbientLights = new List<AmbientLight>();
            PointLights = new List<PointLight>();
            DrawOffset = Vector2.Zero;
            EditorMode = false;

            LightMode = 0;
        }

        public static void Initialize(GraphicsDevice GraphicsDevice, int RenderTargetWidth, int RenderTargetHeight)
        {
            graphicsDevice = GraphicsDevice;
            spriteBatch = new SpriteBatch(graphicsDevice);

            renderTargetWidth = RenderTargetWidth;
            renderTargetHeight = RenderTargetHeight;
            lightingTarget = new RenderTarget2D(graphicsDevice, renderTargetWidth, renderTargetHeight);
            mainTarget = new RenderTarget2D(graphicsDevice, renderTargetWidth, renderTargetHeight);

            AmbientLights = new List<AmbientLight>();
            EditorMode = true;

            LightMode = 0;
        }

        public static void LoadContent(ContentManager Content)
        {
            pixelShader = Content.Load<Effect>("Shaders/PixelShader.mgfxo");
            pointLight = Content.Load<Effect>("Shaders/PointLight.mgfxo");


            if (EditorMode == false)
            {
                pointLight.Parameters["ScreenDimensions"].SetValue(new Vector2(1000, 800));
            }
            else
            {
                pointLight.Parameters["ScreenDimensions"].SetValue(new Vector2(1600, 1000));
            }
            
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

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            
            if (LightMode != 1 && LightMode != 3 && LightMode != 4)
            {
                foreach (AmbientLight Light in AmbientLights)
                    Light.Draw(spriteBatch);
            }
            
            spriteBatch.End();

            if (LightMode != 2 && LightMode != 3 && LightMode != 4)
                foreach (PointLight pLight in PointLights)
                    DrawPointLight(pLight);
        }

        private static void DrawPointLight(PointLight pLight)
        {
            Vector2 offset, lightPos;
            if (!EditorMode)
            {
                offset = new Vector2(Camera.Position.X - Camera.ViewPort.Width / 2, Camera.Position.Y - Camera.ViewPort.Height / 2);
                lightPos = new Vector2(pLight.Position.X - offset.X, pLight.Position.Y - offset.Y);
                pointLight.Parameters["LightPos"].SetValue(lightPos);
            }
            else
            {
                lightPos = new Vector2(pLight.Position.X, pLight.Position.Y);
                pointLight.Parameters["LightPos"].SetValue(lightPos);
            }
            

            
            pointLight.Parameters["Radius"].SetValue(pLight.Radius);
            pointLight.Parameters["LightPow"].SetValue(pLight.Power);
            pointLight.Parameters["LightColor"].SetValue(pLight.Color.ToVector4());

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, pointLight);

            spriteBatch.Draw(lightingTarget, new Rectangle(0, 0, renderTargetWidth, renderTargetHeight), Color.White);
            spriteBatch.End();
        }
    }
}
