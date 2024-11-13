using System;
using MyGame.Consts;
using MyGame.Structs;

namespace Entities
{
    public class PowerUp: Entity
    {
        public PowerUps powerUps;
        public PowerUp(ref Image sprite, int pwUp, int width, int height, int inputSpeed = 3, int health = 1, bool isAlive = false, int x = 0, int y = 0) :
                        base(ref sprite, width, height)
        {
            powerUps = new PowerUps();
            switch (pwUp)
            {
                case Globals.PowerUps.reload:
                    powerUps.reload = true;
                    break;
                case Globals.PowerUps.speed:
                    powerUps.speed = true;
                    break;
                case Globals.PowerUps.fireRate:
                    powerUps.fireRate = true;
                    break;
                /*case Globals.PowerUps.multiShoot:
                    powerUps.multiShoot = true;
                    break;*/
                case Globals.PowerUps.health:
                    powerUps.health = true;
                    break;
            }
            powerUps.time = 5;
            maxHealth = health;
            this.health = health;
            this.isAlive = isAlive;
            posX = x;
            posY = y;
            this.width = width;
            this.height = height;
            speed = inputSpeed;
            this.sprite = sprite;
        }
        public bool Move(int limit)
        {
            if(isAlive)
            {
                if ((posY + speed) > 0 && (posY + speed) < limit)
                {
                    posY += speed;
                    return true;
                }
                if (posY >= limit)
                {
                    isAlive = false;
                    return false;
                }
            }
            return false;
        }
    }
}
