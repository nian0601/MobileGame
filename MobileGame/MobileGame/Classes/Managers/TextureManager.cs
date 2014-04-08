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

        public static void LoadContents(ContentManager Content)
        {
            AirTile = Content.Load<Texture2D>("Tiles/AirTile");
            PlatformTile = Content.Load<Texture2D>("Tiles/PlatformTile");
            PlayerTile = Content.Load<Texture2D>("Tiles/PlayerTile");
            GoalTile = Content.Load<Texture2D>("Tiles/GoalTile");
        }
    }
}
