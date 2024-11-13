using Entities;
using MyGame.Consts;
using MyGame.Structs;
using System;

namespace MyGame.Classes
{
    public class Menu
    {
        private int actualOption;
        public int ActualOption {get;set;}
        private Button[] buttons;
        public Button[] Buttons {get;set;}
        public ScreenLabel title;
        private Entity optionsSelector;
        public Entity OptionsSelector { get; set; }
        public bool isInGame;
        public DateTime lastTimeTouched;
        
        public Background backGround;
        
        public Menu(ScreenLabel title, Button[] buttns, Entity selector, bool isInGame = false) 
        {
            
            this.title = title;
            Buttons = buttns;
            OptionsSelector = selector;
            actualOption = 0;
            this.isInGame = isInGame;
            lastTimeTouched = DateTime.Now;
            if(isInGame) 
            {
                backGround.image = Engine.LoadImage("assets/images/menu_in_game.png");
                backGround.width = Engine.ancho / 2;
                backGround.height = Engine.alto / 2;
                backGround.posX = Engine.ancho / 2 - backGround.width/2; 
                backGround.posY = Engine.alto / 2 - backGround.height/2;
                title.PosX = (Engine.ancho / 2) - (title.Width/2);
                title.PosY = backGround.posY + title.Height;
                for (int i = 0; i < Buttons.Length; i++)
                {
                    Buttons[i].label.PosX = (Engine.ancho / 2) - (Buttons[i].label.Width/2);
                    Buttons[i].label.PosY = i == 0 ? title.PosY + title.Height + Buttons[i].label.Height : Buttons[i - 1].label.PosY + Buttons[i - 1].label.Height + Buttons[i].label.Height;
                    
                }
            }
            else
            {
                title.PosX = Engine.ancho / 2 - title.Width/2 ;
                title.PosY = title.Height;
                for (int i = 0; i < Buttons.Length; i++)
                {
                    Buttons[i].label.PosX = (Engine.ancho / 2) - (Buttons[i].label.Width/2);
                    Buttons[i].label.PosY = i == 0 ? title.PosY + title.Height + Buttons[i].label.Height : Buttons[i - 1].label.PosY + Buttons[i - 1].label.Height + Buttons[i].label.Height;
                }
            }

            OptionsSelector.posX = Buttons[ActualOption].label.PosX + Buttons[ActualOption].label.Width;
            OptionsSelector.posY = Buttons[ActualOption].label.PosY;
        }
        public void CheckInputs(ref int screen, ref bool gamePaused)
        {
           
            double tiempoTranscurrido = (DateTime.Now - lastTimeTouched).TotalMilliseconds;
            // Control de input y tiempos del mismo 
            if (gamePaused && tiempoTranscurrido >= 300) 
            { 

                if (Engine.KeyPress(Engine.KEY_W) && tiempoTranscurrido >= 300)
                {
                    if (ActualOption > 0 && ActualOption < Buttons.Length)
                    {
                        ActualOption--;
                        OptionsSelector.posX = Buttons[ActualOption].label.PosX + Buttons[ActualOption].label.Width;
                        OptionsSelector.posY = Buttons[ActualOption].label.PosY;
                    }
                    lastTimeTouched = DateTime.Now;
                }

                if (Engine.KeyPress(Engine.KEY_S) && tiempoTranscurrido >= 300)
                {
                    if (ActualOption >= 0 && ActualOption < Buttons.Length - 1)
                    {
                        ActualOption++;
                        OptionsSelector.posX = Buttons[ActualOption].label.PosX + Buttons[ActualOption].label.Width;
                        OptionsSelector.posY = Buttons[ActualOption].label.PosY;
                    }
                    lastTimeTouched = DateTime.Now;
                }
                if (Engine.KeyPress(Engine.KEY_ESC) && tiempoTranscurrido >= 300 && isInGame)
                {
                    gamePaused = false;
                    lastTimeTouched = DateTime.Now;
                    //return;
                }
                if (Buttons[ActualOption].CheckClick() || Engine.KeyPress(Engine.KEY_ESP))
                {
                    if (isInGame && Buttons[ActualOption].screenToLink == screen)
                    {
                        gamePaused = false;
                    }

                    if(!isInGame && Buttons[ActualOption].screenToLink == Stages.GAME)
                    {
                        gamePaused = false;
                    }
                    screen = Buttons[ActualOption].screenToLink;
                    lastTimeTouched = DateTime.Now;
                }         
            }
        }
        public void Draw()
        {
            if(backGround.image!=null)
            {
                Engine.Draw(backGround.image,backGround.posX, backGround.posY);
            }
            title.Draw();
            for (int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i].label.Draw();  
            }
            OptionsSelector.DrawSprite();
        }
    }
}
