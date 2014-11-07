using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using MobileGame.CameraManagement;

namespace MobileGame.LightingSystem
{
    class LightRenderer
    {
        public List<SpotLight> spotLights;
        public List<PointLight> pointLights;

        private RenderTarget2D backBufferCache;
        private RenderTarget2D midGroundTarget;
        private RenderTarget2D lightMap;
        private RenderTarget2D unwrapTarget;
        private RenderTarget2D occlusionMap;
        private RenderTarget2D postProcessTarget;
        private RenderTarget2D horizontalBlurTarget;
        private RenderTarget2D verticalBlurTarget;
        private RenderTarget2D ambientTarget;

        private Effect unwrapSpotlight;
        private Effect unwrap;
        private Effect spotLight;
        private Effect pointLight;
        private Effect horizontalBlur;
        private Effect verticalBlur;
        private Effect lightBlend;
        private Effect ambientLight;

        private Rectangle fullScreen;
        private SpriteBatch spriteBatch;

        private Vector2 screenDims;
        private Vector2 cameraTopLeft;

        private BlendState collapseBlendState;

        public GraphicsDeviceManager graphics;

        public float minLight = -1f;
        public float lightBias = -1f;

        public float minBlur = 0.0f;
        public float maxBlur = 5.0f;


        public LightRenderer(GraphicsDeviceManager _graphics)
        {
            graphics = _graphics;
        }

        public void Initialize()
        {
            spotLights = new List<SpotLight>();
            pointLights = new List<PointLight>();

            backBufferCache = new RenderTarget2D(graphics.GraphicsDevice, graphics.GraphicsDevice.PresentationParameters.BackBufferWidth, graphics.GraphicsDevice.PresentationParameters.BackBufferHeight);
            midGroundTarget = new RenderTarget2D(graphics.GraphicsDevice, graphics.GraphicsDevice.PresentationParameters.BackBufferWidth, graphics.GraphicsDevice.PresentationParameters.BackBufferHeight);
            lightMap = new RenderTarget2D(graphics.GraphicsDevice, graphics.GraphicsDevice.PresentationParameters.BackBufferWidth, graphics.GraphicsDevice.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.None, 1, RenderTargetUsage.PreserveContents);
            unwrapTarget = new RenderTarget2D(graphics.GraphicsDevice, graphics.GraphicsDevice.PresentationParameters.BackBufferWidth, graphics.GraphicsDevice.PresentationParameters.BackBufferWidth, false, SurfaceFormat.HdrBlendable, DepthFormat.None);
            occlusionMap = new RenderTarget2D(graphics.GraphicsDevice, graphics.GraphicsDevice.PresentationParameters.BackBufferWidth, 1, false, SurfaceFormat.HdrBlendable, DepthFormat.None);
            postProcessTarget = new RenderTarget2D(graphics.GraphicsDevice, graphics.GraphicsDevice.PresentationParameters.BackBufferWidth, graphics.GraphicsDevice.PresentationParameters.BackBufferHeight);
            horizontalBlurTarget = new RenderTarget2D(graphics.GraphicsDevice, graphics.GraphicsDevice.PresentationParameters.BackBufferWidth, graphics.GraphicsDevice.PresentationParameters.BackBufferHeight);
            verticalBlurTarget = new RenderTarget2D(graphics.GraphicsDevice, graphics.GraphicsDevice.PresentationParameters.BackBufferWidth, graphics.GraphicsDevice.PresentationParameters.BackBufferHeight);
            ambientTarget = new RenderTarget2D(graphics.GraphicsDevice, graphics.GraphicsDevice.PresentationParameters.BackBufferWidth, graphics.GraphicsDevice.PresentationParameters.BackBufferHeight);

            //backBufferCache = new RenderTarget2D(graphics.GraphicsDevice, Camera.ViewPort.Width, Camera.ViewPort.Height);
            //midGroundTarget = new RenderTarget2D(graphics.GraphicsDevice, Camera.ViewPort.Width, Camera.ViewPort.Height);
            //lightMap = new RenderTarget2D(graphics.GraphicsDevice, Camera.ViewPort.Width, Camera.ViewPort.Height, false, SurfaceFormat.Color, DepthFormat.None, 1, RenderTargetUsage.PreserveContents);
            //unwrapTarget = new RenderTarget2D(graphics.GraphicsDevice, Camera.ViewPort.Width, Camera.ViewPort.Width, false, SurfaceFormat.HdrBlendable, DepthFormat.None);
            //occlusionMap = new RenderTarget2D(graphics.GraphicsDevice, Camera.ViewPort.Width, 1, false, SurfaceFormat.HdrBlendable, DepthFormat.None);
            //postProcessTarget = new RenderTarget2D(graphics.GraphicsDevice, Camera.ViewPort.Width, Camera.ViewPort.Height);
            //horizontalBlurTarget = new RenderTarget2D(graphics.GraphicsDevice, Camera.ViewPort.Width, Camera.ViewPort.Height);
            //verticalBlurTarget = new RenderTarget2D(graphics.GraphicsDevice, Camera.ViewPort.Width, Camera.ViewPort.Height);
            //ambientTarget = new RenderTarget2D(graphics.GraphicsDevice, Camera.ViewPort.Width, Camera.ViewPort.Height);

            fullScreen = new Rectangle(0, 0, graphics.GraphicsDevice.PresentationParameters.BackBufferWidth, graphics.GraphicsDevice.PresentationParameters.BackBufferHeight);

            collapseBlendState = new BlendState();
            collapseBlendState.ColorBlendFunction = BlendFunction.Min;
            collapseBlendState.AlphaBlendFunction = BlendFunction.Min;
            collapseBlendState.ColorSourceBlend = Blend.One;
            collapseBlendState.ColorDestinationBlend = Blend.One;
            collapseBlendState.AlphaSourceBlend = Blend.One;
            collapseBlendState.AlphaDestinationBlend = Blend.One;

            screenDims = new Vector2(graphics.GraphicsDevice.PresentationParameters.BackBufferWidth, graphics.GraphicsDevice.PresentationParameters.BackBufferHeight);
            cameraTopLeft = new Vector2(Camera.Position.X - Camera.ViewPort.Width / 2, Camera.Position.Y - Camera.ViewPort.Height / 2);
        }

        public void LoadContent(ContentManager Content)
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            unwrap = Content.Load<Effect>("Effects/Unwrap.mgfxo");
            unwrapSpotlight = Content.Load<Effect>("Effects/UnwrapSpotlight.mgfxo");
            spotLight = Content.Load<Effect>("Effects/SpotLight.mgfxo");
            pointLight = Content.Load<Effect>("Effects/PointLight.mgfxo");
            verticalBlur = Content.Load<Effect>("Effects/VerticalBlur.mgfxo");
            horizontalBlur = Content.Load<Effect>("Effects/HorizontalBlur.mgfxo");
            lightBlend = Content.Load<Effect>("Effects/LightBlend.mgfxo");
            ambientLight = Content.Load<Effect>("Effects/AmbientLight.mgfxo");
        }

        public void BeginDrawBackground()
        {
            graphics.GraphicsDevice.SetRenderTarget(backBufferCache);
        }

        public void EndDrawBackground()
        {
        }

        public void BeginDrawShadowCasters()
        {
            graphics.GraphicsDevice.SetRenderTarget(midGroundTarget);
            graphics.GraphicsDevice.Clear(Color.Transparent);
        }

        public void EndDrawShadowCasters()
        {
        }

        public void DrawLitScene()
        {
            //Error checking
            //.....
            //

            PrepareResources();

            for (int i = 0; i < spotLights.Count; i++)
            {
                float lightDirAngle = spotLights[i].GetBiasedAngle();

                float angleBias = spotLights[i].GetAngleBias();

                UnwrapShadowCasters(spotLights[i], lightDirAngle, angleBias);

                CreateOcclusionMap();

                CreateLightMap(spotLights[i], lightDirAngle, angleBias);

                BlurLightMaps(spotLights[i]);

                AccumulateLightMaps(spotLights[i]);
            }

            for (int i = 0; i < pointLights.Count; i++)
            {
                UnwrapShadowCasters(pointLights[i]);

                CreateOcclusionMap();

                CreateLightMap(pointLights[i]);

                BlurLightMaps(pointLights[i]);

                AccumulateLightMaps(pointLights[i]);
            }

            RenderFinalScene();
        }

        private void PrepareResources()
        {
            //fullScreen = new Rectangle((int)Camera.Position.X - Camera.ViewPort.Width/2, (int)Camera.Position.Y - Camera.ViewPort.Height/2, Camera.ViewPort.Width, Camera.ViewPort.Height);
            //screenDims = new Vector2(Camera.ViewPort.Width, Camera.ViewPort.Height);
            //cameraTopLeft = new Vector2(Camera.Position.X - Camera.ViewPort.Width / 2, Camera.Position.Y - Camera.ViewPort.Height / 2);

            unwrap.Parameters["TextureWidth"].SetValue(screenDims.X);
            unwrap.Parameters["TextureHeight"].SetValue(screenDims.Y);
            unwrap.Parameters["DiagonalLength"].SetValue(screenDims.Length());

            spotLight.Parameters["ScreenDimensions"].SetValue(screenDims);
            spotLight.Parameters["DiagonalLength"].SetValue(screenDims.Length());
            spotLight.Parameters["Bias"].SetValue(lightBias);

            unwrapSpotlight.Parameters["TextureWidth"].SetValue(screenDims.X);
            unwrapSpotlight.Parameters["TextureHeight"].SetValue(screenDims.Y);
            unwrapSpotlight.Parameters["DiagonalLength"].SetValue(screenDims.Length());

            verticalBlur.Parameters["ScreenDims"].SetValue(screenDims);
            verticalBlur.Parameters["minBlur"].SetValue(minBlur);
            verticalBlur.Parameters["maxBlur"].SetValue(maxBlur);

            horizontalBlur.Parameters["ScreenDims"].SetValue(screenDims);
            horizontalBlur.Parameters["minBlur"].SetValue(minBlur);
            horizontalBlur.Parameters["maxBlur"].SetValue(maxBlur);

            pointLight.Parameters["ScreenDimensions"].SetValue(screenDims);
            pointLight.Parameters["DiagonalLength"].SetValue(screenDims.Length());
            pointLight.Parameters["Bias"].SetValue(lightBias);

            ambientLight.Parameters["lightColor"].SetValue(Color.White.ToVector4() * 0.5f);

            

            graphics.GraphicsDevice.SetRenderTarget(lightMap);
            graphics.GraphicsDevice.Clear(Color.Black);
        }

        private void UnwrapShadowCasters(SpotLight sLight, float lightDirAngle, float angleBias)
        {
            graphics.GraphicsDevice.SetRenderTarget(unwrapTarget);
            graphics.GraphicsDevice.Clear(Color.Transparent);

            unwrapSpotlight.Parameters["LightPos"].SetValue(sLight.Position);
            unwrapSpotlight.Parameters["MinAngle"].SetValue(lightDirAngle - (sLight.outerAngle / 2f));
            unwrapSpotlight.Parameters["MaxAngle"].SetValue(lightDirAngle + (sLight.outerAngle / 2f));

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, null, null, unwrapSpotlight);
            spriteBatch.Draw(midGroundTarget, new Rectangle(0, 0, fullScreen.Width, fullScreen.Width), Color.White);
            //spriteBatch.Draw(midGroundTarget, new Rectangle((int)cameraTopLeft.X, (int)cameraTopLeft.Y, fullScreen.Width, fullScreen.Width), Color.White);
            spriteBatch.End();
        }

        private void UnwrapShadowCasters(PointLight pLight)
        {
            graphics.GraphicsDevice.SetRenderTarget(unwrapTarget);
            graphics.GraphicsDevice.Clear(Color.Transparent);

            unwrap.Parameters["LightPos"].SetValue(pLight.Position);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, null, null, unwrap);
            spriteBatch.Draw(midGroundTarget, new Rectangle(0, 0, fullScreen.Width, fullScreen.Width), Color.White);
            //spriteBatch.Draw(midGroundTarget, new Rectangle((int)cameraTopLeft.X, (int)cameraTopLeft.Y, fullScreen.Width, fullScreen.Width), Color.White);
            spriteBatch.End();
        }

        private void CreateOcclusionMap()
        {
            graphics.GraphicsDevice.SetRenderTarget(occlusionMap);
            graphics.GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.Deferred, collapseBlendState, SamplerState.PointClamp, null, null, null);
            for (int i = 0; i < fullScreen.Width; i++)
            {
                spriteBatch.Draw(unwrapTarget, new Rectangle(0, 0, graphics.GraphicsDevice.PresentationParameters.BackBufferWidth, 1), new Rectangle(0, i, graphics.GraphicsDevice.PresentationParameters.BackBufferWidth, 1), Color.White);
                //spriteBatch.Draw(unwrapTarget, new Rectangle((int)cameraTopLeft.X, (int)cameraTopLeft.Y, Camera.ViewPort.Width, 1), new Rectangle(0, i, Camera.ViewPort.Width, 1), Color.White);
            }
            spriteBatch.End();
        }

        private void CreateLightMap(SpotLight sLight, float lightDirAngle, float angleBias)
        {
            graphics.GraphicsDevice.SetRenderTarget(postProcessTarget);
            graphics.GraphicsDevice.Clear(Color.Black);

            spotLight.Parameters["LightPos"].SetValue(sLight.Position);
            spotLight.Parameters["LightPow"].SetValue(sLight.Power);
            spotLight.Parameters["Radius"].SetValue(sLight.radius);
            spotLight.Parameters["OuterMinAngle"].SetValue(lightDirAngle - (sLight.outerAngle / 2f));
            spotLight.Parameters["OuterMaxAngle"].SetValue(lightDirAngle + (sLight.outerAngle / 2f));
            spotLight.Parameters["CenterAngle"].SetValue(lightDirAngle);
            spotLight.Parameters["HalfInnerArc"].SetValue(sLight.innerAngle / 2f);
            spotLight.Parameters["HalfOuterArc"].SetValue(sLight.outerAngle / 2f);
            spotLight.Parameters["AngleBias"].SetValue(angleBias);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, null, null, spotLight);
            spriteBatch.Draw(occlusionMap, fullScreen, Color.White);
            spriteBatch.End();
        }

        private void CreateLightMap(PointLight pLight)
        {
            graphics.GraphicsDevice.SetRenderTarget(postProcessTarget);
            graphics.GraphicsDevice.Clear(Color.Black);

            //Set params
            pointLight.Parameters["LightPos"].SetValue(pLight.Position);
            pointLight.Parameters["LightPow"].SetValue(pLight.Power);
            pointLight.Parameters["Radius"].SetValue(pLight.radius);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, null, null, pointLight);
            spriteBatch.Draw(occlusionMap, fullScreen, Color.White);
            spriteBatch.End();
        }

        private void BlurLightMaps(Light light)
        {
            graphics.GraphicsDevice.SetRenderTarget(horizontalBlurTarget);
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            //Set some params
            horizontalBlur.Parameters["LightPos"].SetValue(light.Position);
            horizontalBlur.Parameters["Radius"].SetValue(light.radius);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, null, null, horizontalBlur);
            spriteBatch.Draw(postProcessTarget, fullScreen, Color.White);
            spriteBatch.End();

            graphics.GraphicsDevice.SetRenderTarget(verticalBlurTarget);
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            //Set some more params
            verticalBlur.Parameters["LightPos"].SetValue(light.Position);
            verticalBlur.Parameters["Radius"].SetValue(light.radius);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, null, null, verticalBlur);
            spriteBatch.Draw(horizontalBlurTarget, fullScreen, Color.White);
            spriteBatch.End();
        }

        private void AccumulateLightMaps(Light light)
        {
            graphics.GraphicsDevice.SetRenderTarget(lightMap);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null);
            spriteBatch.Draw(verticalBlurTarget, fullScreen, light.color);
            spriteBatch.End();
        }

        private void RenderFinalScene()
        {
            graphics.GraphicsDevice.SetRenderTarget(ambientTarget);
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            graphics.GraphicsDevice.Textures[1] = lightMap;
            lightBlend.Parameters["MinLight"].SetValue(minLight);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, lightBlend);
            spriteBatch.Draw(backBufferCache, fullScreen, Color.White);
            spriteBatch.End();
            graphics.GraphicsDevice.Textures[1] = null;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null);
            spriteBatch.Draw(midGroundTarget, fullScreen, Color.White);
            spriteBatch.End();

            graphics.GraphicsDevice.SetRenderTarget(null);
            graphics.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, ambientLight);
            spriteBatch.Draw(ambientTarget, fullScreen, Color.White);
            spriteBatch.End();
        }
    }
}
