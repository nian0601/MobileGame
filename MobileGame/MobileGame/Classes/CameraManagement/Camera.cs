using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MobileGame.CameraManagement
{
    class Camera
    {
        private static float zoom = 1f;
        private static Matrix transform;
        private static Vector2 position = Vector2.Zero;
        private static float rotation = 0f;
        private static GraphicsDevice device;
        private static List<IFocusable> focusList = new List<IFocusable>();
        private static float moveSpeed = 2f;
        private static Vector2 target = Vector2.Zero;
        private static float maxDistance = 200f;
        private static Texture2D texture;

        public static GraphicsDevice GraphicsDevice
        {
            get { return device; }
            set { device = value; }
        }

        public static float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; } //Negative zoomvalue will flip images, dont want that
        }

        public static float MoveSpeed
        {
            get { return moveSpeed; }
            set { moveSpeed = value; }
        }

        //Lets keep the property here incase i would want it in the future, for now its not to any use for me though
        //public float Rotation
        //{
        //    get { return rotation; }
        //    set { rotation = value; }
        //}

        public static Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public static void LoadStuff(ContentManager content)
        {
            texture = content.Load<Texture2D>("Tiles/TestBigTile");
        }

        public static void Update(GameTime gameTime)
        {
            if (focusList.Count > 0)
            {
                var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                target = CalculateTarget();

                position.X += (target.X - position.X) * moveSpeed * delta;
                position.Y += (target.Y - position.Y) * moveSpeed * delta;

                CheckRanges();
            } 
        }

        public static void Draw(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle((int)target.X, (int)target.Y, 10, 10);
            //Vector2 origin = new Vector2(5f, 5f);

            //sb.Draw(texture, rect, null, Color.White, 0f, origin, SpriteEffects.None, 0f);
            sb.Draw(texture, rect, Color.White);
        }

        public static void AddFocusObject(IFocusable Object)
        {
            focusList.Add(Object);
        }

        public static void RemoveFocusObject(IFocusable Object)
        {
            focusList.Remove(Object);
        }

        public static void ClearFocusList()
        {
            focusList.Clear();
        }

        public static void Move(Vector2 amount)
        {
            position += amount;
        }

        public static Matrix Get_Transformation()
        {
            transform =
                Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(device.Viewport.Width * 0.5f, device.Viewport.Height * 0.5f, 0));

            return transform;
        }

        public static Vector2 ScreenToWorldCoordinates(Vector2 coordinates)
        {
            return Vector2.Transform(coordinates, Matrix.Invert(Get_Transformation()));
        }


        private static Vector2 CalculateTarget()
        {
            Vector2 Target = new Vector2();

            foreach (IFocusable Object in focusList)
            {
                Target += Object.Position;
            }

            Target /= focusList.Count;

            return Target;
        }

        private static void CheckRanges()
        {
            for (int i = focusList.Count-1; i > 0; i--)
            {
                if (focusList.Count == 1)
                    return;

                float distance = Math.Abs((focusList[i].Position - Position).Length());
                Console.WriteLine("Distance: " + distance + ", MaxDistance: " + maxDistance);
                if (distance > maxDistance)
                    focusList.RemoveAt(i);
            }

            //foreach (IFocusable Object in focusList)
            //{
            //    if (focusList.Count == 1)
            //        return;

            //    if ((Object.Position - Position).Length() > maxDistance)
            //        focusList.Remove(Object);
            //}
        }
    }
}
