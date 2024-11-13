using Entities;
using System;

namespace MyGame.Structs
{
    public struct Level
    {
        public int enemiesNumber;
        public int enemiesSpeed;
        public double enemiesFireRate;
        public int enemiesHealth;
    }

    public struct PowerUps
    {
        public bool reload;
        public bool speed;
        public bool fireRate;
        public bool multiShoot;
        public bool health;
        public int time;
    }
    public struct Background
    {
        public Image image;
        public int posX;
        public int posY;
        public int width;
        public int height;
    }
}
