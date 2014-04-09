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
        void Update();
        void Draw(SpriteBatch spritebatch);
        void CollideWithPlayer(Player player);
        void CollideWithEnemyCollider();
        Rectangle HitBox();
    }
}
