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

        public enum TileTypes
        {
            BottomClosedLeftCornerTile,
            BottomClosedRightCornerTile,
            BottomClosedTile,
            BottomLeftCorner,
            BottomLeftOpenTile,
            BottomLeftTile,
            BottomMiddleTile,
            BottomOpenTile,
            BottomRightCorner,
            BottomRightOpenTile,
            BottomRightTile,
            BottomTopOpenTile,
            FourCornersTile,
            LeftClosedBottomCornerTile,
            LeftClosedTile,
            LeftClosedTopCornerTile,
            LeftOpenTile,
            LeftRightOpenTile,
            MiddleLeftTile,
            MiddleRightTile,
            MiddleTile,
            RightClosedBottomCenterTile,
            RightClosedTile,
            RightClosedTopCenterTile,
            RightOpenTile,
            SingleTile,
            ThreeCornersBottomLeftTile,
            ThreeCornersBottomRightTile,
            ThreeCornersTopLeftTile,
            ThreeCornersTopRightTile,
            TopClosedLeftCornerTile,
            TopClosedrightCornerTile,
            TopClosedTile,
            TopLeftBottomRightCornerTile,
            TopLeftCorner,
            TopLeftOpenTile,
            TopLeftTile,
            TopMiddleTile,
            TopOpenTile,
            TopRightBottomLeftCornerTile,
            TopRightCorner,
            TopRightOpenTile,
            TopRightTile,
            TwoCornersBottomTile,
            TwoCornersLeftTile,
            TwoCornersRightTile,
            TwoCornersTopTile

        }

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

            EnemyTex = Content.Load<Texture2D>("Units/Enemy");
            SmallerEnemyTex = Content.Load<Texture2D>("Units/SmallerEnemy");
            SmallerPlayerTex = Content.Load<Texture2D>("Units/SmallerPlayer");

            TileTextures = new List<Texture2D>();

            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BottomClosedLeftCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BottomClosedRightCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BottomClosedTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BottomLeftCorner"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BottomLeftOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BottomLeftTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BottomMiddleTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BottomOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BottomRightCorner"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BottomRightOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BottomRightTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/BottomTopOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/FourCornersTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/LeftClosedBottomCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/LeftClosedTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/LeftClosedTopCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/LeftOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/LeftRightOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/MiddleLeftTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/MiddleRightTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/MiddleTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/RightClosedBottomCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/RightClosedTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/RightClosedTopCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/RightOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/SingleTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/ThreeCornersBottomLeftTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/ThreeCornersBottomRightTIle"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/ThreeCornersTopLeftTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/ThreeCornersTopRightTIle"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TopClosedLeftCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TopClosedRightCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TopClosedTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TopLeftBottomRightCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TopLeftCorner"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TopLeftOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TopLeftTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TopMiddleTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TopOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TopRightBottomLeftCornerTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TopRightCorner"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TopRightOpenTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TopRightTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TwoCornersBottomTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TwoCornersLeftTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TwoCornersRightTile"));
            TileTextures.Add(Content.Load<Texture2D>("Tiles/NewTiles/TwoCornersTopTile"));
        }
    }
}
