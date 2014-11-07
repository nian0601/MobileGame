using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GUI_System.GameStateManagement
{
    public class MessageBoxScreen : MenuScreen
    {
        private string message;

        public EventHandler<EventArgs> Accepted;
        public EventHandler<EventArgs> Cancelled;

        public MessageBoxScreen(string message):this(message, true)
        {
            
        }

        public MessageBoxScreen(string message, bool showInstructions):base("")
        {
            const string instructions = "\n Enter = Ok" +
                                        "\n Escape = Cancel";
            if (showInstructions)
                this.message = message + instructions;
            else
                this.message = message;

            IsPopup = true;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public MessageBoxScreen(): base("")
        {
            message = null;
            IsPopup = true;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void HandleInput(GameTime gameTime)
        {
            if (KeyMouseReader.KeyClick(Keys.Enter))
            {
                if(Accepted != null)
                    Accepted(this, new EventArgs());

                ExitScreen();
            }
            else if (KeyMouseReader.KeyClick(Keys.Escape))
            {
                if (Cancelled != null)
                    Cancelled(this, new EventArgs());

                ExitScreen();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            Viewport viewport = ScreenManager.Game.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;

            // Fade the popup alpha during transitions.
            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin();

            // Draw the message box text.
            if(message != null)
                spriteBatch.DrawString(font, message, textPosition, color);

            spriteBatch.End();
        }
    }
}
