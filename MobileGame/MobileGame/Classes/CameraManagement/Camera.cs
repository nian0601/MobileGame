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
            set { defaultFocus = value; defaultFocus.InterestCircle = CreateCircle(defaultFocus.InterestRadius); defaultFocus.ControlCircle = CreateCircle(defaultFocus.ControlRadius); }
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

        //Used to create fancy circles
        private static GraphicsDevice graphicsDevice;

        private static Vector2 pastDefaultFocusPos;

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

        /// <summary>
        /// This is used as the camera size, its get set to the size of the gameWindow
        /// </summary>
        private static Viewport viewPort;
        public static Viewport ViewPort
        {
            get { return viewPort; }
            set { viewPort = value; }
        }

        /// <summary>
        /// The zoomvalue of the camera
        /// </summary>
        private static float zoom;
        public static float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; ValidateZoom(); } //Negative zoomvalue will flip images, dont want that
        }

        /// <summary>
        /// The position of the camera, also calls ValidatePos() when the setter is used
        /// </summary>
        private static Vector2 position;
        public static Vector2 Position
        {
            get { return position; }
            set { position = value; ValidatePosition(); }
        }

        /// <summary>
        /// This is the total area the camera should be able to show, so this should be equal to the pixelsize of the entire map
        /// </summary>
        private static Rectangle? limits;
        public static Rectangle? Limits { set { limits = value; ValidateZoom(); ValidatePosition(); } }

        #endregion

        public static void Initialize(GraphicsDevice GraphicsDevice)
        {
            graphicsDevice = GraphicsDevice;

            viewPort = graphicsDevice.Viewport;

            activeList = new List<IFocusable>();
            allFocusPoints = new List<IFocusable>();
            target = Vector2.Zero;
            rotation = 0f;
            zoom = 0.9f;
            moveSpeed = 1.5f;
            targetChangeSpeed = 2.5f;
            setFocusSpeed = 2.5f;
            position = Vector2.Zero;
            pastDefaultFocusPos = Vector2.Zero;

            defaultFocus = null;
            currentFocus = null;
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

            if (KeyMouseReader.KeyClick(Keys.Z))
                zoom -= 0.1f;
            else if (KeyMouseReader.KeyClick(Keys.X))
                zoom += 0.1f;

            #region Target-Follow-And-Select-Code
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
                //This gets the differencial of current and past focus-positions
                //This is used to determine in what direction we want to push the camera
                Vector2 currentPos = defaultFocus.Position;
                float xValue = currentPos.X - pastDefaultFocusPos.X;
                float yValue = currentPos.Y - pastDefaultFocusPos.Y;

                //Create a new vector to hold the distance that we want to push the camnera
                Vector2 targetOffset = Vector2.Zero;

                //If the xValue is negative that means we are moving to the left
                if (xValue < -1)
                    //So we push the camera infront of us to the left
                    targetOffset.X = -200;
                //if the xValue is positive it means we are moving to the right
                else if (xValue > 1)
                    //So we push the camera infront of us to the right
                    targetOffset.X = 200;

                //If the yValue is negative that means we are going upwards (jumping)
                if (yValue < -1)
                    //So we push the camera above us a lil bit
                    targetOffset.Y = -200;
                //If the yValue is greater than 10 that means we are falling rather quickly (like when jumping of a ledge)
                else if (yValue > 10)
                    //So we push the camera quite far below us
                    targetOffset.Y = 600;
                //If the yValue is positive but less than 10 that means we are falling, but not very quickly (a short drop)
                else if (yValue > 1)
                    //So we push the camera just a lil bit below us
                    targetOffset.Y = 200;

                //Set our desired target to the defaultFocus
                finalTarget = defaultFocus.Position + targetOffset;

                finalChangeSpeed = setFocusSpeed;

                pastDefaultFocusPos = defaultFocus.Position;
            }
            #endregion

            //The camera position is updated in two steps:
            //First we lerp the targetPosition towards the finalTargetPosition
            target.X += (finalTarget.X - target.X) * finalChangeSpeed * delta;
            target.Y += (finalTarget.Y - target.Y) * finalChangeSpeed * delta;

            //and then we lerp the cameraPosition towards the targetPosition;
            position.X += (target.X - position.X) * moveSpeed * delta;
            position.Y += (target.Y - position.Y) * moveSpeed * delta;
            ValidatePosition();
        } 

        public static void Draw(SpriteBatch sb)
        {
            if (Game1.Debugging)
            {
                //Draw debugtextures for all focuspoints
                foreach (IFocusable Object in allFocusPoints)
                {
                    Vector2 InterestPos = new Vector2(Object.Position.X - Object.InterestRadius, Object.Position.Y - Object.InterestRadius);
                    Vector2 ControlPos = new Vector2(Object.Position.X - Object.ControlRadius, Object.Position.Y - Object.ControlRadius);

                    sb.Draw(Object.InterestCircle, InterestPos, Color.Pink);
                    sb.Draw(Object.ControlCircle, ControlPos, Color.Black);
                }

                //Draw debugtexture for cameratarget
                Rectangle rect = new Rectangle((int)target.X, (int)target.Y, 10, 10);
                sb.Draw(filledCircle, rect, null, Color.Blue, 1f, Vector2.Zero, SpriteEffects.None, 1f); 

                //Draw debugtexture for cameraposition
                Rectangle cameraPos = new Rectangle((int)position.X, (int)position.Y, 10, 10);
                sb.Draw(filledCircle, cameraPos, null, Color.Green, 1f, Vector2.Zero, SpriteEffects.None, 1f); 

                //Draw debugtexture for defaultfocus
                //Vector2 defInterestPos = new Vector2(defaultFocus.Position.X - defaultFocus.InterestRadius, defaultFocus.Position.Y - defaultFocus.InterestRadius);
                //Vector2 defControlPos = new Vector2(defaultFocus.Position.X - defaultFocus.ControlRadius, defaultFocus.Position.Y - defaultFocus.ControlRadius);
                
                //sb.Draw(defaultFocus.InterestCircle, defInterestPos, Color.Pink);
                //sb.Draw(defaultFocus.ControlCircle, defControlPos, Color.Black);
            }  
        }

        public static void AddFocusObject(IFocusable Object)
        {
            Object.InterestCircle = CreateCircle(Object.InterestRadius);
            Object.ControlCircle = CreateCircle(Object.ControlRadius);
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

        public static Matrix Get_Transformation()
        {
            transform =
                Matrix.CreateTranslation(new Vector3(-position.X - viewPort.Width/2, -position.Y - viewPort.Height/2, 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(viewPort.Width, viewPort.Height, 0));

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

        private static Texture2D CreateCircle(int radius)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new Texture2D(graphicsDevice, outerRadius, outerRadius);

            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = Color.White;
            }

            texture.SetData(data);
            return texture;
        }

        private static void ValidateZoom()
        {
            if (limits.HasValue)
            {
                float minZoomX = (float)viewPort.Width / limits.Value.Width;
                float minZoomY = (float)viewPort.Height / limits.Value.Height;
                zoom = MathHelper.Max(zoom, MathHelper.Max(minZoomX, minZoomY));
            }
        }

        private static void ValidatePosition()
        {
            if (limits.HasValue)
            {
                Vector2 cameraWorldMin = Vector2.Transform(Vector2.Zero, Matrix.Invert(Get_Transformation()));
                Vector2 cameraSize = new Vector2(viewPort.Width, viewPort.Height) / zoom;
                Vector2 limitWorldMin = new Vector2(limits.Value.Left, limits.Value.Top);
                Vector2 limitWorldMax = new Vector2(limits.Value.Right, limits.Value.Bottom);
                Vector2 positionOffset = position - cameraWorldMin;
                position = Vector2.Clamp(cameraWorldMin, limitWorldMin, limitWorldMax - cameraSize) + positionOffset;
            }
        }
    }
}
