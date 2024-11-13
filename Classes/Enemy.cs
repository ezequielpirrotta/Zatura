using MyGame.Consts;
using System;

namespace Entities
{
    public class Enemy: Entity
    {
        public Entity[] bullets;
        const int maxAmmo = 10;
        public int ammo;
        public double shootRate;
        private double cooldown;
        private DateTime lastShootTime;
        private DateTime lastReloadTime;
        private Sound shootSound;
        private Sound deathSound;
        private DateTime lastTimePlayed;
        public Enemy(Image sprite, int width = 32, int height = 32, int inputSpeed = 3, int health=2, 
            double shootRate = 1, int x = 0, int y = 0, double cooldown = 1): base(ref sprite)
        {
            this.health = health;
            isAlive = health > 0;
            posX = x;
            posY = y;
            speed = inputSpeed;
            this.sprite = sprite;
            this.width = width;
            this.height = height;
            bullets = new Entity[maxAmmo];
            ammo = maxAmmo;
            this.shootRate = shootRate;
            this.cooldown = cooldown;
            lastShootTime = DateTime.Now;
            lastReloadTime = DateTime.Now;
            lastTimePlayed = DateTime.Now;
            shootSound = new Sound(Assets.Sounds.ShootSound, true);
            deathSound = new Sound(Assets.Sounds.DeathSound, true);
        }

        public void InitializeBullets(ref Image bulletSprite)
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = new Entity(ref bulletSprite, 64, 64, 5, 1, false, posX + (width / 2), posY + (height / 2));
            }
        }
        public void ReloadAmmo()
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                if (!bullets[i].isAlive)
                {
                    bullets[i].isAlive = false;
                    bullets[i].posX = posX + (width / 2);
                    bullets[i].posY = posY + (height / 2);
                }
            }
            this.ammo = maxAmmo;
        }
        public void Shoot()
        {
            if (ammo > 0)
            {
                if ((DateTime.Now - lastShootTime).TotalSeconds > shootRate)
                {
                    for (int i = 0; i < maxAmmo; i++)
                    {

                        if (!bullets[i].isAlive)
                        {
                            bullets[i].isAlive = true;
                            bullets[i].posX = posX + (width / 2);
                            bullets[i].posY = posY + (height / 2);
                            ammo--;
                            lastShootTime = DateTime.Now;
                            if ((DateTime.Now - lastTimePlayed).TotalSeconds > 1)
                            {
                                shootSound.PlayOnce();
                                lastTimePlayed = DateTime.Now;
                            }
                            break;
                        }

                    }
                }
            }
            if(ammo == 0 && (DateTime.Now - lastReloadTime).TotalSeconds > cooldown)
            {
                ReloadAmmo();
                lastReloadTime = DateTime.Now;
            }
        }

        public bool CheckBulletCollision(Entity entity)
        {
            bool didCollide = false;
            for (int i = 0; i < bullets.Length; i++)
            {
                if (bullets[i].CheckCollision(entity))
                {
                    entity.TakeDamage(1);
                    bullets[i].isAlive = false;
                    didCollide = true;
                    break;
                }
            }
            return didCollide;
        }
        public void Move(int limit)
        {
            if ((posX + speed) > 0 && (posX + speed) < limit)
            {
                posX += speed;
            }
            if((posX + speed) <= 0 || (posX + speed + width) >= limit)
            {
                speed *= -1;
                //posX += speed;
            }
            for (int i = 0; i < maxAmmo; i++)
            {
                if (!bullets[i].isAlive)
                {
                    bullets[i].posX = this.posX + (this.width / 2);
                    bullets[i].posY = this.posY + (this.height / 2);
                }
            }

        }
        public void MoveBullets(int limit)
        {
            for (int i = 0; i < maxAmmo; i++)
            {
                if (bullets[i].isAlive)
                {
                    bullets[i].posY += bullets[i].speed;

                    if (bullets[i].posY >= limit)
                    {
                        bullets[i].isAlive = false;
                        break;
                    }
                }
            }
        }
        public bool HasBulletsAlive()
        {
            for (int i = 0; i < maxAmmo; i++)
            {
                if (bullets[i].isAlive)
                {
                    return true;
                }
            }
            return false;
        }
        public void DrawBullets()
        {
            for (int i = 0; i < maxAmmo; i++)
            {
                if (bullets[i].isAlive)
                {
                    Engine.Draw(bullets[i].sprite, bullets[i].posX, bullets[i].posY);
                }
            }
        }
        public void PlayDeathSound()
        {
            deathSound.PlayOnce();
        }
    }
}
