using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace MobileGame.LightingSystem
{
    public class PointLight : Light
    {
        public PointLight(Vector2 pos)
            : base(pos, 0f, Color.White)
        {
        }

        public PointLight(Vector2 pos, float power, float radius)
            : base(pos, 0f, Color.White)
        {
            this.Radius = radius;
        }

        public PointLight(Vector2 pos, Color color)
            : base(pos, 0f, color)
        {
        }

        public PointLight(Vector2 pos, float radius)
            : base(pos, 0f, Color.White)
        {
            this.Radius = radius;
        }

        public PointLight(Vector2 pos, float power, float radius, Color color)
            : base(pos, power, color)
        {
            this.Radius = radius;
        }
    }
}
