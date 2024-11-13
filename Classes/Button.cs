using System;

namespace MyGame.Classes
{
    public class Button
    {
        public ScreenLabel label;
        public int screenToLink;
        public Button(ScreenLabel label, int linkTo)
        {
            this.label = label;
            this.screenToLink = linkTo;
        }
        public bool CheckClick()
        {
            int mouseX, mouseY;
            if (Engine.MouseClick(Engine.MOUSE_LEFT, out mouseX, out mouseY))
            {
                Console.WriteLine("Mouse Pos X e Y: " + mouseX + ", " + mouseY);
                Console.WriteLine("Ancho + posicion X de texto: " + (label.PosX + label.Width));
                Console.WriteLine("Alto + posicion Y de texto: " + (label.PosY + label.Height));

                return (mouseX < label.PosX + label.Width) &&
                        (mouseX > label.PosX) && (mouseY > label.PosY) &&
                        (mouseY < label.PosY + label.Height);
            }
            return false;

        }
    }
}
