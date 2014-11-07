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
        private Vector2 myPosition;
        public Vector2 Position { get { return myPosition; } }

        private float myNormalRadius;
        private float myRadius;
        public float Radius { get { return myRadius; } }

        private float myPower;
        public float Power { get { return myPower; } }

        private Color myColor;
        public Color Color { get { return myColor; } }

        private bool myFlickerLight;
        private float currTime;
        private bool myGrowing;

        public PointLight(Vector2 Pos, bool aFlickerLight) : this(Pos, 0f, 0f, Color.White, aFlickerLight) { }

        public PointLight(Vector2 Pos, float Radius, bool aFlickerLight) : this(Pos, Radius, 0f, Color.White, aFlickerLight) { }

        public PointLight(Vector2 Pos, float Radius, Color Color, bool aFlickerLight) : this(Pos, Radius, 0f, Color, aFlickerLight) { }

        public PointLight(Vector2 Pos, float Radius, float Power, bool aFlickerLight) : this(Pos, Radius, Power, Color.White, aFlickerLight) { }

        public PointLight(Vector2 Pos, float Radius, float Power, Color Color, bool aFlickerLight)
        {
            myPosition = Pos;
            myNormalRadius = Radius;
            myRadius = myNormalRadius;
            myPower = Power;
            myColor = Color;
            myFlickerLight = aFlickerLight;

            currTime = 0;
            myGrowing = true;
            //maxTime = 2;
        }

        public void Update(float elapsedTime)
        {
            currTime += elapsedTime;
            if (myFlickerLight == true)
            {

                if (myGrowing == true)
                {
                    if (myRadius < myNormalRadius + 20)
                    {
                        myRadius = LinearTween(currTime, myNormalRadius - 20, 20, 200);
                    }
                    else 
                    {
                        currTime -= currTime;
                        myGrowing = false;
                    }
                }
                else
                {
                    if (myRadius > myNormalRadius - 20)
                    {
                        myRadius = LinearTween(currTime, myNormalRadius + 20, -20, 200);
                    }
                    else
                    {
                        currTime -= currTime;
                        myGrowing = true;
                    }
                }
            }
            
        }

        public void SetPosition(Vector2 aNewPos)
        {
            myPosition = aNewPos;
        }

        private float LinearTween(float time, float startValue, int changeInValue, int duration)
        {
            return changeInValue * time / duration + startValue;
        }

        private float QuadricInTween(float time, float startValue, int changeInValue, int duration)
        {
            float tempTime = time / duration;
            return changeInValue * tempTime * tempTime + duration;
        }

        private float QuadricOutTween(float time, float startValue, int changeInValue, int duration)
        {
            float tempTime = time / duration;
            return -changeInValue * tempTime * (tempTime-2) + duration;
        }

        private float QuadricInOutTween(float time, float startValue, int changeInValue, int duration)
        {
            float tempTime = time / duration;
            if(tempTime < 1)
                return changeInValue * tempTime * tempTime + duration;

            tempTime--;

            return -changeInValue * tempTime * (tempTime - 1) + duration;
        }
    }
}
