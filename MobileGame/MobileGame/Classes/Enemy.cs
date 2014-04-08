using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MobileGame
{
    class Enemy
    {
        private Texture2D enemyTex;

        private Vector2 position;
        private Vector2 velocity;

        private Random rand;

        public Enemy(int x, int y)
        {
            rand = new Random();

            enemyTex = TextureManager.EnemyTex;

            position = new Vector2(x * enemyTex.Width, y * enemyTex.Height);

            //Retared way to do this, but was the first way i thought of at the time. Should probbably do this a more propper way later.
            int value = rand.Next(1, 3);
            if (value == 1)
                velocity = new Vector2(1, 0);
            else if (value == 2)
                velocity = new Vector2(-1, 0);
        }

        public void Update()
        {
            position += velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(enemyTex, position, Color.White);
        }

        public Rectangle HitBox()
        {
            Rectangle temp = new Rectangle((int)position.X, (int)position.Y, enemyTex.Width, enemyTex.Height);

            return new Rectangle((int)position.X, (int)position.Y, enemyTex.Width, enemyTex.Height);
        }

        public void FlipVelocity()
        {
            velocity *= -1;
        }

        //This function is empty for now. But later on this should be used to preform a more detailed collision detection than simple rect to rect
        //This will make it possible to use enemies that are of more irregular shapes.
        //It will also make the game more fair, since the player wont instantly die/take damage/w.e from simply being inside the enemy rect
        public void CollideWithPlayer(Player player)
        {

        }
    }
}
