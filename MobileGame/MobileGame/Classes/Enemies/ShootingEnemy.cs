﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MobileGame.Managers;
using MobileGame.Units;

namespace MobileGame.Enemies
{
    class ShootingEnemy : SimpleEnemy
    {
        private ProjectileManager projectileManager;

        private float range;
        private float fireRate;
        private float lastShot;
        private float projectileSpeed;

        private bool canShoot;

        public ShootingEnemy(int x, int y): base(x, y, false)
        {
            projectileManager = new ProjectileManager();

            range = 200;
            fireRate = 1000;
            lastShot = 0;
            projectileSpeed = 5f;
            canShoot = true;
        }

        public override void Update(float elapsedTime, Player player)
        {
            lastShot += elapsedTime;

            if (lastShot >= fireRate)
            {
                canShoot = true;
                lastShot = fireRate;
            }

            projectileManager.Update(player, elapsedTime);

            base.Update(elapsedTime, player);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            projectileManager.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        public override void SpecialAbility(Player player)
        {
            if (GetDistance(position, player.Position) < range && canShoot)
                Shoot(player.Position);
        }

        private void Shoot(Vector2 Target)
        {
            projectileManager.AddProjectile(position, projectileSpeed, Target);
            lastShot = 0;
            canShoot = false;
        }

        public float Range
        {
            get { return range; }
        }

        public bool CanShoot()
        {
            return canShoot;
        }

        private float GetDistance(Vector2 PosOne, Vector2 PosTwo)
        {
            Vector2 tempDis;

            tempDis.X = PosTwo.X - PosOne.X;
            tempDis.Y = PosTwo.Y - PosTwo.Y;

            float dist = tempDis.Length();

            return Math.Abs(dist);
        }
    }
}
