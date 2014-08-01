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
        public static Texture2D PlayerColTex { get; private set; }

        public static List<Texture2D> TileTextures { get; private set; }

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
            PlayerColTex = Content.Load<Texture2D>("Units/PlayerHitData");

            TileTextures = new List<Texture2D>();
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_00"));     // 0
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_01"));     // 1
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_02"));     // 2
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_03"));     // 3
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_04"));     // 4
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_05"));     // 5
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_06"));     // 6
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_07"));     // 7
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_08"));     // 8
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_09"));     // 9
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_10"));    // 10
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_11"));    // 11
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_12"));    // 12
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_13"));    // 13
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_14"));    // 14
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/Tile_15"));    // 15

            #region OldTiles
            //TileTextures.Add(Content.Load<Texture2D>("Tiles/Tiles/0Tile"));     // 0
            //TileTextures.Add(Content.Load<Texture2D>("Tiles/Tiles/1Tile"));     // 1
            //TileTextures.Add(Content.Load<Texture2D>("Tiles/Tiles/2Tile"));     // 2
            //TileTextures.Add(Content.Load<Texture2D>("Tiles/Tiles/3Tile"));     // 3
            //TileTextures.Add(Content.Load<Texture2D>("Tiles/Tiles/4Tile"));     // 4
            //TileTextures.Add(Content.Load<Texture2D>("Tiles/Tiles/5Tile"));     // 5
            //TileTextures.Add(Content.Load<Texture2D>("Tiles/Tiles/6Tile"));     // 6
            //TileTextures.Add(Content.Load<Texture2D>("Tiles/Tiles/7Tile"));     // 7
            //TileTextures.Add(Content.Load<Texture2D>("Tiles/Tiles/8Tile"));     // 8
            //TileTextures.Add(Content.Load<Texture2D>("Tiles/Tiles/9Tile"));     // 9
            //TileTextures.Add(Content.Load<Texture2D>("Tiles/Tiles/10Tile"));    // 10
            //TileTextures.Add(Content.Load<Texture2D>("Tiles/Tiles/11Tile"));    // 11
            //TileTextures.Add(Content.Load<Texture2D>("Tiles/Tiles/12Tile"));    // 12
            //TileTextures.Add(Content.Load<Texture2D>("Tiles/Tiles/13Tile"));    // 13
            //TileTextures.Add(Content.Load<Texture2D>("Tiles/Tiles/14Tile"));    // 14
            //TileTextures.Add(Content.Load<Texture2D>("Tiles/Tiles/15Tile"));    // 15
            #endregion

            TileSize = TileTextures[0].Width;

        }
    }
}
