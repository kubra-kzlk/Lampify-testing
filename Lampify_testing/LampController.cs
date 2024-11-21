using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lampify_testing
{// Verantwoordelijk voor de bediening van de lamp, bv het in- en uitschakelen.
    public class LampController
    {
        private readonly Lamp _lamp;

        public LampController(Lamp lamp)
        {
            _lamp = lamp;
        }

        public void ToggleLamp() //zet de lamp aan als deze uit is, of uit als deze aan is
        {
            if (_lamp.IsOn)
            {
                _lamp.TurnOff();
            }
            else
            {
                _lamp.TurnOn();
            }
        }

        public void AdjustSettings(int brightness, string color) //past helderheid en kleur aan, int helderheid, string kleur
        {
            _lamp.SetBrightness(brightness);
            _lamp.SetColor(color);
        }
    }

}
