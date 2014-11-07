using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using GUI_System.GameStateManagement;
using GUI_System.GUIObjects;

using LevelEditor.Managers;

namespace LevelEditor.Screens
{
    public class ConfirmationPopUp : MessageBoxScreen
    {
        private MenuButton YesButton, NoButton;
        private Texture2D TitleTexture;
        private Vector2 TitlePos;

        private string Message;
        private Vector2 MessagePos;
        private SpriteFont MessageFont;

        public ConfirmationPopUp() : base() 
        {
            YesButton = new MenuButton(new YesButtonStyle(ScreenManager.Game.Content));
            NoButton = new MenuButton(new NoButtonStyle(ScreenManager.Game.Content));
            TitleTexture = ScreenManager.Game.Content.Load<Texture2D>("Editor/ConfirmationBox/TitleTexture");
            Message = null;
        }

        public ConfirmationPopUp(string Message): base("")
        {
            YesButton = new MenuButton(new YesButtonStyle(ScreenManager.Game.Content));
            NoButton = new MenuButton(new NoButtonStyle(ScreenManager.Game.Content));
            TitleTexture = null;
            this.Message = Message + "\nAre you sure you want to continue?";
            MessageFont = ScreenManager.Game.Content.Load<SpriteFont>("Fonts/DejaVuSans_20");
        }

        public override void HandleInput(GameTime gameTime)
        {
            YesButton.Update(this, gameTime);
            NoButton.Update(this, gameTime);

            if (YesButton.LeftClick())
            {
                if (Accepted != null)
                    Accepted(this, new EventArgs());

                ExitScreen();
            }
            else if (NoButton.LeftClick())
            {
                if (Cancelled != null)
                    Cancelled(this, new EventArgs());

                ExitScreen();
            }
        }

        protected override void UpdateMenyEntryLocations()
        {
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
            Viewport viewport = ScreenManager.Game.GraphicsDevice.Viewport;

            if (TitleTexture != null)
            {
                int titleXPos = viewport.Width / 2 - TitleTexture.Width / 2;
                int titleYPos = viewport.Height / 2 - TitleTexture.Height / 2 - 100;
                TitlePos = new Vector2(titleXPos, titleYPos);
            }

            // Center the message text in the viewport.

            if (Message != null)
            {
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                Vector2 messageSize = MessageFont.MeasureString(Message);
                MessagePos = (viewportSize - messageSize) / 2;
                MessagePos.Y -= 110;
            }
            

            int yesXPos = viewport.Width / 2 - YesButton.GetWidth(this);
            int yesYPos = viewport.Height / 2 + YesButton.GetHeight(this) - 100;
            Vector2 YesPos = new Vector2(yesXPos, yesYPos);

            int noXPos = viewport.Width / 2 + NoButton.GetWidth(this);
            int noYPos = viewport.Height / 2 + NoButton.GetHeight(this) - 100;
            Vector2 NoPos = new Vector2(noXPos, noYPos);

            if (CurrentScreenState == ScreenState.TransitionOn)
            {
                TitlePos.Y -= transitionOffset * 256;
                YesPos.Y -= transitionOffset * 256;
                NoPos.Y -= transitionOffset * 256;
                MessagePos.Y -= transitionOffset * 256;
            }
            else
            {
                TitlePos.Y += transitionOffset * 512;
                YesPos.Y += transitionOffset * 512;
                NoPos.Y += transitionOffset * 512;
                MessagePos.Y += transitionOffset * 512;
            }

            YesButton.Position = YesPos;
            NoButton.Position = NoPos;
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            UpdateMenyEntryLocations();

            GraphicsDevice graphics = ScreenManager.Game.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();

            YesButton.Draw(this, gameTime);
            NoButton.Draw(this, gameTime);

            if(TitleTexture != null)
                spriteBatch.Draw(TitleTexture, TitlePos, Color.White);

            if (Message != null)
                spriteBatch.DrawString(MessageFont, Message, MessagePos, Color.White);


            spriteBatch.End();
        }
        
    }
}
