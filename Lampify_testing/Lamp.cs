using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lampify_testing
{ //Bevat de logica vr aan/uit zetten, aanpassen helderheid en kleur.
    public class Lamp
    {
        public bool IsOn { get; private set; } = false; //geeft aan of de lamp aan of uit is
        public int Brightness { get; private set; } = 0;
        public string Color { get; private set; } = "White";
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
            /* if (brightness >= 0 && brightness <= 100)
             {
                 Brightness = brightness;
             }*/
            if (brightness < 0 || brightness > 100)
                throw new ArgumentOutOfRangeException(nameof(brightness), "Brightness must be between 0 and 100.");
            Brightness = brightness;
        }

        public void SetColor(string color)//verandert de kleur van de lamp
        {
            //Color = color;
            if (string.IsNullOrWhiteSpace(color))
                throw new ArgumentException("Color cannot be null or empty.", nameof(color));
            Color = color;
        }
    }

}
