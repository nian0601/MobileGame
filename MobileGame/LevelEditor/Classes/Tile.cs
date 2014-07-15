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
        
        protected bool ShouldDraw;
        protected int TileSize;

        public Vector2 IndexPos;
        public bool Collidable;

        public Tile(int X, int Y, Texture2D TileTexture, bool Draw, bool Collidable)
        {
            Texture = TileTexture;
            ShouldDraw = Draw;
            IndexPos = new Vector2(X, Y);
            
            this.TileSize = MapManager.TileSize;
            this.Collidable = Collidable;
        }

        public void Draw(SpriteBatch spritebatch, int x, int y, Vector2 offset)
        {
            if (ShouldDraw)
            {
                Vector2 DrawPos;
                if(Collidable)
                    DrawPos = new Vector2(x * TileSize, y * TileSize) + offset;
                else
                    DrawPos = new Vector2(x * TileSize, y * TileSize - Texture.Height/2) + offset;

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
        public PlayerTile(int X, int Y, Texture2D TileTexture, bool Draw): base(X, Y, TileTexture, Draw, false)
        {
        }
    }

    class GoalTile : Tile
    {
        public GoalTile(int X, int Y, Texture2D TileTexture, bool Draw): base(X, Y, TileTexture, Draw, false)
        {
        }
    }

    class TeleportTile : Tile
    {
        public TeleportTile(int X, int Y, Texture2D TileTexture, bool Draw): base(X, Y, TileTexture, Draw, false)
        {
        }
    }

    class JumpTile : Tile
    {
        public JumpTile(int X, int Y, Texture2D TileTexture, bool Draw): base(X, Y, TileTexture, Draw, false)
        {
        }
    }

}
