﻿#region Using Statements
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
using MobileGame.Managers;
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

        public static ScreenManager screenManager;

        public static bool Debugging;
        public static bool Debugg2;

        public static int ScreenWidth { get; private set; }
        public static int ScreenHeight { get; private set; }

        public static RenderTarget2D ShaderTarget;
        public static RenderTarget2D MainTarget;

        public Game1()
            : base()
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

            ScreenWidth = 800;
            ScreenHeight = 600;

            IsMouseVisible = true;
            Debugging = false;
            Debugg2 = false;


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

            var pp = GraphicsDevice.PresentationParameters;
            ShaderTarget = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            MainTarget = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
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
            if (KeyMouseReader.KeyClick(Keys.T))
                Debugg2 = !Debugg2;

            screenManager.Update(gameTime);

            //The game will break if i try to pring FPS here, do it in GameManager instead, yay
            //Console.WriteLine("FPS: " + (1000 / gameTime.ElapsedGameTime.Milliseconds));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            screenManager.Draw(gameTime);

            base.Draw(gameTime);
        }

        public static void SetScreenSize(int Width, int Height)
        {
            graphics.PreferredBackBufferWidth = Width;
            graphics.PreferredBackBufferHeight = Height;

            graphics.ApplyChanges();

            ScreenWidth = Width;
            ScreenHeight = Height;
        }
    }
}
