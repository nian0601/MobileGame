using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MobileGame
{
    class ProjectileManager
    {
        private List<Projectile> projectiles;

        public ProjectileManager()
        {
            projectiles = new List<Projectile>();
        }

        public void Update(Player player, float elapsedTime)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                if(projectiles[i].Active)
                    projectiles[i].Update(player, elapsedTime);
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i].Active)
                    projectiles[i].Draw(spritebatch);
            } 
        }

        public void AddProjectile(Vector2 startPos, float speed, Vector2 targetPos)
        {
            //Here we do a clever thing, instead of just adding new projectiles all the time, making the list longer and longer
            //We loop through the list and look for any inactive projectiles, if we find one we then "restart" it with new values
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (!projectiles[i].Active)
                {
                    projectiles[i].ResetValues(startPos, speed, targetPos);
                    return;
                }
            }

            //If we however didnt find any inactive projectiles we simply add a new one
            projectiles.Add(new Projectile(startPos, speed, targetPos));
        }
    }
}
