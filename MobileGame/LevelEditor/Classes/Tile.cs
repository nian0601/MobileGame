using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LevelEditor.Managers;

namespace LevelEditor
{
    class Tile
    {
        private Texture2D Texture;
        private Vector2 Pos;
        public Vector2 IndexPos;
        private bool ShouldDraw;
        public bool Collidable;

        public Tile(int X, int Y, Texture2D TileTexture, int TileSize, bool Draw, bool Collidable)
        {
            Texture = TileTexture;
            ShouldDraw = Draw;
            IndexPos = new Vector2(X, Y);
            Pos = new Vector2(X * TileSize, Y * TileSize);
            this.Collidable = Collidable;
        }

        public void Draw(SpriteBatch spritebatch, int x, int y, Vector2 offset)
        {
            if (ShouldDraw)
            {
                Vector2 DrawPos = new Vector2(x * MapManager.TileSize, y * MapManager.TileSize) + offset;
                spritebatch.Draw(Texture, DrawPos, Color.White);
            }
        }

        public void SetTexture(Texture2D NewTexture)
        {
            Texture = NewTexture;
        }
    }

    class PlayerTile : Tile
    {
        public PlayerTile(int X, int Y, Texture2D TileTexture, int TileSize, bool Draw)
            : base(X, Y, TileTexture, TileSize, Draw, false)
        { }
    }

    class GoalTile : Tile
    {
        public GoalTile(int X, int Y, Texture2D TileTexture, int TileSize, bool Draw)
            : base(X, Y, TileTexture, TileSize, Draw, false)
        { }
    }

}
