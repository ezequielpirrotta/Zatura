namespace MyGame.Consts
{
    readonly struct Globals
    {   
        public const double MaxShootSpeed = 0.25;
        public const double MinShootSpeed = 1;
        public const int MaxHealth = 20;
        public const int MaxAmmo = 20;
        public const int MaxSpeed = 10;
        public const int MaxLevel = 4;
        public const int FontSizeTexts = 40;
        public const int FontSizeTitles = 60;
        public readonly struct PowerUps
        {
            public const int reload = 1;
            public const int speed = 2;
            public const int fireRate = 3;
            //public const int multiShoot = 4;
            public const int health = 5;
        }
    }
    readonly struct Assets
    {
        public const string MainBackGround = "assets/images/fondo_principal.png";
        public readonly struct Sounds
        {
            public const string DeathSound = "assets/sounds/explosion_01.wav";
            public const string ShootSound = "assets/sounds/disparo-1_01.wav";
            public const string BackgroundMusic = "assets/sounds/game.wav";
        }
        public readonly struct PowerUps
        {
            public const string reloadSprite = "assets/images/power_up_reload.png";
            public const string speedSprite = "assets/images/power_up_speed.png";
            public const string fireRateSprite = "assets/images/power_up_fire_rate.png";
            //public const string multishootSprite = "assets/images/power_up_multishoot.png";
            public const string healthSprite = "assets/images/power_up_health.png";
        }
        public readonly struct Entities
        {
            public const string Enemy = "assets/images/enemy.png";
            public const string Player = "assets/images/player_ship.png";
            public const string Bullet = "assets/images/player_ship_bullet.png";
            public const string MenuSelector = "assets/images/menu_gun.png";
        }
        public readonly struct Fonts
        {
            public const string MainFont = "assets/fonts/bod-blar.ttf";
        }
    }
    readonly struct Stages
    {
        public const int CLOSE = -1;
        public const int MAIN_MENU = 0;
        public const int GAME = 1;
        public const int WIN_SCREEN = 2;
        public const int LOSE_SCREEN = 3;
        public const int RESET = 4;
    }
    readonly struct Labels
    {
        public const string PLAY = "Jugar";
        public const string QUIT = "Salir";
        public const string RESUME = "Volver";
        public const string MENU = "Menu Principal";
        public const string WON = "Ganaste!";
        public const string LOST = "Perdiste!";
        public const string RESET = "Reiniciar";
    }
}
