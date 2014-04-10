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
        }
    }
}
