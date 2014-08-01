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
        protected Texture2D Texture;
        protected int TileSize;
        protected Vector2 TileDrawOffset;

        public Vector2 IndexPos;
        public Color Color;
        public bool ShouldDraw;
        public bool Collidable;
        public bool CanJumpThrough;

        public Tile(int X, int Y, bool Draw)
        {
            ShouldDraw = Draw;
            IndexPos = new Vector2(X, Y);
            Color = Color.White;
            TileDrawOffset = Vector2.Zero;
            
            this.TileSize = MapManager.TileSize;

            Collidable = false;
            CanJumpThrough = false;
        }

        public void Draw(SpriteBatch spritebatch, int x, int y, Vector2 offset)
        {
            if (ShouldDraw)
            {
                Vector2 DrawPos = new Vector2(x * TileSize + TileDrawOffset.X, y * TileSize + TileDrawOffset.Y) + offset;

                if (Game1.EditMode == 1 && Collidable)
                    Color = Color.Red;
                else if (Game1.EditMode == 2 && CanJumpThrough)
                    Color = Color.Black;

                spritebatch.Draw(Texture, DrawPos, Color);

                Color = Color.White;
            }
        }

        public void SetTileType(int TileValue)
        {
            Texture = TextureManager.TileTextures[TileValue];
        }
    }

    class PlayerTile : Tile
    {
        public PlayerTile(int X, int Y, bool Draw): base(X, Y, Draw)
        {
            Texture = TextureManager.PlayerTexture;
            TileDrawOffset = new Vector2(0, -Texture.Height/2);
        }
    }

    class GoalTile : Tile
    {
        public GoalTile(int X, int Y, bool Draw): base(X, Y, Draw)
        {
            Texture = TextureManager.GoalTexture;
            TileDrawOffset = new Vector2(0, -Texture.Height / 2);
        }
    }

    class TeleportTile : Tile
    {
        public TeleportTile(int X, int Y, bool Draw): base(X, Y, Draw)
        {
            Texture = TextureManager.TeleportTileTexture;
            TileDrawOffset = new Vector2(0, -Texture.Height / 2);
        }
    }

    class JumpTile : Tile
    {
        public JumpTile(int X, int Y, bool Draw): base(X, Y, Draw)
        {
            Texture = TextureManager.JumpTileTexture;
            TileDrawOffset = new Vector2(0, -Texture.Height / 2);
        }
    }

    class EnemyTile : Tile
    {
        public EnemyTile(int X, int Y, bool Draw): base(X, Y, Draw)
        {
            Texture = TextureManager.EnemyTexture;
            TileDrawOffset = new Vector2(0, -Texture.Height / 2);
        }
    }

}
