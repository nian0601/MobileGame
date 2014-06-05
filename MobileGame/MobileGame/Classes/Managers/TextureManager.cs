using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MobileGame
{
    static class TextureManager
    {
        public static int TileSize { get; private set; }

        public static Texture2D AirTile { get; private set; }
        public static Texture2D PlatformTile { get; private set; }
        public static Texture2D PlayerTile { get; private set; }
        public static Texture2D GoalTile { get; private set; }
        public static Texture2D LayerdGoalTile { get; private set; }
        public static Texture2D JumpTile { get; private set; }
        public static Texture2D TeleportTile { get; private set; }
        public static Texture2D TestBigTile { get; private set; }

        public static Texture2D EnemyTex { get; private set; }
        public static Texture2D SmallerEnemyTex { get; private set; }
        public static Texture2D SmallerPlayerTex { get; private set; }

        public static List<Texture2D> TileTextures { get; private set; }

        public static void LoadContents(ContentManager Content)
        {
            AirTile = Content.Load<Texture2D>("Tiles/AirTile");
            PlatformTile = Content.Load<Texture2D>("Tiles/PlatformTile");
            PlayerTile = Content.Load<Texture2D>("Tiles/PlayerTile");
            GoalTile = Content.Load<Texture2D>("Tiles/GoalTile");
            LayerdGoalTile = Content.Load<Texture2D>("Tiles/LayerdGoalTile");
            JumpTile = Content.Load<Texture2D>("Tiles/JumpTile");
            TeleportTile = Content.Load<Texture2D>("Tiles/TeleportTile");
            TestBigTile = Content.Load<Texture2D>("Tiles/TestBigTile");

            TileSize = AirTile.Width;

            EnemyTex = Content.Load<Texture2D>("Units/Enemy");
            SmallerEnemyTex = Content.Load<Texture2D>("Units/SmallerEnemy");
            SmallerPlayerTex = Content.Load<Texture2D>("Units/SmallerPlayer");

            TileTextures = new List<Texture2D>();
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/SingleTile"));          // 0
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TopOpenTile"));         // 1
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/RightOpenTile"));       // 2
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TopRightOpenTile"));    // 3
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BottomOpenTile"));      // 4
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BottomTopOpenTile"));   // 5
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BottomRightOpenTile")); // 6
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/LeftClosedTile"));      // 7
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/LeftOpenTile"));        // 8
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TopLeftOpenTile"));     // 9
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/LeftRightOpenTile"));   // 10
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BottomClosedTile"));    // 11
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BottomLeftOpenTile"));  // 12
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/RightClosedTile"));     // 13
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TopClosedTile"));       // 14
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/MiddleTile"));          // 15

        }
    }
}
