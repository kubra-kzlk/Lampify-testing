using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lampify_testing
{ //Bevat de logica vr aan/uit zetten, aanpassen helderheid en kleur.
    public class Lamp
    {
        public bool IsOn { get; private set; } //geeft aan of de lamp aan of uit is
        public int Brightness { get; private set; } 
        public string Color { get; private set; }

        public void TurnOn() //lamp aanzetten
        {
            IsOn = true;
        }

        public void TurnOff() //lamp uitzetten
        {
            IsOn = false;
        }

        public void SetBrightness(int brightness) //helderheid aanpassen
        {
            if (brightness >= 0 && brightness <= 100)
            {
                Brightness = brightness;
            }
        }

        public void SetColor(string color)//verandert de kleur van de lamp
        {
            Color = color;
        }
    }

}
