using MyGame.Consts;
using MyGame.Structs;
using System;

namespace Entities
{
    public class Player : Entity
    {

        public Entity[] bullets;
        public const int maxAmmo = 10;
        public int ammo;
        private Sound shootSound;
        private Sound deathSound;

        public DateTime lastTimePlayed;
        private double shootRate { get; set; }
        public double ShootRate;
        private double originalShootRate { get; set; }
        public double OriginalShootRate;
        private DateTime lastShootTime { get; set; }
        public DateTime LastShootTime;

        public PowerUps powerUp;

        public DateTime powerUpTimeLastApply;
        public Player(ref Image sprite, int inputSpeed, int x, int y, int health = 10) : base(ref sprite)
        {
            this.health = health;
            maxHealth = health;
            this.isAlive = this.health > 0;
            posX = x;
            posY = y;
            speed = inputSpeed;
            originalSpeed = inputSpeed;
            this.sprite = sprite;
            this.width = 64;
            this.height = 64;
            LastShootTime = DateTime.Now;
            lastTimePlayed = DateTime.Now;
            bullets = new Entity[maxAmmo];
            ammo = maxAmmo;
            ShootRate = 1;
            OriginalShootRate = ShootRate;
            shootSound = new Sound(Assets.Sounds.ShootSound, true);
            deathSound = new Sound(Assets.Sounds.DeathSound, true);
        }
        public Player(Image sprite, int width, int height) : base(ref sprite)
        {
            this.sprite = sprite;
            this.width = width;
            this.height = height;
        }

        public void InitializeBullets(ref Image bulletSprite)
        {
            for (int i = 0; i < maxAmmo; i++)
            {
                bullets[i] = new Entity(ref bulletSprite, 64, 64, -5, 1, false, this.posX + (this.width / 2), this.posY + (this.height / 2));
            }
        }
        public void ReloadAmmo()
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                if(!bullets[i].isAlive)
                {
                    bullets[i].isAlive = false;
                    bullets[i].posX = this.posX + (this.width / 2);
                    bullets[i].posY = this.posY + (this.height / 2);
                }
            }
            this.ammo = maxAmmo;
        }
        private bool CanShoot()
        {
            return (this.ammo > 0) && (DateTime.Now - lastShootTime).TotalSeconds > ShootRate;
        }
        public void Shoot()
        {
            if (isAlive)
            {
                if (CanShoot())
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
                            if((DateTime.Now - lastTimePlayed).TotalSeconds > 1)
                            {
                                shootSound.PlayOnce();
                                lastTimePlayed = DateTime.Now;
                            }
                            break;
                        }

                    }

                }
            }
        }
        public bool CheckBulletCollision(Entity enemy)
        {
            bool didCollide = false;
            for (int i = 0; i < maxAmmo; i++)
            {
                if (bullets[i].CheckCollision(enemy))
                {
                    enemy.TakeDamage(1);
                    bullets[i].isAlive = false;
                    didCollide = true;
                    break;
                }
            }
            return didCollide;
        }
        new public void Move(int limit, string direction)
        {
            switch (direction)
            {
                case "left":
                    if ((posX - speed) > 0)
                    {
                        posX -= speed;

                    }
                    break;
                case "right":
                    if ((posX + width + speed) < limit)
                    {
                        posX += speed;
                    }
                    break;
                case "top":
                    if ((posY - speed) > limit / 2)
                    {
                        posY -= speed;
                    }
                    break;
                case "bottom":
                    if ((posY + height + speed) < limit)
                    {
                        posY += speed;
                    }
                    break;
            }
            for (int i = 0; i < maxAmmo; i++)
            {
                if (!bullets[i].isAlive)
                {
                    bullets[i].posX = posX + (width / 2);
                    bullets[i].posY = posY + (height / 2);
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

                    if (bullets[i].posY <= limit)
                    {
                        bullets[i].isAlive = false;
                        break;
                    }
                }
            }
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
        public void ApplyPowerUp(PowerUps p)
        {
            powerUp = p;
            if (powerUp.speed)
            {
                speed = speed + 2 < Globals.MaxSpeed ? speed + 2 : Globals.MaxSpeed;
                powerUpTimeLastApply = DateTime.Now;
            }
            if (powerUp.reload)
            {
                ReloadAmmo();
                powerUp.reload = false;
            }
            if (powerUp.health)
            {
                Heal();
                powerUp.health = false;
            }
            if (powerUp.fireRate)
            {
                ShootRate = ShootRate + Globals.MinShootSpeed <= Globals.MaxShootSpeed ? ShootRate + Globals.MinShootSpeed : ShootRate;
                powerUpTimeLastApply = DateTime.Now;
            }
            /*if (powerUp.multiShoot)
            {
                Heal();
                powerUpTimeLastApply = DateTime.Now;
            }*/
        }
        public void RemovePowerUp()
        {
            if (powerUp.speed)
            {
                speed = originalSpeed;
            }
            if (powerUp.fireRate)
            {
                shootRate = originalShootRate;
            }
            if (powerUp.multiShoot)
            {
                Heal();
            }
        }
        public bool CheckPowerUpActive()
        {
            if (powerUp.speed)
            { 
                return true;
            }
            if (powerUp.fireRate)
            {
                return true;
            }
            if (powerUp.multiShoot)
            {
                return true;
            }
            return false;
        }
        public void PlayDeathSound()
        {
            deathSound.PlayOnce();
        }
    }
}