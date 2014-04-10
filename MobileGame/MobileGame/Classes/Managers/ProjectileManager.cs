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
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (!projectiles[i].Active)
                {
                    projectiles[i].ResetValues(startPos, speed, targetPos);
                    return;
                }
            }

            projectiles.Add(new Projectile(startPos, speed, targetPos));
        }
    }
}
