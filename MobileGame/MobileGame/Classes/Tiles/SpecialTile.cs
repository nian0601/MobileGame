using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MobileGame.Managers;
using MobileGame.Enemies;
using MobileGame.Units;
using MobileGame.CameraManagement;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MobileGame.Tiles
{
    class SpecialTile : IFocusable
    {
        protected Vector2 myPixelPos;
        protected int myInterestRadius;
        protected int myControlRadius;
        protected Texture2D myInterestCircle;
        protected Texture2D myControlCircle;

        public Vector2 Position { get { return myPixelPos; } }

        public int Width { get { return MapManager.TileSize; } }

        public int Height { get { return MapManager.TileSize; } }

        public int InterestRadius { get { return myInterestRadius; } }

        public int ControlRadius { get { return myControlRadius; } }

        public Texture2D InterestCircle
        {
            get { return myInterestCircle; }
            set { myInterestCircle = value; }
        }

        public Texture2D ControlCircle
        {
            get { return myControlCircle; }
            set { myControlCircle = value; }
        }

        public SpecialTile(int x, int y) { }

        public virtual void CollideWithUnit(Player Unit) { }

        public virtual void CollideWithUnit(Player Unit, float ElapsedTime) { }

        public virtual void CollideWithUnit(IEnemy Unit) { }

        public virtual Rectangle HitBox() { return Rectangle.Empty; }
    }
}
