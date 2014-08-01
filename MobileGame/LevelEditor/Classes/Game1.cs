#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

using LevelEditor.Managers;
using LevelEditor.Screens;
using GUI_System.GameStateManagement;

#endregion

namespace LevelEditor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ScreenManager screenManager;

        public static int EditMode;

        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            graphics.PreferredBackBufferWidth = 1040;
            graphics.PreferredBackBufferHeight = 640;

            graphics.ApplyChanges();

            IsMouseVisible = true;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            screenManager = new ScreenManager(this);
            screenManager.SpriteBatch = spriteBatch;
            screenManager.Font = Content.Load<SpriteFont>("Fonts/DejaVuSans_20");
            screenManager.BlankTexture = Content.Load<Texture2D>("BlankTexture");

            screenManager.AddScreen(new EditorScreen());

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyMouseReader.Update();
            screenManager.Update(gameTime);

            if (KeyMouseReader.KeyClick(Keys.PageUp))
            {
                EditMode++;
                if (EditMode > 3)
                    EditMode = 3;
                Console.WriteLine("EditMode: " + EditMode);
            }

            if (KeyMouseReader.KeyClick(Keys.PageDown))
            {
                EditMode--;
                if (EditMode < 0)
                    EditMode = 0;
                Console.WriteLine("EditMode: " + EditMode);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            screenManager.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
