using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using GUI_System.GameStateManagement;

namespace MobileGame.Screens
{
    class BackgroundScreen : GameScreen
    {
        ContentManager content;
        Texture2D backgroundTexture;

        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void Activate()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            backgroundTexture = content.Load<Texture2D>("GUI Textures/Background");
        }

        public override void Unload()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewPort = ScreenManager.Game.GraphicsDevice.Viewport;
            Rectangle fullScreen = new Rectangle(0, 0, viewPort.Width, viewPort.Height);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture, fullScreen, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            spriteBatch.End();
        }
    }
}
