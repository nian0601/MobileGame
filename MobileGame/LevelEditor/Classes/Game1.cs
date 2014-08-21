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

        private static int editMode;
        public static int EditMode
        {
            get { return editMode; }
        }

        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            graphics.PreferredBackBufferWidth = 1100;
            graphics.PreferredBackBufferHeight = 800;

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
            KeyMouseReader.Update();
            screenManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SaddleBrown);

            screenManager.Draw(gameTime);

            base.Draw(gameTime);
        }

        public static void ChangeEditMode(int value)
        {
            editMode = value;
            if (editMode > 3)
                editMode = 3;

            else if (editMode < 0)
                editMode = 0;

            Console.WriteLine("EditMode: " + editMode);
        }
    }
}
