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
#endregion

namespace MobileGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        GameManager gameManager;
        ScreenManager screenManager;

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

            base.Initialize();
        }
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TextureManager.LoadContents(Content);
            gameManager = new GameManager();

            screenManager = new ScreenManager(this);
            screenManager.SpriteBatch = spriteBatch;
            screenManager.Font = Content.Load<SpriteFont>("GUI Textures/Fonts/DejaVuSans_20");
            screenManager.BlankTexture = Content.Load<Texture2D>("Units/SmallerEnemy");

            screenManager.AddScreen(new BackgroundScreen());
            screenManager.AddScreen(new MainMenuScreen());

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
            //gameManager.Update(gameTime.ElapsedGameTime.Milliseconds);

            //Console.WriteLine("FPS: " + (1000 / gameTime.ElapsedGameTime.Milliseconds));

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            screenManager.Draw(gameTime);

            spriteBatch.Begin();
            //gameManager.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
