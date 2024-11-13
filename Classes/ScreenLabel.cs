using System;
using Tao.Sdl;

namespace MyGame.Classes
{
    public class ScreenLabel
    {
        
        public string text;
        public Font font;
        private int posX, posY, width, height;
        public int PosX {get;set;}
        public int PosY {get;set;}
        public int Width {get;set;}
        public int Height {get;set;}
        public ScreenLabel(string text, Font font, int posX, int posY, int width=0, int height=0) 
        {
            this.text = text;
            this.font = font;
            this.PosX = posX;
            this.PosY = posY;
            IntPtr fontPtr = font.ReadPointer();
            int didGetSize = SdlTtf.TTF_SizeUNICODE(fontPtr, text, out width, out height);
            if (didGetSize == 0)
            {
                this.Width = width;
                this.Height = height;
            }

        }
        
        public void Draw()
        {
            Engine.DrawText(text, PosX, PosY, Width, Height, 255, 255, 255, font);
        }

    }
}
