using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LevelEditor.Managers;
using LevelEditor.FileManagement;

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
        public bool Selected;
        public int TileValue;
        public int TileType;
        public float DrawDepth;

        public Tile(int X, int Y, bool Draw)
        {
            ShouldDraw = Draw;
            IndexPos = new Vector2(X, Y);
            Color = Color.White;
            TileDrawOffset = Vector2.Zero;
            
            this.TileSize = MapManager.TileSize;

            Collidable = false;
            CanJumpThrough = false;
            Selected = false;
            TileValue = 0;

            if (ShouldDraw)
                TileType = 1;
            else
                TileType = 0;

            DrawDepth = 0;
        }

        public void Draw(SpriteBatch spritebatch, int x, int y, Vector2 offset)
        {
            if (ShouldDraw || Selected)
            {
                Vector2 DrawPos = new Vector2(x * TileSize + TileDrawOffset.X, y * TileSize + TileDrawOffset.Y) + offset;

                if (Game1.EditMode == 1 && Collidable)
                    Color = Color.Red;
                else if (Game1.EditMode == 2)
                {
                    if (!Collidable)
                        Color *= 0.25f;
                    else if (CanJumpThrough)
                        Color = Color.Red;
                }

                if (Selected)
                    Color = Color.LightBlue;

                if (Texture == null)
                    Texture = TextureManager.GameTextures[15];

                //spritebatch.Draw(Texture, DrawPos, Color);
                spritebatch.Draw(Texture, DrawPos, null, Color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                Color = Color.White;
            }
        }

        public void SetTileType(int TileValue)
        {
            Texture = TextureManager.GameTextures[TileValue];
            this.TileValue = TileValue;
        }

        public void ImportTileData(TileData TileData)
        {
            SetTileType(TileData.TileValue);
            Collidable = TileData.Collidable;
            CanJumpThrough = TileData.CanJumpThrough;

            if (TileData.TileType == 0)
                ShouldDraw = false;
        }
    }

    class PlayerTile : Tile
    {
        public PlayerTile(int X, int Y, bool Draw): base(X, Y, Draw)
        {
            Texture = TextureManager.GameTextures[19];
            TileDrawOffset = new Vector2(0, -Texture.Height/2);
            TileType = 9;
        }
    }

    class GoalTile : Tile
    {
        public GoalTile(int X, int Y, bool Draw): base(X, Y, Draw)
        {
            Texture = TextureManager.GameTextures[18];
            TileDrawOffset = new Vector2(0, -Texture.Height / 2);
            TileType = 3;
        }
    }

    class TeleportTile : Tile
    {
        public TeleportTile(int X, int Y, bool Draw): base(X, Y, Draw)
        {
            Texture = TextureManager.GameTextures[17];
            TileDrawOffset = new Vector2(0, -Texture.Height / 2);
            TileType = 2;
        }
    }

    class JumpTile : Tile
    {
        public JumpTile(int X, int Y, bool Draw): base(X, Y, Draw)
        {
            Texture = TextureManager.GameTextures[16];
            TileDrawOffset = new Vector2(0, -Texture.Height / 2);
            TileType = 1;
        }
    }

    class EnemyTile : Tile
    {
        public EnemyTile(int X, int Y, bool Draw): base(X, Y, Draw)
        {
            Texture = TextureManager.GameTextures[20];
            TileDrawOffset = new Vector2(0, -Texture.Height / 2);
            TileType = 4;
        }
    }

}
