using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MobileGame
{
    interface IEnemy
    {
        void Update(float elapsedTime);
        void Draw(SpriteBatch spriteBatch);
        void CollideWithPlayer(Player player);
        void CollideWithEnemyCollider();
        Rectangle HitBox();
    }
}
