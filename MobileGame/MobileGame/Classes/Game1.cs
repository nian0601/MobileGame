#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

using GUI_System.GUIObjects;
using GUI_System.GameStateManagement;

using MobileGame.Screens;
using MobileGame.CameraManagement;
#endregion

namespace MobileGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ScreenManager screenManager;

        public static bool Debugging;

        public Game1(): base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            
            graphics.ApplyChanges();

            IsMouseVisible = true;
            Debugging = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            screenManager = new ScreenManager(this);
            screenManager.SpriteBatch = spriteBatch;
            screenManager.Font = Content.Load<SpriteFont>("GUI Textures/Fonts/DejaVuSans_20");
            screenManager.BlankTexture = Content.Load<Texture2D>("GUI Textures/BlankTexture");
            //screenManager.TraceEnabled = true;

            screenManager.AddScreen(new BackgroundScreen());
            screenManager.AddScreen(new MainMenuScreen());

            Camera.Initialize(GraphicsDevice);
            Camera.Limits = new Rectangle(0, 0, 800, 600);
            Camera.LoadStuff(Content);
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
            if (KeyMouseReader.KeyClick(Keys.I))
                Debugging = !Debugging;

            screenManager.Update(gameTime);
            
            //The game will break if i try to pring FPS here, do it in GameManager instead, yay
            //Console.WriteLine("FPS: " + (1000 / gameTime.ElapsedGameTime.Milliseconds));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SaddleBrown);

            screenManager.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
