using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace MobileGame.LightingSystem
{
    public class SpotLight : Light
    {
        public Vector2 direction;
        public float innerAngle;
        public float outerAngle;

        public SpotLight(Vector2 pos, Vector2 dir, float inner, float outer, float power, float _radius, Color col)
            : base(pos, power, col)
        {
            direction = dir;
            innerAngle = inner;
            outerAngle = outer;
            radius = _radius;
        }

        public SpotLight(Vector2 pos, Vector2 dir, float angle, float power, Color col)
            : base(pos, power, col)
        {
            direction = dir;
            innerAngle = angle;
            outerAngle = angle;
        }

        public SpotLight(Vector2 pos, Vector2 dir, float angle, Color col)
            : base(pos, 0f, col)
        {
            direction = dir;
            innerAngle = outerAngle = angle;
        }

        public float GetAngleBias()
        {
            float diffAngle = (float)Math.Acos(Vector2.Dot(direction, Vector2.UnitY));
            if (float.IsNaN(diffAngle))
                diffAngle = (float)(((Math.Sign(-direction.Y) + 1) / 2f) * Math.PI);
            if (diffAngle - (outerAngle / 2f) < 0)
                return 0;
            return MathHelper.Pi * 2f;
        }

        public float GetBiasedAngle()
        {
            float lightDirAngle = Vector2.Dot(direction, Vector2.UnitY);
            lightDirAngle = (float)(Math.Acos(lightDirAngle) * Math.Sign(direction.X));

            if (float.IsNaN(lightDirAngle))
                lightDirAngle = ((Math.Sign(-direction.Y) + 1) / 2f) * (float)Math.PI;

            float angleBias = GetAngleBias();
            lightDirAngle += (Math.Abs(Math.Sign(lightDirAngle))) * (angleBias * (1 - ((Math.Sign(lightDirAngle) + 1) / 2f)));

            return lightDirAngle;
        }
    }
}
