namespace Entities
{
    public class Entity
    {
        public int posX, posY, speed, originalSpeed, maxHealth, health, width, height;
        public bool isAlive;
        public Image sprite;

        public Entity(ref Image sprite, int width = 32, int height = 32, int inputSpeed = 3, int health = 5, bool isAlive = true, int x=0, int y=0)
        {
            this.maxHealth = health;
            this.health = health;
            this.isAlive = isAlive;
            posX = x;
            posY = y;
            speed = inputSpeed;
            originalSpeed = speed;
            this.sprite = sprite;
            this.width = width;
            this.height = height;
        }
        public bool TakeDamage(int damage)
        {
            bool gotHit = false;
            if (isAlive && health > 0)
            {
                gotHit = true;
                health = health - damage >= 0? health - damage : 0;
                if (health == 0)
                {
                    isAlive = false;
                }
            }
            return gotHit;
        }
        public void Heal()
        {
            isAlive = true;
            health = maxHealth;
        }
        public void Move(int limit, string direction)
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
                    if ((posY - speed) > 0)
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
        }

        
        public bool CheckCollision(Entity entity)
        {

            return posX < entity.posX + entity.width &&
                   posX + width > entity.posX &&
                   posY < entity.posY + entity.height &&
                   posY + height > entity.posY;
        }
        public void DrawSprite()
        {
            Engine.Draw(sprite, posX, posY);
        }
    }
}
