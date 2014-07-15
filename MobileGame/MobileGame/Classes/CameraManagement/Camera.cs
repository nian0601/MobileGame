using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace MobileGame.CameraManagement
{
    class Camera
    {
        /// <summary>
        /// This will contain the object that we want to follow when there are no other focuspoints. Will most often be the player
        /// </summary>
        private static IFocusable defaultFocus;
        public static IFocusable DefaultFocus
        {
            get { return defaultFocus; }
            set { defaultFocus = value; }
        }

        //This will contain the object that has full focus, if no object has full focus this will be null
        private static IFocusable currentFocus;

        //Simply a list containing all active focuspoints. A focuspoint is active if the defaultFocus is inside its interestrange
        private static List<IFocusable> activeList;

        //List containing all focuspoints, so that we can loop through them to check if they are in interestrange
        private static List<IFocusable> allFocusPoints;

        //The average of all focuspoints, this is where the camera want to be looking
        private static Vector2 target;

        //This is simply used to draw something at the targetvector, to see where the camera is looking. --DEBUG STUFF--
        private static Texture2D emptyCircle, filledCircle;

        private static Matrix transform;
        private static float rotation;

        //Should we draw debugtextures or not?!?!?!?
        private static bool DrawDebug;

        /// <summary>
        /// The speed of the acctual camera
        /// </summary>
        private static float moveSpeed;
        /// <summary>
        /// How fast the target should change its position
        /// </summary>
        private static float targetChangeSpeed;
        /// <summary>
        /// How fast the target should move to currentFocus/defaultFocus
        /// </summary>
        private static float setFocusSpeed;

        #region Properties

        private static float zoom;
        public static float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; } //Negative zoomvalue will flip images, dont want that
        }

        private static Vector2 position;
        public static Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        private static int cameraWidth;
        public static int CameraWidth
        {
            get { return cameraWidth; }
        }

        private static int cameraHeight;
        public static int CameraHeight
        {
            get { return cameraHeight; }
        }

        private static int xBoundary;
        public static int XBoundary
        {
            get { return xBoundary; }
            set { xBoundary = value - cameraWidth/2; Console.WriteLine("xBoundary: " + xBoundary); }
        }

        private static int yBoundary;
        public static int YBoundary
        {
            get { return yBoundary; }
            set { yBoundary = value - cameraHeight/2; Console.WriteLine("yBoundary: " + yBoundary); }
        }

        #endregion

        //private static Vector2 defaultFocusOffset;

        public static void Initialize(int Height, int Width)
        {
            activeList = new List<IFocusable>();
            allFocusPoints = new List<IFocusable>();
            target = Vector2.Zero;
            rotation = 0f;
            zoom = 1f;
            moveSpeed = 1.5f;
            targetChangeSpeed = 2.5f;
            setFocusSpeed = 4.5f;
            position = Vector2.Zero;

            defaultFocus = null;
            currentFocus = null;
            //defaultFocusOffset = new Vector2(200, 0);

            cameraHeight = Height;
            cameraWidth = Width;

            DrawDebug = false;
        }

        public static void LoadStuff(ContentManager content)
        {
            emptyCircle = content.Load<Texture2D>("DebugTextures/EmptyCircle");
            filledCircle = content.Load<Texture2D>("DebugTextures/FilledCircle");
        }

        public static void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 finalTarget;
            float finalChangeSpeed;
            CheckRanges();

            if (KeyMouseReader.KeyClick(Keys.I))
                DrawDebug = !DrawDebug;

            if (currentFocus != null) //If we have a currentFocus we wnat to focus soley on that
            {
                //Set our desired target to currentFocus
                finalTarget = currentFocus.Position;

                finalChangeSpeed = setFocusSpeed;

                //Check if the player have moved out of currentFocus's control-range
                CheckCurrentVsDefaultRange();
            }
            else if (activeList.Count > 0) //If we have focuspoints in range we want to let them influence the camera
            {
                //Calculate where to look based on which focuspoints is in interest-range
                finalTarget = CalculateTarget();

                finalChangeSpeed = targetChangeSpeed;

                //Check if the player have moved out of any interest-ranges
                CheckRanges();
            }
            else //If we dont have any focuspoints in range we want to follow the defaultFocus
            {
                //Set our desired target to the defaultFocus
                finalTarget = defaultFocus.Position;// + defaultFocusOffset;

                finalChangeSpeed = setFocusSpeed;
            }

            //The camera position is updated in two steps:
            //First we lerp the targetPosition towards the finalTargetPosition
            target.X += (finalTarget.X - target.X) * finalChangeSpeed * delta;
            target.Y += (finalTarget.Y - target.Y) * finalChangeSpeed * delta;

            //and then we lerp the cameraPosition towards the targetPosition;
            position.X += (target.X - position.X) * moveSpeed * delta;
            position.Y += (target.Y - position.Y) * moveSpeed * delta;
            //This makes sure that the camera keeps a smooth movementcurve at all times, even when changeing focus, adding interestpoints etc


            position.X = MathHelper.Clamp(position.X, cameraWidth / 2, xBoundary);
            position.Y = MathHelper.Clamp(position.Y, cameraHeight / 2, yBoundary);

            //Console.WriteLine("CameraPos.x: " + position.X + ", CameraPos.y: " + position.Y);
        }

        public static void Draw(SpriteBatch sb)
        {
            if (DrawDebug)
            {
                //Draw debugtextures for all focuspoints
                foreach (IFocusable Object in allFocusPoints)
                {
                    Rectangle interestRect = new Rectangle((int)Object.Position.X - Object.InterestRadius, (int)Object.Position.Y - Object.InterestRadius, Object.InterestRadius * 2, Object.InterestRadius * 2);
                    Rectangle controlRect = new Rectangle((int)Object.Position.X - Object.ControlRadius, (int)Object.Position.Y - Object.ControlRadius, Object.ControlRadius * 2, Object.ControlRadius * 2);
                    sb.Draw(emptyCircle, interestRect, Color.Pink);
                    sb.Draw(emptyCircle, controlRect, Color.Black);
                }

                //Draw debugtexture for cameratarget
                Rectangle rect = new Rectangle((int)target.X, (int)target.Y, 10, 10);
                sb.Draw(filledCircle, rect, Color.Blue);

                //Draw debugtexture for cameraposition
                Rectangle cameraPos = new Rectangle((int)position.X, (int)position.Y, 10, 10);
                sb.Draw(filledCircle, cameraPos, Color.Green);

                //Draw debugtexture for defaultfocus
                //Rectangle defInterestRect = new Rectangle((int)defaultFocus.Position.X - defaultFocus.InterestRadius, (int)defaultFocus.Position.Y - defaultFocus.InterestRadius, defaultFocus.InterestRadius * 2, defaultFocus.InterestRadius * 2);
                //Rectangle defControlRect = new Rectangle((int)defaultFocus.Position.X - defaultFocus.ControlRadius, (int)defaultFocus.Position.Y - defaultFocus.ControlRadius, defaultFocus.ControlRadius * 2, defaultFocus.ControlRadius * 2);
                //sb.Draw(emptyCircle, defInterestRect, Color.Pink);
                //sb.Draw(emptyCircle, defControlRect, Color.Black);
            }  
        }

        public static void AddFocusObject(IFocusable Object)
        {
            allFocusPoints.Add(Object);
        }

        public static void RemoveFocusObject(IFocusable Object)
        {
            allFocusPoints.Remove(Object);
            //Make sure that we try to remove the object from the activelist aswell, so we dont get left with ghostpoints
            //(RemoveActivePoint() does its own checks to see if the point exists, so will only remove the point if it acctualy exists inside activelist)
            RemoveActivePoint(Object);
        }

        public static void ClearFocusList()
        {
            allFocusPoints.Clear();
            activeList.Clear();
        }

        public static void Move(Vector2 amount)
        {
            position += amount;
        }

        public static Matrix Get_Transformation()
        {
            transform =
                Matrix.CreateTranslation(new Vector3(-position.X - cameraWidth/2, -position.Y - cameraHeight/2, 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(cameraWidth, cameraHeight, 0));

            return transform;
        }

        public static Vector2 ScreenToWorldCoordinates(Vector2 coordinates)
        {
            return Vector2.Transform(coordinates, Matrix.Invert(Get_Transformation()));
        }

        private static Vector2 CalculateTarget()
        {
            Vector2 Target = new Vector2();

            foreach (IFocusable Object in activeList)
            {
                Target += Object.Position;
            }

            Target += defaultFocus.Position;
            Target /= activeList.Count + 1;

            return Target;
        }

        private static void CheckRanges()
        {
            for (int i = allFocusPoints.Count-1; i >= 0; i--)
            {
                float distance = Math.Abs((allFocusPoints[i].Position - defaultFocus.Position).Length());
                if (distance < allFocusPoints[i].InterestRadius)
                {
                    AddActivePoint(allFocusPoints[i]);
                }
                if (distance < allFocusPoints[i].ControlRadius)
                {
                    currentFocus = allFocusPoints[i];
                    break;
                }
            }

            for (int i = activeList.Count - 1; i >= 0; i--)
            {
                float distance = Math.Abs((activeList[i].Position - defaultFocus.Position).Length());

                if (distance > activeList[i].InterestRadius)
                    RemoveActivePoint(activeList[i]);
            }
        }

        private static void CheckCurrentVsDefaultRange()
        {
            float distance = Math.Abs((defaultFocus.Position - currentFocus.Position).Length());

            if (distance > currentFocus.ControlRadius)
                currentFocus = null;
        }

        private static void AddActivePoint(IFocusable Point)
        {
            if (!activeList.Contains(Point))
                activeList.Add(Point);
        }

        private static void RemoveActivePoint(IFocusable Point)
        {
            if(activeList.Contains(Point))
                activeList.Remove(Point);
        }
    }
}
