using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LevelEditor.Managers
{
    static class TextureManager
    {
        public static int TileSize { get; private set; }

        public static Texture2D GridTexture { get; private set; }
        public static Texture2D Background { get; private set; }

        public static Texture2D SelectionTexture { get; private set; }
        public static Texture2D GridCell { get; private set; }

        public static List<Texture2D> GameTextures { get; private set; }

        public static void LoadContents(ContentManager Content)
        {
            GridTexture = Content.Load<Texture2D>("Editor/GridTexture");
            Background = Content.Load<Texture2D>("Editor/Background");

            SelectionTexture = Content.Load<Texture2D>("FilledSquare");
            GridCell = Content.Load<Texture2D>("Editor/GridCell");

            GameTextures = new List<Texture2D>();
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_00"));        // 0
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_01"));        // 1
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_02"));        // 2
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_03"));        // 3
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_04"));        // 4
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_05"));        // 5
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_06"));        // 6
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_07"));        // 7
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_08"));        // 8
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_09"));        // 9
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_10"));        // 10
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_11"));        // 11
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_12"));        // 12
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_13"));        // 13
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_14"));        // 14
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_15"));        // 15
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/JumpTile"));             // JumpTile (16)
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/GoalTile"));             // GoalTile (17)
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Spike_01"));        // Spike1 (18)
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Spike_02"));        // Spike2 (19)
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BGTile0"));        // BG-0 (20)
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BGTile1"));        // BG-1 (21)
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BGTile2"));        // BG-2 (22)
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BGTile3"));        // BG-3 (23)
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BGTile4"));        // BG-4 (24)
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BGTile5"));        // BG-5 (25)
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BGTile6"));        // BG-6 (26)
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BGTile7"));        // BG-7 (27)
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/BGTile8"));        // BG-8 (28)

            GameTextures.Add(Content.Load<Texture2D>("GameTextures/OneFramePlayer"));        // PlayerSpawn (29)
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/SmallerEnemy"));         // Enemy (30)
            GameTextures.Add(Content.Load<Texture2D>("GameTextures/TeleportTile"));         // TeleporTile (31)

            TileSize = GameTextures[0].Width;

        }
    }
}
