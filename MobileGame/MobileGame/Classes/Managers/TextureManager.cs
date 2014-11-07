using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MobileGame.Managers
{
    static class TextureManager
    {
        public static int TileSize { get; private set; }
        public static Texture2D PlayerTile { get; private set; }
        public static Texture2D GoalTile { get; private set; }
        public static Texture2D JumpTile { get; private set; }
        public static Texture2D TeleportTile { get; private set; }

        public static Texture2D EnemyTex { get; private set; }
        public static Texture2D SmallerEnemyTex { get; private set; }
        public static Texture2D SmallerPlayerTex { get; private set; }

        public static Texture2D SpriteSheet { get; private set; }
        public static Texture2D PlayerSheet { get; private set; }
        public static Texture2D PlayerSheet2 { get; private set; }
        public static Texture2D PlayerColTex { get; private set; }

        public static Texture2D TileSet { get; private set; }
        public static Texture2D TileSetEditorEdition { get; private set; }

        public static List<Texture2D> SpikeTextures { get; private set; }

        public static List<Texture2D> GameTextures { get; private set; }

        public static Texture2D FilledSquare { get; private set; }
        public static Effect Ambient { get; private set; }
        public static Effect Diffuse { get; private set; }
        public static Effect PixelShader { get; private set; }
        public static Texture2D LightSource { get; private set; }

        public static void LoadContents(ContentManager Content)
        {
            PlayerTile = Content.Load<Texture2D>("Tiles/PlayerTile");
            GoalTile = Content.Load<Texture2D>("Tiles/GoalTile");
            JumpTile = Content.Load<Texture2D>("Tiles/JumpTile");
            TeleportTile = Content.Load<Texture2D>("Tiles/TeleportTile");

            EnemyTex = Content.Load<Texture2D>("Units/Enemy");
            SmallerEnemyTex = Content.Load<Texture2D>("Units/SmallerEnemy");
            SmallerPlayerTex = Content.Load<Texture2D>("Units/SmallerPlayer");

            SpriteSheet = Content.Load<Texture2D>("TestSpriteSheet");
            PlayerSheet = Content.Load<Texture2D>("Units/PlayerWalkingAnimation");
            PlayerSheet2 = Content.Load<Texture2D>("Units/PlayerWalkingAnimation2");
            PlayerColTex = Content.Load<Texture2D>("Units/PlayerHitData");

            FilledSquare = Content.Load<Texture2D>("DebugTextures/FilledSquare");

            TileSet = Content.Load<Texture2D>("Tiles/TileSet");
            TileSetEditorEdition = Content.Load<Texture2D>("Editor/ToolButtons/TileSetEditorButtons");

            SpikeTextures = new List<Texture2D>();
            SpikeTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Spike_00"));
            SpikeTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Spike_01"));
            SpikeTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Spike_02"));

            Ambient = Content.Load<Effect>("Shaders/Ambient.mgfxo");
            Diffuse = Content.Load<Effect>("Shaders/Diffuse.mgfxo");
            PixelShader = Content.Load<Effect>("Shaders/PixelShader.mgfxo");
            LightSource = Content.Load<Texture2D>("Shaders/LightSource");

            GameTextures = new List<Texture2D>();
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_00"));        // 0
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_01"));        // 1
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_02"));        // 2
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_03"));        // 3
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_04"));        // 4
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_05"));        // 5
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_06"));        // 6
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_07"));        // 7
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_08"));        // 8
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_09"));        // 9
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_10"));        // 10
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_11"));        // 11
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_12"));        // 12
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_13"));        // 13
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_14"));        // 14
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_15"));        // 15
            GameTextures.Add(Content.Load<Texture2D>("Tiles/JumpTile"));                // JumpTile (16)
            GameTextures.Add(Content.Load<Texture2D>("Tiles/GoalTile"));                // GoalTile (17)
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Spike_01"));       // Spike1 (18)
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Spike_02"));       // Spike2 (19)
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BGTile0"));        // BG-0 (20)
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BGTile1"));        // BG-1 (21)
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BGTile2"));        // BG-2 (22)
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BGTile3"));        // BG-3 (23)
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BGTile4"));        // BG-4 (24)
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BGTile5"));        // BG-5 (25)
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BGTile6"));        // BG-6 (26)
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BGTile7"));        // BG-7 (27)
            GameTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BGTile8"));        // BG-8 (28)

            GameTextures.Add(Content.Load<Texture2D>("Units/OneFramePlayer"));        // PlayerSpawn (29)
            GameTextures.Add(Content.Load<Texture2D>("Units/SmallerEnemy"));         // Enemy (30)
            GameTextures.Add(Content.Load<Texture2D>("Tiles/TeleportTile"));         // TeleporTile (31)

            TileSize = GameTextures[0].Width;

        }
    }
}
