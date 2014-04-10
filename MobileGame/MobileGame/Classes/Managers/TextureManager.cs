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

        public static Texture2D BottomLeftCorner { get; private set; }
        public static Texture2D BottomLeftTile { get; private set; }
        public static Texture2D BottomMiddleTile { get; private set; }
        public static Texture2D BottomRightCorner { get; private set; }
        public static Texture2D BottomRightTile { get; private set; }

        public static Texture2D MiddleLeftTile { get; private set; }
        public static Texture2D MiddleTile { get; private set; }
        public static Texture2D MiddleRightTile { get; private set; }

        public static Texture2D TopLeftCorner { get; private set; }
        public static Texture2D TopLeftTile { get; private set; }
        public static Texture2D TopMiddleTile { get; private set; }
        public static Texture2D TopRightCorner { get; private set; }
        public static Texture2D TopRightTile { get; private set; }

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

            BottomLeftCorner = Content.Load<Texture2D>("Tiles/NewTiles/BottomLeftCorner");
            BottomLeftTile = Content.Load<Texture2D>("Tiles/NewTiles/BottomLeftTile");
            BottomMiddleTile = Content.Load<Texture2D>("Tiles/NewTiles/BottomMiddleTile");
            BottomRightCorner = Content.Load<Texture2D>("Tiles/NewTiles/BottomRightCorner");
            BottomRightTile = Content.Load<Texture2D>("Tiles/NewTiles/BottomRightTile");

            MiddleLeftTile = Content.Load<Texture2D>("Tiles/NewTiles/MiddleLeftTile");
            MiddleTile = Content.Load<Texture2D>("Tiles/NewTiles/MiddleTile");
            MiddleRightTile = Content.Load<Texture2D>("Tiles/NewTiles/MiddleRightTile");

            TopLeftCorner = Content.Load<Texture2D>("Tiles/NewTiles/TopLeftCorner");
            TopLeftTile = Content.Load<Texture2D>("Tiles/NewTiles/TopLeftTile");
            TopMiddleTile = Content.Load<Texture2D>("Tiles/NewTiles/TopMiddleTile");
            TopRightCorner = Content.Load<Texture2D>("Tiles/NewTiles/TopRightCorner");
            TopRightTile = Content.Load<Texture2D>("Tiles/NewTiles/TopRightTile");
        }
    }
}
