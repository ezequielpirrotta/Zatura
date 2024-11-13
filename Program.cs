using System;
using Tao.Sdl;
using Entities;
using MyGame.Classes;
using MyGame.Structs;
using MyGame.Consts;

namespace Zatura
{
    class Program
    {
        static Level[] levels = new Level[Globals.MaxLevel];
        static int actualLevel;
        static Background backGround;
        static Player player;
        static Enemy[] enemies;
        static PowerUp[] powerUps; 
        static Font optionsFont;
        static Font titlesFont;
        static Menu mainMenu;
        static Menu pauseMenu;
        static Menu wonScreen;
        static Menu loseScreen;
        static int screen;
        static bool gamePaused;
        static bool resetFromMenu;
        static int scrollSpeed;
        static DateTime lastTimeTouched;
        static DateTime lastTimeDropedPowerUp;
        static Image reloadSprite;
        static Image speedSprite;
        static Image fireRateSprite;
        //static Image multishootSprite;
        static Image healthSprite;
        static Image enemySprite;
        static Image playerSprite;
        static Image bulletSprite;
        static Image menuSelectorSprite;
        static Sound backGroundMusic;
        static void SetEnemies()
        {
            Random random = new Random();
            if(actualLevel < levels.Length)
            {
                enemies = new Enemy[levels[actualLevel].enemiesNumber];
                for (int i = 0; i < enemies.Length; i++)
                {
                    int posX = random.Next(0, Engine.ancho - 64);
                    int posY = random.Next(0, (Engine.alto / 2) - 64);
                    enemies[i] = new Enemy(enemySprite, 64, 64, levels[actualLevel].enemiesSpeed, levels[actualLevel].enemiesHealth, levels[actualLevel].enemiesFireRate, posX, posY,2);
                    enemies[i].InitializeBullets(ref bulletSprite);
                }
            }
        }
        static void SetPowerUps()
        {
            // Inicializo power ups
            Random random = new Random();
            int[] powerUpIds = new int[4]{
                Globals.PowerUps.reload,
                Globals.PowerUps.fireRate,
                Globals.PowerUps.health,
                //Globals.PowerUps.multiShoot,
                Globals.PowerUps.speed
            };
            Image[] powerUpSprites = new Image[4]{
                reloadSprite,
                fireRateSprite,
                healthSprite,
                //multishootSprite,
                speedSprite
            };

            powerUps = new PowerUp[4];
            for (int i = 0; i < powerUps.Length; i++)
            {
                int rdmId = random.Next(0, powerUpIds.Length);
                powerUps[i] = new PowerUp(ref powerUpSprites[rdmId], powerUpIds[rdmId], 32, 32);
            }
        }
        static bool PlayerWon(Player player)
        {
            return player.isAlive && !EnemiesAlive() && actualLevel == Globals.MaxLevel;
        }
        static bool PlayerLost(Player player)
        {
            return !player.isAlive && player.health <= 0;
        }
        static Menu CreateMenu(string title, String[] labels, int[] actions, Font optionFont, bool inGame = false)
        {

            Button[] menuButtons = new Button[labels.Length];
            for (int i = 0; i < labels.Length;i++)
            {
                if(labels.Length == actions.Length)
                {
                    menuButtons[i] = new Button(new ScreenLabel(labels[i], optionsFont, 0, 0), actions[i]);
                }
            }
            

            Entity menuSelector = new Entity(ref menuSelectorSprite, 152, 85, 0, 0, true,0,0);

            return new Menu(new ScreenLabel(title, titlesFont, 0, 0), menuButtons, menuSelector, inGame);
        }
        static void InitializeSprites()
        {
            reloadSprite = Engine.LoadImage(Assets.PowerUps.reloadSprite);
            speedSprite = Engine.LoadImage(Assets.PowerUps.speedSprite);
            fireRateSprite = Engine.LoadImage(Assets.PowerUps.fireRateSprite);
            //multishootSprite = Engine.LoadImage(Assets.PowerUps.multishootSprite);
            healthSprite = Engine.LoadImage(Assets.PowerUps.healthSprite);
            enemySprite = Engine.LoadImage(Assets.Entities.Enemy);
            playerSprite = Engine.LoadImage(Assets.Entities.Player);
            bulletSprite = Engine.LoadImage(Assets.Entities.Bullet);
            menuSelectorSprite = Engine.LoadImage(Assets.Entities.MenuSelector);
            backGround.image = Engine.LoadImage(Assets.MainBackGround);
        }
        static void InitializeMenus()
        {
            //Carga de fuentes
            optionsFont = Engine.LoadFont(Assets.Fonts.MainFont, Globals.FontSizeTexts);
            titlesFont = Engine.LoadFont(Assets.Fonts.MainFont, Globals.FontSizeTitles);

            //Menu principal
            string[] labels = new string[] { Labels.PLAY, Labels.QUIT };
            int[] actions = new int[] { Stages.GAME, Stages.CLOSE };
            mainMenu = CreateMenu("Zatura", labels, actions, optionsFont);

            //Menu de pausa
            labels = new string[] { Labels.RESUME, Labels.RESET, Labels.MENU };
            actions = new int[] { Stages.GAME, Stages.RESET, Stages.MAIN_MENU };
            pauseMenu = CreateMenu("Pausa", labels, actions, optionsFont, true);

            //Pantallas
           
            labels = new string[] { Labels.RESET, Labels.MENU };
            actions = new int[] { Stages.RESET, Stages.MAIN_MENU };
            wonScreen = CreateMenu(Labels.WON, labels, actions, optionsFont,true);
            loseScreen = CreateMenu(Labels.LOST, labels, actions, optionsFont, true);

        }
        static void InitializeEntities()
        {
            // Inicializo jugador y balas
            player = new Player(ref playerSprite, 6, (Engine.ancho / 2), (Engine.alto / 2));
            player.InitializeBullets(ref bulletSprite);

            // Inicializo enemigos y balas
            double accum = Globals.MinShootSpeed;
            for (int i = 0; i < levels.Length; i++)
            {
                levels[i].enemiesNumber = i == 0? 1 : i;
                levels[i].enemiesSpeed = i + 2;
                if(accum <= Globals.MaxShootSpeed)
                {
                    levels[i].enemiesFireRate = Globals.MaxShootSpeed;
                }
                levels[i].enemiesFireRate = accum;
                levels[i].enemiesHealth = i + 1;
                accum -= Globals.MaxShootSpeed;
            }
            SetEnemies();
            SetPowerUps();
        }
        static bool CanCheckInputs()
        {
            return (DateTime.Now - lastTimeTouched).TotalMilliseconds >= 130;
        }
        static bool EnemiesAlive()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].isAlive)
                {
                    return true;
                }
            }
            return false;
        }
        static void ResetGame()
        {
            player.posX = Engine.ancho / 2;
            player.posY = Engine.alto / 2;
            player.Heal();
            player.ReloadAmmo();
            actualLevel = 0;
            SetEnemies();
            SetPowerUps();
        }
        static void Main(string[] args)
        {
            Engine.Initialize();
            InitializeSprites();
            
            backGround.posX = 0;
            backGround.posY = 0;
            backGround.width = Engine.ancho;
            backGround.height = Engine.alto;
            gamePaused = true;
            actualLevel = 1;
            screen = Stages.MAIN_MENU;
            resetFromMenu = false;
            scrollSpeed = 2;
            InitializeMenus();
            InitializeEntities(); 
            lastTimeTouched = DateTime.Now;
            
            backGroundMusic = new Sound(Assets.Sounds.BackgroundMusic,false,20);
            backGroundMusic.Play();

            while (true)
            {
                switch(screen)
                {
                    case Stages.MAIN_MENU:
                        if (CanCheckInputs())
                        {
                            mainMenu.CheckInputs(ref screen,ref gamePaused);
                            lastTimeTouched = DateTime.Now;
                        }
                        break;

                    case Stages.GAME:
                        if (!gamePaused)
                        {
                            CheckInputsInGame();
                        }
                        pauseMenu.CheckInputs(ref screen, ref gamePaused);
                        if(screen == Stages.MAIN_MENU)
                        {
                            lastTimeTouched = DateTime.Now;
                            resetFromMenu = true;
                        }
                        break;

                    case Stages.WIN_SCREEN:
                        if(PlayerWon(player))
                        {
                            if (CanCheckInputs())
                            {
                                wonScreen.CheckInputs(ref screen,ref gamePaused);
                                lastTimeTouched = DateTime.Now;
                            }
                            if (screen == Stages.MAIN_MENU)
                            {
                                resetFromMenu = true;
                            }
                        }
                        break;

                    case Stages.LOSE_SCREEN: 
                        if (PlayerLost(player))
                        {
                            if (CanCheckInputs())
                            { 
                                loseScreen.CheckInputs(ref screen, ref gamePaused);
                                lastTimeTouched = DateTime.Now;
                            }
                            if (screen == Stages.MAIN_MENU)
                            {
                                resetFromMenu = true;
                            }
                        }
                        break;

                    case Stages.CLOSE:
                        Environment.Exit(0);
                        break;
                }
                CheckInputVolume();
                Update();
                Render();
                Sdl.SDL_Delay(20);  
            }
        }
        static void CheckInputVolume()
        {
            if (Engine.KeyPress(Engine.KEY_UP))
            {
                backGroundMusic.ChangeVolume(+5);
                
            }
            if (Engine.KeyPress(Engine.KEY_DOWN))
            {
                backGroundMusic.ChangeVolume(-5);
            }
        }
        static void CheckInputsInGame()
        {
            if (player != null && !gamePaused && screen == Stages.GAME) 
            {
                 
                if (Engine.KeyPress(Engine.KEY_A))
                {

                    player.Move(Engine.ancho, "left");
                }

                if (Engine.KeyPress(Engine.KEY_D))
                {    
                    player.Move(Engine.ancho, "right");
                }

                if (Engine.KeyPress(Engine.KEY_W))
                {
                    player.Move(Engine.alto, "top");
                }

                if (Engine.KeyPress(Engine.KEY_S))
                {

                    player.Move(Engine.alto, "bottom");
                }
                if (Engine.KeyPress(Engine.KEY_E))
                {
                    player.Shoot();
                }
                if (Engine.KeyPress(Engine.KEY_ESC))
                {
                    gamePaused = true;
                    pauseMenu.CheckInputs(ref screen, ref gamePaused);
                }

            }
        }
        static void Update()
        {
            if(screen == Stages.MAIN_MENU && resetFromMenu)
            {
                ResetGame();
                resetFromMenu = false;
            }
            if(screen == Stages.RESET)
            {
                ResetGame();
                gamePaused = false;
                screen = Stages.GAME;
            }
            if (PlayerWon(player))
            {
                screen = Stages.WIN_SCREEN;
                gamePaused = true;
            };
            if(PlayerLost(player))
            {
                screen = Stages.LOSE_SCREEN;
                gamePaused = true;
            }
            
            if (!gamePaused && screen == Stages.GAME && player.isAlive)
            {
                // Screen wrapping
                backGround.posY += scrollSpeed;
                if (backGround.posY >= Engine.alto)
                {
                    backGround.posY = 0;
                    
                }
                
                if (actualLevel < Globals.MaxLevel)
                {
                    // Remuevo power ups al pasar el tiempo especifico
                    if(player.CheckPowerUpActive() && (DateTime.Now - player.powerUpTimeLastApply).TotalSeconds > player.powerUp.time)
                    {
                        player.RemovePowerUp();
                    }
                    // Suelto el power up en el mapa
                    if ((DateTime.Now - lastTimeDropedPowerUp).TotalSeconds > 6)
                    {
                        Random random = new Random();
                        for (int i = 0; i < powerUps.Length; i++)
                        {
                            if (!powerUps[i].isAlive)
                            {
                                powerUps[i].isAlive = true;
                                powerUps[i].posX = random.Next(0, Engine.ancho - powerUps[i].width);
                                powerUps[i].posY = 0;
                                lastTimeDropedPowerUp = DateTime.Now;
                                break;
                            }

                        }
                    }

                    player.MoveBullets(0);
                    // Chequo colisiones con power ups y los aplico
                    for(int i = 0; i < powerUps.Length;i++)
                    {
                        powerUps[i].Move(Engine.alto);
                        if(player.CheckCollision(powerUps[i]))
                        {
                            player.ApplyPowerUp(powerUps[i].powerUps);
                            powerUps[i].isAlive = false;
                            break;
                        }
                    }

                    //Movimientos, disparos y colisiones
                    bool anyBulletsAlive = false;
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        player.CheckBulletCollision(enemies[i]);
                        if (enemies[i].isAlive)
                        {
                            bool nearPlayer = enemies[i].posX <= player.posX + (player.width * 2) && enemies[i].posX >= player.posX - (player.width * 2);
                            enemies[i].Move(Engine.ancho);
                            if(nearPlayer)
                            {
                                enemies[i].Shoot();
                            }
                        }
                        else
                        {
                            enemies[i].PlayDeathSound();
                        }
                        
                        enemies[i].MoveBullets(Engine.alto);
                        enemies[i].CheckBulletCollision(player);
                        if(enemies[i].HasBulletsAlive())
                        {
                            anyBulletsAlive = true;
                        }
                    }
                    // Avanzo de nivel
                    if (!EnemiesAlive() && !anyBulletsAlive && actualLevel < Globals.MaxLevel)
                    {
                        actualLevel++;
                        SetEnemies();
                        SetPowerUps();
                    }
                }
            }
            
        }
        static void Render()
        {
            Engine.Clear();
              
            Engine.Draw(backGround.image, backGround.posX, backGround.posY);
            Engine.Draw(backGround.image, backGround.posX, backGround.posY - backGround.height);

            // Chequeo sobre qué pantalla estoy parado y dibujo
            switch (screen)
            { 
                case Stages.MAIN_MENU:
                    mainMenu.Draw();
                    break;

                case Stages.GAME:
                    Engine.DrawText("Balas: " + player.ammo, 0, 0, 255, 255, 255, optionsFont);
                    Engine.DrawText("Vida: " + player.health, 0, Globals.FontSizeTexts, 255, 255, 255, optionsFont);
                    Engine.DrawText("Nivel: " + (actualLevel+1), 0, Globals.FontSizeTexts * 2, 255, 255, 255, optionsFont);
                    player.DrawSprite();
                    player.DrawBullets();
                    // Power ups
                    for (int i = 0; i < powerUps.Length; i++)
                    {
                        if(powerUps[i].isAlive)
                        {
                            powerUps[i].DrawSprite();
                        }
                    }
                    // Enemigos
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        if (enemies[i].isAlive)
                        {
                            enemies[i].DrawSprite();
                        }
                        enemies[i].DrawBullets();
                    }
                    // Menu de pausa
                    if (gamePaused)
                    {
                        pauseMenu.Draw();
                    }
                    break;

                case Stages.WIN_SCREEN:
                    wonScreen.Draw();
                    break;

                case Stages.LOSE_SCREEN:
                    loseScreen.Draw();
                    break;

            }
            Engine.Show();
        }
    }
}