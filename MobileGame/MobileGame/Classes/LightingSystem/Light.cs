using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace MobileGame.LightingSystem
{
    public abstract class Light
    {
        public Vector2 Position;
        public float Power = 0f;
        public Color Color;
        public float Radius;

        public Light(Vector2 pos)
            : this(pos, 0f, Color.White)
        {
        }

        public Light(Vector2 pos, float power, Color color)
        {
            Position = pos;
            Power = power;
            this.Color = color;
        }
    }
}
