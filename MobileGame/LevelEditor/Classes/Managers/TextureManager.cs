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

        public static Texture2D EnemyTexture { get; private set; }
        public static Texture2D JumpTileTexture { get; private set; }
        public static Texture2D TeleportTileTexture { get; private set; }
        public static Texture2D GoalTexture { get; private set; }
        public static Texture2D PlayerTexture { get; private set; }
        public static Texture2D GridTexture { get; private set; }
        public static Texture2D Background { get; private set; }

        public static List<Texture2D> TileTextures { get; private set; }

        public static void LoadContents(ContentManager Content)
        {
            EnemyTexture = Content.Load<Texture2D>("GameTextures/SmallerEnemy");
            JumpTileTexture = Content.Load<Texture2D>("GameTextures/JumpTile");
            TeleportTileTexture = Content.Load<Texture2D>("GameTextures/TeleportTile");
            GoalTexture = Content.Load<Texture2D>("GameTextures/GoalTile");
            PlayerTexture = Content.Load<Texture2D>("GameTextures/SmallerPlayer");
            GridTexture = Content.Load<Texture2D>("Editor/GridTexture");
            Background = Content.Load<Texture2D>("Editor/Background");

            TileTextures = new List<Texture2D>();
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_00"));     // 0
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_01"));     // 1
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_02"));     // 2
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_03"));     // 3
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_04"));     // 4
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_05"));     // 5
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_06"));     // 6
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_07"));     // 7
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_08"));     // 8
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_09"));     // 9
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_10"));    // 10
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_11"));    // 11
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_12"));    // 12
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_13"));    // 13
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_14"));    // 14
            TileTextures.Add(Content.Load<Texture2D>("GameTextures/Tiles/Tile_15"));    // 15

            TileSize = TileTextures[0].Width;

        }
    }
}
