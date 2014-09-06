using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MobileGame.Lights
{
    class PointLight
    {
        public Vector2 Position;
        public float Radius;
        public float Power;
        public Color Color;

        public PointLight(Vector2 Pos): this(Pos, 0f, 0f, Color.White) { }

        public PointLight(Vector2 Pos, float Radius): this(Pos, Radius, 0f, Color.White) { }

        public PointLight(Vector2 Pos, float Radius, Color Color): this(Pos, Radius, 0f, Color) {}

        public PointLight(Vector2 Pos, float Radius, float Power): this(Pos, Radius, Power, Color.White) { }

        public PointLight(Vector2 Pos, float Radius, float Power, Color Color)
        {
            this.Position = Pos;
            this.Radius = Radius;
            this.Power = Power;
            this.Color = Color;
        }
    }
}
